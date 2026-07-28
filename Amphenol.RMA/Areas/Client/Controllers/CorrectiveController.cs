using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.AccesoDatos.Data.Repository;

using Amphenol.RMA.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml.VBA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace Amphenol.RMA.Controllers
{
    [Area("Client")]
    [Authorize]
    public class CorrectiveController : Controller
    {
        private const int _maxOrdersReturned = 8192;
        private const double _defaultExchangeRate = 1.0000;
        private readonly IConfiguration _configuration;
        private readonly IContenedorTrabajo _contenedorTrabajo;

        private readonly DbContext100 _context2;

        private readonly DbContextM10 dbContext;


        public CorrectiveController(IConfiguration configuration, IContenedorTrabajo contenedorTrabajo, DbContext100 context2, DbContextM10 dbContext)
        {

            _configuration = configuration;
            _contenedorTrabajo = contenedorTrabajo;


            this.dbContext = dbContext;

            _context2 = context2;

        }




        [HttpGet]
        public IActionResult GetUsername()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string username = valores[1].Trim();
            username = "aioitmgr";
            var addministrador = dbContext.HRRoles.Where(a => a.RoleID == 0);


            foreach (var item in addministrador)
            {

                var user = dbContext.humres.FirstOrDefault(a => a.res_id == item.EmpID).usr_id;

                if ((user == username))
                {
                    return Ok(username);

                }


            }

            return Ok("not found");
        }



        public IActionResult GetAllpersona()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string id = valores[1];


            string var = _contenedorTrabajo.csexsw_dibujo.usuario(id.Trim());

            id = var.Trim();
            return Json(new { data = var });
        }

        [HttpGet]

        public IActionResult GetAllA()
        {
            var q = (from data in _context2.arcusfil_sql
                     from f in _context2.Rate.Where(a => a.DateL == _context2.Rate.Max(a => a.DateL))

                        .Where(f => f.SourceCurrency == data.curr_cd)
                        .DefaultIfEmpty()

                     select new
                     {
                         data.cus_no,
                         data.cus_name,
                         data.curr_cd,
                         rateExchange = f.RateExchange == null ? _defaultExchangeRate : f.RateExchange
                     }).ToList();




            return Json(new { data = q.OrderBy(a => a.cus_no) });
        }

        [HttpGet]
        public IActionResult ExisteLocation(string itemno, string loc)
        {
            bool exists = false;
            var locs = _context2.iminvloc_sql.Where(s => s.ItemNo.Trim() == itemno.Trim()).Select(s => s.Loc).ToList();
            if (locs.Count > 0)
            {
                exists = locs.Contains(loc);
            }
            return Json(exists);
        }

        [HttpGet]
        public IActionResult GetPricesByInvoice(int invoicenumber, string loc)
        {

            var prices = _context2.OELINHST_SQL.Where(s => s.InvNo.Trim() == invoicenumber.ToString().Trim()).Select(r => new { price = r.UnitPrice, std = r.UnitCost, pn = r.ItemNo, loc = r.Loc });
            return Json(prices);
        }

        [HttpGet]
        public IActionResult GetAllI2(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return Json(new { data = Array.Empty<object>() });
            }

            var formattedCustomerId = filtro.Trim().PadLeft(8, ' ');

            var data = _context2.OEHDRHST_SQL
            .Where(x => x.CusNo == formattedCustomerId && !string.IsNullOrWhiteSpace(x.InvNo))
            .OrderByDescending(x => x.OrdDt)
            .Take(_maxOrdersReturned)
            .ToList();

            return Json(new
            {
                data = data
            });
        }
        [HttpGet]

        public IActionResult GetAllM()
        {



            var data = new
            {
                data = _context2.imitmidx_sql.Select(p => new { p.item_no, p.item_desc_1 })
            };

            return Json(data);
        }

        [HttpGet]
        public IActionResult GetAllS()
        {
            var codes = _context2.SYCDEFIL_SQL.Where(a => a.cd_type == "R");

            return Json(new { data = codes });
        }

        [HttpGet]
        public IActionResult GetAllSt(string Client)
        {
            var data = StoreProcedures.Ships(Client.Trim());
            return Json(data);
        }

        [HttpGet]
        public IActionResult getSecuencias(string factura)
        {

            var secuencias = StoreProcedures.GetSecuencias(factura);

            return Json(secuencias);
        }

        [HttpGet]
        public IActionResult GetPartNumbers(int page = 1, int pageSize = 10)
        {
            var data = StoreProcedures.GetItems();
            return Json(new { data = data });
        }


        [HttpGet]
        public IActionResult GetAllInv2(string cus_no)
        {
            ArrayList objs = new ArrayList();
            string connectionString = _configuration.GetConnectionString("Connection100").ToString();
            var values = new List<Dictionary<string, object>>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"select  inv_no as 'Invoice No',cus_no as Customer,hst_dt as Date,line_seq_no as 'Seq No',Item_no as 'Item No',item_desc_1 as 'Description',
        unit_price as 'Price', unit_cost as 'Cost',qty_ordered as 'Quantity Ordered',qty_to_ship as 'Quantity Shipped'
        from oelinhst_hst where cus_no = '" + cus_no + "'";
                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    objs.Add(new
                    {
                        item_no = rdr["Item No"],
                        inv = rdr["Invoice No"],
                        seq = rdr["Seq No"],
                        price = rdr["Price"],
                        qty_to_ship = rdr["Quantity Shipped"],
                        ordered = rdr["Quantity Ordered"],
                        date = rdr["Date"],
                        cus_no = rdr["Customer"],
                        description = rdr["Description"],
                        cost = rdr["Cost"]
                    });
                }
                cn.Close();
            }
            return Json(JsonConvert.SerializeObject(objs));
        }
        [HttpGet]
        public IActionResult GetAllInv5(string cus_no)
        {
            cus_no = cus_no.Trim();
            ArrayList objs = new ArrayList();
            string connectionString = ConnectionM10.Connection100;
            var values = new List<Dictionary<string, object>>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"select top 1  inv_no as 'Invoice No',cus_no as Customer,hst_dt as Date,line_seq_no as 'Seq No',Item_no as 'Item No',item_desc_1 as 'Description',
        unit_price as 'Price', unit_cost as 'Cost',qty_ordered as 'Quantity Ordered',qty_to_ship as 'Quantity Shipped'
        from oelinhst_hst where cus_no = '" + cus_no + "'";
                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    objs.Add(new
                    {
                        item_no = rdr["Item No"],
                        inv = rdr["Invoice No"],
                        seq = rdr["Seq No"],
                        price = rdr["Price"],
                        qty_to_ship = rdr["Quantity Shipped"],
                        ordered = rdr["Quantity Ordered"],
                        date = rdr["Date"],
                        cus_no = rdr["Customer"],
                        description = rdr["Description"],
                        cost = rdr["Cost"]
                    });
                }
                cn.Close();
            }
            return Json(JsonConvert.SerializeObject(objs));
        }
        [HttpGet]
        public IActionResult GetAllInv6(string cus_no, string orders = "0", double pagina = 1, double por_pagina = 10)
        {
            int cuantostodos = _context2.OelinhstHst.Where(a => a.CusNo == cus_no).Count();
            int cuantos = _context2.OelinhstHst.Where(emp => emp.CusNo == cus_no && emp.InvNo.Contains(orders)).Count();

            double total_paginas = Math.Ceiling(cuantos / por_pagina);

            if (pagina > total_paginas)
            {
                pagina = total_paginas;
            }

            double pag_siguiente;
            double pag_anterior;

            pagina -= 1;
            double desde = pagina * por_pagina;

            if (pagina >= total_paginas - 1)
            {
                pag_siguiente = 1;
            }
            else
            {
                pag_siguiente = pagina + 2;
            }

            if (pagina < 1)
            {
                pag_anterior = total_paginas;
            }
            else
            {
                pag_anterior = pagina;
            }

            ArrayList objs = new ArrayList();
            string connectionString = _configuration.GetConnectionString("Connection100").ToString();
            var values = new List<Dictionary<string, object>>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"DECLARE @pagina INT
        DECLARE @por_pagina INT
        set @pagina = " + Int32.Parse(desde.ToString()).ToString() + @"
        set  @por_pagina =(@pagina + " + Int32.Parse((desde + por_pagina).ToString()).ToString() + @" ) -1
        SELECT *
        FROM
        (
         SELECT ROW_NUMBER() OVER(ORDER BY ord_no desc) AS Row#,inv_no as 'Invoice No',cus_no as Customer,hst_dt as Date,line_seq_no as 'Seq No',Item_no as 'Item No',item_desc_1 as 'Description',
        unit_price as 'Price', unit_cost as 'Cost',qty_ordered as 'Quantity Ordered',qty_to_ship as 'Quantity Shipped'
         FROM oelinhst_hst WHERE cus_no = '" + cus_no + @"' and inv_no LIKE '%" + orders + @"%' 
        ) oelinhsthst
        WHERE  Row# BETWEEN  @pagina and @por_pagina ";
                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    objs.Add(new
                    {
                        item_no = rdr["Item No"],
                        inv = rdr["Invoice No"],
                        seq = rdr["Seq No"],
                        price = rdr["Price"],
                        qty_to_ship = rdr["Quantity Shipped"],
                        ordered = rdr["Quantity Ordered"],
                        date = rdr["Date"],
                        cus_no = rdr["Customer"],
                        description = rdr["Description"],
                        cost = rdr["Cost"]
                    });
                }
                cn.Close();
            }
            ArrayList arrPaginas = new ArrayList();
            for (var i = 0; i < total_paginas; i++)
            {
                arrPaginas.Add(i + 1);
            }
            return Json(new
            {
                data = objs,
                err = false,
                conteo = cuantostodos,
                pag_actual = (pagina + 1),
                pag_siguiente = pag_siguiente,
                pag_anterior = pag_anterior,
                total_paginas = total_paginas,
                paginas = arrPaginas
            });
        }

        [HttpGet]
        public IActionResult GetAllCos(int filtro)
        {


            return Json(new { data = _contenedorTrabajo.csexsw_coustumer.GetAll(a => a.RmaId == filtro) });
        }

        [HttpGet]
        public IActionResult GETclientscodigo(string term)
        {

            if (term == null)
            {
                term = "";
            }
            var dato = "no";
            int existe = _context2.arcusfil_sql.Where(a => a.cus_no.Trim() == term.Trim()).Count();
            if (existe != 0)
            {
                dato = "existe";
            }
            return Json(new { existe = dato, result = _context2.arcusfil_sql.Where(a => a.cus_no.Contains(term)).Select(a => a.cus_no.Trim()).Take(20).ToList() });

        }
    }
}
