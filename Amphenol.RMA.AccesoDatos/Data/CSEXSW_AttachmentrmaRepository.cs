using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class CSEXSW_AttachmentrmaRepository : Repository<CSEXSW_Attachmentrma>, ICSEXSW_AttachmentrmaRepository
    {
        private readonly DbContextM10 _db;

        public CSEXSW_AttachmentrmaRepository(DbContextM10 db) : base(db)
        {
            _db = db;
        }
    }
}
