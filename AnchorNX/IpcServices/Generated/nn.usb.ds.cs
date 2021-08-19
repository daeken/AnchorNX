#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Usb.Ds {
	public unsafe partial class IDsService : _Base_IDsService {}
	public class _Base_IDsService : IpcInterface {
		static readonly Logger Logger = new("IDsService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // BindDevice
					BindDevice(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // BindClientProcess
					BindClientProcess(im.GetCopy(0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // RegisterInterface
					var ret = RegisterInterface(im.GetData<byte>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 3: { // GetStateChangeEvent
					var ret = GetStateChangeEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 4: { // GetState
					var ret = GetState();
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 5: { // ClearDeviceData
					ClearDeviceData();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // AddUsbStringDescriptor
					var ret = AddUsbStringDescriptor(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 1);
					om.SetData(8, ret);
					break;
				}
				case 7: { // DeleteUsbStringDescriptor
					DeleteUsbStringDescriptor(im.GetData<byte>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // SetUsbDeviceDescriptor
					SetUsbDeviceDescriptor(im.GetData<Nn.Usb.UsbDeviceSpeed>(8), im.GetBuffer<Nn.Usb.UsbDeviceDescriptor>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // SetBinaryObjectStore
					SetBinaryObjectStore(im.GetBuffer<Nn.Usb.UsbBosDescriptor>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Enable
					Enable();
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // Disable
					Disable();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDsService: {im.CommandId}");
			}
		}
		
		public virtual void BindDevice(uint complexId) => "Stub hit for Nn.Usb.Ds.IDsService.BindDevice [0]".Debug(Log);
		public virtual void BindClientProcess(uint _0) => "Stub hit for Nn.Usb.Ds.IDsService.BindClientProcess [1]".Debug(Log);
		public virtual Nn.Usb.Ds.IDsInterface RegisterInterface(byte address) => throw new NotImplementedException();
		public virtual uint GetStateChangeEvent() => throw new NotImplementedException();
		public virtual uint GetState() => throw new NotImplementedException();
		public virtual void ClearDeviceData() => "Stub hit for Nn.Usb.Ds.IDsService.ClearDeviceData [5]".Debug(Log);
		public virtual byte AddUsbStringDescriptor(Buffer<byte> string_descriptor) => throw new NotImplementedException();
		public virtual void DeleteUsbStringDescriptor(byte index) => "Stub hit for Nn.Usb.Ds.IDsService.DeleteUsbStringDescriptor [7]".Debug(Log);
		public virtual void SetUsbDeviceDescriptor(Nn.Usb.UsbDeviceSpeed speed_mode, Buffer<Nn.Usb.UsbDeviceDescriptor> descriptor) => "Stub hit for Nn.Usb.Ds.IDsService.SetUsbDeviceDescriptor [8]".Debug(Log);
		public virtual void SetBinaryObjectStore(Buffer<Nn.Usb.UsbBosDescriptor> _0) => "Stub hit for Nn.Usb.Ds.IDsService.SetBinaryObjectStore [9]".Debug(Log);
		public virtual void Enable() => "Stub hit for Nn.Usb.Ds.IDsService.Enable [10]".Debug(Log);
		public virtual void Disable() => "Stub hit for Nn.Usb.Ds.IDsService.Disable [11]".Debug(Log);
	}
	
	public unsafe partial class IDsEndpoint : _Base_IDsEndpoint {}
	public class _Base_IDsEndpoint : IpcInterface {
		static readonly Logger Logger = new("IDsEndpoint");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // PostBufferAsync
					var ret = PostBufferAsync(im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 1: { // Cancel
					Cancel();
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // GetCompletionEvent
					var ret = GetCompletionEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 3: { // GetReportData
					GetReportData(om.GetDataSpan<Nn.Usb.UsbReportEntry>(8), out var _1);
					om.Initialize(0, 0, 4);
					om.SetData(8, _1);
					break;
				}
				case 4: { // Stall
					Stall();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // SetZlt
					SetZlt(im.GetData<bool>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDsEndpoint: {im.CommandId}");
			}
		}
		
		public virtual uint PostBufferAsync(uint size, ulong buffer) => throw new NotImplementedException();
		public virtual void Cancel() => "Stub hit for Nn.Usb.Ds.IDsEndpoint.Cancel [1]".Debug(Log);
		public virtual uint GetCompletionEvent() => throw new NotImplementedException();
		public virtual void GetReportData(Span< Nn.Usb.UsbReportEntry> entries, out uint report_count) => throw new NotImplementedException();
		public virtual void Stall() => "Stub hit for Nn.Usb.Ds.IDsEndpoint.Stall [4]".Debug(Log);
		public virtual void SetZlt(bool _0) => "Stub hit for Nn.Usb.Ds.IDsEndpoint.SetZlt [5]".Debug(Log);
	}
	
	public unsafe partial class IDsInterface : _Base_IDsInterface {}
	public class _Base_IDsInterface : IpcInterface {
		static readonly Logger Logger = new("IDsInterface");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // RegisterEndpoint
					var ret = RegisterEndpoint(im.GetData<byte>(8));
					om.Initialize(1, 0, 0);
					om.Move(0, await CreateHandle(ret));
					break;
				}
				case 1: { // GetSetupEvent
					var ret = GetSetupEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 2: { // GetSetupPacket
					GetSetupPacket(im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // EnableInterface
					EnableInterface();
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // DisableInterface
					DisableInterface();
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // CtrlInPostBufferAsync
					var ret = CtrlInPostBufferAsync(im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 6: { // CtrlOutPostBufferAsync
					var ret = CtrlOutPostBufferAsync(im.GetData<uint>(8), im.GetData<ulong>(16));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 7: { // GetCtrlInCompletionEvent
					var ret = GetCtrlInCompletionEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 8: { // GetCtrlInReportData
					GetCtrlInReportData(om.GetDataSpan<Nn.Usb.UsbReportEntry>(8), out var _1);
					om.Initialize(0, 0, 4);
					om.SetData(8, _1);
					break;
				}
				case 9: { // GetCtrlOutCompletionEvent
					var ret = GetCtrlOutCompletionEvent();
					om.Initialize(0, 1, 0);
					om.Copy(0, await CreateHandle(ret, copy: true));
					break;
				}
				case 10: { // GetCtrlOutReportData
					GetCtrlOutReportData(om.GetDataSpan<Nn.Usb.UsbReportEntry>(8), out var _1);
					om.Initialize(0, 0, 4);
					om.SetData(8, _1);
					break;
				}
				case 11: { // StallCtrl
					StallCtrl();
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // AppendConfigurationData
					AppendConfigurationData(im.GetData<byte>(8), im.GetData<Nn.Usb.UsbDeviceSpeed>(12), im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IDsInterface: {im.CommandId}");
			}
		}
		
		public virtual Nn.Usb.Ds.IDsEndpoint RegisterEndpoint(byte address) => throw new NotImplementedException();
		public virtual uint GetSetupEvent() => throw new NotImplementedException();
		public virtual void GetSetupPacket(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void EnableInterface() => "Stub hit for Nn.Usb.Ds.IDsInterface.EnableInterface [3]".Debug(Log);
		public virtual void DisableInterface() => "Stub hit for Nn.Usb.Ds.IDsInterface.DisableInterface [4]".Debug(Log);
		public virtual uint CtrlInPostBufferAsync(uint size, ulong buffer) => throw new NotImplementedException();
		public virtual uint CtrlOutPostBufferAsync(uint size, ulong buffer) => throw new NotImplementedException();
		public virtual uint GetCtrlInCompletionEvent() => throw new NotImplementedException();
		public virtual void GetCtrlInReportData(Span< Nn.Usb.UsbReportEntry> entries, out uint report_count) => throw new NotImplementedException();
		public virtual uint GetCtrlOutCompletionEvent() => throw new NotImplementedException();
		public virtual void GetCtrlOutReportData(Span< Nn.Usb.UsbReportEntry> entries, out uint report_count) => throw new NotImplementedException();
		public virtual void StallCtrl() => "Stub hit for Nn.Usb.Ds.IDsInterface.StallCtrl [11]".Debug(Log);
		public virtual void AppendConfigurationData(byte interface_number, Nn.Usb.UsbDeviceSpeed speed_mode, Buffer<byte> descriptor) => "Stub hit for Nn.Usb.Ds.IDsInterface.AppendConfigurationData [12]".Debug(Log);
	}
}
