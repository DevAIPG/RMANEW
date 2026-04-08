using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class RolRepository : Repository<CSEXSW_Roles>, IRolRepository
    {
        private readonly DbContextM10 _db;

        public RolRepository(DbContextM10 db) : base(db)
        {
            _db = db;
        }



    }
}
