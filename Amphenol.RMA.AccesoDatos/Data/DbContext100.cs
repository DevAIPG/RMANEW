using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Models100;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class DbContext100 : DbContext
    {

        public DbContext100(DbContextOptions<DbContext100> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<oecusitm_sql>().ToTable(nameof(oecusitm_sql)).HasNoKey();
            modelBuilder.Entity<CreateOrder>().HasNoKey();
            modelBuilder.Entity<Oelincmt_sql>(entity =>
            {
                entity.ToTable("Oelincmt_sql", tb =>
                {
                    tb.HasTrigger("cs_OrderLineCommentChanges");
                });
            });
        }


        //public async Task CreateOEOrderFromRMAAsync(
        //      string rmaNo,
        //      string? userName,
        //      byte ctlKey,
        //      bool allowInsufficient,
        //      string? defaultLoc)
        //{
        //    var rmaNoParam = new SqlParameter("@RmaNo", SqlDbType.Char, 8) { Value = rmaNo };
        //    var userNameParam = new SqlParameter("@UserName", SqlDbType.VarChar, 20)
        //    { Value = (object?)userName ?? DBNull.Value };
        //    var ctlKeyParam = new SqlParameter("@CtlKey", ctlKey);
        //    var allowInsufficientParam = new SqlParameter("@AllowInsufficient", allowInsufficient);
        //    var defaultLocParam = new SqlParameter("@DefaultLoc", SqlDbType.Char, 3)
        //    { Value = (object?)defaultLoc ?? DBNull.Value };

        //    // Execute stored procedure, no results expected
        //    await Database.ExecuteSqlRawAsync(
        //        "EXEC dbo.usp_CreateOEOrderFromRMA @RmaNo, @UserName, @CtlKey, @AllowInsufficient, @DefaultLoc",
        //        rmaNoParam, userNameParam, ctlKeyParam, allowInsufficientParam, defaultLocParam);
        //}


        public DbSet<ArtypfilSql> ArtypfilSql { get; set; }
        public DbSet<Cicmpy> Cicmpy { get; set; }
        public DbSet<Rate> Rate { get; set; }

        public DbSet<AraltadrSql> AraltadrSql { get; set; }
        public DbSet<ImctlfilSql> ImctlfilSql { get; set; }
        public DbSet<OelinhstHst> OelinhstHst { get; set; }

        /*CUSTOMER*/
        public DbSet<oecusitm_sql> oecusitm_sql { get; set; }


        public DbSet<OERMACTL_SQL> OERMACTL_SQL { get; set; }
        public DbSet<OERDTFIL_SQL> OERDTFIL_SQL { get; set; }
        public DbSet<imitmidx_sql> imitmidx_sql { get; set; }

        public DbSet<iminvloc_sql> iminvloc_sql { get; set; }

        public DbSet<arcusfil_sql> arcusfil_sql { get; set; }

        public DbSet<OEHDRHST_SQL> OEHDRHST_SQL { get; set; }

        public DbSet<OELINHST_SQL> OELINHST_SQL { get; set; }
        public DbSet<OERHDFIL_SQL> OERHDFIL_SQL { get; set; }
        public DbSet<SYCDEFIL_SQL> SYCDEFIL_SQL { get; set; }

        public DbSet<CreateOrder> CreateOrders { get; set; }

        public DbSet<Oeordhdr_sql> OEORDHDR_SQL { get; set; }
        public DbSet<Oelincmt_sql> OELINCMT_SQL { get; set; }

    }
}
