using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    
    public class iminvloc_sql
    {
       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID", TypeName = "numeric(9, 0)")]
        public decimal Id { get; set; }


        [Required]
        [Column("item_no")]
        [StringLength(30)]
        public string ItemNo { get; set; }
        [Required]
        [Column("loc")]
        [StringLength(3)]
        public string Loc { get; set; }
        [Column("status")]
        [StringLength(1)]
        public string Status { get; set; }
        [Column("prev_status")]
        [StringLength(1)]
        public string PrevStatus { get; set; }
        [Column("mult_bin_fg")]
        [StringLength(1)]
        public string MultBinFg { get; set; }
        [Column("qty_on_hand", TypeName = "decimal(13, 4)")]
        public decimal? QtyOnHand { get; set; }
        [Column("qty_allocated", TypeName = "decimal(13, 4)")]
        public decimal? QtyAllocated { get; set; }
        [Column("qty_bkord", TypeName = "decimal(13, 4)")]
        public decimal? QtyBkord { get; set; }
        [Column("qty_on_ord", TypeName = "decimal(13, 4)")]
        public decimal? QtyOnOrd { get; set; }
        [Column("reorder_lvl", TypeName = "decimal(13, 4)")]
        public decimal? ReorderLvl { get; set; }
        [Column("ord_up_to_lvl", TypeName = "decimal(13, 4)")]
        public decimal? OrdUpToLvl { get; set; }
        [Column("price", TypeName = "decimal(16, 6)")]
        public decimal? price { get; set; }
        [Column("avg_cost", TypeName = "decimal(16, 6)")]
        public decimal? AvgCost { get; set; }
        [Column("last_cost", TypeName = "decimal(16, 6)")]
        public decimal? LastCost { get; set; }
        [Column("std_cost", TypeName = "decimal(16, 6)")]
        public decimal? std_cost { get; set; }
        [Column("prices_apply_flag")]
        [StringLength(1)]
        public string PricesApplyFlag { get; set; }
        [Column("discs_apply_fg")]
        [StringLength(1)]
        public string DiscsApplyFg { get; set; }
        [Column("starting_sls_dt", TypeName = "datetime")]
        public DateTime? StartingSlsDt { get; set; }
        [Column("ending_sls_dt", TypeName = "datetime")]
        public DateTime? EndingSlsDt { get; set; }
        [Column("last_sold_dt", TypeName = "datetime")]
        public DateTime? LastSoldDt { get; set; }
        [Column("sls_price", TypeName = "decimal(16, 6)")]
        public decimal? SlsPrice { get; set; }
        [Column("qty_last_sold", TypeName = "decimal(13, 4)")]
        public decimal? QtyLastSold { get; set; }
        [Column("cycle_count_cd")]
        [StringLength(1)]
        public string CycleCountCd { get; set; }
        [Column("last_count_dt", TypeName = "datetime")]
        public DateTime? LastCountDt { get; set; }
        [Column("tms_cntd_ytd")]
        public short? TmsCntdYtd { get; set; }
        [Column("pct_err_last_cnt", TypeName = "decimal(13, 6)")]
        public decimal? PctErrLastCnt { get; set; }
        [Column("frz_cost", TypeName = "decimal(16, 6)")]
        public decimal? FrzCost { get; set; }
        [Column("frz_qty", TypeName = "decimal(13, 4)")]
        public decimal? FrzQty { get; set; }
        [Column("frz_dt", TypeName = "datetime")]
        public DateTime? FrzDt { get; set; }
        [Column("frz_tm", TypeName = "datetime")]
        public DateTime? FrzTm { get; set; }
        [Column("usage_ptd", TypeName = "decimal(13, 4)")]
        public decimal? UsagePtd { get; set; }
        [Column("qty_sld_ptd", TypeName = "decimal(13, 4)")]
        public decimal? QtySldPtd { get; set; }
        [Column("qty_scrp_ptd", TypeName = "decimal(13, 4)")]
        public decimal? QtyScrpPtd { get; set; }
        [Column("sls_ptd", TypeName = "decimal(16, 2)")]
        public decimal? SlsPtd { get; set; }
        [Column("cost_ptd", TypeName = "decimal(16, 2)")]
        public decimal? CostPtd { get; set; }
        [Column("usage_ytd", TypeName = "decimal(13, 4)")]
        public decimal? UsageYtd { get; set; }
        [Column("qty_sold_ytd", TypeName = "decimal(13, 4)")]
        public decimal? QtySoldYtd { get; set; }
        [Column("qty_scrp_ytd", TypeName = "decimal(13, 4)")]
        public decimal? QtyScrpYtd { get; set; }
        [Column("qty_returned_ytd", TypeName = "decimal(13, 4)")]
        public decimal? QtyReturnedYtd { get; set; }
        [Column("sls_ytd", TypeName = "decimal(16, 2)")]
        public decimal? SlsYtd { get; set; }
        [Column("cost_ytd", TypeName = "decimal(16, 2)")]
        public decimal? CostYtd { get; set; }
        [Column("prior_year_usage", TypeName = "decimal(13, 4)")]
        public decimal? PriorYearUsage { get; set; }
        [Column("qty_sold_last_yr", TypeName = "decimal(13, 4)")]
        public decimal? QtySoldLastYr { get; set; }
        [Column("qty_scrp_last_yr", TypeName = "decimal(13, 4)")]
        public decimal? QtyScrpLastYr { get; set; }
        [Column("prior_year_sls", TypeName = "decimal(16, 2)")]
        public decimal? PriorYearSls { get; set; }
        [Column("cost_last_yr", TypeName = "decimal(16, 2)")]
        public decimal? CostLastYr { get; set; }
        [Column("recom_min_ord", TypeName = "decimal(13, 4)")]
        public decimal? RecomMinOrd { get; set; }
        [Column("economic_ord_qty", TypeName = "decimal(13, 4)")]
        public decimal? EconomicOrdQty { get; set; }
        [Column("avg_usage", TypeName = "decimal(13, 4)")]
        public decimal? AvgUsage { get; set; }
        [Column("po_lead_tm")]
        public short? PoLeadTm { get; set; }
        [Column("byr_plnr")]
        public int? ByrPlnr { get; set; }
        [Column("doc_to_stk_ld_tm")]
        public short? DocToStkLdTm { get; set; }
        [Column("rollup_prc")]
        [StringLength(1)]
        public string RollupPrc { get; set; }
        [Column("target_margin")]
        public short? TargetMargin { get; set; }
        [Column("inv_class")]
        [StringLength(1)]
        public string InvClass { get; set; }
        [Column("po_min", TypeName = "decimal(13, 4)")]
        public decimal? PoMin { get; set; }
        [Column("po_max", TypeName = "decimal(13, 4)")]
        public decimal? PoMax { get; set; }
        [Column("safety_stk", TypeName = "decimal(13, 4)")]
        public decimal? SafetyStk { get; set; }
        [Column("avg_frcst_error", TypeName = "decimal(13, 4)")]
        public decimal? AvgFrcstError { get; set; }
        [Column("sum_of_errors", TypeName = "decimal(13, 4)")]
        public decimal? SumOfErrors { get; set; }
        [Column("usg_wght_fctr", TypeName = "decimal(9, 5)")]
        public decimal? UsgWghtFctr { get; set; }
        [Column("safety_fctr", TypeName = "decimal(9, 5)")]
        public decimal? SafetyFctr { get; set; }
        [Column("usage_filter", TypeName = "decimal(9, 5)")]
        public decimal? UsageFilter { get; set; }
        [Column("po_mult")]
        public int? PoMult { get; set; }
        [Column("active_ords")]
        public int? ActiveOrds { get; set; }
        [Column("vend_no")]
        [StringLength(20)]
        public string VendNo { get; set; }
        [Column("tax_sched")]
        [StringLength(5)]
        public string TaxSched { get; set; }
        [Required]
        [Column("prod_cat")]
        [StringLength(3)]
        public string ProdCat { get; set; }
        [Column("picking_seq")]
        [StringLength(8)]
        public string PickingSeq { get; set; }
        [Column("cube_width_uom")]
        [StringLength(2)]
        public string CubeWidthUom { get; set; }
        [Column("cube_length_uom")]
        [StringLength(2)]
        public string CubeLengthUom { get; set; }
        [Column("cube_height_uom")]
        [StringLength(2)]
        public string CubeHeightUom { get; set; }
        [Column("cube_width", TypeName = "decimal(9, 5)")]
        public decimal? CubeWidth { get; set; }
        [Column("cube_length", TypeName = "decimal(9, 5)")]
        public decimal? CubeLength { get; set; }
        [Column("cube_height", TypeName = "decimal(9, 5)")]
        public decimal? CubeHeight { get; set; }
        [Column("cube_qty_per", TypeName = "decimal(13, 4)")]
        public decimal? CubeQtyPer { get; set; }
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
        [Column("landed_cost_cd")]
        [StringLength(4)]
        public string LandedCostCd { get; set; }
        [Column("landed_cost_cd_2")]
        [StringLength(4)]
        public string LandedCostCd2 { get; set; }
        [Column("landed_cost_cd_3")]
        [StringLength(4)]
        public string LandedCostCd3 { get; set; }
        [Column("landed_cost_cd_4")]
        [StringLength(4)]
        public string LandedCostCd4 { get; set; }
        [Column("landed_cost_cd_5")]
        [StringLength(4)]
        public string LandedCostCd5 { get; set; }
        [Column("landed_cost_cd_6")]
        [StringLength(4)]
        public string LandedCostCd6 { get; set; }
        [Column("landed_cost_cd_7")]
        [StringLength(4)]
        public string LandedCostCd7 { get; set; }
        [Column("landed_cost_cd_8")]
        [StringLength(4)]
        public string LandedCostCd8 { get; set; }
        [Column("landed_cost_cd_9")]
        [StringLength(4)]
        public string LandedCostCd9 { get; set; }
        [Column("landed_cost_cd_10")]
        [StringLength(4)]
        public string LandedCostCd10 { get; set; }
        [Column("loc_qty_fld", TypeName = "decimal(13, 4)")]
        public decimal? LocQtyFld { get; set; }
        [Column("tag_qty", TypeName = "decimal(13, 4)")]
        public decimal? TagQty { get; set; }
        [Column("tag_cost", TypeName = "decimal(16, 6)")]
        public decimal? TagCost { get; set; }
        [Column("tag_frz_dt", TypeName = "datetime")]
        public DateTime? TagFrzDt { get; set; }
        [Column("qty_reject_ptd", TypeName = "decimal(13, 4)")]
        public decimal? QtyRejectPtd { get; set; }
        [Column("qty_reject_ytd", TypeName = "decimal(13, 4)")]
        public decimal? QtyRejectYtd { get; set; }
        [Column("qty_reject_last_yr", TypeName = "decimal(13, 4)")]
        public decimal? QtyRejectLastYr { get; set; }
        [Column("include_par_cost", TypeName = "decimal(16, 6)")]
        public decimal? IncludeParCost { get; set; }
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
        [Column("qty_rtn_ptd", TypeName = "decimal(13, 4)")]
        public decimal? QtyRtnPtd { get; set; }
        [Column("qty_rtn_lyr", TypeName = "decimal(13, 4)")]
        public decimal? QtyRtnLyr { get; set; }
        [Column("inv_loc_return_sales_ptd", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnSalesPtd { get; set; }
        [Column("inv_loc_return_cost_ptd", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnCostPtd { get; set; }
        [Column("inv_loc_return_sales_ytd", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnSalesYtd { get; set; }
        [Column("inv_loc_return_cost_ytd", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnCostYtd { get; set; }
        [Column("inv_loc_return_sales_lyr", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnSalesLyr { get; set; }
        [Column("inv_loc_return_cost_lyr", TypeName = "decimal(16, 2)")]
        public decimal? InvLocReturnCostLyr { get; set; }
        [Column("filler_0002")]
        [StringLength(82)]
        public string Filler0002 { get; set; }











    }
}
