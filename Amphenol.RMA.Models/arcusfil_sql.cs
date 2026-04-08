using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class arcusfil_sql
    {
       [Key]
        


        [Column("cus_no")]
        [StringLength(20)]
        public string cus_no { get; set; }
        [Column("debnr")]
        [StringLength(6)]
        public string Debnr { get; set; }
        [Column("cus_name")]
        [StringLength(50)]
        public string cus_name { get; set; }
        [Column("addr_1")]
        [StringLength(100)]
        public string Addr1 { get; set; }
        [Column("addr_2")]
        [StringLength(100)]
        public string Addr2 { get; set; }
        [Column("addr_3")]
        [StringLength(100)]
        public string Addr3 { get; set; }
        [Column("city")]
        [StringLength(100)]
        public string City { get; set; }
        [Column("state")]
        [StringLength(3)]
        public string State { get; set; }
        [Column("zip")]
        [StringLength(20)]
        public string Zip { get; set; }
        [Column("country")]
        [StringLength(3)]
        public string Country { get; set; }
        [Column("phone_no")]
        [StringLength(25)]
        public string PhoneNo { get; set; }
        [Column("fax_no")]
        [StringLength(25)]
        public string FaxNo { get; set; }
        [Column("start_dt", TypeName = "datetime")]
        public DateTime StartDt { get; set; }
        [Column("slspsn_no")]
        public int? SlspsnNo { get; set; }
        [Column("cus_type_cd")]
        [StringLength(5)]
        public string CusTypeCd { get; set; }
        [Column("stm_freq")]
        [StringLength(1)]
        public string StmFreq { get; set; }
        [Column("cr_lmt")]
        public double CrLmt { get; set; }
        [Column("cr_rating")]
        [StringLength(4)]
        public string CrRating { get; set; }
        [Column("hold_fg")]
        [StringLength(1)]
        public string HoldFg { get; set; }
        [Required]
        [Column("fin_chg_fg")]
        [StringLength(1)]
        public string FinChgFg { get; set; }
        [Column("origin")]
        [StringLength(3)]
        public string Origin { get; set; }
        [Column("curr_cd")]
        [StringLength(3)]
        public string curr_cd { get; set; }
        [Column("par_cus_no")]
        [StringLength(20)]
        public string ParCusNo { get; set; }
        [Column("par_cus_fg")]
        public int? ParCusFg { get; set; }
        [Column("ship_via_cd")]
        [StringLength(3)]
        public string ShipViaCd { get; set; }
        [Column("ups_zone")]
        [StringLength(4)]
        public string UpsZone { get; set; }
        [Column("ar_terms_cd")]
        [StringLength(2)]
        public string ArTermsCd { get; set; }
        [Column("dsc_pct")]
        public double DscPct { get; set; }
        [Required]
        [Column("txbl_fg")]
        [StringLength(1)]
        public string TxblFg { get; set; }
        [Column("tax_cd")]
        [StringLength(3)]
        public string TaxCd { get; set; }
        [Column("tax_cd_2")]
        [StringLength(3)]
        public string TaxCd2 { get; set; }
        [Column("tax_cd_3")]
        [StringLength(3)]
        public string TaxCd3 { get; set; }
        [Column("exempt_no")]
        [StringLength(20)]
        public string ExemptNo { get; set; }
        [Column("balance", TypeName = "decimal(16, 2)")]
        public decimal? Balance { get; set; }
        [Required]
        [Column("allow_sb_item")]
        [StringLength(1)]
        public string AllowSbItem { get; set; }
        [Required]
        [Column("allow_bo")]
        [StringLength(1)]
        public string AllowBo { get; set; }
        [Required]
        [Column("allow_part_ship")]
        [StringLength(1)]
        public string AllowPartShip { get; set; }
        [Required]
        [Column("print_dunn_fg")]
        [StringLength(1)]
        public string PrintDunnFg { get; set; }
        [Column("cmt_1")]
        [StringLength(30)]
        public string Cmt1 { get; set; }
        [Column("cmt_2")]
        [StringLength(30)]
        public string Cmt2 { get; set; }
        [Column("tax_sched")]
        [StringLength(5)]
        public string TaxSched { get; set; }
        [Column("cr_card_1_desc")]
        [StringLength(15)]
        public string CrCard1Desc { get; set; }
        [Column("cr_card_1_acct")]
        [MaxLength(34)]
        public byte[] CrCard1Acct { get; set; }
        [Column("cr_card_1_exp_dt", TypeName = "datetime")]
        public DateTime? CrCard1ExpDt { get; set; }
        [Column("user_def_fld_1")]
        [StringLength(80)]
        public string UserDefFld1 { get; set; }
        [Column("user_def_fld_2")]
        [StringLength(80)]
        public string UserDefFld2 { get; set; }
        [Column("user_def_fld_3")]
        [StringLength(80)]
        public string UserDefFld3 { get; set; }
        [Column("user_def_fld_4")]
        [StringLength(80)]
        public string UserDefFld4 { get; set; }
        [Column("user_def_fld_5")]
        [StringLength(80)]
        public string UserDefFld5 { get; set; }
        [Column("dflt_inv_form")]
        public byte? DfltInvForm { get; set; }
        [Column("loc")]
        [StringLength(3)]
        public string Loc { get; set; }
        [Column("cus_note_1")]
        [StringLength(80)]
        public string CusNote1 { get; set; }
        [Column("cus_note_2")]
        [StringLength(80)]
        public string CusNote2 { get; set; }
        [Column("cus_note_3")]
        [StringLength(80)]
        public string CusNote3 { get; set; }
        [Column("cus_note_4")]
        [StringLength(80)]
        public string CusNote4 { get; set; }
        [Column("cus_note_5")]
        [StringLength(80)]
        public string CusNote5 { get; set; }
        [Column("terr")]
        [StringLength(6)]
        public string Terr { get; set; }
        [Column("user_dt", TypeName = "datetime")]
        public DateTime? UserDt { get; set; }
        [Column("user_amount")]
        public double UserAmount { get; set; }
        [Column("email_address")]
        [StringLength(128)]
        public string EmailAddress { get; set; }
        [Column("tax_id")]
        [StringLength(13)]
        public string TaxId { get; set; }
        [Required]
        [Column("bill_parent_fg")]
        [StringLength(1)]
        public string BillParentFg { get; set; }
        [Column("url")]
        [StringLength(128)]
        public string Url { get; set; }
        [Column("cr_card_1_hldr")]
        [StringLength(40)]
        public string CrCard1Hldr { get; set; }
        [Column("cus_alt_adr_cd")]
        [StringLength(15)]
        public string CusAltAdrCd { get; set; }
        [Column("payment_method")]
        [StringLength(1)]
        public string PaymentMethod { get; set; }
        [Column("ID")]
        public int Id { get; set; }
    }
}
