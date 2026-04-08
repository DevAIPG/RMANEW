using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface Icsexsw_coustumerRepository : IRepository<csexsw_coustumer>
    {
        void lineas(CSEXSW_Rma rma, decimal[] acttion, string[] invoice, short[] seq, decimal[] qty, string[] coustumer, int idRMA, string[] code, decimal[] unit, string[] checkcar, string[] loc, bool inicio,string[] actions);
        void archivos(string archivodocumento, int idRMA);

    }
}