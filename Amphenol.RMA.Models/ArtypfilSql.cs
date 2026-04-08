using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models
{
    [Keyless]
    [Table("artypfil_sql")]
    [Index(nameof(Id), Name = "pk_artypfil_sql", IsUnique = true)]
    public partial class ArtypfilSql
    {
        [Required]
        [Column("cus_type_cd")]
        [StringLength(5)]
        public string CusTypeCd { get; set; }
        [Column("cus_type_desc")]
        [StringLength(15)]
        public string CusTypeDesc { get; set; }
        [Column("ar_mn_no")]
        [StringLength(9)]
        public string ArMnNo { get; set; }
        [Column("ar_sb_no")]
        [StringLength(8)]
        public string ArSbNo { get; set; }
        [Column("ar_dp_no")]
        [StringLength(8)]
        public string ArDpNo { get; set; }
        [Column("sls_mn_no")]
        [StringLength(9)]
        public string SlsMnNo { get; set; }
        [Column("sls_sb_no")]
        [StringLength(8)]
        public string SlsSbNo { get; set; }
        [Column("sls_dp_no")]
        [StringLength(8)]
        public string SlsDpNo { get; set; }
        [Column("misc_mn_no")]
        [StringLength(9)]
        public string MiscMnNo { get; set; }
        [Column("misc_sb_no")]
        [StringLength(8)]
        public string MiscSbNo { get; set; }
        [Column("misc_dp_no")]
        [StringLength(8)]
        public string MiscDpNo { get; set; }
        [Column("frt_mn_no")]
        [StringLength(9)]
        public string FrtMnNo { get; set; }
        [Column("frt_sb_no")]
        [StringLength(8)]
        public string FrtSbNo { get; set; }
        [Column("frt_dp_no")]
        [StringLength(8)]
        public string FrtDpNo { get; set; }
        [Column("disc_mn_no")]
        [StringLength(9)]
        public string DiscMnNo { get; set; }
        [Column("disc_sb_no")]
        [StringLength(8)]
        public string DiscSbNo { get; set; }
        [Column("disc_dp_no")]
        [StringLength(8)]
        public string DiscDpNo { get; set; }
        [Column("allow_mn_no")]
        [StringLength(9)]
        public string AllowMnNo { get; set; }
        [Column("allow_sb_no")]
        [StringLength(8)]
        public string AllowSbNo { get; set; }
        [Column("allow_dp_no")]
        [StringLength(8)]
        public string AllowDpNo { get; set; }
        [Column("fchrg_mn_no")]
        [StringLength(9)]
        public string FchrgMnNo { get; set; }
        [Column("fchrg_sb_no")]
        [StringLength(8)]
        public string FchrgSbNo { get; set; }
        [Column("fchrg_dp_no")]
        [StringLength(8)]
        public string FchrgDpNo { get; set; }
        [Column("svsls_mn_no")]
        [StringLength(9)]
        public string SvslsMnNo { get; set; }
        [Column("svsls_sb_no")]
        [StringLength(8)]
        public string SvslsSbNo { get; set; }
        [Column("svsls_dp_no")]
        [StringLength(8)]
        public string SvslsDpNo { get; set; }
        [Column("svcogs_mn_no")]
        [StringLength(9)]
        public string SvcogsMnNo { get; set; }
        [Column("svcogs_sb_no")]
        [StringLength(8)]
        public string SvcogsSbNo { get; set; }
        [Column("svcogs_dp_no")]
        [StringLength(8)]
        public string SvcogsDpNo { get; set; }
        [Column("wrtoff_mn_no")]
        [StringLength(9)]
        public string WrtoffMnNo { get; set; }
        [Column("wrtoff_sb_no")]
        [StringLength(8)]
        public string WrtoffSbNo { get; set; }
        [Column("wrtoff_dp_no")]
        [StringLength(8)]
        public string WrtoffDpNo { get; set; }
        [Column("user_def_fld_1")]
        [StringLength(75)]
        public string UserDefFld1 { get; set; }
        [Column("user_def_fld_2")]
        [StringLength(75)]
        public string UserDefFld2 { get; set; }
        [Column("user_def_fld_3")]
        [StringLength(75)]
        public string UserDefFld3 { get; set; }
        [Column("user_def_fld_4")]
        [StringLength(75)]
        public string UserDefFld4 { get; set; }
        [Column("user_def_fld_5")]
        [StringLength(75)]
        public string UserDefFld5 { get; set; }
        [Column("user_fld_6")]
        [StringLength(75)]
        public string UserFld6 { get; set; }
        [Column("user_fld_7")]
        [StringLength(75)]
        public string UserFld7 { get; set; }
        [Column("user_fld_8", TypeName = "decimal(18, 4)")]
        public decimal? UserFld8 { get; set; }
        [Column("user_fld_9", TypeName = "decimal(18, 4)")]
        public decimal? UserFld9 { get; set; }
        [Column("user_fld_10", TypeName = "decimal(18, 4)")]
        public decimal? UserFld10 { get; set; }
        [Column("user_fld_11", TypeName = "decimal(18, 4)")]
        public decimal? UserFld11 { get; set; }
        [Column("user_fld_12", TypeName = "decimal(18, 4)")]
        public decimal? UserFld12 { get; set; }
        [Column("user_fld_13", TypeName = "decimal(18, 4)")]
        public decimal? UserFld13 { get; set; }
        [Column("user_14_date", TypeName = "datetime")]
        public DateTime? User14Date { get; set; }
        [Column("user_15_date", TypeName = "datetime")]
        public DateTime? User15Date { get; set; }
        [Column("user_16_date", TypeName = "datetime")]
        public DateTime? User16Date { get; set; }
        [Column("user_fld_17")]
        public short? UserFld17 { get; set; }
        [Column("user_fld_18")]
        public short? UserFld18 { get; set; }
        [Column("user_fld_19")]
        public short? UserFld19 { get; set; }
        [Column("user_fld_20")]
        public short? UserFld20 { get; set; }
        [Column("ar_exch_mn_no")]
        [StringLength(9)]
        public string ArExchMnNo { get; set; }
        [Column("ar_exch_sb_no")]
        [StringLength(8)]
        public string ArExchSbNo { get; set; }
        [Column("ar_exch_dp_no")]
        [StringLength(8)]
        public string ArExchDpNo { get; set; }
        [Column("curr_cd")]
        [StringLength(3)]
        public string CurrCd { get; set; }
        [Column("whole_order_fg")]
        [StringLength(1)]
        public string WholeOrderFg { get; set; }
        [Column("doc_field_1", TypeName = "decimal(12, 0)")]
        public decimal? DocField1 { get; set; }
        [Column("doc_field_2", TypeName = "decimal(12, 0)")]
        public decimal? DocField2 { get; set; }
        [Column("doc_field_3", TypeName = "decimal(12, 0)")]
        public decimal? DocField3 { get; set; }
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
        [Column("sales_disc_main_no")]
        [StringLength(9)]
        public string SalesDiscMainNo { get; set; }
        [Column("sales_disc_sub_no")]
        [StringLength(8)]
        public string SalesDiscSubNo { get; set; }
        [Column("sales_disc_dp_no")]
        [StringLength(8)]
        public string SalesDiscDpNo { get; set; }
        [Column("filler_0001")]
        [StringLength(126)]
        public string Filler0001 { get; set; }
        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal Id { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }
    }
}
