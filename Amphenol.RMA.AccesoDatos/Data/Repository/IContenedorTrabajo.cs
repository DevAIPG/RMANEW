using System;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface IContenedorTrabajo : IDisposable
    {



        IDibujoRepository csexsw_dibujo { get; }

        IOERDTFIL_SQLRepository OERDTFIL_SQL { get; }
        ICSEXSW_AttachmentrmaRepository CSEXSW_Attachmentrma { get; }
        ICSEXSW_ApproverRepository CSEXSW_Approver { get; }

        ICSEXSW_RmaRepository CSEXSW_Rma { get; }



        ICarRepository csexsw_car { get; }

        IRolRepository CSEXSW_Roles { get; }

        Icsexsw_coustumerRepository csexsw_coustumer { get; }


        void Save();
    }
}
