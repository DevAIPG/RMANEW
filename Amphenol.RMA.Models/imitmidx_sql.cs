
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amphenol.RMA.Models
{


    public class imitmidx_sql
    {
        [Key]

        [Required]
        [Column("item_no")]
        [StringLength(30)]
        public string item_no { get; set; }
        [Required]
        [Column("search_desc")]
        [StringLength(30)]
        public string SearchDesc { get; set; }
        [Column("item_desc_1")]
        [StringLength(30)]
        public string item_desc_1 { get; set; }
        [Column("item_desc_2")]
        [StringLength(30)]
        public string item_desc_2 { get; set; }
        [Column("prod_cat")]
        [StringLength(3)]
        public string prod_cat { get; set; }
        [Column("loc")]
        [StringLength(3)]
        public string loc { get; set; }
        [Column("uom")]
        [StringLength(2)]
        public string uom { get; set; }
        [Column("price_uom")]
        [StringLength(2)]
        public string PriceUom { get; set; }
        [Column("price_ratio", TypeName = "decimal(11, 6)")]
        public decimal? PriceRatio { get; set; }
        [Column("pur_uom")]
        [StringLength(2)]
        public string PurUom { get; set; }
        [Column("pur_to_inv_ratio", TypeName = "decimal(11, 6)")]
        public decimal? PurToInvRatio { get; set; }
        [Column("mfg_uom")]
        [StringLength(2)]
        public string MfgUom { get; set; }
        [Column("mfg_to_inv_ratio", TypeName = "decimal(11, 6)")]
        public decimal? MfgToInvRatio { get; set; }
        [Column("item_weight_uom")]
        [StringLength(2)]
        public string ItemWeightUom { get; set; }
        [Column("item_weight", TypeName = "decimal(13, 6)")]
        public decimal? ItemWeight { get; set; }
        [Column("yield_pct", TypeName = "decimal(4, 3)")]
        public decimal? YieldPct { get; set; }
        [Column("bkord_fg")]
        [StringLength(1)]
        public string BkordFg { get; set; }
        [Column("tax_fg")]
        [StringLength(1)]
        public string TaxFg { get; set; }
        [Column("end_item_cd")]
        [StringLength(1)]
        public string EndItemCd { get; set; }
        [Column("kit_feat_fg")]
        [StringLength(1)]
        public string KitFeatFg { get; set; }
        [Column("kit_prc_rollup")]
        [StringLength(1)]
        public string KitPrcRollup { get; set; }
        [Column("feature_prc_opt")]
        [StringLength(1)]
        public string FeaturePrcOpt { get; set; }
        [Column("kit_cst_rollup")]
        [StringLength(1)]
        public string KitCstRollup { get; set; }
        [Column("mat_cost_type")]
        [StringLength(3)]
        public string MatCostType { get; set; }
        [Column("p_and_ic_cd")]
        [StringLength(3)]
        public string PAndIcCd { get; set; }
        [Column("activity_cd")]
        [StringLength(1)]
        public string activity_cd { get; set; }
        [Column("activity_dt", TypeName = "datetime")]
        public DateTime? ActivityDt { get; set; }
        [Column("stocked_fg")]
        [StringLength(1)]
        public string StockedFg { get; set; }
        [Column("controlled_fg")]
        [StringLength(1)]
        public string ControlledFg { get; set; }
        [Column("pur_or_mfg")]
        [StringLength(1)]
        public string pur_or_mfg { get; set; }
        [Column("ms_item_fg")]
        [StringLength(1)]
        public string MsItemFg { get; set; }
        [Column("commodity_cd")]
        [StringLength(4)]
        public string CommodityCd { get; set; }
        [Column("byr_plnr")]
        public int? ByrPlnr { get; set; }
        [Column("mrp_stat_item")]
        [StringLength(1)]
        public string MrpStatItem { get; set; }
        [Column("last_item_revision")]
        [StringLength(8)]
        public string last_item_revision { get; set; }
        [Column("lec_revision")]
        [StringLength(8)]
        public string LecRevision { get; set; }
        [Column("lec_dt", TypeName = "datetime")]
        public DateTime? LecDt { get; set; }
        [Column("consumed_fg")]
        [StringLength(1)]
        public string ConsumedFg { get; set; }
        [Column("mrp_min_ord", TypeName = "decimal(13, 4)")]
        public decimal? MrpMinOrd { get; set; }
        [Column("mrp_safety_stk", TypeName = "decimal(13, 4)")]
        public decimal? MrpSafetyStk { get; set; }
        [Column("mrp_ord_up_to", TypeName = "decimal(13, 4)")]
        public decimal? MrpOrdUpTo { get; set; }
        [Column("planning_lead_tm", TypeName = "decimal(4, 1)")]
        public decimal? PlanningLeadTm { get; set; }
        [Column("fix_var_ld_tm_fac", TypeName = "decimal(4, 3)")]
        public decimal? FixVarLdTmFac { get; set; }
        [Column("approved_vend_req")]
        [StringLength(1)]
        public string ApprovedVendReq { get; set; }
        [Column("drawing_release_no")]
        [StringLength(8)]
        public string DrawingReleaseNo { get; set; }
        [Column("drawing_revision_no")]
        [StringLength(8)]
        public string drawing_revision_no { get; set; }
        [Column("rtg_release_no")]
        [StringLength(8)]
        public string RtgReleaseNo { get; set; }
        [Column("rtg_revision_no")]
        [StringLength(8)]
        public string RtgRevisionNo { get; set; }
        [Column("rtg_no")]
        [StringLength(8)]
        public string RtgNo { get; set; }
        [Column("ord_policy_cd")]
        [StringLength(1)]
        public string OrdPolicyCd { get; set; }
        [Column("planning_prd")]
        public short? PlanningPrd { get; set; }
        [Column("planning_ord_mult")]
        public short? PlanningOrdMult { get; set; }
        [Column("mrp_tm_fence_shop")]
        public short? MrpTmFenceShop { get; set; }
        [Column("lot_size", TypeName = "decimal(13, 4)")]
        public decimal? LotSize { get; set; }
        [Column("stk_stat_cd")]
        [StringLength(1)]
        public string StkStatCd { get; set; }
        [Column("low_lvl_cd")]
        public short? LowLvlCd { get; set; }
        [Column("comm_pct_amt", TypeName = "decimal(8, 2)")]
        public decimal? CommPctAmt { get; set; }
        [Column("calc_comm_tp")]
        [StringLength(1)]
        public string CalcCommTp { get; set; }
        [Column("ser_lot_fg")]
        [StringLength(1)]
        public string SerLotFg { get; set; }
        [Column("cad_drawing_name")]
        [StringLength(8)]
        public string CadDrawingName { get; set; }
        [Column("shelf_life_days")]
        public short? ShelfLifeDays { get; set; }
        [Column("ser_warranty_days")]
        public short? SerWarrantyDays { get; set; }
        [Column("inspection_cd")]
        [StringLength(1)]
        public string InspectionCd { get; set; }
        [Column("qty_pct_to_inspect", TypeName = "decimal(13, 4)")]
        public decimal? QtyPctToInspect { get; set; }
        [Required]
        [Column("upc_cd")]
        [StringLength(16)]
        public string UpcCd { get; set; }
        [Column("trx_aud_fg")]
        [StringLength(1)]
        public string TrxAudFg { get; set; }
        [Column("group_tech_cd")]
        [StringLength(8)]
        public string GroupTechCd { get; set; }
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
        [Column("item_note_1")]
        [StringLength(30)]
        public string ItemNote1 { get; set; }
        [Column("item_note_2")]
        [StringLength(30)]
        public string ItemNote2 { get; set; }
        [Column("item_note_3")]
        [StringLength(30)]
        public string ItemNote3 { get; set; }
        [Column("item_note_4")]
        [StringLength(30)]
        public string item_note_4 { get; set; }
        [Column("item_note_5")]
        [StringLength(30)]
        public string ItemNote5 { get; set; }
        [Column("user_dt", TypeName = "datetime")]
        public DateTime? UserDt { get; set; }
        [Column("user_amt", TypeName = "decimal(16, 4)")]
        public decimal? UserAmt { get; set; }
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
        [Column("user_def_cd")]
        [StringLength(2)]
        public string UserDefCd { get; set; }
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
        [Column("mfg_method")]
        [StringLength(2)]
        public string MfgMethod { get; set; }
        [Column("forced_demand")]
        [StringLength(1)]
        public string ForcedDemand { get; set; }
        [Column("var_lead_tm", TypeName = "decimal(13, 0)")]
        public decimal? VarLeadTm { get; set; }
        [Column("comm_cd")]
        [StringLength(8)]
        public string CommCd { get; set; }
        [Column("country_origin")]
        [StringLength(2)]
        public string CountryOrigin { get; set; }
        [Column("web_item")]
        [StringLength(1)]
        public string WebItem { get; set; }
        [Column("po_req_fg")]
        [StringLength(1)]
        public string PoReqFg { get; set; }
        [Column("po_req_consolidate")]
        [StringLength(1)]
        public string PoReqConsolidate { get; set; }
        [Column("create_po")]
        [StringLength(1)]
        public string CreatePo { get; set; }
        [Column("bol_code")]
        [StringLength(2)]
        public string BolCode { get; set; }
        [Column("sl_static_length")]
        public byte? SlStaticLength { get; set; }
        [Column("sl_static_value")]
        [StringLength(15)]
        public string SlStaticValue { get; set; }
        [Column("sl_numeric_length")]
        public byte? SlNumericLength { get; set; }
        [Column("sl_numeric_value", TypeName = "decimal(12, 0)")]
        public decimal? SlNumericValue { get; set; }
        [Column("default_rec_loc")]
        [StringLength(3)]
        public string DefaultRecLoc { get; set; }
        [Column("force_ecm_fg")]
        [StringLength(1)]
        public string ForceEcmFg { get; set; }
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
        [Column("filler_0002")]
        [StringLength(150)]
        public string Filler0002 { get; set; }
        [Column("ID", TypeName = "numeric(9, 0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }







    }
}
