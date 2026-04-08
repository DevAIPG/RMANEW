using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models
{
    [Keyless]
    [Table("imctlfil_sql")]
    [Index(nameof(Id), Name = "pk_imctlfil_sql", IsUnique = true)]
    public partial class ImctlfilSql
    {
        [Column("im_ctl_key_1")]
        public byte ImCtlKey1 { get; set; }
        [Column("ave_last_cst_fg")]
        [StringLength(1)]
        public string AveLastCstFg { get; set; }
        [Column("multi_acct_fg")]
        [StringLength(1)]
        public string MultiAcctFg { get; set; }
        [Column("loc")]
        [StringLength(3)]
        public string Loc { get; set; }
        [Column("mat_cost_type")]
        [StringLength(3)]
        public string MatCostType { get; set; }
        [Column("audit_trail_fg")]
        [StringLength(1)]
        public string AuditTrailFg { get; set; }
        [Column("no_of_days_in_prd", TypeName = "decimal(5, 2)")]
        public decimal? NoOfDaysInPrd { get; set; }
        [Column("curr_prd")]
        public byte? CurrPrd { get; set; }
        [Column("non_stk_trx_aud")]
        [StringLength(1)]
        public string NonStkTrxAud { get; set; }
        [Column("chg_fg")]
        [StringLength(1)]
        public string ChgFg { get; set; }
        [Column("no_of_prds")]
        public byte? NoOfPrds { get; set; }
        [Column("trx_aud_fg")]
        [StringLength(1)]
        public string TrxAudFg { get; set; }
        [Column("next_doc_no")]
        public int? NextDocNo { get; set; }
        [Column("aud_file_bb_dt", TypeName = "datetime")]
        public DateTime? AudFileBbDt { get; set; }
        [Column("user_name")]
        [StringLength(20)]
        public string UserName { get; set; }
        [Column("online_upd_fg")]
        [StringLength(1)]
        public string OnlineUpdFg { get; set; }
        [Column("update_dist_phy_cnt")]
        [StringLength(1)]
        public string UpdateDistPhyCnt { get; set; }
        [Column("proces_nonstk_bomp")]
        [StringLength(1)]
        public string ProcesNonstkBomp { get; set; }
        [Column("distribute_qty_amt")]
        [StringLength(1)]
        public string DistributeQtyAmt { get; set; }
        [Column("use_job_nos_fg")]
        [StringLength(1)]
        public string UseJobNosFg { get; set; }
        [Column("next_tag_no")]
        public int? NextTagNo { get; set; }
        [Column("next_text_no")]
        public int? NextTextNo { get; set; }
        [Column("mn_no")]
        [StringLength(9)]
        public string MnNo { get; set; }
        [Column("sb_no")]
        [StringLength(8)]
        public string SbNo { get; set; }
        [Column("dp_no")]
        [StringLength(8)]
        public string DpNo { get; set; }
        [Column("receiving_mn_no")]
        [StringLength(9)]
        public string ReceivingMnNo { get; set; }
        [Column("receiving_sb_no")]
        [StringLength(8)]
        public string ReceivingSbNo { get; set; }
        [Column("receiving_dp_no")]
        [StringLength(8)]
        public string ReceivingDpNo { get; set; }
        [Column("issue_mn_no")]
        [StringLength(9)]
        public string IssueMnNo { get; set; }
        [Column("issue_sb_no")]
        [StringLength(8)]
        public string IssueSbNo { get; set; }
        [Column("issue_dp_no")]
        [StringLength(8)]
        public string IssueDpNo { get; set; }
        [Column("receipt_mn_no")]
        [StringLength(9)]
        public string ReceiptMnNo { get; set; }
        [Column("receipt_sb_no")]
        [StringLength(8)]
        public string ReceiptSbNo { get; set; }
        [Column("receipt_dp_no")]
        [StringLength(8)]
        public string ReceiptDpNo { get; set; }
        [Column("qty_adj_mn_no")]
        [StringLength(9)]
        public string QtyAdjMnNo { get; set; }
        [Column("qty_adj_sb_no")]
        [StringLength(8)]
        public string QtyAdjSbNo { get; set; }
        [Column("qty_adj_dp_no")]
        [StringLength(8)]
        public string QtyAdjDpNo { get; set; }
        [Column("cost_adj_mn_no")]
        [StringLength(9)]
        public string CostAdjMnNo { get; set; }
        [Column("cost_adj_sb_no")]
        [StringLength(8)]
        public string CostAdjSbNo { get; set; }
        [Column("cost_adj_dp_no")]
        [StringLength(8)]
        public string CostAdjDpNo { get; set; }
        [Column("wip_var_mn_no")]
        [StringLength(9)]
        public string WipVarMnNo { get; set; }
        [Column("wip_var_sb_no")]
        [StringLength(8)]
        public string WipVarSbNo { get; set; }
        [Column("wip_var_dp_no")]
        [StringLength(8)]
        public string WipVarDpNo { get; set; }
        [Column("ppv_var_mn_no")]
        [StringLength(9)]
        public string PpvVarMnNo { get; set; }
        [Column("ppv_var_sb_no")]
        [StringLength(8)]
        public string PpvVarSbNo { get; set; }
        [Column("ppv_var_dp_no")]
        [StringLength(8)]
        public string PpvVarDpNo { get; set; }
        [Column("ppv_qty_var_mn_no")]
        [StringLength(9)]
        public string PpvQtyVarMnNo { get; set; }
        [Column("ppv_qty_var_sb_no")]
        [StringLength(8)]
        public string PpvQtyVarSbNo { get; set; }
        [Column("ppv_qty_var_dp_no")]
        [StringLength(8)]
        public string PpvQtyVarDpNo { get; set; }
        [Column("xfr_var_mn_no")]
        [StringLength(9)]
        public string XfrVarMnNo { get; set; }
        [Column("xfr_var_sb_no")]
        [StringLength(8)]
        public string XfrVarSbNo { get; set; }
        [Column("xfr_var_dp_no")]
        [StringLength(8)]
        public string XfrVarDpNo { get; set; }
        [Column("cyc_phy_cnt_mn_no")]
        [StringLength(9)]
        public string CycPhyCntMnNo { get; set; }
        [Column("cyc_phy_cnt_sb_no")]
        [StringLength(8)]
        public string CycPhyCntSbNo { get; set; }
        [Column("cyc_phy_cnt_dp_no")]
        [StringLength(8)]
        public string CycPhyCntDpNo { get; set; }
        [Column("use_mult_bin_fg")]
        [StringLength(1)]
        public string UseMultBinFg { get; set; }
        [Column("bin_meth")]
        [StringLength(1)]
        public string BinMeth { get; set; }
        [Column("use_ri_mrb_loc")]
        [StringLength(1)]
        public string UseRiMrbLoc { get; set; }
        [Column("use_land_cst_fg")]
        [StringLength(1)]
        public string UseLandCstFg { get; set; }
        [Column("use_ser_lot_fg")]
        [StringLength(1)]
        public string UseSerLotFg { get; set; }
        [Column("ser_lot_cnt_tags")]
        [StringLength(1)]
        public string SerLotCntTags { get; set; }
        [Column("use_disc_cst_fg")]
        [StringLength(1)]
        public string UseDiscCstFg { get; set; }
        [Column("ri_accr_mn_no")]
        [StringLength(9)]
        public string RiAccrMnNo { get; set; }
        [Column("ri_accr_sb_no")]
        [StringLength(8)]
        public string RiAccrSbNo { get; set; }
        [Column("ri_accr_dp_no")]
        [StringLength(8)]
        public string RiAccrDpNo { get; set; }
        [Column("ser_no_auto_assign")]
        [StringLength(1)]
        public string SerNoAutoAssign { get; set; }
        [Column("ser_no_static_len")]
        public byte? SerNoStaticLen { get; set; }
        [Column("ser_no_static_val")]
        [StringLength(15)]
        public string SerNoStaticVal { get; set; }
        [Column("ser_no_numeric_len")]
        public byte? SerNoNumericLen { get; set; }
        [Column("ser_no_numeric_val", TypeName = "decimal(12, 0)")]
        public decimal? SerNoNumericVal { get; set; }
        [Column("filler_0001")]
        [StringLength(2)]
        public string Filler0001 { get; set; }
        [Column("lot_no_auto_assign")]
        [StringLength(1)]
        public string LotNoAutoAssign { get; set; }
        [Column("lot_no_static_len")]
        public byte? LotNoStaticLen { get; set; }
        [Column("lot_no_static_val")]
        [StringLength(15)]
        public string LotNoStaticVal { get; set; }
        [Column("lot_no_numeric_len")]
        public byte? LotNoNumericLen { get; set; }
        [Column("lot_no_numeric_val", TypeName = "decimal(12, 0)")]
        public decimal? LotNoNumericVal { get; set; }
        [Column("filler_0002")]
        [StringLength(2)]
        public string Filler0002 { get; set; }
        [Column("disab_prot_fld_fg")]
        [StringLength(1)]
        public string DisabProtFldFg { get; set; }
        [Column("batch_ctl_fg")]
        [StringLength(1)]
        public string BatchCtlFg { get; set; }
        [Column("alloc_meth_fg")]
        [StringLength(1)]
        public string AllocMethFg { get; set; }
        [Column("dflt_tag_frm")]
        public byte? DfltTagFrm { get; set; }
        [Column("dflt_lbl_frm")]
        public byte? DfltLblFrm { get; set; }
        [Column("prd_size")]
        public byte? PrdSize { get; set; }
        [Column("day_of_week")]
        public byte? DayOfWeek { get; set; }
        [Column("bin_receipt_meth")]
        [StringLength(1)]
        public string BinReceiptMeth { get; set; }
        [Column("ser_lot_rec_meth")]
        [StringLength(1)]
        public string SerLotRecMeth { get; set; }
        [Column("lf_init_fg")]
        [StringLength(1)]
        public string LfInitFg { get; set; }
        [Column("note_lit_1")]
        [StringLength(10)]
        public string NoteLit1 { get; set; }
        [Column("note_lit_2")]
        [StringLength(10)]
        public string NoteLit2 { get; set; }
        [Column("note_lit_3")]
        [StringLength(10)]
        public string NoteLit3 { get; set; }
        [Column("note_lit_4")]
        [StringLength(10)]
        public string NoteLit4 { get; set; }
        [Column("note_lit_5")]
        [StringLength(10)]
        public string NoteLit5 { get; set; }
        [Column("dt_lit")]
        [StringLength(10)]
        public string DtLit { get; set; }
        [Column("amt_lit")]
        [StringLength(10)]
        public string AmtLit { get; set; }
        [Column("use_expire_sl_fg")]
        [StringLength(1)]
        public string UseExpireSlFg { get; set; }
        [Column("show_zero_qty_bins")]
        [StringLength(1)]
        public string ShowZeroQtyBins { get; set; }
        [Column("auto_except_fg")]
        [StringLength(1)]
        public string AutoExceptFg { get; set; }
        [Column("insuff_display_fg")]
        [StringLength(1)]
        public string InsuffDisplayFg { get; set; }
        [Column("bypass_gl_dist_fg")]
        [StringLength(1)]
        public string BypassGlDistFg { get; set; }
        [Column("atp_def_atp_prds")]
        public byte? AtpDefAtpPrds { get; set; }
        [Column("atp_def_use_stk")]
        [StringLength(1)]
        public string AtpDefUseStk { get; set; }
        [Column("atp_def_use_ord")]
        [StringLength(1)]
        public string AtpDefUseOrd { get; set; }
        [Column("atp_def_use_coq")]
        [StringLength(1)]
        public string AtpDefUseCoq { get; set; }
        [Column("atp_def_min_cd")]
        public byte? AtpDefMinCd { get; set; }
        [Column("atp_def_use_cob")]
        [StringLength(1)]
        public string AtpDefUseCob { get; set; }
        [Column("atp_def_use_unr")]
        [StringLength(1)]
        public string AtpDefUseUnr { get; set; }
        [Column("atp_def_use_fp")]
        [StringLength(1)]
        public string AtpDefUseFp { get; set; }
        [Column("atp_def_use_cp")]
        [StringLength(1)]
        public string AtpDefUseCp { get; set; }
        [Column("stock_work_file_dt", TypeName = "datetime")]
        public DateTime? StockWorkFileDt { get; set; }
        [Column("sl_range_fg")]
        [StringLength(1)]
        public string SlRangeFg { get; set; }
        [Column("close_dt", TypeName = "datetime")]
        public DateTime? CloseDt { get; set; }
        [Column("close_purge_dt", TypeName = "datetime")]
        public DateTime? ClosePurgeDt { get; set; }
        [Column("extra_1")]
        [StringLength(1)]
        public string Extra1 { get; set; }
        [Column("extra_2")]
        [StringLength(1)]
        public string Extra2 { get; set; }
        [Column("extra_3")]
        [StringLength(1)]
        public string Extra3 { get; set; }
        [Column("extra_4")]
        [StringLength(1)]
        public string Extra4 { get; set; }
        [Column("extra_5")]
        [StringLength(1)]
        public string Extra5 { get; set; }
        [Column("extra_6")]
        [StringLength(8)]
        public string Extra6 { get; set; }
        [Column("extra_7")]
        [StringLength(8)]
        public string Extra7 { get; set; }
        [Column("extra_8")]
        [StringLength(12)]
        public string Extra8 { get; set; }
        [Column("extra_9")]
        [StringLength(12)]
        public string Extra9 { get; set; }
        [Column("extra_10", TypeName = "decimal(16, 6)")]
        public decimal? Extra10 { get; set; }
        [Column("extra_11", TypeName = "decimal(16, 6)")]
        public decimal? Extra11 { get; set; }
        [Column("extra_12", TypeName = "decimal(16, 2)")]
        public decimal? Extra12 { get; set; }
        [Column("extra_13", TypeName = "decimal(16, 2)")]
        public decimal? Extra13 { get; set; }
        [Column("extra_14")]
        public int? Extra14 { get; set; }
        [Column("extra_15")]
        public int? Extra15 { get; set; }
        [Column("activity_days")]
        public short? ActivityDays { get; set; }
        [Column("labor_in_view")]
        [StringLength(1)]
        public string LaborInView { get; set; }
        [Column("mat_cost_main_no")]
        [StringLength(9)]
        public string MatCostMainNo { get; set; }
        [Column("mat_cost_sub_no")]
        [StringLength(8)]
        public string MatCostSubNo { get; set; }
        [Column("mat_cost_dpt_no")]
        [StringLength(8)]
        public string MatCostDptNo { get; set; }
        [Column("mat_qty_main_no")]
        [StringLength(9)]
        public string MatQtyMainNo { get; set; }
        [Column("mat_qty_sub_no")]
        [StringLength(8)]
        public string MatQtySubNo { get; set; }
        [Column("mat_qty_dpt_no")]
        [StringLength(8)]
        public string MatQtyDptNo { get; set; }
        [Column("cycle_freq_A_days")]
        public short? CycleFreqADays { get; set; }
        [Column("cycle_freq_B_days")]
        public short? CycleFreqBDays { get; set; }
        [Column("cycle_freq_C_days")]
        public short? CycleFreqCDays { get; set; }
        [Column("cycle_freq_X_days")]
        public short? CycleFreqXDays { get; set; }
        [Column("filler_0003")]
        [StringLength(80)]
        public string Filler0003 { get; set; }
        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal Id { get; set; }
    }
}
