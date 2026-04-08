using Amphenol.RMA.Models;
using Amphenol.RMA.Models.ModelsM10;

using Microsoft.EntityFrameworkCore;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class DbContextM10 : DbContext
    {
        public DbContextM10(DbContextOptions<DbContextM10> options) : base(options)
        {

        }


        public DbSet<humres> humres { get; set; }
        public DbSet<HRRolesDefs> HRRoleDefs { get; set; }
        public DbSet<HRRoles> HRRoles { get; set; }
        public DbSet<items> items { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<BacoDiscussions> BacoDiscussions { get; set; }
        public DbSet<CSBOMAttachments> CSBOMAttachments { get; set; }






        public DbSet<csexsw_coustumer> csexsw_coustumer { get; set; }
        public DbSet<CSEXSW_Rma> CSEXSW_Rma { get; set; }
        public DbSet<CSEXSW_Attachmentrma> CSEXSW_Attachmentrma { get; set; }
        public DbSet<CSEXSW_Approver> CSEXSW_Approver { get; set; }

        public DbSet<csexsw_dibujo> csexsw_dibujo { get; set; }

        public DbSet<csexsw_documentos> csexsw_documentos { get; set; }

        public DbSet<csexsw_d8encabezado> csexsw_d8encabezado { get; set; }

        public DbSet<csexsw_d3> csexsw_d3 { get; set; }


        public DbSet<csexsw_d6> csexsw_d6 { get; set; }

        public DbSet<csexsw_d7_1> csexsw_d7_1 { get; set; }


        public DbSet<csexsw_d7_2> csexsw_d7_2 { get; set; }
        public DbSet<csexsw_d7_3> csexsw_d7_3 { get; set; }


        public DbSet<csexsw_tarea> csexsw_tarea { get; set; }
        public DbSet<csexsw_revision> csexsw_revision { get; set; }


        public DbSet<CSEXSW_Roles> CSEXSW_Roles { get; set; }
        public DbSet<csexsw_causa> csexsw_causa { get; set; }

        public DbSet<csexsw_car> csexsw_car { get; set; }

        public DbSet<csexsw_whys> csexsw_whys { get; set; }

        public DbSet<csexsw_bomexcel> csexsw_bomexcel { get; set; }
    }

}
