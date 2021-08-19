#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Mmnv {
	public unsafe partial class IRequest : _Base_IRequest {}
	public class _Base_IRequest : IpcInterface {
		static readonly Logger Logger = new("IRequest");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // InitializeOld
					InitializeOld(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<uint>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // FinalizeOld
					FinalizeOld(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // SetAndWaitOld
					SetAndWaitOld(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<uint>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // GetOld
					var ret = GetOld(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 4: { // Initialize
					var ret = Initialize(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<uint>(16));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				case 5: { // Finalize
					Finalize(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // SetAndWait
					SetAndWait(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<uint>(16));
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Get
					var ret = Get(im.GetData<uint>(8));
					om.Initialize(0, 0, 4);
					om.SetData(8, ret);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IRequest: {im.CommandId}");
			}
		}
		
		public virtual void InitializeOld(uint _0, uint _1, uint _2) => "Stub hit for Nn.Mmnv.IRequest.InitializeOld [0]".Debug(Log);
		public virtual void FinalizeOld(uint _0) => "Stub hit for Nn.Mmnv.IRequest.FinalizeOld [1]".Debug(Log);
		public virtual void SetAndWaitOld(uint _0, uint _1, uint _2) => "Stub hit for Nn.Mmnv.IRequest.SetAndWaitOld [2]".Debug(Log);
		public virtual uint GetOld(uint _0) => throw new NotImplementedException();
		public virtual uint Initialize(uint _0, uint _1, uint _2) => throw new NotImplementedException();
		public virtual void Finalize(uint _0) => "Stub hit for Nn.Mmnv.IRequest.Finalize [5]".Debug(Log);
		public virtual void SetAndWait(uint _0, uint _1, uint _2) => "Stub hit for Nn.Mmnv.IRequest.SetAndWait [6]".Debug(Log);
		public virtual uint Get(uint _0) => throw new NotImplementedException();
	}
}
