using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Ryujinx.Memory;

namespace AnchorNX.IpcServices.Nns.Nvdrv.NvDrvServices.NvDispCtrl {
	[StructLayout(LayoutKind.Sequential)]
	struct GetDisplayProperties {
		public uint Unk0;
		public uint DisplayIndex;
		public uint Value;
		public uint DisplayIndexAgain;
		public uint DisplayIndexOff;
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct QueryEdid {
		public fixed byte Data[0x210];
	}
	
	class NvDispCtrlDeviceFile : NvDeviceFile {
        static readonly Logger Logger = new("NvDispCtrlDeviceFile");
        static Action<string> Log = Logger.Log;

        readonly HosEvent UnkEvent0, UnkEvent1, UnkEvent2;
        
        public NvDispCtrlDeviceFile(IVirtualMemoryManager memory, long owner) : base(owner) {
	        UnkEvent0 = Box.EventManager.GetEvent();
	        UnkEvent1 = Box.EventManager.GetEvent();
	        UnkEvent2 = Box.EventManager.GetEvent();
        }

        public override NvInternalResult Ioctl(NvIoctl command, Span<byte> arguments) {
        	var result = NvInternalResult.NotImplemented;
            Log($"Ioctl to NvDispCtrlDeviceFile! Command 0x{command.Number:X} Type 0x{command.Type:X}");

            if(command.Type == NvIoctl.NvDispCtrlMagic)
        		switch(command.Number) {
	                case 0x12:
		                result = CallIoctlMethod<uint>(GetNumOutputs, arguments);
		                break;
	                case 0x13:
		                arguments.Hexdump(Logger);
		                result = CallIoctlMethod<GetDisplayProperties>(GetDisplayProperties, arguments);
		                break;
	                case 0x14:
		                Log("Stub for NVDISP_CTRL_QUERY_EDID");
		                arguments.Hexdump(Logger);
		                result = CallIoctlMethod<QueryEdid>(QueryEdid, arguments);
		                break;
	                case 0x24:
		                Log("Stub for unknown ioctl 0x80010224");
		                result = NvInternalResult.Success;
		                break;
	                default:
		                Log($"Unsupported ioctl to NvDispCtrlDeviceFile: 0x{command.Number:X}");
		                break;
        		}

        	return result;
        }

        public override NvInternalResult Ioctl3(NvIoctl command, Span<byte> arguments, Span<byte> inlineOutBuffer) {
        	var result = NvInternalResult.NotImplemented;

        	if(command.Type == NvIoctl.NvDispCtrlMagic)
        		switch(command.Number) {
	                default:
		                Log($"Unsupported ioctl3 to NvDispCtrlDeviceFile: 0x{command.Number:X}");
		                break;
        		}

        	return result;
        }
        
        NvInternalResult GetNumOutputs(ref uint num) {
	        num = 2;
	        return NvInternalResult.Success;
        }

        NvInternalResult GetDisplayProperties(ref GetDisplayProperties arg) {
	        Log("GetDisplayProperties??");
	        /*arg.Unk0 = new uint[] { 1, 3, 0, 0, 1, 5 }[0];
	        arg.DisplayIndexAgain = arg.DisplayIndex;
	        arg.DisplayIndexOff = 1U << (int) arg.DisplayIndex;
	        arg.Value = 0xFF;*/
	        arg.Unk0 = 0;
	        arg.DisplayIndex = 0;
	        arg.Value = 1;
	        arg.DisplayIndexAgain = 0;
	        arg.DisplayIndexOff = 1;
	        return NvInternalResult.Success;
        }

        unsafe NvInternalResult QueryEdid(ref QueryEdid arg) {
	        fixed(byte* bd = arg.Data) {
		        var ud = (uint*) bd;
		        ud[0] = 0x80;
		        File.ReadAllBytes("edid_1280x720.bin").CopyTo(new Span<byte>(bd + 4, 0x100));
	        }
	        return NvInternalResult.Success;
        }

        public override NvInternalResult QueryEvent(out HosEvent eventHandle, uint eventId) {
	        Log($"QueryEvent 0x{eventId:X}");
	        eventHandle = eventId switch {
		        0 => UnkEvent0, 
		        1 => UnkEvent1, 
		        2 => UnkEvent2, 
		        _ => throw new NotImplementedException()
	        };
	        return NvInternalResult.Success;
        }

        public override void Close() { }
	}
}