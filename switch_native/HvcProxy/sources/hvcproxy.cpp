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
    wakeHvc();
    while(true) {
        log("Going back to sleep...");
        Result rc = svcWaitSynchronizationSingle(wakeInterruptEvent, -1);
        log("Woken up; running iteration of command loop");
        switch(sharedMem[0]) {
            case 0: { // WaitSynchronization
                int32_t index;
                log("Calling waitsync");
                rc = svcWaitSynchronization(&index, (Handle*) &sharedMem[4], sharedMem[1], sharedMem[2]);
                log("Waitsync returned");
                sharedMem[0] = rc;
                sharedMem[1] = (uint64_t) (int64_t) index;
                log("About to wake hvc");
                wakeHvc();
                log("Woke hvc");
                break;
            }
            case 1: { // Register service
                Handle port;
                rc = smRegisterService(&port, smServiceNameFromU64(sharedMem[1]), false, (s32) sharedMem[2]);
                sharedMem[0] = rc;
                sharedMem[1] = port;
                log("About to wake hvc...");
                wakeHvc();
                log("Woke hvc?!");
                break;
            }
            case 2: { // Accept session
                Handle session;
                rc = svcAcceptSession(&session, (Handle) sharedMem[1]);
                sharedMem[0] = rc;
                sharedMem[1] = session;
                log("About to wake hvc...");
                wakeHvc();
                log("Woke hvc?!");
                break;
            }
            case 3: { // Receive and reply
                int32_t index;
                rc = svcReplyAndReceive(&index, (Handle*) &sharedMem[1], 1, (Handle) sharedMem[2], sharedMem[3]);
                sharedMem[0] = rc;
                log("About to wake hvc...");
                wakeHvc();
                log("Woke hvc?!");
                break;
            }
            default: {
                log("Unknown message! Bailing");
                return 0;
            }
        }
        log("End of iteration...");
    }
    log("Loop ended?!");

    /*rc = smRegisterService(&g_port, smEncodeName("foobar"), false, 1000);
    if(R_FAILED(rc)) {
        log("Service registration failed!");
        return 0;
    }
    log("Service registration succeeded!");*/
    return 0;
}