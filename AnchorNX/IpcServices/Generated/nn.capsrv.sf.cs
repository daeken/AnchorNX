#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Capsrv.Sf {
	public unsafe partial class IAlbumAccessorService : _Base_IAlbumAccessorService {}
	public class _Base_IAlbumAccessorService : IpcInterface {
		static readonly Logger Logger = new("IAlbumAccessorService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 0: { // Unknown0
					var ret = Unknown0(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1: { // Unknown1
					Unknown1(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // Unknown3
					Unknown3(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // Unknown4
					Unknown4(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // Unknown5
					var ret = Unknown5(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // Unknown6
					var ret = Unknown6(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // Unknown7
					var ret = Unknown7(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8: { // Unknown8
					Unknown8(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // Unknown9
					Unknown9(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // Unknown10
					Unknown10(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // Unknown11
					var ret = Unknown11(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // Unknown12
					Unknown12(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // Unknown13
					Unknown13(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // Unknown14
					Unknown14(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 301: { // Unknown301
					Unknown301(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 401: { // Unknown401
					var ret = Unknown401();
					om.Initialize(0, 0, 0);
					break;
				}
				case 501: { // Unknown501
					var ret = Unknown501(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // Unknown1001
					Unknown1001(null, out var _0, im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1002: { // Unknown1002
					Unknown1002(null, im.GetBuffer<byte>(0x16, 0), im.GetBuffer<byte>(0x46, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 8001: { // Unknown8001
					Unknown8001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8002: { // Unknown8002
					Unknown8002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8011: { // Unknown8011
					Unknown8011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8012: { // Unknown8012
					var ret = Unknown8012(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 8021: { // Unknown8021
					var ret = Unknown8021(null, im.Pid);
					om.Initialize(0, 0, 0);
					break;
				}
				case 10011: { // Unknown10011
					Unknown10011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumAccessorService: {im.CommandId}");
			}
		}
		
		public virtual object Unknown0(object _0) => throw new NotImplementedException();
		public virtual void Unknown1(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown3(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown3 [3]".Debug(Log);
		public virtual void Unknown4(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown4 [4]".Debug(Log);
		public virtual object Unknown5(object _0) => throw new NotImplementedException();
		public virtual object Unknown6(object _0) => throw new NotImplementedException();
		public virtual object Unknown7(object _0) => throw new NotImplementedException();
		public virtual void Unknown8(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown9(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown10(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual object Unknown11(object _0) => throw new NotImplementedException();
		public virtual void Unknown12(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown13(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown14(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown301(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual object Unknown401() => throw new NotImplementedException();
		public virtual object Unknown501(object _0) => throw new NotImplementedException();
		public virtual void Unknown1001(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown1002(object _0, Buffer<byte> _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void Unknown8001(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown8001 [8001]".Debug(Log);
		public virtual void Unknown8002(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown8002 [8002]".Debug(Log);
		public virtual void Unknown8011(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown8011 [8011]".Debug(Log);
		public virtual object Unknown8012(object _0) => throw new NotImplementedException();
		public virtual object Unknown8021(object _0, ulong _1) => throw new NotImplementedException();
		public virtual void Unknown10011(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorService.Unknown10011 [10011]".Debug(Log);
	}
	
	public unsafe partial class IAlbumApplicationService : _Base_IAlbumApplicationService {}
	public class _Base_IAlbumApplicationService : IpcInterface {
		static readonly Logger Logger = new("IAlbumApplicationService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 102: { // GetAlbumFileListByAruid
					var ret = GetAlbumFileListByAruid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 103: { // DeleteAlbumFileByAruid
					var ret = DeleteAlbumFileByAruid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 104: { // GetAlbumFileSizeByAruid
					var ret = GetAlbumFileSizeByAruid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 110: { // LoadAlbumScreenShotImageByAruid
					var ret = LoadAlbumScreenShotImageByAruid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 120: { // LoadAlbumScreenShotThumbnailImageByAruid
					var ret = LoadAlbumScreenShotThumbnailImageByAruid(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 60002: { // OpenAccessorSessionForApplication
					var ret = OpenAccessorSessionForApplication(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumApplicationService: {im.CommandId}");
			}
		}
		
		public virtual object GetAlbumFileListByAruid(object _0) => throw new NotImplementedException();
		public virtual object DeleteAlbumFileByAruid(object _0) => throw new NotImplementedException();
		public virtual object GetAlbumFileSizeByAruid(object _0) => throw new NotImplementedException();
		public virtual object LoadAlbumScreenShotImageByAruid(object _0) => throw new NotImplementedException();
		public virtual object LoadAlbumScreenShotThumbnailImageByAruid(object _0) => throw new NotImplementedException();
		public virtual object OpenAccessorSessionForApplication(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAlbumControlService : _Base_IAlbumControlService {}
	public class _Base_IAlbumControlService : IpcInterface {
		static readonly Logger Logger = new("IAlbumControlService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 2001: { // Unknown2001
					Unknown2001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2002: { // Unknown2002
					Unknown2002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2011: { // Unknown2011
					Unknown2011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2012: { // Unknown2012
					Unknown2012(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2013: { // Unknown2013
					var ret = Unknown2013(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2014: { // Unknown2014
					Unknown2014(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2101: { // Unknown2101
					var ret = Unknown2101(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2102: { // Unknown2102
					var ret = Unknown2102(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2201: { // Unknown2201
					Unknown2201(null, im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2301: { // Unknown2301
					Unknown2301(null, im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumControlService: {im.CommandId}");
			}
		}
		
		public virtual void Unknown2001(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2001 [2001]".Debug(Log);
		public virtual void Unknown2002(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2002 [2002]".Debug(Log);
		public virtual void Unknown2011(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2011 [2011]".Debug(Log);
		public virtual void Unknown2012(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2012 [2012]".Debug(Log);
		public virtual object Unknown2013(object _0) => throw new NotImplementedException();
		public virtual void Unknown2014(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2014 [2014]".Debug(Log);
		public virtual object Unknown2101(object _0) => throw new NotImplementedException();
		public virtual object Unknown2102(object _0) => throw new NotImplementedException();
		public virtual void Unknown2201(object _0, Buffer<byte> _1) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2201 [2201]".Debug(Log);
		public virtual void Unknown2301(object _0, Buffer<byte> _1) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlService.Unknown2301 [2301]".Debug(Log);
	}
	
	public unsafe partial class IScreenShotApplicationService : _Base_IScreenShotApplicationService {}
	public class _Base_IScreenShotApplicationService : IpcInterface {
		static readonly Logger Logger = new("IScreenShotApplicationService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 201: { // SaveScreenShot
					SaveScreenShot(im.GetData<uint>(8), im.GetData<uint>(12), im.GetData<ulong>(16), im.Pid, im.GetBuffer<byte>(0x45, 0), out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				case 203: { // SaveScreenShotEx0
					SaveScreenShotEx0(im.GetBytes(8, 0x40), im.GetData<uint>(72), im.GetData<ulong>(80), im.Pid, im.GetBuffer<byte>(0x45, 0), out var _0);
					om.Initialize(0, 0, 32);
					om.SetBytes(8, _0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IScreenShotApplicationService: {im.CommandId}");
			}
		}
		
		public virtual void SaveScreenShot(uint _0, uint _1, ulong _2, ulong _3, Buffer<byte> _4, out byte[] _5) => throw new NotImplementedException();
		public virtual void SaveScreenShotEx0(byte[] _0, uint _1, ulong _2, ulong _3, Buffer<byte> _4, out byte[] _5) => throw new NotImplementedException();
	}
	
	public unsafe partial class IScreenShotControlService : _Base_IScreenShotControlService {}
	public class _Base_IScreenShotControlService : IpcInterface {
		static readonly Logger Logger = new("IScreenShotControlService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // Unknown1
					Unknown1(null, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // Unknown1001
					Unknown1001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1002: { // Unknown1002
					Unknown1002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1003: { // Unknown1003
					Unknown1003(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1011: { // Unknown1011
					Unknown1011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1012: { // Unknown1012
					Unknown1012(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1201: { // Unknown1201
					var ret = Unknown1201(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1202: { // Unknown1202
					Unknown1202();
					om.Initialize(0, 0, 0);
					break;
				}
				case 1203: { // Unknown1203
					Unknown1203(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IScreenShotControlService: {im.CommandId}");
			}
		}
		
		public virtual void Unknown1(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown1001(object _0) => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1001 [1001]".Debug(Log);
		public virtual void Unknown1002(object _0) => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1002 [1002]".Debug(Log);
		public virtual void Unknown1003(object _0) => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1003 [1003]".Debug(Log);
		public virtual void Unknown1011(object _0) => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1011 [1011]".Debug(Log);
		public virtual void Unknown1012(object _0) => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1012 [1012]".Debug(Log);
		public virtual object Unknown1201(object _0) => throw new NotImplementedException();
		public virtual void Unknown1202() => "Stub hit for Nn.Capsrv.Sf.IScreenShotControlService.Unknown1202 [1202]".Debug(Log);
		public virtual void Unknown1203(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IScreenShotService : _Base_IScreenShotService {}
	public class _Base_IScreenShotService : IpcInterface {
		static readonly Logger Logger = new("IScreenShotService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 201: { // Unknown201
					var ret = Unknown201(null, im.Pid, im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 202: { // Unknown202
					var ret = Unknown202(null, im.GetBuffer<byte>(0x45, 0), im.GetBuffer<byte>(0x45, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 203: { // Unknown203
					var ret = Unknown203(null, im.Pid, im.GetBuffer<byte>(0x45, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 204: { // Unknown204
					var ret = Unknown204(null, im.GetBuffer<byte>(0x45, 0), im.GetBuffer<byte>(0x45, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IScreenShotService: {im.CommandId}");
			}
		}
		
		public virtual object Unknown201(object _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown202(object _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown203(object _0, ulong _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown204(object _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAlbumAccessorApplicationSession : _Base_IAlbumAccessorApplicationSession {}
	public class _Base_IAlbumAccessorApplicationSession : IpcInterface {
		static readonly Logger Logger = new("IAlbumAccessorApplicationSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 2001: { // OpenAlbumMovieReadStream
					var ret = OpenAlbumMovieReadStream(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2002: { // CloseAlbumMovieReadStream
					var ret = CloseAlbumMovieReadStream(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2003: { // GetAlbumMovieReadStreamMovieDataSize
					var ret = GetAlbumMovieReadStreamMovieDataSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2004: { // ReadMovieDataFromAlbumMovieReadStream
					var ret = ReadMovieDataFromAlbumMovieReadStream(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2005: { // GetAlbumMovieReadStreamBrokenReason
					var ret = GetAlbumMovieReadStreamBrokenReason(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumAccessorApplicationSession: {im.CommandId}");
			}
		}
		
		public virtual object OpenAlbumMovieReadStream(object _0) => throw new NotImplementedException();
		public virtual object CloseAlbumMovieReadStream(object _0) => throw new NotImplementedException();
		public virtual object GetAlbumMovieReadStreamMovieDataSize(object _0) => throw new NotImplementedException();
		public virtual object ReadMovieDataFromAlbumMovieReadStream(object _0) => throw new NotImplementedException();
		public virtual object GetAlbumMovieReadStreamBrokenReason(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAlbumAccessorSession : _Base_IAlbumAccessorSession {}
	public class _Base_IAlbumAccessorSession : IpcInterface {
		static readonly Logger Logger = new("IAlbumAccessorSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 2001: { // Unknown2001
					var ret = Unknown2001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2002: { // Unknown2002
					Unknown2002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2003: { // Unknown2003
					var ret = Unknown2003(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2004: { // Unknown2004
					Unknown2004(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2005: { // Unknown2005
					Unknown2005(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2006: { // Unknown2006
					var ret = Unknown2006(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2007: { // Unknown2007
					Unknown2007(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2008: { // Unknown2008
					var ret = Unknown2008(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumAccessorSession: {im.CommandId}");
			}
		}
		
		public virtual object Unknown2001(object _0) => throw new NotImplementedException();
		public virtual void Unknown2002(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorSession.Unknown2002 [2002]".Debug(Log);
		public virtual object Unknown2003(object _0) => throw new NotImplementedException();
		public virtual void Unknown2004(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown2005(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumAccessorSession.Unknown2005 [2005]".Debug(Log);
		public virtual object Unknown2006(object _0) => throw new NotImplementedException();
		public virtual void Unknown2007(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown2008(object _0) => throw new NotImplementedException();
	}
	
	public unsafe partial class IAlbumControlSession : _Base_IAlbumControlSession {}
	public class _Base_IAlbumControlSession : IpcInterface {
		static readonly Logger Logger = new("IAlbumControlSession");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 2001: { // Unknown2001
					var ret = Unknown2001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2002: { // Unknown2002
					Unknown2002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2003: { // Unknown2003
					var ret = Unknown2003(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2004: { // Unknown2004
					Unknown2004(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2005: { // Unknown2005
					Unknown2005(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2006: { // Unknown2006
					var ret = Unknown2006(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2007: { // Unknown2007
					Unknown2007(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2008: { // Unknown2008
					var ret = Unknown2008(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2401: { // Unknown2401
					var ret = Unknown2401(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2402: { // Unknown2402
					Unknown2402(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2403: { // Unknown2403
					Unknown2403(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2404: { // Unknown2404
					Unknown2404(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2405: { // Unknown2405
					Unknown2405(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2411: { // Unknown2411
					Unknown2411(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2412: { // Unknown2412
					Unknown2412(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2413: { // Unknown2413
					Unknown2413(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2414: { // Unknown2414
					Unknown2414(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2421: { // Unknown2421
					Unknown2421(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2422: { // Unknown2422
					Unknown2422(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2424: { // Unknown2424
					Unknown2424(null, im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2431: { // Unknown2431
					Unknown2431(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2433: { // Unknown2433
					var ret = Unknown2433(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2434: { // Unknown2434
					Unknown2434(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IAlbumControlSession: {im.CommandId}");
			}
		}
		
		public virtual object Unknown2001(object _0) => throw new NotImplementedException();
		public virtual void Unknown2002(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2002 [2002]".Debug(Log);
		public virtual object Unknown2003(object _0) => throw new NotImplementedException();
		public virtual void Unknown2004(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown2005(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2005 [2005]".Debug(Log);
		public virtual object Unknown2006(object _0) => throw new NotImplementedException();
		public virtual void Unknown2007(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown2008(object _0) => throw new NotImplementedException();
		public virtual object Unknown2401(object _0) => throw new NotImplementedException();
		public virtual void Unknown2402(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2402 [2402]".Debug(Log);
		public virtual void Unknown2403(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2403 [2403]".Debug(Log);
		public virtual void Unknown2404(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2404 [2404]".Debug(Log);
		public virtual void Unknown2405(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2405 [2405]".Debug(Log);
		public virtual void Unknown2411(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2411 [2411]".Debug(Log);
		public virtual void Unknown2412(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2412 [2412]".Debug(Log);
		public virtual void Unknown2413(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2413 [2413]".Debug(Log);
		public virtual void Unknown2414(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2414 [2414]".Debug(Log);
		public virtual void Unknown2421(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void Unknown2422(object _0, Buffer<byte> _1) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2422 [2422]".Debug(Log);
		public virtual void Unknown2424(object _0, Buffer<byte> _1) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2424 [2424]".Debug(Log);
		public virtual void Unknown2431(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2431 [2431]".Debug(Log);
		public virtual object Unknown2433(object _0) => throw new NotImplementedException();
		public virtual void Unknown2434(object _0) => "Stub hit for Nn.Capsrv.Sf.IAlbumControlSession.Unknown2434 [2434]".Debug(Log);
	}
	
	public unsafe partial class ICaptureControllerService : _Base_ICaptureControllerService {}
	public class _Base_ICaptureControllerService : IpcInterface {
		static readonly Logger Logger = new("ICaptureControllerService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // Unknown1
					Unknown1(null, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // Unknown2
					Unknown2(null, im.GetBuffer<byte>(0x46, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // Unknown1001
					Unknown1001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1002: { // Unknown1002
					Unknown1002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1011: { // Unknown1011
					Unknown1011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2001: { // Unknown2001
					Unknown2001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2002: { // Unknown2002
					Unknown2002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to ICaptureControllerService: {im.CommandId}");
			}
		}
		
		public virtual void Unknown1(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown2(object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void Unknown1001(object _0) => "Stub hit for Nn.Capsrv.Sf.ICaptureControllerService.Unknown1001 [1001]".Debug(Log);
		public virtual void Unknown1002(object _0) => "Stub hit for Nn.Capsrv.Sf.ICaptureControllerService.Unknown1002 [1002]".Debug(Log);
		public virtual void Unknown1011(object _0) => "Stub hit for Nn.Capsrv.Sf.ICaptureControllerService.Unknown1011 [1011]".Debug(Log);
		public virtual void Unknown2001(object _0) => "Stub hit for Nn.Capsrv.Sf.ICaptureControllerService.Unknown2001 [2001]".Debug(Log);
		public virtual void Unknown2002(object _0) => "Stub hit for Nn.Capsrv.Sf.ICaptureControllerService.Unknown2002 [2002]".Debug(Log);
	}
}
