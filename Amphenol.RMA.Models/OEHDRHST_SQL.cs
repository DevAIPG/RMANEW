using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class OEHDRHST_SQL
    {
       [Key]
        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal Id { get; set; }
        [Required]
        [Column("ord_type")]
        [StringLength(1)]
        public string OrdType { get; set; }
        [Required]
        [Column("ord_no")]
        [StringLength(8)]
        public string OrdNo { get; set; }
        [Column("status")]
        [StringLength(1)]
        public string Status { get; set; }
        [Column("entered_dt", TypeName = "datetime")]
        public DateTime? EnteredDt { get; set; }
        [Column("ord_dt", TypeName = "datetime")]
        public DateTime? OrdDt { get; set; }
        [Column("apply_to_no")]
        [StringLength(8)]
        public string ApplyToNo { get; set; }
        [Required]
        [Column("oe_po_no")]
        [StringLength(25)]
        public string OePoNo { get; set; }
        [Required]
        [Column("cus_no")]
        [StringLength(20)]
        public string CusNo { get; set; }
        [Column("bal_meth")]
        [StringLength(1)]
        public string BalMeth { get; set; }
        [Column("bill_to_name")]
        [StringLength(40)]
        public string BillToName { get; set; }
        [Column("bill_to_addr_1")]
        [StringLength(40)]
        public string BillToAddr1 { get; set; }
        [Column("bill_to_addr_2")]
        [StringLength(40)]
        public string BillToAddr2 { get; set; }
        [Column("bill_to_addr_3")]
        [StringLength(40)]
        public string BillToAddr3 { get; set; }
        [Column("bill_to_addr_4")]
        [StringLength(44)]
        public string BillToAddr4 { get; set; }
        [Column("bill_to_country")]
        [StringLength(3)]
        public string BillToCountry { get; set; }
        [Required]
        [Column("cus_alt_adr_cd")]
        [StringLength(15)]
        public string CusAltAdrCd { get; set; }
        [Column("ship_to_name")]
        [StringLength(40)]
        public string ShipToName { get; set; }
        [Column("ship_to_addr_1")]
        [StringLength(40)]
        public string ShipToAddr1 { get; set; }
        [Column("ship_to_addr_2")]
        [StringLength(40)]
        public string ShipToAddr2 { get; set; }
        [Column("ship_to_addr_3")]
        [StringLength(40)]
        public string ShipToAddr3 { get; set; }
        [Column("ship_to_addr_4")]
        [StringLength(44)]
        public string ShipToAddr4 { get; set; }
        [Column("ship_to_country")]
        [StringLength(3)]
        public string ShipToCountry { get; set; }
        [Column("shipping_dt", TypeName = "datetime")]
        public DateTime? ShippingDt { get; set; }
        [Column("ship_via_cd")]
        [StringLength(3)]
        public string ShipViaCd { get; set; }
        [Column("ar_terms_cd")]
        [StringLength(2)]
        public string ArTermsCd { get; set; }
        [Column("frt_pay_cd")]
        [StringLength(1)]
        public string FrtPayCd { get; set; }
        [Column("ship_instruction_1")]
        [StringLength(40)]
        public string ShipInstruction1 { get; set; }
        [Column("ship_instruction_2")]
        [StringLength(40)]
        public string ShipInstruction2 { get; set; }
        [Column("slspsn_no")]
        public int SlspsnNo { get; set; }
        [Column("slspsn_pct_comm", TypeName = "decimal(6, 3)")]
        public decimal? SlspsnPctComm { get; set; }
        [Column("slspsn_comm_amt", TypeName = "decimal(16, 2)")]
        public decimal? SlspsnCommAmt { get; set; }
        [Column("slspsn_no_2")]
        public int? SlspsnNo2 { get; set; }
        [Column("slspsn_pct_comm_2", TypeName = "decimal(6, 3)")]
        public decimal? SlspsnPctComm2 { get; set; }
        [Column("slspsn_comm_amt_2", TypeName = "decimal(16, 2)")]
        public decimal? SlspsnCommAmt2 { get; set; }
        [Column("slspsn_no_3")]
        public int? SlspsnNo3 { get; set; }
        [Column("slspsn_pct_comm_3", TypeName = "decimal(6, 3)")]
        public decimal? SlspsnPctComm3 { get; set; }
        [Column("slspsn_comm_amt_3", TypeName = "decimal(16, 2)")]
        public decimal? SlspsnCommAmt3 { get; set; }
        [Column("tax_cd")]
        [StringLength(3)]
        public string TaxCd { get; set; }
        [Column("tax_pct", TypeName = "decimal(6, 4)")]
        public decimal? TaxPct { get; set; }
        [Column("tax_cd_2")]
        [StringLength(3)]
        public string TaxCd2 { get; set; }
        [Column("tax_pct_2", TypeName = "decimal(6, 4)")]
        public decimal? TaxPct2 { get; set; }
        [Column("tax_cd_3")]
        [StringLength(3)]
        public string TaxCd3 { get; set; }
        [Column("tax_pct_3", TypeName = "decimal(6, 4)")]
        public decimal? TaxPct3 { get; set; }
        [Column("discount_pct", TypeName = "decimal(5, 2)")]
        public decimal? DiscountPct { get; set; }
        [Column("job_no")]
        [StringLength(20)]
        public string JobNo { get; set; }
        [Column("mfg_loc")]
        [StringLength(3)]
        public string MfgLoc { get; set; }
        [Column("profit_center")]
        [StringLength(8)]
        public string ProfitCenter { get; set; }
        [Column("dept")]
        [StringLength(8)]
        public string Dept { get; set; }
        [Column("ar_reference")]
        [StringLength(45)]
        public string ArReference { get; set; }
        [Column("tot_sls_amt", TypeName = "decimal(16, 2)")]
        public decimal? TotSlsAmt { get; set; }
        [Column("tot_sls_disc", TypeName = "decimal(16, 2)")]
        public decimal? TotSlsDisc { get; set; }
        [Column("tot_tax_amt", TypeName = "decimal(16, 2)")]
        public decimal? TotTaxAmt { get; set; }
        [Column("tot_cost", TypeName = "decimal(16, 2)")]
        public decimal? TotCost { get; set; }
        [Column("tot_weight", TypeName = "decimal(10, 3)")]
        public decimal? TotWeight { get; set; }
        [Column("misc_amt", TypeName = "decimal(16, 2)")]
        public decimal? MiscAmt { get; set; }
        [Column("misc_mn_no")]
        [StringLength(9)]
        public string MiscMnNo { get; set; }
        [Column("misc_sb_no")]
        [StringLength(8)]
        public string MiscSbNo { get; set; }
        [Column("misc_dp_no")]
        [StringLength(8)]
        public string MiscDpNo { get; set; }
        [Column("frt_amt", TypeName = "decimal(16, 2)")]
        public decimal? FrtAmt { get; set; }
        [Column("frt_mn_no")]
        [StringLength(9)]
        public string FrtMnNo { get; set; }
        [Column("frt_sb_no")]
        [StringLength(8)]
        public string FrtSbNo { get; set; }
        [Column("frt_dp_no")]
        [StringLength(8)]
        public string FrtDpNo { get; set; }
        [Column("sls_tax_amt_1", TypeName = "decimal(16, 2)")]
        public decimal? SlsTaxAmt1 { get; set; }
        [Column("sls_tax_amt_2", TypeName = "decimal(16, 2)")]
        public decimal? SlsTaxAmt2 { get; set; }
        [Column("sls_tax_amt_3", TypeName = "decimal(16, 2)")]
        public decimal? SlsTaxAmt3 { get; set; }
        [Column("comm_pct", TypeName = "decimal(4, 2)")]
        public decimal? CommPct { get; set; }
        [Column("comm_amt", TypeName = "decimal(16, 2)")]
        public decimal? CommAmt { get; set; }
        [Column("cmt_1")]
        [StringLength(35)]
        public string Cmt1 { get; set; }
        [Column("cmt_2")]
        [StringLength(35)]
        public string Cmt2 { get; set; }
        [Column("cmt_3")]
        [StringLength(35)]
        public string Cmt3 { get; set; }
        [Column("payment_amt", TypeName = "decimal(16, 2)")]
        public decimal? PaymentAmt { get; set; }
        [Column("payment_disc_amt", TypeName = "decimal(16, 2)")]
        public decimal? PaymentDiscAmt { get; set; }
        [Column("chk_no")]
        [StringLength(8)]
        public string ChkNo { get; set; }
        [Column("chk_dt", TypeName = "datetime")]
        public DateTime? ChkDt { get; set; }
        [Column("cash_mn_no")]
        [StringLength(9)]
        public string CashMnNo { get; set; }
        [Column("cash_sb_no")]
        [StringLength(8)]
        public string CashSbNo { get; set; }
        [Column("cash_dp_no")]
        [StringLength(8)]
        public string CashDpNo { get; set; }
        [Column("ord_dt_picked", TypeName = "datetime")]
        public DateTime? OrdDtPicked { get; set; }
        [Column("ord_dt_billed", TypeName = "datetime")]
        public DateTime? OrdDtBilled { get; set; }
        [Required]
        [Column("inv_no")]
        [StringLength(8)]
        public string InvNo { get; set; }
        [Column("inv_dt", TypeName = "datetime")]
        public DateTime? InvDt { get; set; }
        [Column("selection_cd")]
        [StringLength(1)]
        public string SelectionCd { get; set; }
        [Column("posted_dt", TypeName = "datetime")]
        public DateTime? PostedDt { get; set; }
        [Column("part_posted_fg")]
        [StringLength(1)]
        public string PartPostedFg { get; set; }
        [Column("ship_to_freefrm_fg")]
        [StringLength(1)]
        public string ShipToFreefrmFg { get; set; }
        [Column("bill_to_freefrm_fg")]
        [StringLength(1)]
        public string BillToFreefrmFg { get; set; }
        [Column("copy_to_bm_fg")]
        [StringLength(1)]
        public string CopyToBmFg { get; set; }
        [Column("edi_fg")]
        [StringLength(1)]
        public string EdiFg { get; set; }
        [Column("closed_fg")]
        [StringLength(1)]
        public string ClosedFg { get; set; }
        [Column("accum_misc_amt", TypeName = "decimal(16, 2)")]
        public decimal? AccumMiscAmt { get; set; }
        [Column("accum_frt_amt", TypeName = "decimal(16, 2)")]
        public decimal? AccumFrtAmt { get; set; }
        [Column("accum_tot_tax_amt", TypeName = "decimal(16, 2)")]
        public decimal? AccumTotTaxAmt { get; set; }
        [Column("accum_sls_tax_amt", TypeName = "decimal(16, 2)")]
        public decimal? AccumSlsTaxAmt { get; set; }
        [Column("accum_tot_sls_amt", TypeName = "decimal(16, 2)")]
        public decimal? AccumTotSlsAmt { get; set; }
        [Column("hold_fg")]
        [StringLength(1)]
        public string HoldFg { get; set; }
        [Column("prepayment_fg")]
        [StringLength(1)]
        public string PrepaymentFg { get; set; }
        [Column("lost_sale_cd")]
        [StringLength(3)]
        public string LostSaleCd { get; set; }
        [Column("orig_ord_type")]
        [StringLength(1)]
        public string OrigOrdType { get; set; }
        [Column("orig_ord_dt", TypeName = "datetime")]
        public DateTime? OrigOrdDt { get; set; }
        [Column("orig_ord_no")]
        [StringLength(8)]
        public string OrigOrdNo { get; set; }
        [Column("award_probability")]
        public byte? AwardProbability { get; set; }
        [Column("oe_cash_no")]
        [StringLength(8)]
        public string OeCashNo { get; set; }
        [Column("exch_rt_fg")]
        [StringLength(1)]
        public string ExchRtFg { get; set; }
        [Column("curr_cd")]
        [StringLength(3)]
        public string CurrCd { get; set; }
        [Column("orig_trx_rt", TypeName = "decimal(11, 6)")]
        public decimal? OrigTrxRt { get; set; }
        [Column("curr_trx_rt", TypeName = "decimal(11, 6)")]
        public decimal? CurrTrxRt { get; set; }
        [Column("tax_sched")]
        [StringLength(5)]
        public string TaxSched { get; set; }
        [Column("user_def_fld_1")]
        [StringLength(30)]
        public string UserDefFld1 { get; set; }
        [Column("user_def_fld_2")]
        [StringLength(30)]
        public string UserDefFld2 { get; set; }
        [Column("user_def_fld_3")]
        [StringLength(30)]
        public string UserDefFld3 { get; set; }
        [Column("user_def_fld_4")]
        [StringLength(30)]
        public string UserDefFld4 { get; set; }
        [Column("user_def_fld_5")]
        [StringLength(30)]
        public string UserDefFld5 { get; set; }
        [Column("deter_rate_by")]
        [StringLength(1)]
        public string DeterRateBy { get; set; }
        [Column("form_no")]
        public byte? FormNo { get; set; }
        [Column("tax_fg")]
        [StringLength(1)]
        public string TaxFg { get; set; }
        [Column("sls_mn_no")]
        [StringLength(9)]
        public string SlsMnNo { get; set; }
        [Column("sls_sb_no")]
        [StringLength(8)]
        public string SlsSbNo { get; set; }
        [Column("sls_dp_no")]
        [StringLength(8)]
        public string SlsDpNo { get; set; }
        [Column("ord_dt_shipped", TypeName = "datetime")]
        public DateTime? OrdDtShipped { get; set; }
        [Column("tot_dollars", TypeName = "decimal(16, 2)")]
        public decimal? TotDollars { get; set; }
        [Column("mult_loc_fg")]
        [StringLength(1)]
        public string MultLocFg { get; set; }
        [Column("tot_tax_cost", TypeName = "decimal(16, 2)")]
        public decimal? TotTaxCost { get; set; }
        [Column("hist_load_record")]
        [StringLength(1)]
        public string HistLoadRecord { get; set; }
        [Column("pre_select_status")]
        [StringLength(1)]
        public string PreSelectStatus { get; set; }
        [Column("packing_no")]
        public int? PackingNo { get; set; }
        [Column("deliv_ar_terms_cd")]
        [StringLength(2)]
        public string DelivArTermsCd { get; set; }
        [Column("inv_batch_id")]
        [StringLength(8)]
        public string InvBatchId { get; set; }
        [Column("bill_to_no")]
        [StringLength(20)]
        public string BillToNo { get; set; }
        [Column("rma_no")]
        [StringLength(8)]
        public string RmaNo { get; set; }
        [Column("prog_term_no")]
        public int? ProgTermNo { get; set; }
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
        [Column("edi_doc_seq")]
        public short? EdiDocSeq { get; set; }
        [Column("contact_1")]
        [StringLength(100)]
        public string Contact1 { get; set; }
        [Column("phone_number")]
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [Column("fax_number")]
        [StringLength(25)]
        public string FaxNumber { get; set; }
        [Column("email_address")]
        [StringLength(128)]
        public string EmailAddress { get; set; }
        [Column("use_email")]
        [StringLength(10)]
        public string UseEmail { get; set; }
        [Column("ship_to_city")]
        [StringLength(100)]
        public string ShipToCity { get; set; }
        [Column("ship_to_state")]
        [StringLength(3)]
        public string ShipToState { get; set; }
        [Column("ship_to_zip")]
        [StringLength(20)]
        public string ShipToZip { get; set; }
        [Column("bill_to_city")]
        [StringLength(100)]
        public string BillToCity { get; set; }
        [Column("bill_to_state")]
        [StringLength(3)]
        public string BillToState { get; set; }
        [Column("bill_to_zip")]
        [StringLength(20)]
        public string BillToZip { get; set; }
        [Column("filler_0001")]
        [StringLength(146)]
        public string Filler0001 { get; set; }
        [Column("hist_dt", TypeName = "datetime")]
        public DateTime? HistDt { get; set; }
        [Column("hist_tm", TypeName = "datetime")]
        public DateTime? HistTm { get; set; }
        [Column("user_name")]
        [StringLength(20)]
        public string UserName { get; set; }
        [Column("user_namex")]
        [StringLength(20)]
        public string UserNamex { get; set; }
        [Required]
        [Column("id_no")]
        [StringLength(50)]
        public string IdNo { get; set; }
        [Column("jnl_src")]
        [StringLength(6)]
        public string JnlSrc { get; set; }
        [Column("batch_id")]
        [StringLength(10)]
        public string BatchId { get; set; }
        [Required]
        [Column("trx_posted_fg")]
        [StringLength(1)]
        public string TrxPostedFg { get; set; }
     
        public Guid Uid { get; set; }

    }
}
