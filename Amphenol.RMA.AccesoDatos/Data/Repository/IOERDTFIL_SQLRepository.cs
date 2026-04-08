using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface IOERDTFIL_SQLRepository : IRepository<OERDTFIL_SQL>
    {
        string lineascrear(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[]  loc);

        string lineas(CSEXSW_Rma rma, int idRMA, string numString, string cus, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, string[] code, decimal[] unit, string[] checkcar, string[] loc);
        public string nuevorma();
    }

}