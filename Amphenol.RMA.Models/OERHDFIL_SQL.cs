using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class OERHDFIL_SQL
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal Id { get; set; }

        [Required]
        [Column("rma_no")]
        [StringLength(8)]
        public string rma_no { get; set; }
        [Required]
        [Column("cus_no")]
        [StringLength(20)]
        public string cus_no { get; set; }
        [Required]
        [Column("cus_ship_to")]
        [StringLength(15)]
        public string cus_ship_to { get; set; }
        [Column("rma_dt_entered", TypeName = "datetime")]
        public DateTime? rma_dt_entered { get; set; }
        [Column("follow_up_dt", TypeName = "datetime")]
        public DateTime? follow_up_dt { get; set; }
        [Required]
        [Column("status")]
        [StringLength(1)]
        public string status { get; set; }
        [Column("oe_po_no")]
        [StringLength(25)]
        public string oe_po_no { get; set; }
        [Column("bill_to_name")]
        [StringLength(40)]
        public string bill_to_name { get; set; }
        [Column("bill_to_addr_1")]
        [StringLength(40)]
        public string bill_to_addr_1 { get; set; }
        [Column("bill_to_addr_2")]
        [StringLength(40)]
        public string bill_to_addr_2 { get; set; }
        [Column("bill_to_addr_3")]
        [StringLength(40)]
        public string bill_to_addr_3 { get; set; }
        [Column("bill_to_addr_4")]
        [StringLength(44)]
        public string bill_to_addr_4 { get; set; }
        [Column("bill_to_country")]
        [StringLength(3)]
        public string bill_to_country { get; set; }
        [Column("ship_to_name")]
        [StringLength(40)]
        public string ship_to_name { get; set; }
        [Column("ship_to_addr_1")]
        [StringLength(40)]
        public string ship_to_addr_1 { get; set; }
        [Column("ship_to_addr_2")]
        [StringLength(40)]
        public string ship_to_addr_2 { get; set; }
        [Column("ship_to_addr_3")]
        [StringLength(40)]
        public string ship_to_addr_3 { get; set; }
        [Column("ship_to_addr_4")]
        [StringLength(44)]
        public string ship_to_addr_4 { get; set; }
        [Column("ship_to_country")]
        [StringLength(3)]
        public string ship_to_country { get; set; }
        [Column("ship_via_cd")]
        [StringLength(3)]
        public string ship_via_cd { get; set; }
        [Column("ar_terms_cd")]
        [StringLength(2)]
        public string ar_terms_cd { get; set; }
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
        public int? slspsn_no { get; set; }
        [Column("slspsn_pct_comm", TypeName = "decimal(6, 3)")]
        public decimal? slspsn_pct_comm { get; set; }
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
        public string tax_cd { get; set; }
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
        public string mfg_loc { get; set; }
        [Column("profit_center")]
        [StringLength(8)]
        public string profit_center { get; set; }
        [Column("dept")]
        [StringLength(8)]
        public string dept { get; set; }
        [Column("ar_reference")]
        [StringLength(45)]
        public string ar_reference { get; set; }
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
        [Column("lost_sale_cd")]
        [StringLength(3)]
        public string LostSaleCd { get; set; }
        [Column("exch_rt_fg")]
        [StringLength(1)]
        public string ExchRtFg { get; set; }
        [Column("curr_cd")]
        [StringLength(3)]
        public string curr_cd { get; set; }
        [Column("orig_trx_rt", TypeName = "decimal(11, 6)")]
        public decimal? orig_trx_rt { get; set; }
        [Column("curr_trx_rt", TypeName = "decimal(11, 6)")]
        public decimal? curr_trx_rt { get; set; }
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
        public string user_def_fld_5 { get; set; }
        [Column("deter_rate_by")]
        [StringLength(1)]
        public string deter_rate_by { get; set; }
        [Column("form_no")]
        public byte? form_no { get; set; }
        [Column("tax_fg")]
        [StringLength(1)]
        public string TaxFg { get; set; }
        [Column("tot_dollars", TypeName = "decimal(16, 2)")]
        public decimal? TotDollars { get; set; }
        [Column("mult_loc_fg")]
        [StringLength(1)]
        public string MultLocFg { get; set; }
        [Column("tot_tax_cost", TypeName = "decimal(16, 2)")]
        public decimal? TotTaxCost { get; set; }
        [Column("deliv_ar_terms_cd")]
        [StringLength(2)]
        public string DelivArTermsCd { get; set; }
        [Column("rma_cmt")]
        [StringLength(30)]
        public string rma_cmt { get; set; }
        [Column("contact")]
        [StringLength(40)]
        public string contact { get; set; }
        [Column("contact_email")]
        [StringLength(128)]
        public string contact_email { get; set; }
        [Column("phone_no")]
        [StringLength(20)]
        public string phone_no { get; set; }
        [Column("phone_ext")]
        [StringLength(4)]
        public string phone_ext { get; set; }
        [Column("filler1")]
        [StringLength(16)]
        public string Filler1 { get; set; }
        [Column("fax_no")]
        [StringLength(20)]
        public string fax_no { get; set; }
        [Column("filler2")]
        [StringLength(20)]
        public string Filler2 { get; set; }
        [Column("last_act_dt", TypeName = "datetime")]
        public DateTime? LastActDt { get; set; }
        [Column("exp_rec_date", TypeName = "datetime")]
        public DateTime? ExpRecDate { get; set; }
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
        [Column("filler_0001")]
        [StringLength(150)]
        public string Filler0001 { get; set; }
       


    }
}
