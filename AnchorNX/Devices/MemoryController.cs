namespace AnchorNX.Devices {
	public class MemoryController : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x70019000, 0x70019fff);
		
		[Mmio(0x70019000)] uint Intstatus { get; set; }
		[Mmio(0x70019004)] uint Intmask { get; set; }
		[Mmio(0x70019008)] uint ErrStatus { get; set; }
		[Mmio(0x7001900C)] uint ErrAdr { get; set; }
		[Mmio(0x70019010)] uint SmmuConfig { get; set; }
		[Mmio(0x70019014)] uint SmmuTlbConfig { get; set; }
		[Mmio(0x70019018)] uint SmmuPtcConfig { get; set; }
		[Mmio(0x7001901C)] uint SmmuPtbAsid { get; set; }
		[Mmio(0x70019020)] uint SmmuPtbData { get; set; }
		[Mmio(0x70019030)] uint SmmuTlbFlush { get; set; }
		[Mmio(0x70019034)] uint SmmuPtcFlush0 { get; set; }
		[Mmio(0x70019038)] uint SmmuAsidSecurity { get; set; }
		[Mmio(0x7001903C)] uint SmmuAsidSecurity1 { get; set; }
		
		[Mmio(0x70019050)] uint EmemCfg => 0x1000;
		
		[Mmio(0x70019054)] uint EmemAdrCfg { get; set; }
		[Mmio(0x70019070)] uint SecurityCfg0 { get; set; }
		[Mmio(0x70019074)] uint SecurityCfg1 { get; set; }
		[Mmio(0x70019090)] uint EmemArbCfg { get; set; }
		[Mmio(0x70019094)] uint EmemArbOutstandingReq { get; set; }
		[Mmio(0x70019098)] uint EmemArbTimingRcd { get; set; }
		[Mmio(0x7001909C)] uint EmemArbTimingRp { get; set; }
		[Mmio(0x700190A0)] uint EmemArbTimingRc { get; set; }
		[Mmio(0x700190A4)] uint EmemArbTimingRas { get; set; }
		[Mmio(0x700190A8)] uint EmemArbTimingFaw { get; set; }
		[Mmio(0x700190AC)] uint EmemArbTimingRrd { get; set; }
		[Mmio(0x700190B0)] uint EmemArbTimingRap2Pre { get; set; }
		[Mmio(0x700190B4)] uint EmemArbTimingWap2Pre { get; set; }
		[Mmio(0x700190B8)] uint EmemArbTimingR2R { get; set; }
		[Mmio(0x700190BC)] uint EmemArbTimingW2W { get; set; }
		[Mmio(0x700190C0)] uint EmemArbTimingR2W { get; set; }
		[Mmio(0x700190C4)] uint EmemArbTimingW2R { get; set; }
		[Mmio(0x700190C8)] uint EmemArbMisc2 { get; set; }
		[Mmio(0x700190D0)] uint EmemArbDaTurns { get; set; }
		[Mmio(0x700190D4)] uint EmemArbDaCovers { get; set; }
		[Mmio(0x700190D8)] uint EmemArbMisc0 { get; set; }
		[Mmio(0x700190DC)] uint EmemArbMisc1 { get; set; }
		[Mmio(0x700190E0)] uint EmemArbRing1Throttle { get; set; }
		[Mmio(0x70019100)] uint StatControl { get; set; }
		[Mmio(0x70019108)] uint StatEmcClockLimit { get; set; }
		[Mmio(0x7001910C)] uint StatEmcClockLimitMsbs { get; set; }
		[Mmio(0x70019118)] uint StatEmcFilterSet0AdrLimitLo { get; set; }
		[Mmio(0x7001911C)] uint StatEmcFilterSet0AdrLimitHi { get; set; }
		[Mmio(0x70019124)] uint StatEmcFilterSet0Spare { get; set; }
		[Mmio(0x70019128)] uint StatEmcFilterSet0Client0 { get; set; }
		[Mmio(0x7001912C)] uint StatEmcFilterSet0Client1 { get; set; }
		[Mmio(0x70019130)] uint StatEmcFilterSet0Client2 { get; set; }
		[Mmio(0x70019134)] uint StatEmcFilterSet0Client3 { get; set; }
		[Mmio(0x70019138)] uint StatEmcSet0Count { get; set; }
		[Mmio(0x7001913C)] uint StatEmcSet0CountMsbs { get; set; }
		[Mmio(0x70019158)] uint StatEmcFilterSet1AdrLimitLo { get; set; }
		[Mmio(0x7001915C)] uint StatEmcFilterSet1AdrLimitHi { get; set; }
		[Mmio(0x70019164)] uint StatEmcFilterSet1Spare { get; set; }
		[Mmio(0x70019168)] uint StatEmcFilterSet1Client0 { get; set; }
		[Mmio(0x7001916C)] uint StatEmcFilterSet1Client1 { get; set; }
		[Mmio(0x70019170)] uint StatEmcFilterSet1Client2 { get; set; }
		[Mmio(0x70019174)] uint StatEmcFilterSet1Client3 { get; set; }
		[Mmio(0x70019178)] uint StatEmcSet1Count { get; set; }
		[Mmio(0x7001917C)] uint StatEmcSet1CountMsbs { get; set; }
		[Mmio(0x70019200)] uint ClientHotresetCtrl { get; set; }
		[Mmio(0x70019204)] uint ClientHotresetStatus { get; set; }
		[Mmio(0x70019228)] uint SmmuTranslationEnable0 { get; set; }
		[Mmio(0x7001922C)] uint SmmuTranslationEnable1 { get; set; }
		[Mmio(0x70019230)] uint SmmuTranslationEnable2 { get; set; }
		[Mmio(0x70019234)] uint SmmuTranslationEnable3 { get; set; }
		[Mmio(0x70019238)] uint SmmuAfiAsid { get; set; }
		[Mmio(0x7001923C)] uint SmmuAvpcAsid { get; set; }
		[Mmio(0x70019240)] uint SmmuDcAsid { get; set; }
		[Mmio(0x70019244)] uint SmmuDcbAsid { get; set; }
		[Mmio(0x70019250)] uint SmmuHcAsid { get; set; }
		[Mmio(0x70019254)] uint SmmuHdaAsid { get; set; }
		[Mmio(0x70019258)] uint SmmuIsp2Asid { get; set; }
		[Mmio(0x70019264)] uint SmmuMsencNvencAsid { get; set; }
		[Mmio(0x70019268)] uint SmmuNvAsid { get; set; }
		[Mmio(0x7001926C)] uint SmmuNv2Asid { get; set; }
		[Mmio(0x70019270)] uint SmmuPpcsAsid { get; set; }
		[Mmio(0x70019274)] uint SmmuSataAsid { get; set; }
		[Mmio(0x70019280)] uint SmmuViAsid { get; set; }
		[Mmio(0x70019284)] uint SmmuVicAsid { get; set; }
		[Mmio(0x70019288)] uint SmmuXusbHostAsid { get; set; }
		[Mmio(0x7001928C)] uint SmmuXusbDevAsid { get; set; }
		[Mmio(0x70019294)] uint SmmuTsecAsid { get; set; }
		[Mmio(0x70019298)] uint SmmuPpcs1Asid { get; set; }
		[Mmio(0x700192E4)] uint LatencyAllowanceAvpc0 { get; set; }
		[Mmio(0x700192E8)] uint LatencyAllowanceDc0 { get; set; }
		[Mmio(0x700192EC)] uint LatencyAllowanceDc1 { get; set; }
		[Mmio(0x700192F4)] uint LatencyAllowanceDcb0 { get; set; }
		[Mmio(0x700192F8)] uint LatencyAllowanceDcb1 { get; set; }
		[Mmio(0x70019310)] uint LatencyAllowanceHc0 { get; set; }
		[Mmio(0x70019314)] uint LatencyAllowanceHc1 { get; set; }
		[Mmio(0x70019320)] uint LatencyAllowanceMpcore0 { get; set; }
		[Mmio(0x70019328)] uint LatencyAllowanceNvenc0 { get; set; }
		[Mmio(0x70019344)] uint LatencyAllowancePpcs0 { get; set; }
		[Mmio(0x70019348)] uint LatencyAllowancePpcs1 { get; set; }
		[Mmio(0x70019370)] uint LatencyAllowanceIsp20 { get; set; }
		[Mmio(0x70019374)] uint LatencyAllowanceIsp21 { get; set; }
		[Mmio(0x7001937C)] uint LatencyAllowanceXusb0 { get; set; }
		[Mmio(0x70019380)] uint LatencyAllowanceXusb1 { get; set; }
		[Mmio(0x70019390)] uint LatencyAllowanceTsec0 { get; set; }
		[Mmio(0x70019394)] uint LatencyAllowanceVic0 { get; set; }
		[Mmio(0x70019398)] uint LatencyAllowanceVi20 { get; set; }
		[Mmio(0x700193AC)] uint LatencyAllowanceGpu0 { get; set; }
		[Mmio(0x700193B8)] uint LatencyAllowanceSdmmca0 { get; set; }
		[Mmio(0x700193BC)] uint LatencyAllowanceSdmmcaa0 { get; set; }
		[Mmio(0x700193C0)] uint LatencyAllowanceSdmmc0 { get; set; }
		[Mmio(0x700193C4)] uint LatencyAllowanceSdmmcab0 { get; set; }
		[Mmio(0x700193D8)] uint LatencyAllowanceNvdec0 { get; set; }
		[Mmio(0x700193E8)] uint LatencyAllowanceGpu20 { get; set; }
		[Mmio(0x7001941C)] uint DisPtsaRate { get; set; }
		[Mmio(0x70019420)] uint DisPtsaMin { get; set; }
		[Mmio(0x70019424)] uint DisPtsaMax { get; set; }
		[Mmio(0x70019428)] uint DisbPtsaRate { get; set; }
		[Mmio(0x7001942C)] uint DisbPtsaMin { get; set; }
		[Mmio(0x70019430)] uint DisbPtsaMax { get; set; }
		[Mmio(0x70019434)] uint VePtsaRate { get; set; }
		[Mmio(0x70019438)] uint VePtsaMin { get; set; }
		[Mmio(0x7001943C)] uint VePtsaMax { get; set; }
		[Mmio(0x7001944C)] uint MllMpcorerPtsaRate { get; set; }
		[Mmio(0x7001947C)] uint Ring1PtsaRate { get; set; }
		[Mmio(0x70019480)] uint Ring1PtsaMin { get; set; }
		[Mmio(0x70019484)] uint Ring1PtsaMax { get; set; }
		[Mmio(0x700194AC)] uint PcxPtsaRate { get; set; }
		[Mmio(0x700194B0)] uint PcxPtsaMin { get; set; }
		[Mmio(0x700194B4)] uint PcxPtsaMax { get; set; }
		[Mmio(0x700194C4)] uint MsePtsaRate { get; set; }
		[Mmio(0x700194C8)] uint MsePtsaMin { get; set; }
		[Mmio(0x700194CC)] uint MsePtsaMax { get; set; }
		[Mmio(0x700194DC)] uint AhbPtsaRate { get; set; }
		[Mmio(0x700194E0)] uint AhbPtsaMin { get; set; }
		[Mmio(0x700194E4)] uint AhbPtsaMax { get; set; }
		[Mmio(0x700194E8)] uint ApbPtsaRate { get; set; }
		[Mmio(0x700194EC)] uint ApbPtsaMin { get; set; }
		[Mmio(0x700194F0)] uint ApbPtsaMax { get; set; }
		[Mmio(0x7001950C)] uint FtopPtsaRate { get; set; }
		[Mmio(0x70019518)] uint HostPtsaRate { get; set; }
		[Mmio(0x7001951C)] uint HostPtsaMin { get; set; }
		[Mmio(0x70019520)] uint HostPtsaMax { get; set; }
		[Mmio(0x70019524)] uint UsbxPtsaRate { get; set; }
		[Mmio(0x70019528)] uint UsbxPtsaMin { get; set; }
		[Mmio(0x7001952C)] uint UsbxPtsaMax { get; set; }
		[Mmio(0x70019530)] uint UsbdPtsaRate { get; set; }
		[Mmio(0x70019534)] uint UsbdPtsaMin { get; set; }
		[Mmio(0x70019538)] uint UsbdPtsaMax { get; set; }
		[Mmio(0x7001953C)] uint GkPtsaRate { get; set; }
		[Mmio(0x70019540)] uint GkPtsaMin { get; set; }
		[Mmio(0x70019544)] uint GkPtsaMax { get; set; }
		[Mmio(0x70019548)] uint AudPtsaRate { get; set; }
		[Mmio(0x7001954C)] uint AudPtsaMin { get; set; }
		[Mmio(0x70019550)] uint AudPtsaMax { get; set; }
		[Mmio(0x70019554)] uint VicpcPtsaRate { get; set; }
		[Mmio(0x70019558)] uint VicpcPtsaMin { get; set; }
		[Mmio(0x7001955C)] uint VicpcPtsaMax { get; set; }
		[Mmio(0x70019584)] uint JpgPtsaRate { get; set; }
		[Mmio(0x70019588)] uint JpgPtsaMin { get; set; }
		[Mmio(0x7001958C)] uint JpgPtsaMax { get; set; }
		[Mmio(0x70019610)] uint Gk2PtsaRate { get; set; }
		[Mmio(0x70019614)] uint Gk2PtsaMin { get; set; }
		[Mmio(0x70019618)] uint Gk2PtsaMax { get; set; }
		[Mmio(0x7001961C)] uint SdmPtsaRate { get; set; }
		[Mmio(0x70019620)] uint SdmPtsaMin { get; set; }
		[Mmio(0x70019624)] uint SdmPtsaMax { get; set; }
		[Mmio(0x70019628)] uint HdapcPtsaRate { get; set; }
		[Mmio(0x7001962C)] uint HdapcPtsaMin { get; set; }
		[Mmio(0x70019630)] uint HdapcPtsaMax { get; set; }
		[Mmio(0x70019648)] uint VideoProtectBom { get; set; }
		[Mmio(0x7001964C)] uint VideoProtectSizeMb { get; set; }
		[Mmio(0x70019650)] uint VideoProtectRegCtrl { get; set; }
		[Mmio(0x7001965C)] uint IramBom { get; set; }
		[Mmio(0x70019660)] uint IramTom { get; set; }
		[Mmio(0x70019670)] uint SecCarveoutBom { get; set; }
		[Mmio(0x70019674)] uint SecCarveoutSizeMb { get; set; }
		[Mmio(0x70019678)] uint SecCarveoutRegCtrl { get; set; }
		[Mmio(0x70019690)] uint ScaledLatencyAllowanceDisplay0A { get; set; }
		[Mmio(0x70019694)] uint ScaledLatencyAllowanceDisplay0Ab { get; set; }
		[Mmio(0x70019698)] uint ScaledLatencyAllowanceDisplay0B { get; set; }
		[Mmio(0x7001969C)] uint ScaledLatencyAllowanceDisplay0Bb { get; set; }
		[Mmio(0x700196A0)] uint ScaledLatencyAllowanceDisplay0C { get; set; }
		[Mmio(0x700196A4)] uint ScaledLatencyAllowanceDisplay0Cb { get; set; }
		[Mmio(0x700196C0)] uint EmemArbTimingRfcpb { get; set; }
		[Mmio(0x700196C4)] uint EmemArbTimingCcdmw { get; set; }
		[Mmio(0x700196F0)] uint EmemArbRefpbHpCtrl { get; set; }
		[Mmio(0x700196F4)] uint EmemArbRefpbBankCtrl { get; set; }
		[Mmio(0x70019948)] uint UntranslatedRegionCheck { get; set; }
		[Mmio(0x70019960)] uint PtsaGrantDecrement { get; set; }
		[Mmio(0x70019964)] uint IramRegCtrl { get; set; }
		[Mmio(0x70019970)] uint ClientHotresetCtrl1 { get; set; }
		[Mmio(0x70019974)] uint ClientHotresetStatus1 { get; set; }
		[Mmio(0x70019984)] uint VideoProtectGpuOverride0 { get; set; }
		[Mmio(0x70019988)] uint VideoProtectGpuOverride1 { get; set; }
		[Mmio(0x700199A0)] uint MtsCarveoutBom { get; set; }
		[Mmio(0x700199A4)] uint MtsCarveoutSizeMb { get; set; }
		[Mmio(0x700199A8)] uint MtsCarveoutAdrHi { get; set; }
		[Mmio(0x700199AC)] uint MtsCarveoutRegCtrl { get; set; }
		[Mmio(0x700199B8)] uint SmmuPtcFlush1 { get; set; }
		[Mmio(0x700199BC)] uint SecurityCfg3 { get; set; }
		[Mmio(0x700199E0)] uint SmmuAsidSecurity2 { get; set; }
		[Mmio(0x700199E4)] uint SmmuAsidSecurity3 { get; set; }
		[Mmio(0x700199E8)] uint SmmuAsidSecurity4 { get; set; }
		[Mmio(0x700199EC)] uint SmmuAsidSecurity5 { get; set; }
		[Mmio(0x700199F0)] uint SmmuAsidSecurity6 { get; set; }
		[Mmio(0x700199F4)] uint SmmuAsidSecurity7 { get; set; }
		[Mmio(0x70019A20)] uint StatEmcFilterSet0AdrLimitUpper { get; set; }
		[Mmio(0x70019A24)] uint StatEmcFilterSet1AdrLimitUpper { get; set; }
		[Mmio(0x70019A88)] uint SmmuDc1Asid { get; set; }
		[Mmio(0x70019A94)] uint SmmuSdmmc1AAsid { get; set; }
		[Mmio(0x70019A98)] uint SmmuSdmmc2AAsid { get; set; }
		[Mmio(0x70019A9C)] uint SmmuSdmmc3AAsid { get; set; }
		[Mmio(0x70019AA0)] uint SmmuSdmmc4AAsid { get; set; }
		[Mmio(0x70019AA4)] uint SmmuIsp2BAsid { get; set; }
		[Mmio(0x70019AA8)] uint SmmuGpuAsid { get; set; }
		[Mmio(0x70019AAC)] uint SmmuGpubAsid { get; set; }
		[Mmio(0x70019AB0)] uint SmmuPpcs2Asid { get; set; }
		[Mmio(0x70019AB4)] uint SmmuNvdecAsid { get; set; }
		[Mmio(0x70019AB8)] uint SmmuApeAsid { get; set; }
		[Mmio(0x70019ABC)] uint SmmuSeAsid { get; set; }
		[Mmio(0x70019AC0)] uint SmmuNvjpgAsid { get; set; }
		[Mmio(0x70019AC4)] uint SmmuHc1Asid { get; set; }
		[Mmio(0x70019AC8)] uint SmmuSe1Asid { get; set; }
		[Mmio(0x70019ACC)] uint SmmuAxiapAsid { get; set; }
		[Mmio(0x70019AD0)] uint SmmuEtrAsid { get; set; }
		[Mmio(0x70019AD4)] uint SmmuTsecbAsid { get; set; }
		[Mmio(0x70019AD8)] uint SmmuTsec1Asid { get; set; }
		[Mmio(0x70019ADC)] uint SmmuTsecb1Asid { get; set; }
		[Mmio(0x70019AE0)] uint SmmuNvdec1Asid { get; set; }
		[Mmio(0x70019B88)] uint StatEmcFilterSet0Client4 { get; set; }
		[Mmio(0x70019B8C)] uint StatEmcFilterSet1Client4 { get; set; }
		[Mmio(0x70019B98)] uint SmmuTranslationEnable4 { get; set; }
		[Mmio(0x70019BC4)] uint StatEmcFilterSet0Client5 { get; set; }
		[Mmio(0x70019BC8)] uint StatEmcFilterSet1Client5 { get; set; }
		[Mmio(0x70019BCC)] uint EmemArbDhystCtrl { get; set; }
		[Mmio(0x70019BD0)] uint EmemArbDhystTimeoutUtil0 { get; set; }
		[Mmio(0x70019BD4)] uint EmemArbDhystTimeoutUtil1 { get; set; }
		[Mmio(0x70019BD8)] uint EmemArbDhystTimeoutUtil2 { get; set; }
		[Mmio(0x70019BDC)] uint EmemArbDhystTimeoutUtil3 { get; set; }
		[Mmio(0x70019BE0)] uint EmemArbDhystTimeoutUtil4 { get; set; }
		[Mmio(0x70019BE4)] uint EmemArbDhystTimeoutUtil5 { get; set; }
		[Mmio(0x70019BE8)] uint EmemArbDhystTimeoutUtil6 { get; set; }
		[Mmio(0x70019BEC)] uint EmemArbDhystTimeoutUtil7 { get; set; }
		[Mmio(0x70019C00)] uint ErrGeneralizedCarveoutStatus { get; set; }
		[Mmio(0x70019C08)] uint SecurityCarveout1Cfg0 { get; set; }
		[Mmio(0x70019C0C)] uint SecurityCarveout1Bom { get; set; }
		[Mmio(0x70019C10)] uint SecurityCarveout1BomHi { get; set; }
		[Mmio(0x70019C14)] uint SecurityCarveout1Size128Kb { get; set; }
		[Mmio(0x70019C18)] uint SecurityCarveout1ClientAccess0 { get; set; }
		[Mmio(0x70019C1C)] uint SecurityCarveout1ClientAccess1 { get; set; }
		[Mmio(0x70019C20)] uint SecurityCarveout1ClientAccess2 { get; set; }
		[Mmio(0x70019C24)] uint SecurityCarveout1ClientAccess3 { get; set; }
		[Mmio(0x70019C28)] uint SecurityCarveout1ClientAccess4 { get; set; }
		[Mmio(0x70019C2C)] uint SecurityCarveout1ClientForceInternalAccess0 { get; set; }
		[Mmio(0x70019C30)] uint SecurityCarveout1ClientForceInternalAccess1 { get; set; }
		[Mmio(0x70019C34)] uint SecurityCarveout1ClientForceInternalAccess2 { get; set; }
		[Mmio(0x70019C38)] uint SecurityCarveout1ClientForceInternalAccess3 { get; set; }
		[Mmio(0x70019C3C)] uint SecurityCarveout1ClientForceInternalAccess4 { get; set; }
		[Mmio(0x70019C58)] uint SecurityCarveout2Cfg0 { get; set; }
		[Mmio(0x70019C5C)] uint SecurityCarveout2Bom { get; set; }
		[Mmio(0x70019C60)] uint SecurityCarveout2BomHi { get; set; }
		[Mmio(0x70019C64)] uint SecurityCarveout2Size128Kb { get; set; }
		[Mmio(0x70019C68)] uint SecurityCarveout2ClientAccess0 { get; set; }
		[Mmio(0x70019C6C)] uint SecurityCarveout2ClientAccess1 { get; set; }
		[Mmio(0x70019C70)] uint SecurityCarveout2ClientAccess2 { get; set; }
		[Mmio(0x70019C74)] uint SecurityCarveout2ClientAccess3 { get; set; }
		[Mmio(0x70019C78)] uint SecurityCarveout2ClientAccess4 { get; set; }
		[Mmio(0x70019C7C)] uint SecurityCarveout2ClientForceInternalAccess0 { get; set; }
		[Mmio(0x70019C80)] uint SecurityCarveout2ClientForceInternalAccess1 { get; set; }
		[Mmio(0x70019C84)] uint SecurityCarveout2ClientForceInternalAccess2 { get; set; }
		[Mmio(0x70019C88)] uint SecurityCarveout2ClientForceInternalAccess3 { get; set; }
		[Mmio(0x70019C8C)] uint SecurityCarveout2ClientForceInternalAccess4 { get; set; }
		[Mmio(0x70019CA8)] uint SecurityCarveout3Cfg0 { get; set; }
		[Mmio(0x70019CAC)] uint SecurityCarveout3Bom { get; set; }
		[Mmio(0x70019CB0)] uint SecurityCarveout3BomHi { get; set; }
		[Mmio(0x70019CB4)] uint SecurityCarveout3Size128Kb { get; set; }
		[Mmio(0x70019CB8)] uint SecurityCarveout3ClientAccess0 { get; set; }
		[Mmio(0x70019CBC)] uint SecurityCarveout3ClientAccess1 { get; set; }
		[Mmio(0x70019CC0)] uint SecurityCarveout3ClientAccess2 { get; set; }
		[Mmio(0x70019CC4)] uint SecurityCarveout3ClientAccess3 { get; set; }
		[Mmio(0x70019CC8)] uint SecurityCarveout3ClientAccess4 { get; set; }
		[Mmio(0x70019CCC)] uint SecurityCarveout3ClientForceInternalAccess0 { get; set; }
		[Mmio(0x70019CD0)] uint SecurityCarveout3ClientForceInternalAccess1 { get; set; }
		[Mmio(0x70019CD4)] uint SecurityCarveout3ClientForceInternalAccess2 { get; set; }
		[Mmio(0x70019CD8)] uint SecurityCarveout3ClientForceInternalAccess3 { get; set; }
		[Mmio(0x70019CDC)] uint SecurityCarveout3ClientForceInternalAccess4 { get; set; }
		[Mmio(0x70019CF8)] uint SecurityCarveout4Cfg0 { get; set; }
		[Mmio(0x70019CFC)] uint SecurityCarveout4Bom { get; set; }
		[Mmio(0x70019D00)] uint SecurityCarveout4BomHi { get; set; }
		[Mmio(0x70019D04)] uint SecurityCarveout4Size128Kb { get; set; }
		[Mmio(0x70019D08)] uint SecurityCarveout4ClientAccess0 { get; set; }
		[Mmio(0x70019D0C)] uint SecurityCarveout4ClientAccess1 { get; set; }
		[Mmio(0x70019D10)] uint SecurityCarveout4ClientAccess2 { get; set; }
		[Mmio(0x70019D14)] uint SecurityCarveout4ClientAccess3 { get; set; }
		[Mmio(0x70019D18)] uint SecurityCarveout4ClientAccess4 { get; set; }
		[Mmio(0x70019D1C)] uint SecurityCarveout4ClientForceInternalAccess0 { get; set; }
		[Mmio(0x70019D20)] uint SecurityCarveout4ClientForceInternalAccess1 { get; set; }
		[Mmio(0x70019D24)] uint SecurityCarveout4ClientForceInternalAccess2 { get; set; }
		[Mmio(0x70019D28)] uint SecurityCarveout4ClientForceInternalAccess3 { get; set; }
		[Mmio(0x70019D2C)] uint SecurityCarveout4ClientForceInternalAccess4 { get; set; }
		[Mmio(0x70019D48)] uint SecurityCarveout5Cfg0 { get; set; }
		[Mmio(0x70019D4C)] uint SecurityCarveout5Bom { get; set; }
		[Mmio(0x70019D50)] uint SecurityCarveout5BomHi { get; set; }
		[Mmio(0x70019D54)] uint SecurityCarveout5Size128Kb { get; set; }
		[Mmio(0x70019D58)] uint SecurityCarveout5ClientAccess0 { get; set; }
		[Mmio(0x70019D5C)] uint SecurityCarveout5ClientAccess1 { get; set; }
		[Mmio(0x70019D60)] uint SecurityCarveout5ClientAccess2 { get; set; }
		[Mmio(0x70019D64)] uint SecurityCarveout5ClientAccess3 { get; set; }
		[Mmio(0x70019D68)] uint SecurityCarveout5ClientAccess4 { get; set; }
		[Mmio(0x70019D6C)] uint SecurityCarveout5ClientForceInternalAccess0 { get; set; }
		[Mmio(0x70019D70)] uint SecurityCarveout5ClientForceInternalAccess1 { get; set; }
		[Mmio(0x70019D74)] uint SecurityCarveout5ClientForceInternalAccess2 { get; set; }
		[Mmio(0x70019D78)] uint SecurityCarveout5ClientForceInternalAccess3 { get; set; }
		[Mmio(0x70019D7C)] uint SecurityCarveout5ClientForceInternalAccess4 { get; set; }
	}
}