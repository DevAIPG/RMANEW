using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    // Preserve old method signatures so serialized Hangfire jobs fail safely.
    // ERP creation is allowed only through CSEXSW_RmaRepository.Updateaprobar.
    public class OERDTFIL_SQLRepository : Repository<OERDTFIL_SQL>, IOERDTFIL_SQLRepository
    {
        public OERDTFIL_SQLRepository(DbContext500 db500, DbContextM10 m10Db, IConfiguration configuration) : base(db500)
        {
        }

        [AutomaticRetry(Attempts = 0)]
        public string nuevorma()
        {
            throw LegacyDisabled();
        }

        [AutomaticRetry(Attempts = 0)]
        public Task<bool> falloRMA(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc)
        {
            throw LegacyDisabled();
        }

        [AutomaticRetry(Attempts = 0)]
        public string lineascrear(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc)
        {
            throw LegacyDisabled();
        }

        [AutomaticRetry(Attempts = 0)]
        public string lineas(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc)
        {
            throw LegacyDisabled();
        }

        private static InvalidOperationException LegacyDisabled() => new InvalidOperationException(
            "Legacy ERP RMA creation is disabled. Reconcile this request and any existing ERP records manually; use the atomic approval process for new approvals.");
    }
}
