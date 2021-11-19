using System;
using System.Text;

namespace AnchorNX.IpcServices.Nn.Nsd.Detail {
	public partial class IManager {
		public override void GetSettingName(Buffer<byte> _0) => throw new NotImplementedException();
		public override void GetEnvironmentIdentifier(Buffer<byte> _0) {
			_0.CopyFrom(Encoding.ASCII.GetBytes("foobar"));
		}

		public override void GetDeviceId(out byte[] _0) => throw new NotImplementedException();
		public override void DeleteSettings(uint _0) => "Stub hit for Nn.Nsd.Detail.IManager.DeleteSettings [13]".Debug(Log);
		public override void ImportSettings(uint _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void Resolve(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void ResolveEx(Buffer<byte> _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void GetNasServiceSetting(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void GetNasServiceSettingEx(Buffer<byte> _0, out uint _1, Buffer<byte> _2) => throw new NotImplementedException();
		public override void GetNasRequestFqdn(Buffer<byte> _0) => throw new NotImplementedException();
		public override void GetNasRequestFqdnEx(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void GetNasApiFqdn(Buffer<byte> _0) => throw new NotImplementedException();
		public override void GetNasApiFqdnEx(out uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void GetCurrentSetting(Buffer<byte> _0) => throw new NotImplementedException();
		public override void ReadSaveDataFromFsForTest(Buffer<byte> _0) => throw new NotImplementedException();
		public override void WriteSaveDataToFsForTest(Buffer<byte> _0) => "Stub hit for Nn.Nsd.Detail.IManager.WriteSaveDataToFsForTest [61]".Debug(Log);
		public override void DeleteSaveDataOfFsForTest() => "Stub hit for Nn.Nsd.Detail.IManager.DeleteSaveDataOfFsForTest [62]".Debug(Log);
	}
}