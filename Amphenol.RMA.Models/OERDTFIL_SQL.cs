using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class OERDTFIL_SQL
    {
       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal ID { get; set; }

        [Required]
        [Column("rma_no")]
        [StringLength(8)]
        public string rma_no { get; set; }
        [Column("rma_seq_no")]
        public short rma_seq_no { get; set; }
        [Required]
        [Column("apply_to_invc_no")]
        [StringLength(8)]
        public string apply_to_invc_no { get; set; }
        [Column("apply_to_seq_no")]
        public short apply_to_seq_no { get; set; }
        [Required]
        [Column("item_no")]
        [StringLength(30)]
        public string item_no { get; set; }
        [Column("reason_cd")]
        [StringLength(3)]
        public string reason_cd { get; set; }
        [Required]
        [Column("loc")]
        [StringLength(3)]
        public string loc { get; set; }
        [Required]
        [Column("pick_seq_no")]
        [StringLength(8)]
        public string pick_seq_no { get; set; }
        [Column("status")]
        [StringLength(1)]
        public string status { get; set; }
        [Column("cus_item_no")]
        [StringLength(30)]
        public string CusItemNo { get; set; }
        [Column("item_desc_1")]
        [StringLength(30)]
        public string item_desc_1 { get; set; }
        [Column("item_desc_2")]
        [StringLength(30)]
        public string item_desc_2 { get; set; }
        [Column("oe_unit_price", TypeName = "decimal(16, 6)")]
        public decimal? oe_unit_price { get; set; }
        [Column("discount_pct", TypeName = "decimal(5, 2)")]
        public decimal? DiscountPct { get; set; }
        [Column("rma_qty_rtn_auth", TypeName = "decimal(13, 4)")]
        public decimal? rma_qty_rtn_auth { get; set; }
        [Column("rma_qty_rtn_actual", TypeName = "decimal(13, 4)")]
        public decimal? RmaQtyRtnActual { get; set; }
        [Column("uom")]
        [StringLength(2)]
        public string uom { get; set; }
        [Column("uom_ratio", TypeName = "decimal(11, 6)")]
        public decimal? UomRatio { get; set; }
        [Column("oe_unit_cost", TypeName = "decimal(16, 6)")]
        public decimal? oe_unit_cost { get; set; }
        [Column("oe_unit_weight", TypeName = "decimal(13, 6)")]
        public decimal? OeUnitWeight { get; set; }
        [Column("comm_calc_type")]
        [StringLength(1)]
        public string CommCalcType { get; set; }
        [Column("tax_fg")]
        [StringLength(1)]
        public string tax_fg { get; set; }
        [Column("oe_bin_fg")]
        [StringLength(1)]
        public string OeBinFg { get; set; }
        [Column("oe_ser_lot_cd")]
        [StringLength(1)]
        public string OeSerLotCd { get; set; }
        [Column("oe_prod_cat")]
        [StringLength(3)]
        public string OeProdCat { get; set; }
        [Column("user_field_1")]
        [StringLength(30)]
        public string UserField1 { get; set; }
        [Column("user_field_2")]
        [StringLength(30)]
        public string UserField2 { get; set; }
        [Column("user_field_3")]
        [StringLength(30)]
        public string UserField3 { get; set; }
        [Column("user_field_4")]
        [StringLength(30)]
        public string UserField4 { get; set; }
        [Column("user_field_5")]
        [StringLength(30)]
        public string UserField5 { get; set; }
        [Column("oe_cus_no")]
        [StringLength(20)]
        public string oe_cus_no { get; set; }
        [Column("oe_unique_seq")]
        public short oe_unique_seq { get; set; }
        [Column("oe_mfg_method")]
        [StringLength(2)]
        public string OeMfgMethod { get; set; }
        [Column("action")]
        [StringLength(1)]
        public string action { get; set; }
        [Column("create_cm_fg")]
        [StringLength(1)]
        public string CreateCmFg { get; set; }
        [Column("oe_cm_created_fg")]
        [StringLength(1)]
        public string OeCmCreatedFg { get; set; }
        [Column("create_oe_fg")]
        [StringLength(1)]
        public string CreateOeFg { get; set; }
        [Column("oe_ord_created_fg")]
        [StringLength(1)]
        public string OeOrdCreatedFg { get; set; }
        [Required]
        [Column("oe_ord_no")]
        [StringLength(8)]
        public string oe_ord_no { get; set; }
        [Column("oe_unique_seq_no")]
        public short oe_unique_seq_no { get; set; }
        [Column("line_user_name")]
        [StringLength(20)]
        public string LineUserName { get; set; }
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
        [Column("price_modified")]
        [StringLength(1)]
        public string PriceModified { get; set; }
        [Column("filler_0001")]
        [StringLength(141)]
        public string Filler0001 { get; set; }
      
    }
}
