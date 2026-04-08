using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data.Repository
{
    public interface IDibujoRepository : IRepository<csexsw_dibujo>
    {
        IEnumerable<SelectListItem> GetListadrawings();
        IEnumerable<csexsw_dibujo> All(int id);
        void stamparfinal(int id);
        void stamparfinalOneDrawing(int id,string stamphid="");
        void Update(csexsw_dibujo dibujo);
        void UpdateStatus1(int id, int dib, string usuario);
        void UpdateStatus2(int id, int dib, string usuario);
        void UpdateStatus3(int id, int dib, string usuario);
        void UpdateStatus4(int id, int dib, string usuario);
        void UpdateStatus5(int id, int dib, string usuario);
        void UpdateStatus6(int id, int dib, string usuario);
        void UpdateStatus7(int id, int dib, string usuario);
        void UpdateStatus8(int id, int dib, string usuario);
        void UpdateStatus9(int id, int dib, string usuario);
        void UpdateStatus10(int id, int dib, string usuario);
        void UpdateStatus11(int id, int dib, string usuario);
        void UpdateStatus22(int id, int dib, string usuario);
        void UpdateStatus33(int id, int dib, string usuario);
        void UpdateStatus44(int id, int dib, string usuario);
        void UpdateStatus55(int id, int dib, string usuario);
        void UpdateStatus66(int id, int dib, string usuario);
        void UpdateStatus77(int id, int dib, string usuario);
        void UpdateStatus88(int id, int dib, string usuario);
        void UpdateStatus99(int id, int dib, string usuario);
        void UpdateStatus1010(int id, int dib, string usuario);

        void UpdateStatusNoDrawing(int dibujo_id, int revision_id, string username);
        void RejectNoDrawing(int dibujo_id, int revision_id, string username);
         void AprroveNoDrawingRelease(csexsw_dibujo dibujo);
         void RejectNoDrawingRelease(csexsw_dibujo dibujo);
        void txt(string fecha, string usuario);
        void Updaterechazo(int dib);
        void Updatecomment(int commentid, string comment, string navegador, string usuario);
        void Updatecomment2(int idd, string comment, string usuario);
        string usuario2(string id);
        void reviewers(string type="");
        void stampar(string documento, string rele);
        void nuevostampado(string dibujopdf, string rele, bool sysc);
        void cambiopdf(int id);
        void statustarea2(int id);
        void statustarea(int id,int idrev=0,bool apruebo=false,bool isUpdate=false);
        void borrarTasks(int id);
        void UpdateAprobado(int us, string id); 
        void AddDrawing(csexsw_dibujo drawing);

        string usuario(string id);
        string Release(int id);

        humres EmployeeByUsername(string resid);

        string EmployeeNameByResId(string resid);
    }
}
