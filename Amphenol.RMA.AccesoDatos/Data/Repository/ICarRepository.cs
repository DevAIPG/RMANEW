using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface ICarRepository : IRepository<csexsw_car>
    {
        void Delayed(int car);
        void Delayedupdate(int car, string internalduedate);
        Task<bool> crearcar(csexsw_car objDesdeDbt, int idCAR);
        void Update(int idcars,
          string part,
       string depa,
         string ownercar,
         string category,
        string issuedate,
        string internalduedate,
        string responsabledate,
         string answeraccepted,
        string status,
         string po,
          string clientsnombre,
           string customerpart,
          string code,
          string assigneto,
          string contact,
          string customerpn,
          string notes,
          string rma,
          string discrepancy,
           string customercar,
          string customercode,
          string sumbit);
        void Create(int idCAR);
    }
}

