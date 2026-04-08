using Amphenol.RMA.Models;
using Amphenol.RMA.Models.ModelsM10;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface ICSEXSW_RmaRepository : IRepository<CSEXSW_Rma>
    {
        List<CSEXSW_Rma_ViewModel> GetRMAListData();
        void Update(CSEXSW_Rma rma);
        void UpdateAprove(int id);
        void UpdateRechazo(int id);
        int intRMA();
        int Releaserma();
        Task<bool> SendMailAsync3(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest);
        Task<bool> SendMailAsync4(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest);
        Task<bool> SendMailAsync5(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest);

        Task<bool> SendMailAsync2(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest);
        Task<bool> SendMail(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest);
        void UpdateRema(int idrema, string commentrema, string var);
        void Updatedesaprobar(int ids, string comment, string var/* string mail1, string mail2, string mail3, string mail4, string mail5, string mail6,  string mailp*/);
        string Updateaprobar(int idsa, string commentt, string var, string mail1, string mail2, string mail3, string mail4, string mail5, string mail6,  string mailp);

    }
}
