using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class OELINHST_SQL
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
        [Column("line_seq_no")]
        public short LineSeqNo { get; set; }
        [Required]
        [Column("item_no")]
        [StringLength(30)]
        public string ItemNo { get; set; }
        [Required]
        [Column("loc")]
        [StringLength(3)]
        public string Loc { get; set; }
        [Column("pick_seq")]
        [StringLength(8)]
        public string PickSeq { get; set; }
        [Column("cus_item_no")]
        [StringLength(30)]
        public string CusItemNo { get; set; }
        [Column("item_desc_1")]
        [StringLength(30)]
        public string ItemDesc1 { get; set; }
        [Column("item_desc_2")]
        [StringLength(30)]
        public string ItemDesc2 { get; set; }
        [Column("qty_ordered", TypeName = "decimal(13, 4)")]
        public decimal? QtyOrdered { get; set; }
        [Column("qty_to_ship", TypeName = "decimal(13, 4)")]
        public decimal? QtyToShip { get; set; }
        [Column("unit_price", TypeName = "decimal(16, 6)")]
        public decimal? UnitPrice { get; set; }
        [Column("discount_pct", TypeName = "decimal(5, 2)")]
        public decimal? DiscountPct { get; set; }
        [Column("request_dt", TypeName = "datetime")]
        public DateTime? RequestDt { get; set; }
        [Column("qty_bkord", TypeName = "decimal(13, 4)")]
        public decimal? QtyBkord { get; set; }
        [Column("qty_return_to_stk", TypeName = "decimal(13, 4)")]
        public decimal? QtyReturnToStk { get; set; }
        [Column("bkord_fg")]
        [StringLength(1)]
        public string BkordFg { get; set; }
        [Column("uom")]
        [StringLength(2)]
        public string Uom { get; set; }
        [Column("uom_ratio", TypeName = "decimal(11, 6)")]
        public decimal? UomRatio { get; set; }
        [Column("unit_cost", TypeName = "decimal(16, 6)")]
        public decimal? UnitCost { get; set; }
        [Column("unit_weight", TypeName = "decimal(13, 6)")]
        public decimal? UnitWeight { get; set; }
        [Column("comm_calc_type")]
        [StringLength(1)]
        public string CommCalcType { get; set; }
        [Column("comm_pct_or_amt", TypeName = "decimal(7, 2)")]
        public decimal? CommPctOrAmt { get; set; }
        [Column("promise_dt", TypeName = "datetime")]
        public DateTime? PromiseDt { get; set; }
        [Column("tax_fg")]
        [StringLength(1)]
        public string TaxFg { get; set; }
        [Column("stocked_fg")]
        [StringLength(1)]
        public string StockedFg { get; set; }
        [Column("controlled_fg")]
        [StringLength(1)]
        public string ControlledFg { get; set; }
        [Column("select_cd")]
        [StringLength(1)]
        public string SelectCd { get; set; }
        [Column("tot_qty_ordered", TypeName = "decimal(13, 4)")]
        public decimal? TotQtyOrdered { get; set; }
        [Column("tot_qty_shipped", TypeName = "decimal(13, 4)")]
        public decimal? TotQtyShipped { get; set; }
        [Column("tax_fg_1")]
        [StringLength(1)]
        public string TaxFg1 { get; set; }
        [Column("tax_fg_2")]
        [StringLength(1)]
        public string TaxFg2 { get; set; }
        [Column("tax_fg_3")]
        [StringLength(1)]
        public string TaxFg3 { get; set; }
        [Column("orig_price", TypeName = "decimal(16, 6)")]
        public decimal? OrigPrice { get; set; }
        [Column("copy_to_bm_fg")]
        [StringLength(1)]
        public string CopyToBmFg { get; set; }
        [Column("explode_kit")]
        [StringLength(1)]
        public string ExplodeKit { get; set; }
        [Column("mfg_ord_no")]
        [StringLength(8)]
        public string MfgOrdNo { get; set; }
        [Column("allocate_dt", TypeName = "datetime")]
        public DateTime? AllocateDt { get; set; }
        [Column("last_post_dt", TypeName = "datetime")]
        public DateTime? LastPostDt { get; set; }
        [Column("post_to_inv_qty", TypeName = "decimal(13, 4)")]
        public decimal? PostToInvQty { get; set; }
        [Column("posted_to_inv", TypeName = "decimal(13, 4)")]
        public decimal? PostedToInv { get; set; }
        [Column("tot_qty_posted", TypeName = "decimal(13, 4)")]
        public decimal? TotQtyPosted { get; set; }
        [Column("qty_allocated", TypeName = "decimal(13, 4)")]
        public decimal? QtyAllocated { get; set; }
        [Column("components_alloc", TypeName = "decimal(13, 4)")]
        public decimal? ComponentsAlloc { get; set; }
        [Column("bin_fg")]
        [StringLength(1)]
        public string BinFg { get; set; }
        [Column("cost_meth")]
        [StringLength(1)]
        public string CostMeth { get; set; }
        [Column("ser_lot_cd")]
        [StringLength(1)]
        public string SerLotCd { get; set; }
        [Column("mult_ftr_fg")]
        [StringLength(1)]
        public string MultFtrFg { get; set; }
        [Column("line_type")]
        [StringLength(1)]
        public string LineType { get; set; }
        [Column("prod_cat")]
        [StringLength(3)]
        public string ProdCat { get; set; }
        [Column("end_item_cd")]
        [StringLength(1)]
        public string EndItemCd { get; set; }
        [Column("reason_cd")]
        [StringLength(3)]
        public string ReasonCd { get; set; }
        [Column("feature_return")]
        [StringLength(1)]
        public string FeatureReturn { get; set; }
        [Column("rec_inspection")]
        [StringLength(1)]
        public string RecInspection { get; set; }
        [Column("ship_from_stk")]
        [StringLength(1)]
        public string ShipFromStk { get; set; }
        [Column("mult_release")]
        [StringLength(1)]
        public string MultRelease { get; set; }
        [Column("req_ship_dt", TypeName = "datetime")]
        public DateTime? ReqShipDt { get; set; }
        [Column("qty_from_stk", TypeName = "decimal(13, 4)")]
        public decimal? QtyFromStk { get; set; }
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
        [Column("picked_dt", TypeName = "datetime")]
        public DateTime? PickedDt { get; set; }
        [Column("shipped_dt", TypeName = "datetime")]
        public DateTime? ShippedDt { get; set; }
        [Column("billed_dt", TypeName = "datetime")]
        public DateTime? BilledDt { get; set; }
        [Column("update_fg")]
        [StringLength(1)]
        public string UpdateFg { get; set; }
        [Column("prc_cd_orig_price", TypeName = "decimal(16, 6)")]
        public decimal? PrcCdOrigPrice { get; set; }
        [Column("tax_sched")]
        [StringLength(5)]
        public string TaxSched { get; set; }
        [Required]
        [Column("cus_no")]
        [StringLength(20)]
        public string CusNo { get; set; }
        [Column("tax_amt", TypeName = "decimal(16, 2)")]
        public decimal? TaxAmt { get; set; }
        [Column("qty_bkord_fg")]
        [StringLength(1)]
        public string QtyBkordFg { get; set; }
        [Column("line_no")]
        public short? LineNo { get; set; }
        [Column("mfg_method")]
        [StringLength(2)]
        public string MfgMethod { get; set; }
        [Column("forced_demand")]
        [StringLength(1)]
        public string ForcedDemand { get; set; }
        [Column("conf_pick_dt", TypeName = "datetime")]
        public DateTime? ConfPickDt { get; set; }
        [Column("item_release_no")]
        [StringLength(8)]
        public string ItemReleaseNo { get; set; }
        [Column("bin_ser_lot_comp")]
        [StringLength(1)]
        public string BinSerLotComp { get; set; }
        [Column("offset_used_fg")]
        [StringLength(1)]
        public string OffsetUsedFg { get; set; }
        [Column("ecs_space")]
        [StringLength(20)]
        public string EcsSpace { get; set; }
        [Column("sfc_order_status")]
        [StringLength(1)]
        public string SfcOrderStatus { get; set; }
        [Column("total_cost", TypeName = "decimal(16, 2)")]
        public decimal? TotalCost { get; set; }
        [Column("po_ord_no")]
        [StringLength(8)]
        public string PoOrdNo { get; set; }
        [Column("rma_seq")]
        public short? RmaSeq { get; set; }
        [Column("vendor_no")]
        [StringLength(20)]
        public string VendorNo { get; set; }
        [Column("posted_unit_cost", TypeName = "decimal(16, 6)")]
        public decimal? PostedUnitCost { get; set; }
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
        [Column("warranty_dt", TypeName = "datetime")]
        public DateTime? WarrantyDt { get; set; }
        [Column("revision_no")]
        [StringLength(8)]
        public string RevisionNo { get; set; }
        [Column("cm_post_flag")]
        [StringLength(1)]
        public string CmPostFlag { get; set; }
        [Required]
        [Column("recalc_sw")]
        [StringLength(1)]
        public string RecalcSw { get; set; }
        [Column("filler_0001")]
        [StringLength(132)]
        public string Filler0001 { get; set; }
        [Column("hst_dt", TypeName = "datetime")]
        public DateTime? HstDt { get; set; }
        [Column("hst_tm", TypeName = "datetime")]
        public DateTime? HstTm { get; set; }
        [Column("user_name")]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [Column("id_no")]
        [StringLength(50)]
        public string IdNo { get; set; }
        [Column("jnl_cd")]
        [StringLength(6)]
        public string JnlCd { get; set; }
        [Column("batch_id")]
        [StringLength(10)]
        public string BatchId { get; set; }
        [Required]
        [Column("inv_no")]
        [StringLength(8)]
        public string InvNo { get; set; }
        [Column("sls_amt", TypeName = "decimal(16, 2)")]
        public decimal? SlsAmt { get; set; }
        [Column("cost_amt", TypeName = "decimal(16, 2)")]
        public decimal? CostAmt { get; set; }
       
        public Guid Uid { get; set; }

    }
}
