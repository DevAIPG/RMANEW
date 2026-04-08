using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class OERDTFIL_SQLRepository : Repository<OERDTFIL_SQL>, IOERDTFIL_SQLRepository
    {
        private readonly DbContext100 _db;
        private readonly DbContextM10 _db3;
        private readonly IConfiguration _configuration;

        public OERDTFIL_SQLRepository(DbContext100 db, DbContextM10 db3, IConfiguration configuration) : base(db)
        {
            _db = db;
            _db3 = db3;
            _configuration = configuration;
        }

        public string nuevorma()
        {
            var nextrma = (from c in _db.OERMACTL_SQL
                           where c.ID == 1
                           select c.ctl_next_order_no).First();


            int x = Int32.Parse(nextrma);

            string numString = x.ToString();
            x = x + 1;
            var numeroFormato = x.ToString("D8");



            var objDesdeDbz = _db.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

            objDesdeDbz.ctl_next_order_no = numeroFormato;

            _db.SaveChanges();

            return numString;
        }

        public async Task<bool> falloRMA(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc)
        {


            string retorno = "Approved";

            var objDesdeDbt = new OERHDFIL_SQL();




            var AccountTypeCode = "";
            var cicmpy = new Cicmpy();
            cicmpy = _db.Cicmpy.Where(a => a.CmpCode == rma.Customer).FirstOrDefault();
            AccountTypeCode = cicmpy.AccountTypeCode;




            var ArtypfilSql = new ArtypfilSql();
            ArtypfilSql = _db.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
            objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

            objDesdeDbt.dept = ArtypfilSql.SlsDpNo;


            var moneda = "";

            objDesdeDbt.orig_trx_rt = 1;

            objDesdeDbt.curr_trx_rt = 1;

            var arcusfil_sql = new arcusfil_sql();
            var oehdrhst_sql = new OEHDRHST_SQL();
            int oehdrhst_sqlcuantos = 0;
            try
            {


                foreach (var filtro in invoice)
                {

                    oehdrhst_sqlcuantos = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To) && a.InvNo == filtro).Count();

                    oehdrhst_sql = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To) && a.InvNo == filtro).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex,
     Color.Red);
            }

            if (oehdrhst_sqlcuantos == 0)
            {


                arcusfil_sql = _db.arcusfil_sql.Where(a => a.cus_no == rma.Customer).FirstOrDefault();

                moneda = arcusfil_sql.curr_cd;
                objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;


                objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
                objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;





                objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
                objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
                objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;




                var AraltadrSql = _db.AraltadrSql.Where(a => a.CusNo == rma.Customer && a.CusAltAdrCd.Contains(rma.Ship_To)).FirstOrDefault();
                objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();

                objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;

                objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;

                objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;

                objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;

                objDesdeDbt.tax_cd = AraltadrSql.TaxCd;

                objDesdeDbt.ship_to_country = AraltadrSql.Country;


                objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                objDesdeDbt.bill_to_country = AraltadrSql.Country;
                objDesdeDbt.ship_to_name = AraltadrSql.CusName;

                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();


                if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
                {
                    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }


            }
            else
            {


                objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                moneda = oehdrhst_sql.CurrCd;
                objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;





                if (moneda != "CNY")
                {
                    objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
                }
                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();
                if (oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "")
                {
                    objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }


                objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;

                objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;

                objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;

                objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
                objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;





                objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
                objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
                objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
                objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;


                objDesdeDbt.bill_to_name = oehdrhst_sql.CusNo;
                objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
                objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

            }
            if (_configuration.GetConnectionString("server") != "m10testna01")
            {

                if (moneda != "CNY")
                {

                    var rate = _db.Rate.Where(a => a.DateL == _db.Rate.Max(a => a.DateL) && a.SourceCurrency == moneda).FirstOrDefault();
                    objDesdeDbt.orig_trx_rt = (decimal?)rate.RateExchange;

                    objDesdeDbt.curr_trx_rt = (decimal?)rate.RateExchange;
                }

            }
            objDesdeDbt.contact = rma.Contact;
            objDesdeDbt.phone_no = rma.Phone;
            objDesdeDbt.fax_no = rma.Fax;
            objDesdeDbt.phone_ext = rma.Ext;
            objDesdeDbt.contact_email = rma.Email;
            objDesdeDbt.user_def_fld_5 = "Normal                        ";
            objDesdeDbt.deter_rate_by = "O";
            objDesdeDbt.form_no = 1;
            objDesdeDbt.rma_no = numString;
            objDesdeDbt.cus_no = cus;
            objDesdeDbt.slspsn_pct_comm = 100;
            objDesdeDbt.cus_ship_to = rma.Ship_To;
            objDesdeDbt.oe_po_no = rma.Customerpo;
            objDesdeDbt.rma_cmt = rma.Description;
            objDesdeDbt.status = "O";
            objDesdeDbt.SlspsnCommAmt = 0;
            objDesdeDbt.SlspsnNo2 = 0;
            objDesdeDbt.SlspsnPctComm2 = 0;
            objDesdeDbt.SlspsnCommAmt2 = 0;
            objDesdeDbt.SlspsnPctComm3 = 0;
            objDesdeDbt.SlspsnCommAmt3 = 0;
            objDesdeDbt.SlspsnNo3 = 0;
            objDesdeDbt.Extra10 = 0;
            objDesdeDbt.Extra11 = 0;
            objDesdeDbt.Extra12 = 0;
            objDesdeDbt.Extra13 = 0;
            objDesdeDbt.Extra14 = 0;
            objDesdeDbt.Extra15 = 0;
            objDesdeDbt.TaxPct = 0;
            objDesdeDbt.TaxPct2 = 0;
            objDesdeDbt.TaxPct3 = 0;
            objDesdeDbt.DiscountPct = 0;
            objDesdeDbt.TotSlsAmt = 0;
            objDesdeDbt.TotSlsDisc = 0;
            objDesdeDbt.TotTaxAmt = 0;
            objDesdeDbt.TotCost = 0;
            objDesdeDbt.TotWeight = 0;
            objDesdeDbt.SlsTaxAmt1 = 0;
            objDesdeDbt.SlsTaxAmt2 = 0;
            objDesdeDbt.SlsTaxAmt3 = 0;
            objDesdeDbt.CommPct = 0;
            objDesdeDbt.CommAmt = 0;
            objDesdeDbt.AccumMiscAmt = 0;
            objDesdeDbt.AccumFrtAmt = 0;
            objDesdeDbt.AccumTotTaxAmt = 0;
            objDesdeDbt.AccumSlsTaxAmt = 0;
            objDesdeDbt.AccumTotSlsAmt = 0;
            objDesdeDbt.TotTaxCost = 0;
            objDesdeDbt.TotDollars = 0;
            objDesdeDbt.TaxFg = "N";

            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
            objDesdeDbt.rma_dt_entered = date;
            objDesdeDbt.LastActDt = date;
            objDesdeDbt.ExpRecDate = date;
            _db.OERHDFIL_SQL.Add(objDesdeDbt);

            _db.SaveChanges();
            var objDesdeDb8 = new CSEXSW_Rma();


            objDesdeDb8.Approver = rma.Approver;
            objDesdeDb8.Contact = rma.Contact;
            objDesdeDb8.Email = rma.Email;

            objDesdeDb8.Ext = rma.Ext;
            objDesdeDb8.Company = rma.Company;
            objDesdeDb8.Fax = rma.Fax;
            objDesdeDb8.Phone = rma.Phone;



            objDesdeDb8.Wherebuilt = rma.Wherebuilt;
            objDesdeDb8.Ship_To = rma.Ship_To;
            objDesdeDb8.Customercomplait = rma.Customercomplait;
            objDesdeDb8.Customerpartno = rma.Customerpartno;
            objDesdeDb8.Customerpo = rma.Customerpo;
            objDesdeDb8.Date = rma.Date;
            objDesdeDb8.Description = rma.Description;
            objDesdeDb8.Preparado = rma.Preparado;
            objDesdeDb8.RMA500 = rma.RMA500;
            objDesdeDb8.Sumbit = rma.Sumbit;
            objDesdeDb8.Rmarequest = rma.Rmarequest; 
            objDesdeDb8.Rmatypeofrequest = rma.Rmatypeofrequest;
            objDesdeDb8.Customer = rma.Customer;
            objDesdeDb8.turno = rma.turno;
            objDesdeDb8.Status = rma.Status;
            objDesdeDb8.Comment = rma.Comment;
            objDesdeDb8.Totalrmavalues = rma.Totalrmavalues;


            _db3.CSEXSW_Rma.Add(objDesdeDb8);

            _db3.SaveChanges();



            for (int i = 0; i < invoice.Length; i++)
            {


                if (invoice[i] == null)
                {
                    invoice[i] = "";
                }
                if (code[i] == null)
                {
                    code[i] = "";
                }

                var objDesdeDb = new csexsw_coustumer();
                objDesdeDb.Invoice = invoice[i];
                objDesdeDb.Qty = qty[i];
                objDesdeDb.Seq = seq[i];
                objDesdeDb.Loc = loc[i];
                objDesdeDb.Coustumer = coustumer[i];

                if (checkcar[i] == "true")
                {
                    objDesdeDb.Car = true;
                }
                else
                {
                    objDesdeDb.Car = false;
                }


                objDesdeDb.Retur = code[i];
                //if (objDesdeDb8.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDb.Action = "C";
                //}
                //else
                //{
                //    objDesdeDb.Action = "C";
                //}
                objDesdeDb.Action = "C";
                objDesdeDb.Cost = acttion[i];
                objDesdeDb.Unit = unit[i];
                objDesdeDb.RmaId = idRMA + 1;

                _db3.csexsw_coustumer.Add(objDesdeDb);

                _db3.SaveChanges();
            }


            Int16 seqq = 1;

            for (int i = 0; i < invoice.Length; i++)
            {





                int total2 = _db.iminvloc_sql.Where(a => a.ItemNo == coustumer[i] && a.Loc == loc[i]).Count();
                if (total2 == 0)
                {
                    var locprincipal = _db.imitmidx_sql.Where(a => a.item_no == coustumer[i]).Select(s => s.loc).FirstOrDefault().ToString();

                    var result = _db.iminvloc_sql.FirstOrDefault(a => a.ItemNo == coustumer[i] && a.Loc == locprincipal);

                    var objDesdeDb5 = new iminvloc_sql();


                    objDesdeDb5.ActiveOrds = 0;
                    objDesdeDb5.AvgCost = 0;
                    objDesdeDb5.AvgFrcstError = 0;
                    objDesdeDb5.AvgUsage = 0;
                    objDesdeDb5.InvClass = result.InvClass;
                    objDesdeDb5.ByrPlnr = result.ByrPlnr;
                    objDesdeDb5.CostLastYr = 0;
                    objDesdeDb5.CostPtd = 0;
                    objDesdeDb5.CostYtd = 0;
                    objDesdeDb5.CubeHeight = 0;
                    objDesdeDb5.CubeLength = 0;
                    objDesdeDb5.CubeQtyPer = 0;
                    objDesdeDb5.CubeWidth = 0;
                    //objDesdeDb5.DocField1 = 0;
                    //objDesdeDb5.DocField2 = 0;
                    //objDesdeDb5.DocField3 = 0;
                    objDesdeDb5.DocToStkLdTm = 0;
                    objDesdeDb5.EconomicOrdQty = 0;

                    objDesdeDb5.Extra10 = 0;
                    objDesdeDb5.Extra11 = 0;
                    objDesdeDb5.Extra12 = 0;
                    objDesdeDb5.Extra13 = 0;
                    objDesdeDb5.Extra14 = 0;
                    objDesdeDb5.Extra15 = 0;

                    objDesdeDb5.FrzCost = 0;
                    objDesdeDb5.FrzQty = 0;
                    objDesdeDb5.IncludeParCost = 0;
                    objDesdeDb5.InvLocReturnCostLyr = 0;

                    objDesdeDb5.InvLocReturnCostPtd = 0;

                    objDesdeDb5.InvLocReturnCostYtd = 0;
                    objDesdeDb5.InvLocReturnSalesLyr = 0;
                    objDesdeDb5.InvLocReturnSalesPtd = 0;
                    objDesdeDb5.InvLocReturnSalesYtd = 0;

                    objDesdeDb5.LastCost = 0;
                    objDesdeDb5.LocQtyFld = 0;

                    objDesdeDb5.OrdUpToLvl = 0;
                    objDesdeDb5.PctErrLastCnt = 0;
                    objDesdeDb5.PoLeadTm = 0;
                    objDesdeDb5.PoMax = 0;
                    objDesdeDb5.PoMin = 0;
                    //objDesdeDb5.PoMult = 0;
                    objDesdeDb5.PriorYearSls = 0;
                    objDesdeDb5.PriorYearUsage = 0;
                    objDesdeDb5.QtyAllocated = 0;

                    objDesdeDb5.QtyBkord = 0;
                    objDesdeDb5.QtyLastSold = 0;
                    objDesdeDb5.QtyOnHand = 0;
                    objDesdeDb5.QtyOnOrd = 0;
                    objDesdeDb5.QtyRejectLastYr = 0;
                    objDesdeDb5.QtyRejectPtd = 0;
                    objDesdeDb5.QtyRejectYtd = 0;

                    objDesdeDb5.QtyReturnedYtd = 0;
                    objDesdeDb5.QtyRtnLyr = 0;
                    objDesdeDb5.QtyRtnPtd = 0;
                    objDesdeDb5.QtyScrpLastYr = 0;
                    objDesdeDb5.QtyScrpPtd = 0;
                    objDesdeDb5.QtyScrpYtd = 0;
                    objDesdeDb5.QtySldPtd = 0;
                    objDesdeDb5.QtySoldLastYr = 0;
                    objDesdeDb5.QtySoldYtd = 0;

                    objDesdeDb5.RecomMinOrd = 0;
                    objDesdeDb5.ReorderLvl = 0;
                    objDesdeDb5.SafetyFctr = 0;
                    objDesdeDb5.SafetyStk = 0;
                    objDesdeDb5.SlsPrice = 0;
                    objDesdeDb5.SlsPtd = 0;
                    objDesdeDb5.SlsYtd = 0;
                    objDesdeDb5.SumOfErrors = 0;
                    objDesdeDb5.TagCost = 0;
                    objDesdeDb5.TagQty = 0;
                    objDesdeDb5.TargetMargin = 0;
                    objDesdeDb5.UsageFilter = 0;
                    objDesdeDb5.TmsCntdYtd = 0;
                    objDesdeDb5.UsagePtd = 0;
                    objDesdeDb5.UsageYtd = 0;

                    objDesdeDb5.UserFld8 = 0;
                    objDesdeDb5.UserFld9 = 0;
                    objDesdeDb5.UserFld10 = 0;
                    objDesdeDb5.UserFld11 = 0;
                    objDesdeDb5.UserFld12 = 0;
                    objDesdeDb5.UserFld13 = 0;

                    objDesdeDb5.UserFld17 = 0;
                    objDesdeDb5.UserFld18 = 0;
                    objDesdeDb5.UserFld19 = 0;
                    objDesdeDb5.UserFld20 = 0;

                    objDesdeDb5.UsgWghtFctr = 0;

                    objDesdeDb5.PricesApplyFlag = "N";
                    objDesdeDb5.DiscsApplyFg = "N";
                    objDesdeDb5.price = result.price;
                    objDesdeDb5.std_cost = result.std_cost;
                    objDesdeDb5.Status = result.Status;
                    objDesdeDb5.MultBinFg = "Y";
                    objDesdeDb5.ProdCat = result.ProdCat;
                    objDesdeDb5.ItemNo = coustumer[i];
                    objDesdeDb5.Loc = loc[i];
                    objDesdeDb5.Id = 0;
                    _db.iminvloc_sql.Add(objDesdeDb5);
                    _db.SaveChanges();


                }

                var objDesdeDby = new OERDTFIL_SQL();
                var objDesdeDbv = _db.imitmidx_sql.FirstOrDefault(s => s.item_no == coustumer[i]);

                var objDesdeDbvL = _db.iminvloc_sql.FirstOrDefault(s => s.ItemNo == coustumer[i] && s.Loc == loc[i]);
                objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;


                objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
                objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
                objDesdeDby.uom = objDesdeDbv.uom;
                objDesdeDby.rma_no = numString;
                objDesdeDby.oe_cus_no = cus;
                objDesdeDby.apply_to_invc_no = invoice[i];
                objDesdeDby.apply_to_seq_no = seq[i];
                objDesdeDby.rma_seq_no = seqq;
                objDesdeDby.item_no = coustumer[i];
                objDesdeDby.reason_cd = code[i];

                objDesdeDby.action = "C";
                //if (objDesdeDb8.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDby.action = "R";
                //}
                //else
                //{
                //    objDesdeDby.action = "C";
                //}
                objDesdeDby.oe_unit_cost = Convert.ToDecimal(acttion[i]);
                objDesdeDby.pick_seq_no = " ";
                objDesdeDby.oe_ord_no = " ";
                objDesdeDby.oe_unique_seq_no = 0;
                objDesdeDby.oe_unique_seq = 0;
                objDesdeDby.oe_unit_price = Convert.ToDecimal(unit[i]);
                objDesdeDby.rma_qty_rtn_auth = Convert.ToDecimal(qty[i]);


                objDesdeDby.loc = loc[i];
                objDesdeDby.status = "O";
                objDesdeDby.DiscountPct = 0;
                objDesdeDby.UomRatio = 1;
                objDesdeDby.RmaQtyRtnActual = 0;


                objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
                objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
                objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
                objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
                objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;
                objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;
                objDesdeDby.Extra10 = 0;
                objDesdeDby.Extra11 = 0;
                objDesdeDby.Extra12 = 0;
                objDesdeDby.Extra13 = 0;
                objDesdeDby.Extra14 = 0;
                objDesdeDby.Extra15 = 0;

                _db.OERDTFIL_SQL.Add(objDesdeDby);




                _db.SaveChanges();
                seqq++;
            }
            await Task.Delay(10000);
            return true;
        }
            public string lineascrear(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc)
        {
           
            string retorno = "Approved";

            var objDesdeDbt = new OERHDFIL_SQL();




            var AccountTypeCode = "";
            var cicmpy = new Cicmpy();
            cicmpy = _db.Cicmpy.Where(a => a.CmpCode.Trim() == rma.Customer.Trim()).FirstOrDefault();
            AccountTypeCode = cicmpy.AccountTypeCode;




            var ArtypfilSql = new ArtypfilSql();
            ArtypfilSql = _db.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
            objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

            objDesdeDbt.dept = ArtypfilSql.SlsDpNo;


            var moneda = "";

            objDesdeDbt.orig_trx_rt = 1;

            objDesdeDbt.curr_trx_rt = 1;
         
            var arcusfil_sql = new arcusfil_sql();
            var oehdrhst_sql = new OEHDRHST_SQL();
            int oehdrhst_sqlcuantos = 0;
            try
            {
               

                    foreach (var filtro in invoice)
                    {

                        oehdrhst_sqlcuantos = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To)&& a.InvNo == filtro).Count();

                        oehdrhst_sql = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To) && a.InvNo == filtro).FirstOrDefault();



                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex,
     Color.Red);
            }

            if (oehdrhst_sqlcuantos == 0)
            {


                arcusfil_sql = _db.arcusfil_sql.Where(a => a.cus_no.Trim() == rma.Customer.Trim()).FirstOrDefault();

                moneda = arcusfil_sql.curr_cd;
                objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;


                objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
                objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;





                objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
                objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
                objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;




               var AraltadrSql = _db.AraltadrSql.Where(a => a.CusNo.Trim() == rma.Customer.Trim() && a.CusAltAdrCd.Contains(rma.Ship_To)).FirstOrDefault();
                objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();

                objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;

                objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;

                objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;

                objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;

                objDesdeDbt.tax_cd = AraltadrSql.TaxCd;

                objDesdeDbt.ship_to_country = AraltadrSql.Country;


                objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                objDesdeDbt.bill_to_country = AraltadrSql.Country;
                objDesdeDbt.ship_to_name = AraltadrSql.CusName;

                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();


                if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
                {
                    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }


            }
            else
            {


                objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                moneda = oehdrhst_sql.CurrCd;
                objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;





                if (moneda != "CNY")
                {
                    objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
                }
                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();
                if (oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "")
                {
                    objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }


                objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;

                objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;

                objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;

                objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
                objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;





                objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
                objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
                objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
                objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;


                objDesdeDbt.bill_to_name = oehdrhst_sql.CusNo;
                objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
                objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

            }
            if (_configuration.GetConnectionString("server") != "m10testna01")
            {

                //if (moneda != "CNY")
                //{

                //    var rate = _db.Rate.Where(a => a.DateL == _db.Rate.Max(a => a.DateL) && a.SourceCurrency == moneda).FirstOrDefault();
                //    objDesdeDbt.orig_trx_rt = (decimal?)rate.RateExchange;

                //    objDesdeDbt.curr_trx_rt = (decimal?)rate.RateExchange;
                //}

            }
            objDesdeDbt.contact = rma.Contact;
            objDesdeDbt.phone_no = rma.Phone;
            objDesdeDbt.fax_no = rma.Fax;
            objDesdeDbt.phone_ext = rma.Ext;
            objDesdeDbt.contact_email = rma.Email;
            objDesdeDbt.user_def_fld_5 = "Normal                        ";
            objDesdeDbt.deter_rate_by = "O";
            objDesdeDbt.form_no = 1;
            objDesdeDbt.rma_no = numString;
            objDesdeDbt.cus_no = cus;
            objDesdeDbt.slspsn_pct_comm = 100;
            objDesdeDbt.cus_ship_to = rma.Ship_To;
            objDesdeDbt.oe_po_no = rma.Customerpo;
            objDesdeDbt.rma_cmt = rma.Description;
            objDesdeDbt.status = "O";
            objDesdeDbt.SlspsnCommAmt = 0;
            objDesdeDbt.SlspsnNo2 = 0;
            objDesdeDbt.SlspsnPctComm2 = 0;
            objDesdeDbt.SlspsnCommAmt2 = 0;
            objDesdeDbt.SlspsnPctComm3 = 0;
            objDesdeDbt.SlspsnCommAmt3 = 0;
            objDesdeDbt.SlspsnNo3 = 0;
            objDesdeDbt.Extra10 = 0;
            objDesdeDbt.Extra11 = 0;
            objDesdeDbt.Extra12 = 0;
            objDesdeDbt.Extra13 = 0;
            objDesdeDbt.Extra14 = 0;
            objDesdeDbt.Extra15 = 0;
            objDesdeDbt.TaxPct = 0;
            objDesdeDbt.TaxPct2 = 0;
            objDesdeDbt.TaxPct3 = 0;
            objDesdeDbt.DiscountPct = 0;
            objDesdeDbt.TotSlsAmt = 0;
            objDesdeDbt.TotSlsDisc = 0;
            objDesdeDbt.TotTaxAmt = 0;
            objDesdeDbt.TotCost = 0;
            objDesdeDbt.TotWeight = 0;
            objDesdeDbt.SlsTaxAmt1 = 0;
            objDesdeDbt.SlsTaxAmt2 = 0;
            objDesdeDbt.SlsTaxAmt3 = 0;
            objDesdeDbt.CommPct = 0;
            objDesdeDbt.CommAmt = 0;
            objDesdeDbt.AccumMiscAmt = 0;
            objDesdeDbt.AccumFrtAmt = 0;
            objDesdeDbt.AccumTotTaxAmt = 0;
            objDesdeDbt.AccumSlsTaxAmt = 0;
            objDesdeDbt.AccumTotSlsAmt = 0;
            objDesdeDbt.TotTaxCost = 0;
            objDesdeDbt.TotDollars = 0;
            objDesdeDbt.TaxFg = "N";

            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
            objDesdeDbt.rma_dt_entered = date;
            objDesdeDbt.LastActDt = date;
            objDesdeDbt.ExpRecDate = date;
            _db.OERHDFIL_SQL.Add(objDesdeDbt);

            _db.SaveChanges();
            var objDesdeDb8 = new CSEXSW_Rma();


            objDesdeDb8.Approver = rma.Approver;
            objDesdeDb8.Contact = rma.Contact;
            objDesdeDb8.Email = rma.Email;

            objDesdeDb8.Ext = rma.Ext;
            objDesdeDb8.Company = rma.Company;
            objDesdeDb8.Fax = rma.Fax;
            objDesdeDb8.Phone = rma.Phone;



            objDesdeDb8.Wherebuilt = rma.Wherebuilt;
            objDesdeDb8.Ship_To = rma.Ship_To;
            objDesdeDb8.Customercomplait = rma.Customercomplait;
            objDesdeDb8.Customerpartno = rma.Customerpartno;
            objDesdeDb8.Customerpo = rma.Customerpo;
            objDesdeDb8.Date = rma.Date;
            objDesdeDb8.Description = rma.Description;
            objDesdeDb8.Preparado = rma.Preparado;
            objDesdeDb8.RMA500 = rma.RMA500;
            objDesdeDb8.Sumbit = rma.Sumbit;
            objDesdeDb8.Rmarequest = rma.Rmarequest;
            objDesdeDb8.Rmatypeofrequest = rma.Rmatypeofrequest;
            objDesdeDb8.Customer = rma.Customer.Trim();
            objDesdeDb8.turno = rma.turno;
            objDesdeDb8.Status = rma.Status;
            objDesdeDb8.Comment = rma.Comment;
            objDesdeDb8.Totalrmavalues = rma.Totalrmavalues;


            _db3.CSEXSW_Rma.Add(objDesdeDb8);

            _db3.SaveChanges();



            for (int i = 0; i < invoice.Length; i++)
            {
                
              
                if (invoice[i] == null)
                {
                    invoice[i] = "";
                }
                if (code[i] == null)
                {
                    code[i] = "";
                }

                var objDesdeDb = new csexsw_coustumer();
                objDesdeDb.Invoice = invoice[i];
                objDesdeDb.Qty = decimal.Round(qty[i], 4);
                objDesdeDb.Seq = seq[i];
                objDesdeDb.Loc = loc[i];
                objDesdeDb.Coustumer = coustumer[i];

                if (checkcar[i] == "true")
                {
                    objDesdeDb.Car = true;
                }
                else
                {
                    objDesdeDb.Car = false;
                }


                objDesdeDb.Retur = code[i];
                objDesdeDb.Action = "C";
                //if (objDesdeDb8.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDb.Action = "R";
                //}
                //else
                //{
                //    objDesdeDb.Action = "C";
                //}
                objDesdeDb.Cost = decimal.Round(acttion[i], 6);
                objDesdeDb.Unit = decimal.Round(unit[i], 6);
                objDesdeDb.RmaId = idRMA + 1;

                _db3.csexsw_coustumer.Add(objDesdeDb);

                _db3.SaveChanges();
            }

           
            Int16 seqq = 1;

            for (int i = 0; i < invoice.Length; i++)
            {




              
                int total2 = _db.iminvloc_sql.Where(a => a.ItemNo == coustumer[i] && a.Loc == loc[i]).Count();
                if (total2 == 0 )
                {
                    var locprincipal = _db.imitmidx_sql.Where(a => a.item_no == coustumer[i]).Select(s => s.loc).FirstOrDefault().ToString();

                    var result = _db.iminvloc_sql.FirstOrDefault(a => a.ItemNo == coustumer[i] && a.Loc == locprincipal);

                    var objDesdeDb5 = new iminvloc_sql();


                    objDesdeDb5.ActiveOrds = 0;
                    objDesdeDb5.AvgCost = 0;
                    objDesdeDb5.AvgFrcstError = 0;
                    objDesdeDb5.AvgUsage = 0;
                    objDesdeDb5.InvClass = result.InvClass;
                    objDesdeDb5.ByrPlnr = result.ByrPlnr;
                    objDesdeDb5.CostLastYr = 0;
                    objDesdeDb5.CostPtd = 0;
                    objDesdeDb5.CostYtd = 0;
                    objDesdeDb5.CubeHeight = 0;
                    objDesdeDb5.CubeLength = 0;
                    objDesdeDb5.CubeQtyPer = 0;
                    objDesdeDb5.CubeWidth = 0;
                    //objDesdeDb5.DocField1 = 0;
                    //objDesdeDb5.DocField2 = 0;
                    //objDesdeDb5.DocField3 = 0;
                    objDesdeDb5.DocToStkLdTm = 0;
                    objDesdeDb5.EconomicOrdQty = 0;

                    objDesdeDb5.Extra10 = 0;
                    objDesdeDb5.Extra11 = 0;
                    objDesdeDb5.Extra12 = 0;
                    objDesdeDb5.Extra13 = 0;
                    objDesdeDb5.Extra14 = 0;
                    objDesdeDb5.Extra15 = 0;

                    objDesdeDb5.FrzCost = 0;
                    objDesdeDb5.FrzQty = 0;
                    objDesdeDb5.IncludeParCost = 0;
                    objDesdeDb5.InvLocReturnCostLyr = 0;

                    objDesdeDb5.InvLocReturnCostPtd = 0;

                    objDesdeDb5.InvLocReturnCostYtd = 0;
                    objDesdeDb5.InvLocReturnSalesLyr = 0;
                    objDesdeDb5.InvLocReturnSalesPtd = 0;
                    objDesdeDb5.InvLocReturnSalesYtd = 0;

                    objDesdeDb5.LastCost = 0;
                    objDesdeDb5.LocQtyFld = 0;

                    objDesdeDb5.OrdUpToLvl = 0;
                    objDesdeDb5.PctErrLastCnt = 0;
                    objDesdeDb5.PoLeadTm = 0;
                    objDesdeDb5.PoMax = 0;
                    objDesdeDb5.PoMin = 0;
                    //objDesdeDb5.PoMult = 0;
                    objDesdeDb5.PriorYearSls = 0;
                    objDesdeDb5.PriorYearUsage = 0;
                    objDesdeDb5.QtyAllocated = 0;

                    objDesdeDb5.QtyBkord = 0;
                    objDesdeDb5.QtyLastSold = 0;
                    objDesdeDb5.QtyOnHand = 0;
                    objDesdeDb5.QtyOnOrd = 0;
                    objDesdeDb5.QtyRejectLastYr = 0;
                    objDesdeDb5.QtyRejectPtd = 0;
                    objDesdeDb5.QtyRejectYtd = 0;

                    objDesdeDb5.QtyReturnedYtd = 0;
                    objDesdeDb5.QtyRtnLyr = 0;
                    objDesdeDb5.QtyRtnPtd = 0;
                    objDesdeDb5.QtyScrpLastYr = 0;
                    objDesdeDb5.QtyScrpPtd = 0;
                    objDesdeDb5.QtyScrpYtd = 0;
                    objDesdeDb5.QtySldPtd = 0;
                    objDesdeDb5.QtySoldLastYr = 0;
                    objDesdeDb5.QtySoldYtd = 0;

                    objDesdeDb5.RecomMinOrd = 0;
                    objDesdeDb5.ReorderLvl = 0;
                    objDesdeDb5.SafetyFctr = 0;
                    objDesdeDb5.SafetyStk = 0;
                    objDesdeDb5.SlsPrice = 0;
                    objDesdeDb5.SlsPtd = 0;
                    objDesdeDb5.SlsYtd = 0;
                    objDesdeDb5.SumOfErrors = 0;
                    objDesdeDb5.TagCost = 0;
                    objDesdeDb5.TagQty = 0;
                    objDesdeDb5.TargetMargin = 0;
                    objDesdeDb5.UsageFilter = 0;
                    objDesdeDb5.TmsCntdYtd = 0;
                    objDesdeDb5.UsagePtd = 0;
                    objDesdeDb5.UsageYtd = 0;

                    objDesdeDb5.UserFld8 = 0;
                    objDesdeDb5.UserFld9 = 0;
                    objDesdeDb5.UserFld10 = 0;
                    objDesdeDb5.UserFld11 = 0;
                    objDesdeDb5.UserFld12 = 0;
                    objDesdeDb5.UserFld13 = 0;

                    objDesdeDb5.UserFld17 = 0;
                    objDesdeDb5.UserFld18 = 0;
                    objDesdeDb5.UserFld19 = 0;
                    objDesdeDb5.UserFld20 = 0;

                    objDesdeDb5.UsgWghtFctr = 0;

                    objDesdeDb5.PricesApplyFlag = "N";
                    objDesdeDb5.DiscsApplyFg = "N";
                    objDesdeDb5.price = result.price;
                    objDesdeDb5.std_cost = result.std_cost;
                    objDesdeDb5.Status = result.Status;
                    objDesdeDb5.MultBinFg = "Y";
                    objDesdeDb5.ProdCat = result.ProdCat;
                    objDesdeDb5.ItemNo = coustumer[i];
                    objDesdeDb5.Loc = loc[i];
                    objDesdeDb5.Id = 0;
                    _db.iminvloc_sql.Add(objDesdeDb5);
                    _db.SaveChanges();


                }

                var objDesdeDby = new OERDTFIL_SQL();
                var objDesdeDbv = _db.imitmidx_sql.FirstOrDefault(s => s.item_no == coustumer[i]);
               
                    var objDesdeDbvL = _db.iminvloc_sql.FirstOrDefault(s => s.ItemNo == coustumer[i] && s.Loc == loc[i]);
                    objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;
                
               
                objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
                objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
                objDesdeDby.uom = objDesdeDbv.uom;
                objDesdeDby.rma_no = numString;
                objDesdeDby.oe_cus_no = cus;
                objDesdeDby.apply_to_invc_no = invoice[i];
                objDesdeDby.apply_to_seq_no = seq[i];
                objDesdeDby.rma_seq_no = seqq;
                objDesdeDby.item_no = coustumer[i];
                objDesdeDby.reason_cd = code[i];

                objDesdeDby.action = "C";
                //if (objDesdeDb8.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDby.action = "R";
                //}
                //else
                //{
                //    objDesdeDby.action = "C";
                //}
                objDesdeDby.oe_unit_cost = Convert.ToDecimal(acttion[i]);
                objDesdeDby.pick_seq_no = " ";
                objDesdeDby.oe_ord_no = " ";
                objDesdeDby.oe_unique_seq_no = 0;
                objDesdeDby.oe_unique_seq = 0;
                objDesdeDby.oe_unit_price = Convert.ToDecimal(unit[i]);
                objDesdeDby.rma_qty_rtn_auth = Convert.ToDecimal(qty[i]);

             
                objDesdeDby.loc = loc[i];
                objDesdeDby.status = "O";
                objDesdeDby.DiscountPct = 0;
                objDesdeDby.UomRatio = 1;
                objDesdeDby.RmaQtyRtnActual = 0;


                objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
                objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
                objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
                objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
                objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;
                objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;
                objDesdeDby.Extra10 = 0;
                objDesdeDby.Extra11 = 0;
                objDesdeDby.Extra12 = 0;
                objDesdeDby.Extra13 = 0;
                objDesdeDby.Extra14 = 0;
                objDesdeDby.Extra15 = 0;

                _db.OERDTFIL_SQL.Add(objDesdeDby);




                _db.SaveChanges();
                seqq++;
            }

            int secreo = _db.OERHDFIL_SQL.Where(a => a.rma_no == numString).Count();



            if (secreo  == 0)
            {


                BackgroundJob.Enqueue(() => falloRMA(rma, idRMA, numString, cus, acttion, invoice, seq, qty, coustumer, code, unit, checkcar, loc));


            }
            return retorno;

        }


        public string lineas(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code,decimal[] unit, string[] checkcar, string[] loc)
        {

            string retorno = "Approved";
            var objDesdeDbt = new OERHDFIL_SQL();










            var AccountTypeCode = "";
            var cicmpy = new Cicmpy();
            cicmpy = _db.Cicmpy.Where(a => a.CmpCode == rma.Customer).FirstOrDefault();
            AccountTypeCode = cicmpy.AccountTypeCode;




            var ArtypfilSql = new ArtypfilSql();
            ArtypfilSql = _db.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
            objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

            objDesdeDbt.dept = ArtypfilSql.SlsDpNo;


            var moneda = "";

            objDesdeDbt.orig_trx_rt = 1;

            objDesdeDbt.curr_trx_rt = 1;
         

            var arcusfil_sql = new arcusfil_sql();
            var oehdrhst_sql = new OEHDRHST_SQL();
            int oehdrhst_sqlcuantos = 0;
            try
            {
                foreach (var filtro in invoice)
                    {

                        oehdrhst_sqlcuantos = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To) && a.InvNo == filtro).Count();

                        oehdrhst_sql = _db.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(rma.Ship_To )&& a.InvNo == filtro).FirstOrDefault();



                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex,
     Color.Red);
            }

            if (oehdrhst_sqlcuantos == 0)
            {


                arcusfil_sql = _db.arcusfil_sql.Where(a => a.cus_no == rma.Customer).FirstOrDefault();

                moneda = arcusfil_sql.curr_cd;
                objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;


                objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
                objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;





                objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
                objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
                objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;




              var  AraltadrSql = _db.AraltadrSql.Where(a => a.CusNo == rma.Customer && a.CusAltAdrCd.Contains(rma.Ship_To)).FirstOrDefault();
                objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();

                objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;

                objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;

                objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;

                objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;

                objDesdeDbt.tax_cd = AraltadrSql.TaxCd;

                objDesdeDbt.ship_to_country = AraltadrSql.Country;


                objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                objDesdeDbt.bill_to_country = AraltadrSql.Country;
                objDesdeDbt.ship_to_name = AraltadrSql.CusName;

                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();


                if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
                {
                    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }
            }
            else
            {


                objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                moneda = oehdrhst_sql.CurrCd;
                objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;





                if (moneda != "CNY")
                {
                    objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
                }
                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db.ImctlfilSql.FirstOrDefault();
                if (oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "")
                {
                    objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }

                objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;

                objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;

                objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;

                objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
                objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;





                objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
                objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
                objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
                objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;


                objDesdeDbt.bill_to_name = oehdrhst_sql.CusNo;
                objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
                objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

            }


            if (moneda != "CNY")
            {

              var  rate = _db.Rate.Where(a => a.DateL == _db.Rate.Max(a => a.DateL) && a.SourceCurrency == moneda).FirstOrDefault();
                objDesdeDbt.orig_trx_rt = (decimal?)rate.RateExchange;

                objDesdeDbt.curr_trx_rt = (decimal?)rate.RateExchange;
            }

            objDesdeDbt.contact = rma.Contact;
            objDesdeDbt.phone_no = rma.Phone;
            objDesdeDbt.fax_no =rma.Fax;
            objDesdeDbt.phone_ext = rma.Ext;
            objDesdeDbt.contact_email = rma.Email;
            objDesdeDbt.user_def_fld_5 = "Normal                        ";
            objDesdeDbt.deter_rate_by = "O";
            objDesdeDbt.form_no = 1;
            objDesdeDbt.rma_no = numString;
            objDesdeDbt.cus_no = cus;
            objDesdeDbt.slspsn_pct_comm = 100;
            objDesdeDbt.cus_ship_to = rma.Ship_To;
            objDesdeDbt.oe_po_no = rma.Customerpo;
            objDesdeDbt.rma_cmt = rma.Description;
            objDesdeDbt.status = "O";
            objDesdeDbt.SlspsnCommAmt = 0;
            objDesdeDbt.SlspsnNo2 = 0;
            objDesdeDbt.SlspsnPctComm2 = 0;
            objDesdeDbt.SlspsnCommAmt2 = 0;
            objDesdeDbt.SlspsnPctComm3 = 0;
            objDesdeDbt.SlspsnCommAmt3 = 0;
            objDesdeDbt.SlspsnNo3 = 0;
            objDesdeDbt.Extra10 = 0;
            objDesdeDbt.Extra11 = 0;
            objDesdeDbt.Extra12 = 0;
            objDesdeDbt.Extra13 = 0;
            objDesdeDbt.Extra14 = 0;
            objDesdeDbt.Extra15 = 0;
            objDesdeDbt.TaxPct = 0;
            objDesdeDbt.TaxPct2 = 0;
            objDesdeDbt.TaxPct3 = 0;
            objDesdeDbt.DiscountPct = 0;
            objDesdeDbt.TotSlsAmt = 0;
            objDesdeDbt.TotSlsDisc = 0;
            objDesdeDbt.TotTaxAmt = 0;
            objDesdeDbt.TotCost = 0;
            objDesdeDbt.TotWeight = 0;
            objDesdeDbt.SlsTaxAmt1 = 0;
            objDesdeDbt.SlsTaxAmt2 = 0;
            objDesdeDbt.SlsTaxAmt3 = 0;
            objDesdeDbt.CommPct = 0;
            objDesdeDbt.CommAmt = 0;
            objDesdeDbt.AccumMiscAmt = 0;
            objDesdeDbt.AccumFrtAmt = 0;
            objDesdeDbt.AccumTotTaxAmt = 0;
            objDesdeDbt.AccumSlsTaxAmt = 0;
            objDesdeDbt.AccumTotSlsAmt = 0;
            objDesdeDbt.TotTaxCost = 0;
            objDesdeDbt.TotDollars = 0;
            objDesdeDbt.TaxFg = "N";

            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
            objDesdeDbt.rma_dt_entered = date;
            objDesdeDbt.LastActDt = date;
            objDesdeDbt.ExpRecDate = date;
            _db.OERHDFIL_SQL.Add(objDesdeDbt);

            _db.SaveChanges();
            var objDesdeDb = _db3.CSEXSW_Rma.FirstOrDefault(s => s.Id == rma.Id);
            objDesdeDb.Approver = rma.Approver;
            objDesdeDb.Contact = rma.Contact;
            objDesdeDb.Email = rma.Email;

            objDesdeDb.Ext = rma.Ext;
            objDesdeDb.Company = rma.Company;
            objDesdeDb.Fax = rma.Fax;
            objDesdeDb.Phone = rma.Phone;



            objDesdeDb.Wherebuilt = rma.Wherebuilt;
            objDesdeDb.Ship_To = rma.Ship_To;
            objDesdeDb.Customercomplait = rma.Customercomplait;
            objDesdeDb.Customerpartno = rma.Customerpartno;
            objDesdeDb.Customerpo = rma.Customerpo;
            objDesdeDb.Date = rma.Date;
            objDesdeDb.Description = rma.Description;
            objDesdeDb.Preparado = rma.Preparado;
            objDesdeDb.RMA500 = rma.RMA500;
            objDesdeDb.Sumbit = rma.Sumbit;
            objDesdeDb.Rmarequest = rma.Rmarequest;
            objDesdeDb.Rmatypeofrequest = rma.Rmatypeofrequest;
            objDesdeDb.Customer = rma.Customer;
            objDesdeDb.turno = rma.turno;
            objDesdeDb.Status = rma.Status;
            objDesdeDb.Comment = rma.Comment;
            objDesdeDb.Totalrmavalues = rma.Totalrmavalues;

            _db.SaveChanges();
            for (int i = 0; i < invoice.Length; i++)
            {
              
                var objDesdeDbt2 = new csexsw_coustumer();
                objDesdeDbt2.Invoice = invoice[i];
                objDesdeDbt2.Qty = qty[i];
                objDesdeDbt2.Seq = seq[i];
                objDesdeDbt2.Loc = loc[i];

                if (checkcar[i] == "true")
                {
                    objDesdeDbt2.Car = true;
                }
                else
                {
                    objDesdeDbt2.Car = false;
                }
                objDesdeDbt2.Coustumer = coustumer[i];
                objDesdeDbt2.Retur = code[i];
                objDesdeDbt2.Unit = unit[i];
                objDesdeDbt2.RmaId = rma.Id;
                objDesdeDbt2.Action = "C";
                //if (objDesdeDb.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDbt2.Action = "R";
                //}
                //else
                //{
                //    objDesdeDbt2.Action = "C";
                //}
                objDesdeDbt2.Cost = acttion[i];

                _db3.csexsw_coustumer.Add(objDesdeDbt2);

                _db3.SaveChanges();
            }


            Int16 seqq = 1;


            for (int i = 0; i < invoice.Length; i++)
            {

               
                int total2 = _db.iminvloc_sql.Where(a => a.ItemNo == coustumer[i] && a.Loc == loc[i]).Count();
                if (total2 == 0 )
                {
                    var locprincipal = _db.imitmidx_sql.Where(a => a.item_no == coustumer[i]).Select(s => s.loc).FirstOrDefault().ToString();

                    var result = _db.iminvloc_sql.FirstOrDefault(a => a.ItemNo == coustumer[i] && a.Loc == locprincipal);


                    var objDesdeDb5 = new iminvloc_sql();



                    objDesdeDb5.ActiveOrds = 0;
                    objDesdeDb5.AvgCost = 0;
                    objDesdeDb5.AvgFrcstError = 0;
                    objDesdeDb5.AvgUsage = 0;
                    objDesdeDb5.InvClass = result.InvClass;
                    objDesdeDb5.ByrPlnr = result.ByrPlnr;
                    objDesdeDb5.CostLastYr = 0;
                    objDesdeDb5.CostPtd = 0;
                    objDesdeDb5.CostYtd = 0;
                    objDesdeDb5.CubeHeight = 0;
                    objDesdeDb5.CubeLength = 0;
                    objDesdeDb5.CubeQtyPer = 0;
                    objDesdeDb5.CubeWidth = 0;
                    //objDesdeDb5.DocField1 = 0;
                    //objDesdeDb5.DocField2 = 0;
                    //objDesdeDb5.DocField3 = 0;
                    objDesdeDb5.DocToStkLdTm = 0;
                    objDesdeDb5.EconomicOrdQty = 0;

                    objDesdeDb5.Extra10 = 0;
                    objDesdeDb5.Extra11 = 0;
                    objDesdeDb5.Extra12 = 0;
                    objDesdeDb5.Extra13 = 0;
                    objDesdeDb5.Extra14 = 0;
                    objDesdeDb5.Extra15 = 0;

                    objDesdeDb5.FrzCost = 0;
                    objDesdeDb5.FrzQty = 0;
                    objDesdeDb5.IncludeParCost = 0;
                    objDesdeDb5.InvLocReturnCostLyr = 0;

                    objDesdeDb5.InvLocReturnCostPtd = 0;

                    objDesdeDb5.InvLocReturnCostYtd = 0;
                    objDesdeDb5.InvLocReturnSalesLyr = 0;
                    objDesdeDb5.InvLocReturnSalesPtd = 0;
                    objDesdeDb5.InvLocReturnSalesYtd = 0;

                    objDesdeDb5.LastCost = 0;
                    objDesdeDb5.LocQtyFld = 0;

                    objDesdeDb5.OrdUpToLvl = 0;
                    objDesdeDb5.PctErrLastCnt = 0;
                    objDesdeDb5.PoLeadTm = 0;
                    objDesdeDb5.PoMax = 0;
                    objDesdeDb5.PoMin = 0;
                    //objDesdeDb5.PoMult = 0;
                    objDesdeDb5.PriorYearSls = 0;
                    objDesdeDb5.PriorYearUsage = 0;
                    objDesdeDb5.QtyAllocated = 0;

                    objDesdeDb5.QtyBkord = 0;
                    objDesdeDb5.QtyLastSold = 0;
                    objDesdeDb5.QtyOnHand = 0;
                    objDesdeDb5.QtyOnOrd = 0;
                    objDesdeDb5.QtyRejectLastYr = 0;
                    objDesdeDb5.QtyRejectPtd = 0;
                    objDesdeDb5.QtyRejectYtd = 0;

                    objDesdeDb5.QtyReturnedYtd = 0;
                    objDesdeDb5.QtyRtnLyr = 0;
                    objDesdeDb5.QtyRtnPtd = 0;
                    objDesdeDb5.QtyScrpLastYr = 0;
                    objDesdeDb5.QtyScrpPtd = 0;
                    objDesdeDb5.QtyScrpYtd = 0;
                    objDesdeDb5.QtySldPtd = 0;
                    objDesdeDb5.QtySoldLastYr = 0;
                    objDesdeDb5.QtySoldYtd = 0;

                    objDesdeDb5.RecomMinOrd = 0;
                    objDesdeDb5.ReorderLvl = 0;
                    objDesdeDb5.SafetyFctr = 0;
                    objDesdeDb5.SafetyStk = 0;
                    objDesdeDb5.SlsPrice = 0;
                    objDesdeDb5.SlsPtd = 0;
                    objDesdeDb5.SlsYtd = 0;
                    objDesdeDb5.SumOfErrors = 0;
                    objDesdeDb5.TagCost = 0;
                    objDesdeDb5.TagQty = 0;
                    objDesdeDb5.TargetMargin = 0;
                    objDesdeDb5.UsageFilter = 0;
                    objDesdeDb5.TmsCntdYtd = 0;
                    objDesdeDb5.UsagePtd = 0;
                    objDesdeDb5.UsageYtd = 0;

                    objDesdeDb5.UserFld8 = 0;
                    objDesdeDb5.UserFld9 = 0;
                    objDesdeDb5.UserFld10 = 0;
                    objDesdeDb5.UserFld11 = 0;
                    objDesdeDb5.UserFld12 = 0;
                    objDesdeDb5.UserFld13 = 0;

                    objDesdeDb5.UserFld17 = 0;
                    objDesdeDb5.UserFld18 = 0;
                    objDesdeDb5.UserFld19 = 0;
                    objDesdeDb5.UserFld20 = 0;


                    objDesdeDb5.UsgWghtFctr = 0;

                    objDesdeDb5.PricesApplyFlag = "N";
                    objDesdeDb5.DiscsApplyFg = "N";
                    objDesdeDb5.price = result.price;
                    objDesdeDb5.std_cost = result.std_cost;
                    objDesdeDb5.Status = result.Status;
                    objDesdeDb5.MultBinFg = "Y";
                    objDesdeDb5.ProdCat = result.ProdCat;
                    objDesdeDb5.ItemNo = coustumer[i];
                    objDesdeDb5.Loc = loc[i];
                    objDesdeDb5.Id = 0;
                    _db.iminvloc_sql.Add(objDesdeDb5);
                    _db.SaveChanges();



                }
                var objDesdeDby = new OERDTFIL_SQL();
                if (invoice[i] == null)
                {
                    invoice[i] = "";
                }
                if (code[i] == null)
                {
                    code[i] = "";
                }
                var objDesdeDbv = _db.imitmidx_sql.FirstOrDefault(s => s.item_no == coustumer[i]);
              
                    var objDesdeDbvL = _db.iminvloc_sql.FirstOrDefault(s => s.ItemNo== coustumer[i] && s.Loc == loc[i]);
                    objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;
                

             
                objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
                objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
                objDesdeDby.uom = objDesdeDbv.uom;
                objDesdeDby.rma_no = numString;
                objDesdeDby.oe_cus_no = cus;
                objDesdeDby.apply_to_invc_no = invoice[i];
                objDesdeDby.apply_to_seq_no = seq[i];
                objDesdeDby.rma_seq_no = seqq;
                objDesdeDby.item_no = coustumer[i];
                objDesdeDby.reason_cd = code[i];
                //if (objDesdeDb.Rmatypeofrequest == "CREDIT & REPLACE")
                //{
                //    objDesdeDby.action = "R";
                //}
                //else
                //{
                //    objDesdeDby.action = "C";
                //}
                objDesdeDby.action = "C";
                objDesdeDby.loc = loc[i];
                
                objDesdeDby.oe_unit_cost = Convert.ToDecimal(acttion[i]);
                objDesdeDby.pick_seq_no = " ";
                objDesdeDby.oe_ord_no = " ";
                objDesdeDby.oe_unique_seq_no = 0;
                objDesdeDby.oe_unique_seq = 0;
                objDesdeDby.oe_unit_price = Convert.ToDecimal(unit[i]);
                objDesdeDby.rma_qty_rtn_auth = Convert.ToDecimal(qty[i]);

                objDesdeDby.status = "O";

              
                objDesdeDby.status = "O";
                objDesdeDby.DiscountPct = 0;
                objDesdeDby.UomRatio = 1;
                objDesdeDby.RmaQtyRtnActual = 0;

                objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
                objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
                objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
                objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
                objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;
                objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;

                objDesdeDby.Extra10 = 0;
                objDesdeDby.Extra11 = 0;
                objDesdeDby.Extra12 = 0;
                objDesdeDby.Extra13 = 0;
                objDesdeDby.Extra14 = 0;
                objDesdeDby.Extra15 = 0;
                _db.OERDTFIL_SQL.Add(objDesdeDby);




                _db.SaveChanges();
                seqq++;
            }

            int secreo = _db.OERHDFIL_SQL.Where(a => a.rma_no == numString).Count();



            if (secreo == 0)
            {


                BackgroundJob.Enqueue(() => falloRMA(rma, idRMA, numString, cus, acttion, invoice, seq, qty, coustumer, code, unit, checkcar, loc));


            }
            return retorno;
        }


    }


}
