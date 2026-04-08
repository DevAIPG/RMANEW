using Amphenol.RMA.AccesoDatos.Data.Repository;
using Microsoft.Extensions.Configuration;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {

        private readonly DbContextM10 _dbm10;
        private readonly DbContext100 _db100;

        private readonly IConfiguration _configuration;
        public ContenedorTrabajo(IConfiguration configuration, DbContextM10 dbm10, DbContext100 db100)
        {
            _configuration = configuration;

            _dbm10 = dbm10;
            _db100 = db100;




            CSEXSW_Rma = new CSEXSW_RmaRepository(_dbm10, _db100, _configuration);
            OERDTFIL_SQL = new OERDTFIL_SQLRepository(_db100, _dbm10, _configuration);

            CSEXSW_Roles = new RolRepository(_dbm10);

            csexsw_car = new CarRepository(_dbm10);



            CSEXSW_Attachmentrma = new CSEXSW_AttachmentrmaRepository(_dbm10);

            CSEXSW_Approver = new CSEXSW_ApproverRepository(_dbm10);
            csexsw_coustumer = new csexsw_coustumerRepository(_dbm10, _configuration);





            csexsw_dibujo = new DibujoRepository(_dbm10, _db100, _configuration);


        }

        public IOERDTFIL_SQLRepository OERDTFIL_SQL { get; private set; }

        public Icsexsw_coustumerRepository csexsw_coustumer { get; private set; }

        public ICSEXSW_ApproverRepository CSEXSW_Approver { get; private set; }
        public ICSEXSW_AttachmentrmaRepository CSEXSW_Attachmentrma { get; private set; }
        public ICSEXSW_RmaRepository CSEXSW_Rma { get; private set; }

        public ICarRepository csexsw_car { get; private set; }

        public IDibujoRepository csexsw_dibujo { get; set; }

        public IRolRepository CSEXSW_Roles { get; private set; }

        public void Dispose()
        {
            _dbm10.Dispose();
            _db100.Dispose();
        }


        public void Save()
        {
            _dbm10.SaveChanges();
            _db100.SaveChanges();
        }
    }
}
