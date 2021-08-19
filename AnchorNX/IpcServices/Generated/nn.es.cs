#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Es {
	public unsafe partial class IETicketService : _Base_IETicketService {}
	public class _Base_IETicketService : IpcInterface {
		static readonly Logger Logger = new("IETicketService");
		new static Action<string> Log = Logger.Log;
		public override async Task _Dispatch(IncomingMessage im, OutgoingMessage om) {
			switch(im.CommandId) {
				case 1: { // ImportTicket
					ImportTicket(im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x5, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 2: { // ImportTicketCertificateSet
					ImportTicketCertificateSet(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 3: { // DeleteTicket
					DeleteTicket(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 4: { // DeletePersonalizedTicket
					DeletePersonalizedTicket(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 5: { // DeleteAllCommonTicket
					DeleteAllCommonTicket();
					om.Initialize(0, 0, 0);
					break;
				}
				case 6: { // DeleteAllPersonalizedTicket
					DeleteAllPersonalizedTicket();
					om.Initialize(0, 0, 0);
					break;
				}
				case 7: { // DeleteAllPersonalizedTicketEx
					DeleteAllPersonalizedTicketEx(im.GetBuffer<byte>(0x5, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 9: { // CountCommonTicket
					var ret = CountCommonTicket();
					om.Initialize(0, 0, 0);
					break;
				}
				case 10: { // CountPersonalizedTicket
					var ret = CountPersonalizedTicket();
					om.Initialize(0, 0, 0);
					break;
				}
				case 11: { // ListCommonTicket
					ListCommonTicket(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 12: { // ListPersonalizedTicket
					ListPersonalizedTicket(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 13: { // ListMissingPersonalizedTicket
					ListMissingPersonalizedTicket(im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 14: { // GetCommonTicketSize
					var ret = GetCommonTicketSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 15: { // GetPersonalizedTicketSize
					var ret = GetPersonalizedTicketSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 16: { // GetCommonTicketData
					GetCommonTicketData(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 17: { // GetPersonalizedTicketData
					GetPersonalizedTicketData(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 18: { // OwnTicket
					OwnTicket(im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 19: { // GetTicketInfo
					GetTicketInfo(im.GetBuffer<byte>(0x5, 0), out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 20: { // ListLightTicketInfo
					ListLightTicketInfo(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 21: { // SignData
					SignData(im.GetBuffer<byte>(0x5, 0), im.GetBuffer<byte>(0x16, 0), im.GetBuffer<byte>(0x16, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 22: { // GetCommonTicketAndCertificateSize
					var ret = GetCommonTicketAndCertificateSize(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 23: { // GetCommonTicketAndCertificateData
					GetCommonTicketAndCertificateData(null, out var _0, im.GetBuffer<byte>(0x6, 0), im.GetBuffer<byte>(0x6, 1));
					om.Initialize(0, 0, 0);
					break;
				}
				case 24: { // ImportPrepurchaseRecord
					ImportPrepurchaseRecord(im.GetBuffer<byte>(0x15, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 25: { // DeletePrepurchaseRecord
					DeletePrepurchaseRecord(im.GetBuffer<byte>(0x15, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 26: { // DeleteAllPrepurchaseRecord
					DeleteAllPrepurchaseRecord();
					om.Initialize(0, 0, 0);
					break;
				}
				case 27: { // CountPrepurchaseRecord
					var ret = CountPrepurchaseRecord();
					om.Initialize(0, 0, 0);
					break;
				}
				case 28: { // ListPrepurchaseRecord
					ListPrepurchaseRecord(out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 29: { // ListPrepurchaseRecordInfo
					ListPrepurchaseRecordInfo(null, out var _0, im.GetBuffer<byte>(0x6, 0));
					om.Initialize(0, 0, 0);
					break;
				}
				case 30: { // Unknown30
					var ret = Unknown30(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 31: { // Unknown31
					var ret = Unknown31(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 32: { // Unknown32
					var ret = Unknown32(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 33: { // Unknown33
					var ret = Unknown33(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 34: { // Unknown34
					var ret = Unknown34(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 35: { // Unknown35
					var ret = Unknown35(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 36: { // Unknown36
					var ret = Unknown36(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 501: { // Unknown501
					var ret = Unknown501(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 502: { // Unknown502
					var ret = Unknown502(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 503: { // GetTitleKey
					var ret = GetTitleKey(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 504: { // Unknown504
					var ret = Unknown504(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 508: { // Unknown508
					var ret = Unknown508(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 509: { // Unknown509
					var ret = Unknown509(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 510: { // Unknown510
					var ret = Unknown510(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1001: { // Unknown1001
					var ret = Unknown1001(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1002: { // Unknown1002
					var ret = Unknown1002(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1003: { // Unknown1003
					var ret = Unknown1003(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1004: { // Unknown1004
					var ret = Unknown1004(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1005: { // Unknown1005
					var ret = Unknown1005(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1006: { // Unknown1006
					var ret = Unknown1006(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1007: { // Unknown1007
					var ret = Unknown1007(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1009: { // Unknown1009
					var ret = Unknown1009(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1010: { // Unknown1010
					var ret = Unknown1010(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1011: { // Unknown1011
					var ret = Unknown1011(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1012: { // Unknown1012
					var ret = Unknown1012(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1013: { // Unknown1013
					var ret = Unknown1013(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1014: { // Unknown1014
					var ret = Unknown1014(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1015: { // Unknown1015
					var ret = Unknown1015(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1016: { // Unknown1016
					var ret = Unknown1016(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1501: { // Unknown1501
					var ret = Unknown1501(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1502: { // Unknown1502
					var ret = Unknown1502(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1503: { // Unknown1503
					var ret = Unknown1503(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1504: { // Unknown1504
					var ret = Unknown1504(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 1505: { // Unknown1505
					var ret = Unknown1505(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2000: { // Unknown2000
					var ret = Unknown2000(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2501: { // Unknown2501
					var ret = Unknown2501(null);
					om.Initialize(0, 0, 0);
					break;
				}
				case 2502: { // Unknown2502
					var ret = Unknown2502(null);
					om.Initialize(0, 0, 0);
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled command ID to IETicketService: {im.CommandId}");
			}
		}
		
		public virtual void ImportTicket(Buffer<byte> _0, Buffer<byte> _1) => "Stub hit for Nn.Es.IETicketService.ImportTicket [1]".Debug(Log);
		public virtual void ImportTicketCertificateSet(Buffer<byte> _0) => "Stub hit for Nn.Es.IETicketService.ImportTicketCertificateSet [2]".Debug(Log);
		public virtual void DeleteTicket(Buffer<byte> _0) => "Stub hit for Nn.Es.IETicketService.DeleteTicket [3]".Debug(Log);
		public virtual void DeletePersonalizedTicket(object _0) => "Stub hit for Nn.Es.IETicketService.DeletePersonalizedTicket [4]".Debug(Log);
		public virtual void DeleteAllCommonTicket() => "Stub hit for Nn.Es.IETicketService.DeleteAllCommonTicket [5]".Debug(Log);
		public virtual void DeleteAllPersonalizedTicket() => "Stub hit for Nn.Es.IETicketService.DeleteAllPersonalizedTicket [6]".Debug(Log);
		public virtual void DeleteAllPersonalizedTicketEx(Buffer<byte> _0) => "Stub hit for Nn.Es.IETicketService.DeleteAllPersonalizedTicketEx [7]".Debug(Log);
		public virtual object CountCommonTicket() => throw new NotImplementedException();
		public virtual object CountPersonalizedTicket() => throw new NotImplementedException();
		public virtual void ListCommonTicket(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ListPersonalizedTicket(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ListMissingPersonalizedTicket(Buffer<byte> _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object GetCommonTicketSize(object _0) => throw new NotImplementedException();
		public virtual object GetPersonalizedTicketSize(object _0) => throw new NotImplementedException();
		public virtual void GetCommonTicketData(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void GetPersonalizedTicketData(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void OwnTicket(Buffer<byte> _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void GetTicketInfo(Buffer<byte> _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void ListLightTicketInfo(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual void SignData(Buffer<byte> _0, Buffer<byte> _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object GetCommonTicketAndCertificateSize(object _0) => throw new NotImplementedException();
		public virtual void GetCommonTicketAndCertificateData(object _0, out object _1, Buffer<byte> _2, Buffer<byte> _3) => throw new NotImplementedException();
		public virtual void ImportPrepurchaseRecord(Buffer<byte> _0) => "Stub hit for Nn.Es.IETicketService.ImportPrepurchaseRecord [24]".Debug(Log);
		public virtual void DeletePrepurchaseRecord(Buffer<byte> _0) => "Stub hit for Nn.Es.IETicketService.DeletePrepurchaseRecord [25]".Debug(Log);
		public virtual void DeleteAllPrepurchaseRecord() => "Stub hit for Nn.Es.IETicketService.DeleteAllPrepurchaseRecord [26]".Debug(Log);
		public virtual object CountPrepurchaseRecord() => throw new NotImplementedException();
		public virtual void ListPrepurchaseRecord(out object _0, Buffer<byte> _1) => throw new NotImplementedException();
		public virtual void ListPrepurchaseRecordInfo(object _0, out object _1, Buffer<byte> _2) => throw new NotImplementedException();
		public virtual object Unknown30(object _0) => throw new NotImplementedException();
		public virtual object Unknown31(object _0) => throw new NotImplementedException();
		public virtual object Unknown32(object _0) => throw new NotImplementedException();
		public virtual object Unknown33(object _0) => throw new NotImplementedException();
		public virtual object Unknown34(object _0) => throw new NotImplementedException();
		public virtual object Unknown35(object _0) => throw new NotImplementedException();
		public virtual object Unknown36(object _0) => throw new NotImplementedException();
		public virtual object Unknown501(object _0) => throw new NotImplementedException();
		public virtual object Unknown502(object _0) => throw new NotImplementedException();
		public virtual object GetTitleKey(object _0) => throw new NotImplementedException();
		public virtual object Unknown504(object _0) => throw new NotImplementedException();
		public virtual object Unknown508(object _0) => throw new NotImplementedException();
		public virtual object Unknown509(object _0) => throw new NotImplementedException();
		public virtual object Unknown510(object _0) => throw new NotImplementedException();
		public virtual object Unknown1001(object _0) => throw new NotImplementedException();
		public virtual object Unknown1002(object _0) => throw new NotImplementedException();
		public virtual object Unknown1003(object _0) => throw new NotImplementedException();
		public virtual object Unknown1004(object _0) => throw new NotImplementedException();
		public virtual object Unknown1005(object _0) => throw new NotImplementedException();
		public virtual object Unknown1006(object _0) => throw new NotImplementedException();
		public virtual object Unknown1007(object _0) => throw new NotImplementedException();
		public virtual object Unknown1009(object _0) => throw new NotImplementedException();
		public virtual object Unknown1010(object _0) => throw new NotImplementedException();
		public virtual object Unknown1011(object _0) => throw new NotImplementedException();
		public virtual object Unknown1012(object _0) => throw new NotImplementedException();
		public virtual object Unknown1013(object _0) => throw new NotImplementedException();
		public virtual object Unknown1014(object _0) => throw new NotImplementedException();
		public virtual object Unknown1015(object _0) => throw new NotImplementedException();
		public virtual object Unknown1016(object _0) => throw new NotImplementedException();
		public virtual object Unknown1501(object _0) => throw new NotImplementedException();
		public virtual object Unknown1502(object _0) => throw new NotImplementedException();
		public virtual object Unknown1503(object _0) => throw new NotImplementedException();
		public virtual object Unknown1504(object _0) => throw new NotImplementedException();
		public virtual object Unknown1505(object _0) => throw new NotImplementedException();
		public virtual object Unknown2000(object _0) => throw new NotImplementedException();
		public virtual object Unknown2501(object _0) => throw new NotImplementedException();
		public virtual object Unknown2502(object _0) => throw new NotImplementedException();
	}
}
