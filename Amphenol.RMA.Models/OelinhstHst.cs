using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models
{
    [Keyless]
    [Table("oelinhst_hst")]
    public partial class OelinhstHst
    {
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
        [StringLength(15)]
        public string ItemNo { get; set; }
        [Required]
        [Column("loc")]
        [StringLength(3)]
        public string Loc { get; set; }
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
        [Column("unit_price", TypeName = "decimal(13, 6)")]
        public decimal? UnitPrice { get; set; }
        [Column("discount_pct", TypeName = "decimal(5, 2)")]
        public decimal? DiscountPct { get; set; }
        [Column("request_dt")]
        public int? RequestDt { get; set; }
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
        [Column("uom_ratio", TypeName = "decimal(9, 5)")]
        public decimal? UomRatio { get; set; }
        [Column("unit_cost", TypeName = "decimal(13, 6)")]
        public decimal? UnitCost { get; set; }
        [Column("comm_calc_type")]
        [StringLength(1)]
        public string CommCalcType { get; set; }
        [Column("comm_pct_or_amt", TypeName = "decimal(7, 2)")]
        public decimal? CommPctOrAmt { get; set; }
        [Column("promise_dt")]
        public int? PromiseDt { get; set; }
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
        [Column("orig_price", TypeName = "decimal(13, 6)")]
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
        [Column("allocate_dt")]
        public int? AllocateDt { get; set; }
        [Column("last_post_dt")]
        public int? LastPostDt { get; set; }
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
        [Column("pur_or_mfg")]
        [StringLength(1)]
        public string PurOrMfg { get; set; }
        [Column("req_ship_dt")]
        public int? ReqShipDt { get; set; }
        [Column("qty_from_stk", TypeName = "decimal(13, 4)")]
        public decimal? QtyFromStk { get; set; }
        [Column("picked_dt")]
        public int? PickedDt { get; set; }
        [Column("shipped_dt")]
        public int? ShippedDt { get; set; }
        [Column("billed_dt")]
        public int? BilledDt { get; set; }
        [Column("update_fg")]
        [StringLength(1)]
        public string UpdateFg { get; set; }
        [Column("prc_cd_orig_price", TypeName = "decimal(13, 6)")]
        public decimal? PrcCdOrigPrice { get; set; }
        [Required]
        [Column("cus_no")]
        [StringLength(12)]
        public string CusNo { get; set; }
        [Column("qty_bkord_fg")]
        [StringLength(1)]
        public string QtyBkordFg { get; set; }
        [Column("line_no")]
        public short? LineNo { get; set; }
        [Column("mfg_method")]
        [StringLength(2)]
        public string MfgMethod { get; set; }
        [Column("conf_pick_dt")]
        public int? ConfPickDt { get; set; }
        [Column("ecs_space")]
        [StringLength(20)]
        public string EcsSpace { get; set; }
        [Column("total_cost", TypeName = "decimal(14, 2)")]
        public decimal? TotalCost { get; set; }
        [Column("po_ord_no")]
        [StringLength(8)]
        public string PoOrdNo { get; set; }
        [Column("hst_dt")]
        public int HstDt { get; set; }
        [Column("hst_tm")]
        public int? HstTm { get; set; }
        [Column("inv_no")]
        public string InvNo { get; set; }
        [Column("sls_amt", TypeName = "decimal(14, 2)")]
        public decimal? SlsAmt { get; set; }
        [Column("cost_amt", TypeName = "decimal(14, 2)")]
        public decimal? CostAmt { get; set; }
    }
}
