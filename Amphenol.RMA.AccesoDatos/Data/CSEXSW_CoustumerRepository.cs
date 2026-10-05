using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using System.Linq;
using System;
namespace Amphenol.RMA.AccesoDatos.Data
{
    public class csexsw_coustumerRepository : Repository<csexsw_coustumer>, Icsexsw_coustumerRepository
    {
        private readonly DbContextM10 _m10Db;
        private readonly IConfiguration _configuration;
        public csexsw_coustumerRepository(DbContextM10 m10Db, IConfiguration configuration) : base(m10Db)
        {
            _m10Db = m10Db;
            _configuration = configuration;
        }

        public void archivos(string archivodocumento, int idRMA)
        {
            using var requestLock = LockEditableRequest(idRMA);
            var objDesdeDb = new CSEXSW_Attachmentrma();
            objDesdeDb.Documento = archivodocumento;

            objDesdeDb.RmaId = idRMA;

            _m10Db.CSEXSW_Attachmentrma.Add(objDesdeDb);

            _m10Db.SaveChanges();
        }



        [AutomaticRetry(Attempts = 0)]
        public void lineas(CSEXSW_Rma rma, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, int idRMA, string[] code, decimal[] unit, string[] checkcar, string[] loc, bool inicio, string[] actions)
        {
            using var requestLock = LockEditableRequest(idRMA);

            //if (inicio == true)
            //{
            //    //if (_m10Db.csexsw_coustumer.Where(x => x.RmaId == idRMA).Count() > 0)
            //    //{
            //    //    string connectionString = _configuration.GetConnectionString("Connection100").ToString();
            //    //    var values = new List<Dictionary<string, object>>();
            //    //    using (SqlConnection cn = new SqlConnection(connectionString))
            //    //    {
            //    //        cn.Open();
            //    //        string query = @"Delete from CSEXSW_Coustumer where RmaId = "  + idRMA;
            //    //        SqlCommand cmd = new SqlCommand(query, cn);
            //    //         SqlDataReader rdr = cmd.ExecuteReader();
            //    //        cn.Close();
            //    //    }
            //    //}
            //    // _m10Db.SaveChanges();
            //}

            for (int i = 0; i < invoice.Length; i++)
            {
                var objDesdeDbr = new csexsw_coustumer
                {
                    Invoice = invoice[i] ?? string.Empty,
                    Qty = decimal.Round(qty[i], 4),
                    Seq = seq[i],
                    Car = checkcar[i] == "true",
                    Coustumer = coustumer[i] ?? string.Empty,
                    Retur = code[i] ?? string.Empty,
                    Loc = loc[i] ?? string.Empty,
                    Action = actions[i] ?? string.Empty,
                    Cost = Math.Round(acttion[i], 4),
                    Unit = Math.Round(unit[i], 4),
                    RmaId = idRMA,
                    rma_seq_no = i + 1
                };

                _m10Db.csexsw_coustumer.Add(objDesdeDbr);
            }

            _m10Db.SaveChanges();
        }

        private RmaApprovalLock LockEditableRequest(int requestId)
        {
            var requestLock = new RmaApprovalLock(_m10Db, requestId);
            try
            {
                var request = _m10Db.CSEXSW_Rma.AsNoTracking().FirstOrDefault(x => x.Id == requestId);
                RmaApprovalGuard.EnsureEditable(request);
                return requestLock;
            }
            catch
            {
                requestLock.Dispose();
                throw;
            }
        }
    }
}
