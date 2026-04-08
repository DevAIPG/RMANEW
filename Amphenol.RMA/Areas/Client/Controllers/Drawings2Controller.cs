using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using BlogCore.Models.ModelsM10;
using Newtonsoft.Json;

namespace BlogCore.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class Drawings2Controller : Controller
    {
        private readonly DbContextM10 _context;
        private DbContext100 _context2;
        IWebHostEnvironment _hostingEnvironment;
        IContenedorTrabajo _contenedorTrabajo;
        public Drawings2Controller(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironmen, DbContextM10 m10)
        {
            this._context = m10;
            _hostingEnvironment = hostingEnvironmen;
            _contenedorTrabajo= contenedorTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string LastRelease(int tareaid)
        {
            var re=(from c in _context.csexsw_tarea
             where c.Id ==  tareaid
             select c.Version).FirstOrDefault();
            return re;
        }
        [HttpPost]
        public IActionResult EditDrawing(string drawing)
        {
            string message = string.Empty;

            try
            {
                var artiVM = JsonConvert.DeserializeObject<DibujoVM>(drawing);
                string rele = LastRelease(artiVM.csexsw_dibujo.TareaId);
                if (artiVM.csexsw_dibujo.Id > 0 && artiVM.csexsw_dibujo.TareaId > 0)
                {
                    string rutaPrincipal = _hostingEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;

                    var articuloDesdeDb = _context.csexsw_dibujo.Find(artiVM.csexsw_dibujo.Id);
                    if (archivos.Count() > 0)
                    {
                        //string nombreArchivo = artiVM.csexsw_dibujo.Nombrepdf;
                        //var subidas = Path.Combine(rutaPrincipal, @"documents\drawings\" + rele);
                        //var extension = Path.GetExtension(archivos[0].FileName);
                        //var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                        //var ruta = @"\documents\drawings\" + rele + @"\" + articuloDesdeDb.Dibujopdf;
                        //var rutaImagen = Path.Combine(rutaPrincipal, ruta.TrimStart('\\'));
                        //var ruta2 = @"\documents\pending\" + rele + @"\" + articuloDesdeDb.Dibujopdf;
                        //var rutaImagen2 = Path.Combine(rutaPrincipal, ruta2.TrimStart('\\'));
                        //var ruta3 = @"\documents\approved_stamped\" + rele + @"\" + articuloDesdeDb.Dibujopdf;
                        //var rutaImagen3 = Path.Combine(rutaPrincipal, ruta3.TrimStart('\\'));

                        //if (System.IO.File.Exists(rutaImagen))
                        //{
                        //    System.IO.File.Delete(rutaImagen);
                        //}
                        //if (System.IO.File.Exists(rutaImagen2))
                        //{
                        //    System.IO.File.Delete(rutaImagen2);
                        //}
                        //if (System.IO.File.Exists(rutaImagen3))
                        //{
                        //    System.IO.File.Delete(rutaImagen3);
                        //}

                        ////subimos nuevamente el archivo
                        //if (artiVM.csexsw_dibujo.Revision != null)
                        //{
                        //    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo.Trim() + "_" + artiVM.csexsw_dibujo.Revision.Trim() + extension), FileMode.Create))
                        //    {
                        //        archivos[0].CopyTo(fileStreams);
                        //    }

                        //    artiVM.csexsw_dibujo.Dibujopdf = nombreArchivo.Trim() + "_" + artiVM.csexsw_dibujo.Revision.Trim() + extension;

                        //}
                        //else
                        //{
                        //    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo.Trim() + extension), FileMode.Create))
                        //    {
                        //        archivos[0].CopyTo(fileStreams);
                        //    }

                        //    artiVM.csexsw_dibujo.Dibujopdf = nombreArchivo.Trim() + extension;


                        //}

                        //_context.csexsw_dibujo.Update(artiVM.csexsw_dibujo);
                        //string var2 = LastRelease(artiVM.csexsw_dibujo.TareaId);
                        //_contenedorTrabajo.csexsw_dibujo.cambiopdf(artiVM.csexsw_dibujo.Id);

                        //_contenedorTrabajo.csexsw_dibujo.statustarea(artiVM.csexsw_dibujo.Id);
                        //_contenedorTrabajo.csexsw_dibujo.nuevostampado(artiVM.csexsw_dibujo.Dibujopdf, rele);
                        message = "ok";
                    }

                }
                var dib = _context.csexsw_dibujo.Where(s => s.Id == artiVM.csexsw_dibujo.Id).FirstOrDefault();
                dib.Comment = DateTime.Now.ToString();
                _context.Entry(dib).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
 
            }
        
            //_contenedorTrabajo.csexsw_dibujo.Update(artiVM.csexsw_dibujo);
            //_contenedorTrabajo.Save();
            //string var = _contenedorTrabajo.csexsw_dibujo.Release(artiVM.csexsw_dibujo.TareaId);
            //_contenedorTrabajo.csexsw_dibujo.statustarea(artiVM.csexsw_dibujo.Id, 0, false, true);
            return Json(message);
        }
        [HttpGet]
        public  IActionResult Edit(int? id)
        {
            //ViewData["usr_id"] = new SelectList(_context.HRRoleDefs.GroupBy(p => p.Description).Select(g => new { name = g.Key, count = g.Count() }).OrderBy(p => p.name), "Description", "Description");
            ViewBag.botones1 = id;
            //ViewData["TareaId"] = new SelectList(_context.csexsw_tarea.Where(s=>s.Id==id).ToList(), "Id", "Encargado", id);
            DibujoVM artivm = new DibujoVM()
            {
                //csexsw_dibujo = new Models.csexsw_dibujo(),
                //csexsw_tarea = new Models.csexsw_tarea() 
            }; 
            try
            {
                if (id != null)
                {
                    //id.GetValueOrDefault();
                    artivm.csexsw_dibujo = _context.csexsw_dibujo.Where(s=>s.Id==id).FirstOrDefault();
                    artivm.csexsw_tarea = _context.csexsw_tarea.Where(r=>r.Id== artivm.csexsw_dibujo.TareaId).FirstOrDefault();
                    string rele = (from c in _context.csexsw_tarea
                                   where c.Id == artivm.csexsw_dibujo.TareaId
                                   select c.Version).FirstOrDefault();
                    ViewBag.botones2 = rele;
                    if (artivm.csexsw_dibujo == null)
                    {
                        return RedirectToAction("notdata", "Tasks", new { });
                    }
                }
                else
                {
                    return RedirectToAction("notdata", "Tasks", new { });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("notdata", "Tasks", new { });
            }
            if (artivm == null)
            {
                return NotFound();
            }
            return View(artivm);
        }
    }
}
