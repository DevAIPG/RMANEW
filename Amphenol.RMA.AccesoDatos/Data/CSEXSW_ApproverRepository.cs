using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class CSEXSW_ApproverRepository : Repository<CSEXSW_Approver>, ICSEXSW_ApproverRepository
    {
        private readonly DbContextM10 _db;

        public CSEXSW_ApproverRepository(DbContextM10 db) : base(db)
        {
            _db = db;
        }

        public void Update(CSEXSW_Approver rma)
        {
            var objDesdeDb = _db.CSEXSW_Approver.FirstOrDefault(s => s.Id == rma.Id);

          
                objDesdeDb.Approver = rma.Approver;
                _db.SaveChanges();
           

        }


        public void Updaterma(CSEXSW_Rma rma)
        {
            var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == rma.Id);

            objDesdeDb.Approver = rma.Approver;
            objDesdeDb.Status = rma.Status;
            objDesdeDb.Customercomplait = rma.Customercomplait;
            objDesdeDb.Customerpartno = rma.Customerpartno;
            objDesdeDb.Customerpo = rma.Customerpo;
            objDesdeDb.Date = rma.Date;
            objDesdeDb.Description = rma.Description;
            objDesdeDb.Preparado = rma.Preparado;
            objDesdeDb.RMA500 = rma.RMA500;
            objDesdeDb.Rmarequest = rma.Rmarequest;
            objDesdeDb.Rmatypeofrequest = rma.Rmatypeofrequest;
            objDesdeDb.Totalrmavalues = rma.Totalrmavalues;
            objDesdeDb.Wherebuilt = rma.Wherebuilt;
            _db.SaveChanges();
        }
    }
}
