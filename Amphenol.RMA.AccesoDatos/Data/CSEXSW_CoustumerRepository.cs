using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.Extensions.Configuration;
using System;
namespace Amphenol.RMA.AccesoDatos.Data
{
    public class csexsw_coustumerRepository : Repository<csexsw_coustumer>, Icsexsw_coustumerRepository
    {
        private readonly DbContextM10 _db;
        private readonly IConfiguration _configuration;
        public csexsw_coustumerRepository(DbContextM10 db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public void archivos(string archivodocumento, int idRMA)
        {
            var objDesdeDb = new CSEXSW_Attachmentrma();
            objDesdeDb.Documento = archivodocumento;

            objDesdeDb.RmaId = idRMA;

            _db.CSEXSW_Attachmentrma.Add(objDesdeDb);

            _db.SaveChanges();
        }



        public void lineas(CSEXSW_Rma rma, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, int idRMA, string[] code, decimal[] unit, string[] checkcar, string[] loc, bool inicio, string[] actions)
        {

            if (inicio == true)
            {



                //if (_db.csexsw_coustumer.Where(x => x.RmaId == idRMA).Count() > 0)
                //{
                //    string connectionString = _configuration.GetConnectionString("Connection100").ToString();
                //    var values = new List<Dictionary<string, object>>();
                //    using (SqlConnection cn = new SqlConnection(connectionString))
                //    {
                //        cn.Open();
                //        string query = @"Delete from CSEXSW_Coustumer where RmaId = "  + idRMA;
                //        SqlCommand cmd = new SqlCommand(query, cn);

                //         SqlDataReader rdr = cmd.ExecuteReader();



                //        cn.Close();
                //    }

                //}

                // _db.SaveChanges();
            }



            for (int i = 0; i < invoice.Length; i++)
            {


                if (code[i] == null)
                {
                    code[i] = "";
                }

                if (coustumer[i] == null)
                {
                    coustumer[i] = "";
                }


                if (invoice[i] == null)
                {
                    invoice[i] = "";
                }
                if (actions[i] == null)
                {
                    actions[i] = "";
                }
                if (loc[i] == null)
                {
                    loc[i] = "";
                }


                var objDesdeDbr = new csexsw_coustumer();
                objDesdeDbr.Invoice = invoice[i];
                objDesdeDbr.Qty = decimal.Round(qty[i], 4);
                objDesdeDbr.Seq = seq[i];




                if (checkcar[i] == "true")
                {
                    objDesdeDbr.Car = true;
                }
                else
                {
                    objDesdeDbr.Car = false;
                }

                objDesdeDbr.Coustumer = coustumer[i];
                objDesdeDbr.Retur = code[i];
                objDesdeDbr.Loc = loc[i];
                objDesdeDbr.Action = actions[i];
                objDesdeDbr.Cost = Math.Round(acttion[i], 4);
                objDesdeDbr.Unit = Math.Round(unit[i], 4);
                objDesdeDbr.RmaId = idRMA;

                int order = i;
                if (i == 0)
                {
                    order = 1;

                }
                else
                {
                    order = order + 1;
                }
                objDesdeDbr.rma_seq_no = order;


                _db.csexsw_coustumer.Add(objDesdeDbr);

                try
                {

                    _db.SaveChanges();

                }
                catch (Exception)
                {
                }

            }


        }
    }
}
