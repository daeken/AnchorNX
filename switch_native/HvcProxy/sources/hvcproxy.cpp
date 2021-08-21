#include <cstdlib>
#include <cstdint>
#include <cstring>
#include <cstdio>
#include <malloc.h>

#include <switch.h>

extern "C" {
    extern u32 __start__;

    u32 __nx_applet_type = AppletType_None;
    u32 __nx_fs_num_sessions = 1;

    #define INNER_HEAP_SIZE 0x100000
    size_t nx_inner_heap_size = INNER_HEAP_SIZE;
    char   nx_inner_heap[INNER_HEAP_SIZE];

    void __libnx_initheap(void);
    void __appInit(void);
    void __appExit(void);
}

void __libnx_initheap(void) {
    void*  addr = nx_inner_heap;
    size_t size = nx_inner_heap_size;

    /* Newlib */
    extern char* fake_heap_start;
    extern char* fake_heap_end;

    fake_heap_start = (char*)addr;
    fake_heap_end   = (char*)addr + size;
}

volatile void* messageBoxBase;
volatile uint64_t* messageBox;
volatile void* sharedMemBase;
volatile uint64_t* sharedMem;
Handle wakeInterruptEvent, eventInterruptEvent;

void log(const char* message) {
    messageBox[1] = (uint64_t) message;
}

void __appInit(void) {
    hosversionSet(MAKEHOSVERSION(12, 0, 0));
    u64 virtAddr;
    u64 outSize;
    svcQueryIoMapping(&virtAddr, &outSize, 0x57000000, 0x10000);
    messageBoxBase = (void*) virtAddr;
    messageBox = (uint64_t*) messageBoxBase;
    log("Mapped message box!");
    svcQueryIoMapping(&virtAddr, &outSize, 0x57010000, 0xF0000);
    sharedMemBase = (void*) virtAddr;
    sharedMem = (uint64_t*) sharedMemBase;
    svcCreateInterruptEvent(&wakeInterruptEvent, 0x2E, 1);
    svcCreateInterruptEvent(&eventInterruptEvent, 0x2F, 1);
}

void __appExit(void) {
}

void wakeHvc() {
    messageBox[0] = 0;
}

void killHvc() {
    messageBox[0] = 1;
}

#pragma pack(push, 0)
struct MagicWaiter {
    uint64_t lastResult, lastIndex, handleCount;
    Handle notificationEvent, startEvent;
    Handle notificationEventWrite, startEventWrite;
    Handle waitList[0x40];
};
#pragma pack(pop)

void waitThread(void* arg) {
    volatile MagicWaiter* waiter = (volatile MagicWaiter*) arg;
    while(true) {
        svcWaitSynchronizationSingle(waiter->startEvent, -1);
        svcClearEvent(waiter->startEvent);
        do {
            waiter->lastResult = svcWaitSynchronization((s32*) &waiter->lastIndex, (const Handle*) waiter->waitList, (int) waiter->handleCount, 100000000);
        } while(waiter->lastResult == 0xea01);
        log("Triggering from waiter thread");
        char buf[1024];
        sprintf(buf, "Waiter thread trigger? %x %i", (Result) waiter->lastResult, (s32) waiter->lastIndex);
        log(buf);
        svcSignalEvent(waiter->notificationEventWrite);
    }
}

int main(int argc, char **argv) {
    smInitialize();
    log("Starting HvcProxy");
    log("Flagging HVC and waiting for wakeup");
    sharedMem[0] = (uint64_t) wakeInterruptEvent;
    sharedMem[1] = (uint64_t) eventInterruptEvent;
    sharedMem[2] = (uint64_t) messageBoxBase;
    uint64_t tlsAddr;
    asm("mrs %0, tpidrro_el0" : "=r" (tlsAddr));
    sharedMem[3] = tlsAddr;
    sharedMem[4] = (uint64_t) malloc(0x2000);
    wakeHvc();
    uint64_t reqBufferAddr = (uint64_t) malloc(0x2000);
    while(reqBufferAddr & 0xFFF)
        reqBufferAddr++;
    void* reqBuffer = (void*) reqBufferAddr;
    while(true) {
        log("Going back to sleep...");
        Result rc = svcWaitSynchronizationSingle(wakeInterruptEvent, -1);
        if(rc != 0) {
            log("Wait for interrupt failed?!");
            return 0;
        }
        rc = svcClearEvent(wakeInterruptEvent);
        if(rc != 0) {
            log("Clear interrupt failed?!");
            return 0;
        }
        log("Woken up; running iteration of command loop");
        switch(sharedMem[0]) {
            case 0: { // WaitSynchronization
                int32_t index = 0;
                auto handles = (Handle*) &sharedMem[4];
                rc = svcWaitSynchronization(&index, handles, sharedMem[1], sharedMem[2]);
                sharedMem[0] = rc;
                sharedMem[1] = (uint64_t) (int64_t) index;
                wakeHvc();
                break;
            }
            case 1: { // Register service
                Handle port;
                rc = smRegisterService(&port, smServiceNameFromU64(sharedMem[1]), false, (s32) sharedMem[2]);
                sharedMem[0] = rc;
                sharedMem[1] = port;
                wakeHvc();
                break;
            }
            case 2: { // Accept session
                Handle session;
                rc = svcAcceptSession(&session, (Handle) sharedMem[1]);
                sharedMem[0] = rc;
                sharedMem[1] = session;
                wakeHvc();
                break;
            }
            case 3: { // Receive
                int32_t index;
                rc = svcReplyAndReceive(&index, (Handle*) &sharedMem[1], 1, 0, sharedMem[2]);
                sharedMem[0] = rc;
                wakeHvc();
                break;
            }
            case 4: { // Reply
                int32_t index;
                rc = svcReplyAndReceive(&index, (Handle*) &sharedMem[0], 0, sharedMem[1], sharedMem[2]);
                sharedMem[0] = rc;
                wakeHvc();
                break;
            }
            case 5: { // CreateSession
                Handle server, client;
                rc = svcCreateSession(&server, &client, 0, 0);
                sharedMem[0] = rc;
                sharedMem[1] = server;
                sharedMem[2] = client;
                wakeHvc();
                break;
            }
            case 6: { // GetService
                Service service;
                rc = smGetService(&service, (const char*) &sharedMem[1]);
                sharedMem[0] = rc;
                sharedMem[1] = service.session;
                wakeHvc();
                break;
            }
            case 7: { // SendAsyncRequest
                Handle doneEvent;
                memcpy(reqBuffer, (void*) tlsAddr, 0x100);
                sharedMem[0] = svcSendAsyncRequestWithUserBuffer(&doneEvent, reqBuffer, 0x1000, (Handle) sharedMem[1]);
                wakeHvc();
                break;
            }
            case 8: { // ResetSignal
                sharedMem[0] = svcResetSignal((Handle) sharedMem[1]);
                wakeHvc();
                break;
            }
            case 9: { // OpenLocationResolver
                lrInitialize();
                LrLocationResolver* resolver = (LrLocationResolver*) malloc(sizeof(LrLocationResolver));
                sharedMem[0] = lrOpenLocationResolver((NcmStorageId) sharedMem[1], resolver);
                sharedMem[1] = (uint64_t) resolver;
                wakeHvc();
                break;
            }
            case 10: { // ResolveDataPath
                sharedMem[0] = lrLrResolveDataPath((LrLocationResolver*) sharedMem[1], sharedMem[2], (char*) &sharedMem[4]);
                wakeHvc();
                break;
            }
            case 11: { // Create waiter thread
                MagicWaiter* waiter = (MagicWaiter*) malloc(sizeof(MagicWaiter));
                Thread t;
                svcCreateEvent(&waiter->notificationEventWrite, &waiter->notificationEvent);
                svcCreateEvent(&waiter->startEventWrite, &waiter->startEvent);
                waiter->handleCount = 0;
                threadCreate(&t, waitThread, (void*) waiter, NULL, 0x1000, 0x2C, -2);
                threadStart(&t);
                sharedMem[0] = (uint64_t) waiter;
                wakeHvc();
                break;
            }
            case 12: { // SignalEvent
                sharedMem[0] = svcSignalEvent((Handle) sharedMem[1]);
                wakeHvc();
                break;
            }
            case 13: { // DumpHandleTable
                svcKernelDebug(3, 0xFFFFFFFFFFFFFFFF, 0, 0);
                wakeHvc();
                break;
            }
            default: {
                log("Unknown message! Bailing");
                return 0;
            }
        }
        log("End of iteration...");
    }
    return 0;
}