#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Nsd.Detail {
	public unsafe partial class IManager : _Base_IManager {}
	public class _Base_IManager : IpcInterface {
		static readonly Logger Logger = new("IManager");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 10: { // GetSettingName
					GetSettingName(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // GetEnvironmentIdentifier
					GetEnvironmentIdentifier(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // GetDeviceId
					GetDeviceId(out var _0);
					om.Initialize(0, 0, 16);
					om.SetBytes(8, _0);
					break;
				}
				case 13: { // DeleteSettings
					DeleteSettings(im.GetData<uint>(8));
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // ImportSettings
					ImportSettings(im.GetData<uint>(8), im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 20: { // Resolve
					Resolve(im.GetBuffer<byte>(0x15, 0), im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 21: { // ResolveEx
					ResolveEx(im.GetBuffer<byte>(0x15, 0), out var _0, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 30: { // GetNasServiceSetting
					GetNasServiceSetting(im.GetBuffer<byte>(0x15, 0), im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 31: { // GetNasServiceSettingEx
					GetNasServiceSettingEx(im.GetBuffer<byte>(0x15, 0), out var _0, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 40: { // GetNasRequestFqdn
					GetNasRequestFqdn(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 41: { // GetNasRequestFqdnEx
					GetNasRequestFqdnEx(out var _0, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 42: { // GetNasApiFqdn
					GetNasApiFqdn(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 43: { // GetNasApiFqdnEx
					GetNasApiFqdnEx(out var _0, im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 4);
					om.SetData(8, _0);
					break;
				}
				case 50: { // GetCurrentSetting
					GetCurrentSetting(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 60: { // ReadSaveDataFromFsForTest
					ReadSaveDataFromFsForTest(im.GetBuffer<byte>(0x16, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 61: { // WriteSaveDataToFsForTest
					WriteSaveDataToFsForTest(im.GetBuffer<byte>(0x15, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 62: { // DeleteSaveDataOfFsForTest
					DeleteSaveDataOfFsForTest();
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IManager: {im.CommandId}");
			}
		}
		
		public virtual void GetSettingName(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetEnvironmentIdentifier(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetDeviceId(out byte[] _0) => throw new NotImplementedException();
		public virtual void DeleteSettings(uint _0) => "Stub hit for Nn.Nsd.Detail.IManager.DeleteSettings [13]".Debug(Log);
		public virtual void ImportSettings(uint _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Resolve(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ResolveEx(Buffer<byte> _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void GetNasServiceSetting(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetNasServiceSettingEx(Buffer<byte> _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void GetNasRequestFqdn(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetNasRequestFqdnEx(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetNasApiFqdn(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void GetNasApiFqdnEx(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetCurrentSetting(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void ReadSaveDataFromFsForTest(Buffer<byte> _0) => throw new NotImplementedException();
		public virtual void WriteSaveDataToFsForTest(Buffer<byte> _0) => "Stub hit for Nn.Nsd.Detail.IManager.WriteSaveDataToFsForTest [61]".Debug(Log);
		public virtual void DeleteSaveDataOfFsForTest() => "Stub hit for Nn.Nsd.Detail.IManager.DeleteSaveDataOfFsForTest [62]".Debug(Log);
	}
}
