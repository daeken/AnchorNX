using System;

namespace AnchorNX.IpcServices.Nn.Ssl.Sf {
	public partial class ISslContext {
		public override void SetOption(uint _0, uint _1) => "Stub hit for Nn.Ssl.Sf.ISslContext.SetOption [0]".Debug(Log);
		public override uint GetOption(uint _0) => throw new NotImplementedException();
		public override Nn.Ssl.Sf.ISslConnection CreateConnection() => throw new NotImplementedException();
		public override uint GetConnectionCount() => throw new NotImplementedException();
		public override ulong ImportServerPki(uint _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override ulong ImportClientPki(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public override void RemoveServerPki(ulong _0) => "Stub hit for Nn.Ssl.Sf.ISslContext.RemoveServerPki [6]".Debug(Log);
		public override void RemoveClientPki(ulong _0) => "Stub hit for Nn.Ssl.Sf.ISslContext.RemoveClientPki [7]".Debug(Log);
		public override ulong RegisterInternalPki(uint _0) => 0;
		public override void AddPolicyOid(Buffer<byte> _0) => "Stub hit for Nn.Ssl.Sf.ISslContext.AddPolicyOid [9]".Debug(Log);
		public override ulong ImportCrl(Buffer<byte> _0) => throw new NotImplementedException();
		public override void RemoveCrl(ulong _0) => "Stub hit for Nn.Ssl.Sf.ISslContext.RemoveCrl [11]".Debug(Log);
	}
}