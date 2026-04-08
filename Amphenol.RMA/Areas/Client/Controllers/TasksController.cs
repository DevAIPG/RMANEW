using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.AccesoDatos.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Amphenol.RMA.Controllers
{
    [Area("Client")]
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly DbContextM10 _context;
        private readonly IConfiguration _configuration;
        public TasksController(IConfiguration configuration, IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironmen, DbContextM10 context, DbContext100 context2)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAlluserloginadmin()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string id = valores[1];
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(id.Trim());

            id = var.Trim();
            if (id == _configuration.GetConnectionString("admin") || id == _configuration.GetConnectionString("owner"))
            {
                return Json(new { data = "admin" });
            }
            else
            {
                return Json(new { data = "Noadmin" });
            }
        }

        [HttpGet]
        public IActionResult GetAlluserlogin()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string id = valores[1];
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(id.Trim());
            id = var.Trim();
            var fullname = _context.humres.Where(s => s.res_id == int.Parse(id)).FirstOrDefault().fullname;
            int count = _contenedorTrabajo.CSEXSW_Roles.GetAll(a => a.fullname == id && a.rol == "Documet Control Drawing").Count();
            if (count != 0)
            {
                return Json(new { data = "admin", nombre = id, fname = fullname });
            }
            else
            {
                return Json(new { data = "Noadmin", nombre = id, fname = fullname });

            }
        }

    }
}

