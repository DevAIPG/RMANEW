using AIO.MailDispatch.Models;
using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Extensions;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.ModelsM10;
using Amphenol.RMA.Models.ViewModels;
using Amphenol.RMA.Services.Email;
using Amphenol.RMA.Services.Email.Templates;
using Amphenol.RMA.Services.Reports;
using Amphenol.RMA.Utilidades;
using Amphenol.RMA.ViewModels;
using AspNetCoreGeneratedDocument;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Hangfire;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Org.BouncyCastle.Ocsp;
using SpreadsheetLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TableStyles = OfficeOpenXml.Table.TableStyles;

namespace Amphenol.RMA.Controllers
{

    public interface IHangFireServices
    {
        Task ScheduleJob3();
        Task ScheduleJob2();
        Task ScheduleJob();
    }
    [Area("Client")]
    [Authorize]

    public class RmaController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly DbContextM10 _context2;
        private readonly DbContext100 _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly CustomerServiceManager _customerServiceManager;
        private readonly IEmailTemplateRenderer _templateRenderer;
        const string rootE = @"E:\CSFiles\documents\";
        const string rootEC = @"E:\CSFiles\";
        private const double TwoStepAuthorizationThreshold = 20_000;
        private readonly ReportService _reportService;

        private Approver _autoApprover = new()
        {
            Id = -4,
            Name = "System"
        };

        private readonly IHttpContextAccessor _httpContextAccessor;


        public RmaController(IConfiguration configuration, IContenedorTrabajo contenedorTrabajo, DbContextM10 context2,
                             DbContext100 context, IWebHostEnvironment hostingEnvironmen,
                             IHttpContextAccessor httpContextAccessor, IEmailService emailService,
                             IOptions<CustomerServiceManager> customerServiceManager, ReportService reportService)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context2 = context2;
            _context = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironmen;
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _customerServiceManager = customerServiceManager.Value;
            _reportService = reportService;
        }
        public IConfiguration Configuration { get; }
        public async Task crearcar(csexsw_car objDesdeDbt, int idCAR)
        {

            await _contenedorTrabajo.csexsw_car.crearcar(objDesdeDbt, idCAR);
        }

        public async Task ScheduleJob2(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        {

            //await _contenedorTrabajo.CSEXSW_Rma.SendMailAsync2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rmarequest);
        }
        public async Task ScheduleJob(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        {
            //await _contenedorTrabajo.CSEXSW_Rma.SendMail(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rmarequest);

        }
        public async Task ScheduleJob3(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        {
            //await _contenedorTrabajo.CSEXSW_Rma.SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rmarequest);

        }
        [HttpGet]
        public IActionResult approvers()
        {

            string QM = _context2.CSEXSW_Approver.Where(a => a.Rango == "QM").Select(a => a.Approver).FirstOrDefault();
            string QD = _context2.CSEXSW_Approver.Where(a => a.Rango == "QD").Select(a => a.Approver).FirstOrDefault();
            string GM = _context2.CSEXSW_Approver.Where(a => a.Rango == "GM").Select(a => a.Approver).FirstOrDefault();

            ViewData["QM"] = new SelectList(_context2.humres, "fullname", "fullname", QM);
            ViewData["QD"] = new SelectList(_context2.humres, "fullname", "fullname", QD);
            ViewData["GM"] = new SelectList(_context2.humres, "fullname", "fullname", GM);




            ViewBag.mail1 = _context2.humres.Where(a => a.fullname == QM).Select(a => a.mail).FirstOrDefault();
            ViewBag.mail2 = _context2.humres.Where(a => a.fullname == QD).Select(a => a.mail).FirstOrDefault();
            ViewBag.mail3 = _context2.humres.Where(a => a.fullname == GM).Select(a => a.mail).FirstOrDefault();


            string Dee = _context2.CSEXSW_Approver.Where(a => a.Rango == "Dee").Select(a => a.Approver).FirstOrDefault();
            string Controller = _context2.CSEXSW_Approver.Where(a => a.Rango == "Controller").Select(a => a.Approver).FirstOrDefault();
            string CSM = _context2.CSEXSW_Approver.Where(a => a.Rango == "CSM ").Select(a => a.Approver).FirstOrDefault();

            ViewData["Dee"] = new SelectList(_context2.humres, "fullname", "fullname", Dee);
            ViewData["Controller"] = new SelectList(_context2.humres, "fullname", "fullname", Controller);
            ViewData["CSM"] = new SelectList(_context2.humres, "fullname", "fullname", CSM);




            ViewBag.mail4 = _context2.humres.Where(a => a.fullname == Dee).Select(a => a.mail).FirstOrDefault();
            ViewBag.mail5 = _context2.humres.Where(a => a.fullname == Controller).Select(a => a.mail).FirstOrDefault();
            ViewBag.mail6 = _context2.humres.Where(a => a.fullname == CSM).Select(a => a.mail).FirstOrDefault();


            return View();
        }
        [HttpPost]
        public IActionResult DoneRemark(int id)
        {
            var rma = _context2.CSEXSW_Rma
                .FirstOrDefault(x => x.Id == id);

            if (rma == null) NotFound();

            if (rma.Totalrmavalues >= TwoStepAuthorizationThreshold)
            {
                var qualityDirector = GetEmployeeByRole(100031);
                var generalManager = GetEmployeeByRole(100032);

                if (rma.res_id_approver == generalManager.Id)
                {
                    rma.Approver = qualityDirector.Name;
                    rma.res_id_approver = qualityDirector.Id;
                }
            }
            rma.Status = RmaRequestStatus.Pending.ToDisplayString();

            _context2.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remark(string commentrema, int idrema)
        {
            _contenedorTrabajo.CSEXSW_Rma.UpdateRema(idrema, commentrema);

            return RedirectToAction(nameof(Control));
        }


        [HttpGet]
        public async Task<IActionResult> RMA_IdTo_OrderCreate(string id)
        {

            if (!int.TryParse(id, out var rmaId))
            {
                return BadRequest("Invalid ID");
            }

            try
            {
                var rma = _context2.CSEXSW_Rma.FirstOrDefault(s => s.Id == rmaId);
                if (rma == null)
                    return NotFound($"RMA with id {id} not found");

                var humres = _context2.humres.FirstOrDefault(s => s.res_id == rma.res_id);
                if (humres == null)
                    return NotFound("User not found for given RMA");

                var idProcess = _context2.csexsw_coustumer.Where(s => s.RmaId == rmaId).Count();


                var createOrder = new CreateOrder
                {
                    rmaNo = rma.turno,
                    orderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    previewOnly = false
                };

                return Ok(createOrder);
            }
            catch (Exception ex)
            {
                // log ex here
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderpost([FromBody] CreateOrder request)
        {
            string cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string username = valores[1].Trim();

            try
            {
                using var conn = _context.Database.GetDbConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.usp_RMA_CreateReplacementOrder";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@RMA_NO", request.rmaNo));
                cmd.Parameters.Add(new SqlParameter("@NewOrderType", "O"));
                cmd.Parameters.Add(new SqlParameter("@UserName", username));
                cmd.Parameters.Add(new SqlParameter("@ProcessAllLines", 1));

                string newOrderNumber = null;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync() && reader["Status"]?.ToString() == "SUCCESS")
                    {
                        newOrderNumber = reader["NewOrderNumber"]?.ToString();
                    }
                }

                if (string.IsNullOrEmpty(newOrderNumber))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Order creation failed."
                    });
                }

                var rmaNo = request.rmaNo.Trim().PadLeft(8, ' ');
                var paddedOrderNumber = newOrderNumber.Trim().PadLeft(8, ' ');

                var comments = await _context.OELINCMT_SQL
                    .AsNoTracking()
                    .Where(c => c.OrdType == "R" && c.OrdNo == rmaNo)
                    .OrderBy(c => c.LineSeqNo)
                    .ThenBy(c => c.CmtSeqNo)
                    .ToListAsync();

                if (comments.Any())
                {
                    var commentsToInsert = comments.Select(c => new Oelincmt_sql
                    {
                        OrdType = "O",
                        OrdNo = paddedOrderNumber,
                        LineSeqNo = c.LineSeqNo,
                        LvlNo = c.LvlNo,
                        CmtType = c.CmtType,
                        CmtSeqNo = c.CmtSeqNo,
                        Cmt = c.Cmt,
                        CmtDocType = c.CmtDocType,
                        Extra1 = c.Extra1,
                        Extra2 = c.Extra2,
                        Extra3 = c.Extra3,
                        Extra4 = c.Extra4,
                        Extra5 = c.Extra5,
                        Extra6 = c.Extra6,
                        Extra7 = c.Extra7,
                        Extra8 = c.Extra8,
                        Extra9 = c.Extra9,
                        Extra10 = c.Extra10,
                        Extra11 = c.Extra11,
                        Extra12 = c.Extra12,
                        Extra13 = c.Extra13,
                        Extra14 = c.Extra14,
                        Extra15 = c.Extra15,
                        IsExt = c.IsExt,
                        Filler0001 = c.Filler0001
                    }).ToList();

                    _context.OELINCMT_SQL.AddRange(commentsToInsert);

                    await _context.SaveChangesAsync();
                }
                return Ok(new { success = true, message = $"Order #<b>{newOrderNumber}</b> created successfully." });
            }
            catch (Exception)
            {
                return BadRequest(new { success = false, message = "An unexpected error occurred while creating the order." });
            }
        }

        [HttpPost]
        public IActionResult Importexcel()
        {
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivos.Count() > 0)
            {
                if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\")))
                {

                    Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\"));

                }
                ArrayList objs = new ArrayList();

                //Editamos imagen
                for (int i = 0; i < archivos.Count(); i++)
                {

                    string nombreArchivo = Path.GetFileName(archivos[i].FileName);
                    var subidas = Path.Combine(rutaPrincipal, @"documents\");

                    //subimos nuevamente el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
                    {
                        archivos[i].CopyTo(fileStreams);

                    }


                    SLDocument sl = new SLDocument(@"wwwroot\documents\" + nombreArchivo.Trim() + "");
                    SLWorksheetStatistics propiedades = sl.GetWorksheetStatistics();
                    int ultimaFila = propiedades.EndRowIndex;


                    for (int x = 2; x <= ultimaFila; x++)
                    {
                        string invoice = sl.GetCellValueAsString("A" + x);
                        if (invoice == "" || invoice == null)
                        {
                            invoice = "0";
                        }
                        string seq = sl.GetCellValueAsString("B" + x);
                        if (seq == "" || seq == null)
                        {
                            seq = "0";
                        }
                        string partnumber = sl.GetCellValueAsString("C" + x);
                        string authorized = sl.GetCellValueAsString("D" + x);
                        string price = sl.GetCellValueAsString("E" + x);
                        string cost = sl.GetCellValueAsString("F" + x);
                        string loc = sl.GetCellValueAsString("G" + x);
                        string car = sl.GetCellValueAsString("H" + x);
                        string retur = sl.GetCellValueAsString("I" + x);
                        objs.Add(new
                        {

                            invoice = invoice,
                            seq = Convert.ToInt32(seq),
                            coustumer = partnumber,
                            qty = Convert.ToDecimal(authorized),
                            unit = Convert.ToDecimal(price),
                            cost = Convert.ToDecimal(cost),
                            loc = loc,
                            car = car,
                            retur = retur






                        });



                    }


                    if (System.IO.File.Exists(@"wwwroot\documents\" + nombreArchivo.Trim() + ""))
                    {
                        System.IO.File.Delete(@"wwwroot\documents\" + nombreArchivo.Trim() + "");
                    }




                }


                return Json(new { success = true, message = "Approved Import", data = objs });


            }

            return Json(new { success = false, message = "Disapproved Import" });

        }
        [HttpGet]
        public IActionResult Datos(int id)
        {





            csexsw_coustumerVM rma = new csexsw_coustumerVM()
            {
                CSEXSW_Rma = new Models.CSEXSW_Rma(),
                ItemLines = _contenedorTrabajo.csexsw_coustumer.GetAll(a => a.RmaId == id),

                Attachments = _contenedorTrabajo.CSEXSW_Attachmentrma.GetAll(a => a.RmaId == id),


            };

            rma.CSEXSW_Rma = _contenedorTrabajo.CSEXSW_Rma.Get(id);

            if (rma == null)
            {
                return NotFound();
            }

            return View(rma);
        }
        [HttpGet]
        public IActionResult resubmitrechazo(int id)
        {
            var rma = _context2.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault();
            rma.Status = RmaRequestStatus.Pending.ToDisplayString();
            _context2.Entry(rma).State = EntityState.Modified;
            _context2.SaveChanges();
            return Json(true);

        }
        [HttpPost]
        public async Task<IActionResult> Desaprobar(string comment, int ids)
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];


            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());

            _contenedorTrabajo.CSEXSW_Rma.Updatedesaprobar(ids, comment, var);
            _contenedorTrabajo.Save();

            var rma = _context2.CSEXSW_Rma.FirstOrDefault(x => x.Id == ids);

            if (rma != null)
            {
                var requester = _context2.humres.FirstOrDefault(x => x.res_id == rma.res_id);
                var firstName = requester.fullname?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "Requester";

                var approver = _context2.humres.FirstOrDefault(x => x.res_id == rma.res_id_approver);


                await _emailService.SendRejectNotification(
                    "RMA Request Rejected",
                    rma.Rmarequest,
                    $"""
                        Hello {firstName},<br><br>
                        We regret to inform you that RMA request #{rma.Rmarequest} has been rejected by {approver.fullname}.<br><br>
                        The approver provided the following comments:<br><br>
                        {comment}<br><br>
                        If you have any questions or require additional clarification, please contact the approver directly.<br><br>
                        Thank you,<br>
                        Amphenol Industrial Operations RMA System
                    """,
                    new EmailAddress(requester.mail)
                );
            }

            return RedirectToAction(nameof(Control));
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Aprobar(string commentt, int idsa, bool isAutoApproved = false)
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];

            string userId = isAutoApproved ? _autoApprover.Id.ToString() : _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());

            var rma = _context2.CSEXSW_Rma.FirstOrDefault(x => x.Id == idsa);
            var originalApprover = rma.Approver;
            //OERHDFIL_SQL
            string retorno = _contenedorTrabajo.CSEXSW_Rma.Updateaprobar(idsa, commentt, userId);

            _contenedorTrabajo.Save();

            if (retorno == "Approved")
            {
                int idCAR = 0;

                ArrayList objs = new ArrayList();
                string connectionString = ConnectionM10.Connection;
                var values = new List<Dictionary<string, object>>();
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    string query = @"SELECT IDENT_CURRENT('csexsw_car') as IdActual,
                        IDENT_SEED('csexsw_car') as IdInicial,
                        IDENT_INCR('csexsw_car') as Incremento";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    //get the data reader, etc.
                    while (rdr.Read())
                    {
                        objs.Add(new
                        {
                            IdActual = rdr["IdActual"],
                            IdInicial = rdr["IdInicial"],
                            Incremento = rdr["Incremento"]
                        });

                        idCAR = Convert.ToInt32(rdr["IdActual"]);
                    }

                    cn.Close();
                }

                var objDesdeDblinea = _context2.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();
                var checkcars = _context2.csexsw_coustumer.Count(r => r.RmaId == idsa && r.Car == true);
                if (checkcars > 0)
                {
                    idCAR = idCAR + 1;
                    var objDesdeDb = _context2.CSEXSW_Rma.FirstOrDefault(s => s.Id == idsa);

                    string numString = idCAR.ToString();
                    objDesdeDb.Car = objDesdeDb.Car + "  " + numString;
                    _context.SaveChanges();
                    string Client = (from c in _context.arcusfil_sql
                                     where c.cus_no.Contains(objDesdeDb.Customer)
                                     select c.cus_name).First();
                    var objDesdeDbt = new csexsw_car();
                    objDesdeDbt.customercode = objDesdeDb.Customer;
                    objDesdeDbt.Status = "In Process";
                    DateTime fecha = DateTime.Now;
                    objDesdeDbt.Issue_date = fecha;
                    objDesdeDbt.Owner = "8";
                    objDesdeDbt.owner_name = "Benjamin Cervantes";
                    objDesdeDbt.Internalduedate = fecha.AddDays(30);
                    objDesdeDbt.Responsabledate = fecha.AddDays(30);
                    objDesdeDbt.customername = Client;
                    objDesdeDbt.Defectcode = "";
                    objDesdeDbt.Partnumber = "";
                    objDesdeDbt.Rmanumber = objDesdeDb.turno;
                    objDesdeDbt.po = objDesdeDb.Customerpo;
                    objDesdeDbt.sumbit = false;
                    objDesdeDbt.Notes = "RMA Requests No" + objDesdeDb.Rmarequest + "Description: " + objDesdeDb.Description + " Customer complait: " + objDesdeDb.Customercomplait + " Approver: " + objDesdeDb.Approver + " RMA type of request: " + objDesdeDb.Rmatypeofrequest + " Where built: " + objDesdeDb.Wherebuilt;
                    BackgroundJob.Enqueue(() => crearcar(objDesdeDbt, idCAR));
                }

                var report = await _reportService.GenerateReturnMaterialsAuthorizationPDF(rma.turno.Trim().ToString());
                var attachment = new EmailAttachment(report.FileName, report.ContentType, report.Data);

                var custormerServiceRepresentative = _context2.humres.FirstOrDefault(x => x.res_id == rma.res_id);

                //Notify Amphenol Team
                await _emailService.SendApproveNotification(
                    "RMA No.",
                    rma.turno.Trim(),
                    $"""
                        RMA request #{rma.Rmarequest} has been approved and assigned RMA number <strong>{rma.turno.Trim()}</strong>.<br><br>
                        The RMA is now ready for customer return. Please coordinate with the customer as needed to ensure the material is returned with the assigned RMA number clearly identified.<br><br>
                        Upon receipt of the material, the Quality team will confirm receipt and begin the evaluation process.<br><br>
                        Thank you,<br>
                        Amphenol Industrial Operations RMA System
                     """,
                    new EmailAddress(custormerServiceRepresentative.mail),
                    new EmailAddress(_customerServiceManager.Email),
                    attachment
                );

                var contact = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rma.Contact.ToLower())?
                 .Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "Requester"; ;

                //Notify Customer
                await _emailService.SendCustomerNotification(
                    "RMA Request Approved",
                    rma.turno.Trim(),
                    $"""
                      Hello {contact},<br><br>
                      Your RMA request #{rma.Rmarequest} has been approved and assigned RMA number <strong>{rma.turno.Trim()}</strong>.<br><br>
                      Please find the approved RMA document attached to this email. The document contains the information needed to proceed with the return of the material.<br><br>
                      If you have any questions or require additional assistance, please contact our customer service team.<br><br>
                      Thank you for choosing Amphenol Industrial Operations.<br><br>
                      Amphenol Industrial Operations RMA System
                     """,
                    new EmailAddress(rma.Email),
                    attachment
                );
            }
            else
            {
                var newApprover = _context2.humres.FirstOrDefault(x => x.res_id == rma.res_id_approver);
                var approverFirstname = newApprover.fullname?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "Approver";

                await _emailService.SendApprovalRequestNotification(
                    "RMA Request",
                    rma.Rmarequest,
                    $"""
                        Hello {approverFirstname},<br><br>
                        {originalApprover} has approved RMA request #{rma.Rmarequest}. As part of the approval workflow, this request now requires your review and secondary approval.<br><br>
                        Please review the request details and provide your approval decision at your earliest convenience.<br><br>
                        Thank you,<br>
                        Amphenol Industrial Operations RMA System
                     """,
                    new EmailAddress(newApprover.mail)
                );
            }

            return isAutoApproved ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Control));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editapprovers(CSEXSW_Approver rma)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.CSEXSW_Approver.Update(rma);

                _contenedorTrabajo.Save();


            }

            return RedirectToAction("approvers", "Rma", new { });

        }

        [HttpGet]
        public IActionResult Create()
        {
            var requestId = 1;
            try
            {
                requestId = _contenedorTrabajo.CSEXSW_Rma.Releaserma();
            }
            catch (Exception)
            {

            }

            var rmaViewModel = new RmaViewModel(requestId);

            return View(rmaViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RmaViewModel vm, string submitAction)
        {
            if (!vm.Lines.Any())
            {
                ModelState.AddModelError(nameof(vm.Lines), "At least one line is required.");
            }

            if (!ModelState.IsValid)
            {
                var requestId = 1;
                try
                {
                    requestId = _contenedorTrabajo.CSEXSW_Rma.Releaserma();
                }
                catch (Exception)
                {

                }

                vm.RequestId = requestId;
                vm.InitializeLookups();
                foreach (var line in vm.Lines)
                {
                    line.InitializeLookups();
                }
                return View(vm);
            }

            var requester = User.Identity?.Name?.Split('\\').LastOrDefault();

            if (string.IsNullOrWhiteSpace(requester))
            {
                throw new InvalidOperationException("Unable to determine current user.");
            }

            var formattedCustomerId = vm.CustomerId.Trim().PadLeft(20, ' ');

            string customerPartNumber = vm.Lines.FirstOrDefault().PartNumber ?? "";

            string requesterUserId = _contenedorTrabajo.csexsw_dibujo.usuario(requester);

            string requesterFullname = _context2.humres.First(x => x.res_id == int.Parse(requesterUserId)).fullname;

            var submitStatus = string.Equals(submitAction, "submit", StringComparison.OrdinalIgnoreCase) ?
                RmaSubmitStatus.Submitted : RmaSubmitStatus.NotSubmitted;

            CSEXSW_Rma rma = new()
            {
                Rmarequest = vm.RequestId.ToString(),
                Customer = vm.CustomerId,
                Customerpartno = customerPartNumber,
                Customerpo = vm.PoNumber,
                Description = vm.Description,
                Contact = vm.Contact,
                Email = vm.ContactEmail,
                Phone = vm.PhoneNumber,
                Ext = vm.ExtensionNumber,
                Fax = vm.Fax,
                Company = vm.CompanyEmail,
                Customercomplait = vm.Complaint,
                Wherebuilt = vm.SelectedBuildLocation,
                Ship_To = vm.ShipTo,
                reason = vm.SelectedReason,
                Status = RmaRequestStatus.Pending.ToDisplayString(),
                Sumbit = submitStatus.ToDisplayString(),
                Preparado = requesterFullname,
                res_id = int.Parse(getResId()),
                Date = vm.Date.ToString("MM/dd/yyyy"),
                Rmatypeofrequest = vm.SelectedRequestType,
                Totalrmavalues = (double)vm.TotalValue,
                RMA500 = vm.HasLineOverThreshold ? "Yes" : "No"
            };

            //set approver
            var qualityManager = GetQualityManager(rma.Wherebuilt);
            var qualityDirector = GetEmployeeByRole(100031);
            var generalManager = GetEmployeeByRole(100032);
            var customerServiceManager = _customerServiceManager;


            //approvalFlow.NotifyEmails =
            //[
            //    qualityDirector.mail,
            //    generalManager.mail,
            //    customerServiceManager.Email
            //];

            Approver approver = null;

            if (rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString())
            {
                if (rma.Totalrmavalues < 5000 && !vm.HasLineOverThreshold)
                {
                    rma.SetApprover(_autoApprover);
                    if (!string.Equals(submitAction, "save", StringComparison.OrdinalIgnoreCase))
                    {
                        rma.ChangeRequestStatus(RmaRequestStatus.AutoApprove);
                    }
                }
                else
                {
                    approver = qualityDirector;
                    rma.SetApprover(approver);
                }
            }
            else
            {
                switch (rma.Totalrmavalues)
                {
                    case > 0 and < 5000:
                        approver = qualityManager;
                        rma.SetApprover(approver);
                        break;
                    case >= 5000 and < 20000:
                        approver = qualityDirector;
                        rma.SetApprover(qualityDirector);
                        break;
                    case > 20000:
                        approver = qualityDirector;
                        rma.SetApprover(qualityDirector);
                        //requires secondary approval
                        break;
                }
            }

            await using var transaction = await _context2.Database.BeginTransactionAsync();

            try
            {
                await _context2.CSEXSW_Rma.AddAsync(rma);
                await _context2.SaveChangesAsync();

                await SaveLines(rma.Id, vm.Lines);
                await SaveAttachments(rma, vm.UploadedFiles);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            //BackgroundJob.Enqueue(() => SendRmaCreationNotification(idRma, approvalFlow));

            var firstName = approver.Name?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "Approver";

            if (rma.Status == RmaRequestStatus.AutoApprove.ToDisplayString())
            {
                await Aprobar("RMA auto-approved", rma.Id, true);
            }
            else
            {
                await _emailService.SendApprovalRequestNotification(
                    "RMA Request",
                    rma.Rmarequest,
                    $"""
                    Hello {firstName},<br><br>
                    You have been assigned as the approver for RMA request #{rma.Rmarequest}.<br><br>
                    Please review the request details and provide your approval decision.<br><br>
                    Thank you,<br>
                    Amphenol Industrial Operations RMA System
                    """,
                    new EmailAddress(approver.Email)
                );
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task SaveLines(int rmaId, List<RmaLineViewModel> vmLines)
        {
            if (rmaId is <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rmaId), rmaId, "RMA ID must be greater than zero.");
            }
            if (vmLines is null || !vmLines.Any())
            {
                throw new ArgumentException("At least one RMA line is required.", nameof(vmLines));
            }

            var lines = vmLines.Select((line, index) => new csexsw_coustumer
            {
                Invoice = line.InvoiceNumber ?? string.Empty,
                Qty = decimal.Round(line.AuthorizedQuantity, 4),
                Seq = (short)line.SequenceNumber,
                Car = line.GenerateCAR,
                Coustumer = line.PartNumber ?? string.Empty,
                Retur = line.ReturnCode ?? string.Empty,
                Loc = line.SelectedLocation ?? string.Empty,
                Action = line.SelectedAction ?? string.Empty,
                Cost = Math.Round(line.UnitCost, 4),
                Unit = Math.Round(line.Price, 4),
                RmaId = rmaId,
                rma_seq_no = index + 1
            });

            await _context2.csexsw_coustumer.AddRangeAsync(lines);

            await _context2.SaveChangesAsync();
        }
        private async Task SaveAttachments(CSEXSW_Rma rma, List<IFormFile> attachments)
        {
            if (attachments is null || !attachments.Any())
            {
                return;
            }

            string directory = Path.Combine(rootEC, "documents", "RMA", rma.Rmarequest.Trim(), "Attachments");

            Directory.CreateDirectory(directory);

            foreach (var file in attachments)
            {
                var fileId = Guid.NewGuid();
                var extension = Path.GetExtension(file.FileName);

                var storedFileName = $"{fileId}{extension}";

                string fullPath = Path.Combine(directory, storedFileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                await _context2.CSEXSW_Attachmentrma.AddAsync(new CSEXSW_Attachmentrma
                {
                    Documento = storedFileName,
                    RmaId = rma.Id
                });
            }

            await _context2.SaveChangesAsync();
        }
        private Approver GetApproverInformation(humres employee)
        {
            return new Approver
            {
                Id = employee.res_id,
                Email = employee.mail,
                Name = employee.fullname
            };
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int requestId, string? returnUrl)
        {
            var resolvedReturnUrl = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action(nameof(Index), "Rma");

            var returnPageTitle = resolvedReturnUrl?.Contains("/Control", StringComparison.OrdinalIgnoreCase) == true ? "Request Approvals" : "RMA Requests";

            RmaViewModel rmaViewModel = new()
            {
                ReturnPageTitle = returnPageTitle,
                ReturnUrl = resolvedReturnUrl,
            };

            if (requestId <= 0)
            {
                TempData["ErrorTitle"] = "RMA Not Found";
                TempData["ErrorMessage"] = "The requested RMA could not be found.";
                return View(rmaViewModel);
            }

            var rma = await _context2.CSEXSW_Rma
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == requestId);

            if (rma is null)
            {
                TempData["ErrorTitle"] = "RMA Not Found";
                TempData["ErrorMessage"] = "The requested RMA could not be found.";
                return View(rmaViewModel);
            }

            var currentUser = User.Identity?.Name?.Split('\\').LastOrDefault();

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                TempData["ErrorTitle"] = "Unable to Verify User";
                TempData["ErrorMessage"] = "Your user account could not be verified.";
                return View(rmaViewModel);
            }

            var requesterUserIdString = _contenedorTrabajo.csexsw_dibujo.usuario(currentUser);

            if (!int.TryParse(requesterUserIdString, out var requesterUserId))
            {
                TempData["ErrorTitle"] = "User Not Found";
                TempData["ErrorMessage"] = "Your user account is not registered in the system.";
                return View(rmaViewModel);
            }

            var qualityManager = GetQualityManager(rma.Wherebuilt);
            var qualityDirector = GetEmployeeByRole(100031);
            var generalManager = GetEmployeeByRole(100032);

            var isRequester = rma.res_id == requesterUserId;

            var isApprover =
                rma.res_id_approver == qualityManager.Id ||
                rma.res_id_approver == qualityDirector.Id ||
                rma.res_id_approver == generalManager.Id;

            var isPending = rma.Status == RmaRequestStatus.Pending.ToDisplayString();

            var isFinalized =
                rma.Status == RmaRequestStatus.Approved.ToDisplayString() ||
                rma.Status == RmaRequestStatus.AutoApprove.ToDisplayString();

            var canEdit =
                (isRequester && !isFinalized) ||
                (isApprover && isPending);

            if (!canEdit)
            {
                TempData["ErrorTitle"] = "Access Denied";
                TempData["ErrorMessage"] = "You don't have permission to edit this RMA.";
                return View(rmaViewModel);
            }

            string directory = Path.Combine(rootEC, "documents", "RMA", rma.Rmarequest.Trim(), "Attachments");

            rmaViewModel = new()
            {
                Attachments = await _context2.CSEXSW_Attachmentrma
                     .AsNoTracking()
                     .Where(x => x.RmaId == rma.Id)
                     .Select(x => new RmaAttachmentViewModel
                     {
                         Id = x.Id,
                         FileName = x.Documento,
                         FilePath = Path.Combine(directory, x.Documento)
                     }).ToListAsync(),
                Comments = rma.Comment,
                CompanyEmail = rma.Company,
                Complaint = rma.Customercomplait,
                Contact = rma.Contact,
                ContactEmail = rma.Email,
                CustomerId = rma.Customer,
                Date = Convert.ToDateTime(rma.Date),
                Description = rma.Description,
                ExtensionNumber = rma.Ext,
                Fax = rma.Fax,
                Lines = await _context2.csexsw_coustumer
                     .AsNoTracking()
                     .Where(x => x.CSEXSW_Rma == rma)
                     .Select(x => new RmaLineViewModel
                     {
                         Id = x.Id,
                         AuthorizedQuantity = (int)x.Qty,
                         GenerateCAR = x.Car,
                         InvoiceNumber = x.Invoice,
                         PartNumber = x.Coustumer,
                         Price = x.Unit,
                         ReturnCode = x.Retur,
                         RmaRequestId = x.RmaId,
                         SelectedAction = x.Action,
                         SelectedLocation = x.Loc,
                         SequenceNumber = x.Seq,
                         UnitCost = x.Cost
                     }).ToListAsync(),
                PhoneNumber = rma.Phone,
                PoNumber = rma.Customerpo,
                ReturnPageTitle = returnPageTitle,
                ReturnUrl = resolvedReturnUrl,
                RequestId = int.Parse(rma.Rmarequest),
                RequestStatus = RmaRequestStatusExtensions.FromDisplayString(rma.Status),
                SelectedBuildLocation = rma.Wherebuilt,
                SelectedReason = rma.reason,
                SelectedRequestType = rma.Rmatypeofrequest,
                ShipTo = rma.Ship_To,
                SubmitStatus = RmaSubmitStatusExtensions.FromDisplayString(rma.Sumbit)
            };
            rmaViewModel.InitializeLookups();

            return View(rmaViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RmaViewModel vm, string submitAction)
        {
            if (!vm.Lines.Any())
            {
                ModelState.AddModelError(nameof(vm.Lines), "At least one line is required.");
            }

            if (!ModelState.IsValid)
            {
                vm.InitializeLookups();
                foreach (var line in vm.Lines)
                {
                    line.InitializeLookups();
                }
                return View(vm);
            }

            var formattedCustomerId = vm.CustomerId.Trim().PadLeft(20, ' ');

            string customerPartNumber = vm.Lines.FirstOrDefault().PartNumber ?? "";

            var rma = await _context2.CSEXSW_Rma.FirstOrDefaultAsync(x => x.Id == vm.RequestId);

            if (rma is null)
            {
                return NotFound();
            }

            string requestStatus = string.Equals(submitAction, "save", StringComparison.OrdinalIgnoreCase) ?
                rma.Status : RmaRequestStatus.Pending.ToDisplayString();

            string submitStatus = string.Equals(submitAction, "save", StringComparison.OrdinalIgnoreCase) ?
                rma.Sumbit : RmaSubmitStatus.Submitted.ToDisplayString();

            // Update RMA fields
            rma.Customer = vm.CustomerId;
            rma.Customerpartno = customerPartNumber;
            rma.Customerpo = vm.PoNumber;
            rma.Description = vm.Description;
            rma.Contact = vm.Contact;
            rma.Email = vm.ContactEmail;
            rma.Phone = vm.PhoneNumber;
            rma.Ext = vm.ExtensionNumber;
            rma.Fax = vm.Fax;
            rma.Company = vm.CompanyEmail;
            rma.Customercomplait = vm.Complaint;
            rma.Wherebuilt = vm.SelectedBuildLocation;
            rma.Ship_To = vm.ShipTo;
            rma.reason = vm.SelectedReason;
            rma.Rmatypeofrequest = vm.SelectedRequestType;
            rma.Date = vm.Date.ToString("MM/dd/yyyy");
            rma.Totalrmavalues = (double)vm.TotalValue;
            rma.RMA500 = vm.HasLineOverThreshold ? "Yes" : "No";
            rma.Status = requestStatus;
            rma.Sumbit = submitStatus;

            //set approver
            var qualityManager = GetQualityManager(rma.Wherebuilt);
            var qualityDirector = GetEmployeeByRole(100031);
            var generalManager = GetEmployeeByRole(100032);
            var customerServiceManager = _customerServiceManager;

            if (rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString())
            {
                if (rma.Totalrmavalues < 5000 && !vm.HasLineOverThreshold)
                {
                    rma.SetApprover(_autoApprover);

                    if (!string.Equals(submitAction, "save", StringComparison.OrdinalIgnoreCase))
                    {
                        rma.ChangeRequestStatus(RmaRequestStatus.AutoApprove);
                    }
                }
                else
                {
                    rma.SetApprover(qualityDirector);
                }
            }
            else
            {
                switch (rma.Totalrmavalues)
                {
                    case > 0 and < 5000:
                        rma.SetApprover(qualityManager);
                        break;
                    case >= 5000 and < 20000:
                        rma.SetApprover(qualityDirector);
                        break;
                    case > 20000:
                        rma.SetApprover(qualityDirector);
                        //requires secondary approval
                        break;
                }
            }

            var filesToDelete = new List<string>();
            var newlyCreatedFiles = new List<string>();

            await using var transaction = await _context2.Database.BeginTransactionAsync();

            try
            {
                //update lines
                await UpdateLines(rma.Id, vm.Lines);
                // Update / insert / delete attachments
                var attachmentResult = await UpdateAttachments(rma, vm.Attachments, vm.UploadedFiles);

                filesToDelete = attachmentResult.FilesToDelete;
                newlyCreatedFiles = attachmentResult.NewlyCreatedFiles;

                await _context2.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                // DB rolled back, so remove files that were created
                foreach (var filePath in newlyCreatedFiles)
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                throw;
            }

            // Only delete old files AFTER successful DB commit
            foreach (var filePath in filesToDelete)
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            if (!string.Equals(submitAction, "save", StringComparison.OrdinalIgnoreCase))
            {
                if (rma.Status == RmaRequestStatus.AutoApprove.ToDisplayString())
                {
                    Aprobar("RMA auto-approved", rma.Id, true);
                }
            }


            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
            {
                return LocalRedirect(vm.ReturnUrl);
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task UpdateLines(int rmaId, List<RmaLineViewModel> vmLines)
        {
            var existingLines = await _context2.csexsw_coustumer
                .Where(x => x.RmaId == rmaId)
                .ToListAsync();

            var submittedIds = vmLines
                .Where(x => x.Id > 0)
                .Select(x => x.Id)
                .ToHashSet();

            // Delete lines that were removed from the form
            var linesToDelete = existingLines
                .Where(x => !submittedIds.Contains(x.Id))
                .ToList();

            if (linesToDelete.Count > 0)
            {
                _context2.csexsw_coustumer.RemoveRange(linesToDelete);
            }

            // Update existing lines / insert new lines
            for (int i = 0; i < vmLines.Count; i++)
            {
                var vmLine = vmLines[i];

                if (vmLine.Id > 0)
                {
                    var existingLine = existingLines.FirstOrDefault(x => x.Id == vmLine.Id);

                    if (existingLine is null)
                    {
                        throw new InvalidOperationException($"RMA line {vmLine.Id} does not belong to RMA {rmaId}.");
                    }

                    existingLine.Invoice = vmLine.InvoiceNumber ?? string.Empty;
                    existingLine.Qty = decimal.Round(vmLine.AuthorizedQuantity, 4);
                    existingLine.Seq = (short)vmLine.SequenceNumber;
                    existingLine.Car = vmLine.GenerateCAR;
                    existingLine.Coustumer = vmLine.PartNumber ?? string.Empty;
                    existingLine.Retur = vmLine.ReturnCode ?? string.Empty;
                    existingLine.Loc = vmLine.SelectedLocation ?? string.Empty;
                    existingLine.Action = vmLine.SelectedAction ?? string.Empty;
                    existingLine.Cost = Math.Round(vmLine.UnitCost, 4);
                    existingLine.Unit = Math.Round(vmLine.Price, 4);

                    // Re-number based on current form order
                    existingLine.rma_seq_no = i + 1;
                }
                else
                {
                    var newLine = new csexsw_coustumer
                    {
                        Invoice = vmLine.InvoiceNumber ?? string.Empty,
                        Qty = decimal.Round(vmLine.AuthorizedQuantity, 4),
                        Seq = (short)vmLine.SequenceNumber,
                        Car = vmLine.GenerateCAR,
                        Coustumer = vmLine.PartNumber ?? string.Empty,
                        Retur = vmLine.ReturnCode ?? string.Empty,
                        Loc = vmLine.SelectedLocation ?? string.Empty,
                        Action = vmLine.SelectedAction ?? string.Empty,
                        Cost = Math.Round(vmLine.UnitCost, 4),
                        Unit = Math.Round(vmLine.Price, 4),
                        RmaId = rmaId,
                        rma_seq_no = i + 1
                    };

                    await _context2.csexsw_coustumer.AddAsync(newLine);
                }
            }
        }
        private async Task<AttachmentUpdateResult> UpdateAttachments(CSEXSW_Rma rma, List<RmaAttachmentViewModel> attachments, List<IFormFile> uploadedFiles)
        {
            var result = new AttachmentUpdateResult();

            var filesToDelete = new List<string>();

            var existingAttachments =
                await _context2.CSEXSW_Attachmentrma
                    .Where(x => x.RmaId == rma.Id)
                    .ToListAsync();

            var directory = Path.Combine(rootEC, "documents", "RMA", rma.Rmarequest.Trim(), "Attachments");

            // Remove existing attachments marked for deletion
            foreach (var attachmentVm in attachments
                .Where(x => x.MarkedForDeletion))
            {
                var attachment = existingAttachments
                    .FirstOrDefault(x => x.Id == attachmentVm.Id);

                if (attachment is null)
                {
                    continue;
                }

                var filePath = Path.Combine(directory, attachment.Documento);

                result.FilesToDelete.Add(filePath);

                _context2.CSEXSW_Attachmentrma.Remove(attachment);
            }

            // Add new uploads
            foreach (var file in uploadedFiles.Where(x => x.Length > 0))
            {
                Directory.CreateDirectory(directory);

                var extension = Path.GetExtension(file.FileName);

                var storedFileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(
                    directory,
                    storedFileName);

                await using var stream =
                    new FileStream(filePath, FileMode.CreateNew);

                await file.CopyToAsync(stream);

                result.NewlyCreatedFiles.Add(filePath);

                await _context2.CSEXSW_Attachmentrma.AddAsync(
                    new CSEXSW_Attachmentrma
                    {
                        Documento = storedFileName,
                        RmaId = rma.Id
                    });
            }

            return result;
        }

        //[HttpPost]
        //public IActionResult Create(int id, string Rmarequest, string rmastatus, string rmaapprover, string rmasumbit, string rmadata, string rmawherebuilt, double total, string client,
        //                            string rmatypeofrequest, string description, string customercomplait, string rma500, string customerpo, string shipto, string contact, string phone, string ext,
        //                            string fax, string contactemail, string companyemail, string comment, bool finalizado, bool inicio, decimal[] acttion, string[] invoice, short[] seq,
        //                            string[] coustumer, decimal[] qty, decimal[] unit, string[] code, string[] checkcar, string[] loc, string rmareason, string[] actions)
        //{

        //    string num = string.Empty;
        //    try
        //    {
        //        num = $"8{_contenedorTrabajo.CSEXSW_Rma.Releaserma():D4}";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(string.Concat(ex, " Error generating RMA number"));
        //    }

        //    var rma = BuildRmaModel(id, num, client, rmatypeofrequest, description, customerpo, shipto, contact, phone, ext, fax, contactemail, companyemail, comment, rmadata,
        //                            rmareason, rmawherebuilt, total);

        //    if (!inicio)
        //    {
        //        int idRma = _contenedorTrabajo.CSEXSW_Rma.Releaserma();

        //        BackgroundJob.Schedule(() =>
        //            _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRma, code, unit, checkcar, loc, inicio, actions), TimeSpan.FromSeconds(10)
        //        );

        //        return Json(new { data = "Lineas creadas" });
        //    }


        //    if (!ModelState.IsValid) return Json(new { data = "Model Invalid" });

        //    double rmaTotal = GetRmaTotal(total, client);

        //ProcessRma(rma, rmaTotal, acttion, invoice, seq, qty, coustumer, code, unit, checkcar, loc, inicio, actions);

        //    return Json(new { data = "Terminado" });
        //}


        [HttpGet]
        public async Task<IActionResult> ViewAttachment(int id)
        {
            var attachment = await _context2.CSEXSW_Attachmentrma.FirstAsync(x => x.Id == id);

            if (attachment == null)
                return NotFound();

            //var fileBytes = await System.IO.File.ReadAllBytesAsync(attachment.);

            //return File(
            //    fileBytes,
            //    attachment.ContentType,
            //    attachment.FileName);

            return NotFound();
        }

        private double GetRmaTotal(double total, string client)
        {
            var customer = _context.arcusfil_sql
                .FirstOrDefault(x => x.cus_no.Trim() == client.Trim());

            if (customer == null)
                return total;

            if (customer.curr_cd != "CNY")
                return total;

            var rate = _context.Rate
                .OrderByDescending(x => x.DateL)
                .FirstOrDefault(x => x.SourceCurrency == "USD");

            if (rate == null)
                return total;

            return total / Convert.ToDouble(rate.RateExchange);
        }

        private csexsw_coustumerVM BuildRmaModel(int id, string rmaNumber, string client, string requestType, string description, string customerPo, string shipTo, string contact,
                                                 string phone, string ext, string fax, string contactEmail, string companyEmail, string comment, string rmaDate, string reason,
                                                 string whereBuilt, double total)
        {
            var customerItem = _context.oecusitm_sql.FirstOrDefault(x => x.cus_no.Trim() == client.Trim());

            string customerPart = customerItem?.cus_item_no?.Trim() ?? "";

            string usuario = User.Identity.Name.Split('\\')[1];

            string resId = _contenedorTrabajo.csexsw_dibujo.usuario(usuario);

            string fullname = _context2.humres.First(x => x.res_id == int.Parse(resId)).fullname;

            return new csexsw_coustumerVM
            {
                CSEXSW_Rma = new CSEXSW_Rma
                {
                    Id = id,
                    Rmarequest = rmaNumber,
                    Customer = client,
                    Customerpartno = customerPart,
                    Customerpo = customerPo,
                    Description = description,
                    Contact = contact,
                    Email = contactEmail,
                    Phone = phone,
                    Ext = ext,
                    Fax = fax,
                    Company = companyEmail,
                    Customercomplait = comment,
                    Wherebuilt = whereBuilt,
                    Ship_To = shipTo,
                    reason = reason,
                    Status = "Pending",
                    Sumbit = "Submitted",
                    Preparado = fullname,
                    res_id = int.Parse(getResId()),
                    Date = rmaDate,
                    Rmatypeofrequest = requestType,
                    Totalrmavalues = total
                }
            };
        }

        private Approver GetEmployeeByRole(int roleId)
        {
            var role = _context2.HRRoles.FirstOrDefault(x => x.RoleID == roleId);

            if (role == null) return null;

            var employee = _context2.humres.FirstOrDefault(x => x.res_id == role.EmpID);
            return new Approver
            {
                Id = employee.res_id,
                Name = employee.fullname,
                Email = employee.mail
            };
        }

        private Approver GetQualityManager(string site)
        {
            var roles = new Dictionary<string, int>
            {
                { "Nogales", 100030 },
                { "Mesa", 100062 },
                { "Endicott", 100039 }
            };

            return roles.TryGetValue(site, out int roleId) ? GetEmployeeByRole(roleId) : null;
        }
        public async Task SendRmaCreationNotification(int rmaId, RmaApprovalFlow approvalFlow)
        {

            var rma = _context2.CSEXSW_Rma.FirstOrDefault(x => x.Id == rmaId);

            var lines = _context2.csexsw_coustumer.Where(x => x.RmaId == rmaId).OrderBy(x => x.rma_seq_no).ToList();


            var subject = $"RMA request {rma.Rmarequest.Trim()} generated";
            var overviewTemplate = await System.IO.File.ReadAllTextAsync(Path.Combine(_hostingEnvironment.ContentRootPath, "Services", "Email", "Templates", "RmaOverview.html"));
            var lineTemplate = await System.IO.File.ReadAllTextAsync(Path.Combine(_hostingEnvironment.ContentRootPath, "Services", "Email", "Templates", "RmaDetailLine.html"));

            overviewTemplate = overviewTemplate.Replace("{Instructions}", "");
            overviewTemplate = overviewTemplate.Replace("{RmaRequestId}", rma.Rmarequest.Trim());
            overviewTemplate = overviewTemplate.Replace("{RequestDate}", rma.Date);
            overviewTemplate = overviewTemplate.Replace("{CustomerNumber}", rma.Customer);
            overviewTemplate = overviewTemplate.Replace("{CustomerName}", rma.cus_name);
            overviewTemplate = overviewTemplate.Replace("{WhereBuilt}", rma.Wherebuilt);
            overviewTemplate = overviewTemplate.Replace("{RequestType}", rma.Rmatypeofrequest);
            overviewTemplate = overviewTemplate.Replace("{Reason}", rma.reason);
            overviewTemplate = overviewTemplate.Replace("{Comments}", rma.Customercomplait);
            overviewTemplate = overviewTemplate.Replace("{Total}", rma.Totalrmavalues.ToString("C", CultureInfo.GetCultureInfo("en-US")));


            var lineSection = string.Empty;
            foreach (var line in lines)
            {
                var newLine = lineTemplate;
                newLine = newLine.Replace("{PartNumber}", line.Coustumer);
                newLine = newLine.Replace("{Qty}", line.Qty.ToString());
                newLine = newLine.Replace("{Price}", line.Unit.ToString("C", CultureInfo.GetCultureInfo("en-US")));
                newLine = newLine.Replace("{LineTotal}", (line.Qty * line.Unit).ToString("C", CultureInfo.GetCultureInfo("en-US")));

                lineSection += newLine;
            }

            overviewTemplate = overviewTemplate.Replace("{RmaLines}", DateTime.Today.Year.ToString());
            overviewTemplate = overviewTemplate.Replace("{Year}", DateTime.Today.Year.ToString());

            //await _emailService.SendEmail(subject, overviewTemplate, [approvalFlow.Approver.Email], approvalFlow.NotifyEmails);
        }


        //[HttpGet]
        //public IActionResult Edit(int id)
        //{

        //    csexsw_coustumerVM rma = new csexsw_coustumerVM()
        //    {
        //        CSEXSW_Rma = new Models.CSEXSW_Rma(),
        //        ItemLines = _contenedorTrabajo.csexsw_coustumer.GetAll(a => a.RmaId == id),

        //        Attachments = _contenedorTrabajo.CSEXSW_Attachmentrma.GetAll(a => a.RmaId == id),


        //    };

        //    rma.CSEXSW_Rma = _contenedorTrabajo.CSEXSW_Rma.Get(id);

        //    if (rma == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(rma);
        //}
        [HttpGet]
        public IActionResult Control()
        {


            return View();
        }
        [HttpPost]
        public IActionResult Save(int id, string Rmarequest, string rmastatus, string rmaapprover, string rmasumbit, string rmadata, string rmawherebuilt, double total, string client, string rmatypeofrequest, string description, string customercomplait, string rma500, string customerpo, string shipto, string contact, string phone, string ext, string fax, string contactemail, string companyemail, string comment,
                bool finalizado, bool inicio, decimal[] acttion, string[] invoice, short[] seq, string[] coustumer, decimal[] qty, decimal[] unit, string[] code, string[] checkcar, string[] loc, string rmareason, string[] actions)
        {

            csexsw_coustumerVM rma = new csexsw_coustumerVM()
            {
                CSEXSW_Rma = new Models.CSEXSW_Rma()
            };

            var num = "1";
            try
            {
                int id2 = _contenedorTrabajo.CSEXSW_Rma.Releaserma();

                num = "8" + id2.ToString("D4");

            }
            catch (Exception)
            {

            }
            rma.CSEXSW_Rma.Approver = rmaapprover;
            rma.CSEXSW_Rma.Contact = contact;
            rma.CSEXSW_Rma.Email = contactemail;
            rma.CSEXSW_Rma.Ext = ext;
            rma.CSEXSW_Rma.Company = companyemail;
            rma.CSEXSW_Rma.Fax = fax;
            rma.CSEXSW_Rma.Phone = phone;
            rma.CSEXSW_Rma.reason = rmareason;
            rma.CSEXSW_Rma.Wherebuilt = rmawherebuilt;
            rma.CSEXSW_Rma.Ship_To = shipto;
            rma.CSEXSW_Rma.Customercomplait = comment;
            rma.CSEXSW_Rma.Customerpartno = customerpo;
            rma.CSEXSW_Rma.Customerpo = customerpo;
            rma.CSEXSW_Rma.Date = rmadata;
            rma.CSEXSW_Rma.Description = description;
            rma.CSEXSW_Rma.Preparado = "";
            rma.CSEXSW_Rma.RMA500 = rma500;
            rma.CSEXSW_Rma.Sumbit = rmasumbit;
            rma.CSEXSW_Rma.Rmarequest = num;
            rma.CSEXSW_Rma.Id = id;
            rma.CSEXSW_Rma.Rmatypeofrequest = rmatypeofrequest;
            rma.CSEXSW_Rma.Customer = client;
            rma.CSEXSW_Rma.turno = "";
            rma.CSEXSW_Rma.Status = rmastatus;
            rma.CSEXSW_Rma.Customercomplait = comment;
            rma.CSEXSW_Rma.Totalrmavalues = double.Parse(total.ToString(""));


            int idRMA;

            idRMA = _contenedorTrabajo.CSEXSW_Rma.intRMA();

            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());
            var fullname = _context2.humres.Where(s => s.res_id == int.Parse(var)).FirstOrDefault().fullname;
            rma.CSEXSW_Rma.Preparado = fullname;
            rma.CSEXSW_Rma.res_id = int.Parse(getResId());
            rma.CSEXSW_Rma.Status = "Pending";
            rma.CSEXSW_Rma.Sumbit = "Not Submitted";


            if (inicio == true)
            {




                _contenedorTrabajo.CSEXSW_Rma.Add(rma.CSEXSW_Rma);
                _contenedorTrabajo.Save();

                idRMA = _contenedorTrabajo.CSEXSW_Rma.intRMA();
                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, actions));

                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
                    {


                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

                    }
                    //Editamos imagen
                    for (int i = 0; i < archivos.Count(); i++)
                    {

                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
                        var extension = Path.GetExtension(archivos[i].FileName);
                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
                        string archivodocumento = nombreArchivo;
                        //subimos nuevamente el archivo
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
                        {
                            archivos[i].CopyTo(fileStreams);
                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
                            _contenedorTrabajo.Save();
                        }






                    }
                }



                return Json(new { data = "Primeras lineas" });

            }
            //actualizar res id de approver
            if (!string.IsNullOrEmpty(rma.CSEXSW_Rma.Approver))
            {
                var residapprover = _context2.humres.Where(s => s.fullname == rma.CSEXSW_Rma.Approver).FirstOrDefault().res_id;
                rma.CSEXSW_Rma.res_id_approver = residapprover;
                _context2.Entry(rma.CSEXSW_Rma).State = EntityState.Modified;
                _context2.SaveChanges();
            }
            if (finalizado != true)
            {


                idRMA = _contenedorTrabajo.CSEXSW_Rma.Releaserma();

                BackgroundJob.Schedule(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, actions),
        TimeSpan.FromSeconds(10));



                return Json(new { data = "Lineas creadas" });

            }


            return Json(new { data = "Terminado" });







        }

        [HttpPost]
        public IActionResult Done(int id, string Rmarequest, string rmastatus, string rmaapprover, string rmasumbit, string rmadata, string rmawherebuilt, double total, string client, string rmatypeofrequest, string description, string customercomplait, string rma500, string customerpo, string shipto, string contact, string phone, string ext, string fax, string contactemail, string companyemail, string comment,
            bool finalizado, bool inicio, decimal[] acttion, string[] invoice, short[] seq, string[] coustumer, decimal[] qty, decimal[] unit, string[] code, string[] checkcar, string[] loc, string[] actions)
        {

            csexsw_coustumerVM rma = new csexsw_coustumerVM()
            {
                CSEXSW_Rma = new Models.CSEXSW_Rma(),

            };
            rma.CSEXSW_Rma.Approver = rmaapprover;
            rma.CSEXSW_Rma.Contact = contact;
            rma.CSEXSW_Rma.Email = contactemail;

            rma.CSEXSW_Rma.Ext = ext;
            rma.CSEXSW_Rma.Company = companyemail;
            rma.CSEXSW_Rma.Fax = fax;
            rma.CSEXSW_Rma.Phone = phone;



            rma.CSEXSW_Rma.Wherebuilt = rmawherebuilt;
            rma.CSEXSW_Rma.Ship_To = shipto;
            rma.CSEXSW_Rma.Customercomplait = customercomplait;
            rma.CSEXSW_Rma.Customerpartno = customerpo;
            rma.CSEXSW_Rma.Customerpo = customerpo;
            rma.CSEXSW_Rma.Date = rmadata;
            rma.CSEXSW_Rma.Description = description;
            rma.CSEXSW_Rma.Preparado = "";
            rma.CSEXSW_Rma.RMA500 = rma500;
            rma.CSEXSW_Rma.Sumbit = rmasumbit;
            rma.CSEXSW_Rma.Rmarequest = Rmarequest;
            rma.CSEXSW_Rma.Id = id;
            rma.CSEXSW_Rma.Rmatypeofrequest = rmatypeofrequest;
            rma.CSEXSW_Rma.Customer = client;
            rma.CSEXSW_Rma.turno = "";
            rma.CSEXSW_Rma.Status = rmastatus;
            rma.CSEXSW_Rma.Comment = comment;
            rma.CSEXSW_Rma.Totalrmavalues = double.Parse(total.ToString("#.####"));



            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];

            int idRMA = rma.CSEXSW_Rma.Id;
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());

            rma.CSEXSW_Rma.Preparado = var.Trim();
            rma.CSEXSW_Rma.Status = "Pending";
            rma.CSEXSW_Rma.Sumbit = "Submitted";
            _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
            _contenedorTrabajo.Save();

            if (inicio == true)
            {
                _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, actions);
                _contenedorTrabajo.Save();
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
                    {
                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));
                    }
                    //Editamos imagen
                    for (int i = 0; i < archivos.Count(); i++)
                    {
                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
                        var extension = Path.GetExtension(archivos[i].FileName);
                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
                        string archivodocumento = nombreArchivo;
                        //subimos nuevamente el archivo
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
                        {
                            archivos[i].CopyTo(fileStreams);
                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
                            _contenedorTrabajo.Save();
                        }
                    }
                }
                return Json(new { data = "Primeras lineas" });


            }

            if (finalizado != true)
            {



                BackgroundJob.Schedule(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, actions),
       TimeSpan.FromSeconds(10));



                return Json(new { data = "Lineas creadas" });

            }


            return Json(new { data = "Terminado" });










        }
        public string getResId()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string id = valores[1];
            var emp = _contenedorTrabajo.csexsw_dibujo.usuario(id);
            return emp;
        }

        [HttpPost]
        public IActionResult Saveedit(int id, string Rmarequest, string rmastatus, string rmaapprover, string rmasumbit, string rmadata, string rmawherebuilt, double total, string client, string rmatypeofrequest, string description, string customercomplait, string rma500, string customerpo, string shipto, string contact, string phone, string ext, string fax, string contactemail, string companyemail, string comment,
            bool finalizado, bool inicio, decimal[] acttion, string[] invoice, short[] seq, string[] coustumer, decimal[] qty, decimal[] unit, string[] code, string[] checkcar, string[] loc)
        {
            var resid = getResId();
            csexsw_coustumerVM rma = new csexsw_coustumerVM()
            {
                CSEXSW_Rma = new Models.CSEXSW_Rma()
            };
            rma.CSEXSW_Rma.Approver = rmaapprover;
            rma.CSEXSW_Rma.Contact = contact;
            rma.CSEXSW_Rma.Email = contactemail;

            rma.CSEXSW_Rma.Ext = ext;
            rma.CSEXSW_Rma.Company = companyemail;
            rma.CSEXSW_Rma.Fax = fax;
            rma.CSEXSW_Rma.Phone = phone;



            rma.CSEXSW_Rma.Wherebuilt = rmawherebuilt;
            rma.CSEXSW_Rma.Ship_To = shipto;
            rma.CSEXSW_Rma.Customercomplait = customercomplait;
            rma.CSEXSW_Rma.Customerpartno = customerpo;
            rma.CSEXSW_Rma.Customerpo = customerpo;
            rma.CSEXSW_Rma.Date = rmadata;
            rma.CSEXSW_Rma.Description = description;
            rma.CSEXSW_Rma.Preparado = "";
            rma.CSEXSW_Rma.RMA500 = rma500;
            rma.CSEXSW_Rma.Sumbit = rmasumbit;
            rma.CSEXSW_Rma.Rmarequest = Rmarequest;
            rma.CSEXSW_Rma.Id = id;
            rma.CSEXSW_Rma.Rmatypeofrequest = rmatypeofrequest;
            rma.CSEXSW_Rma.Customer = client;
            rma.CSEXSW_Rma.turno = "";
            rma.CSEXSW_Rma.Status = rmastatus;
            rma.CSEXSW_Rma.Comment = comment;
            rma.CSEXSW_Rma.Totalrmavalues = double.Parse(total.ToString("#.####"));



            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];

            int idRMA = rma.CSEXSW_Rma.Id;
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());
            string fullname = _context2.humres.Where(s => s.usr_id == usuario.Trim()).FirstOrDefault().fullname;

            rma.CSEXSW_Rma.Preparado = fullname;
            rma.CSEXSW_Rma.res_id = int.Parse(resid);
            _context2.Entry(rma.CSEXSW_Rma).State = EntityState.Modified;
            _context2.SaveChanges();
            //_contenedorTrabajo.Save();
            if (inicio == true)
            {

                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));

                //_contenedorTrabajo.Save();

                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
                    {


                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

                    }
                    //Editamos imagen
                    for (int i = 0; i < archivos.Count(); i++)
                    {

                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
                        var extension = Path.GetExtension(archivos[i].FileName);
                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
                        string archivodocumento = nombreArchivo;
                        //subimos nuevamente el archivo
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
                        {
                            archivos[i].CopyTo(fileStreams);
                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
                            _contenedorTrabajo.Save();
                        }
                    }
                }

                return Json(new { data = "Primeras lineas" });

            }





            if (finalizado != true)
            {



                BackgroundJob.Schedule(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null),
       TimeSpan.FromSeconds(10));



                return Json(new { data = "Lineas creadas" });

            }


            return Json(new { data = "Terminado" });







        }

        [HttpGet]
        public IActionResult DeleteLinea(int id)
        {
            var d = _context2.csexsw_coustumer.Where(s => s.Id == id).FirstOrDefault();
            _context2.csexsw_coustumer.Remove(d);
            var r = _context2.SaveChanges();
            return Json(r > 0 ? true : false);
        }
        [HttpGet]
        public IActionResult AddNewLine(string line)
        {

            var l = JsonConvert.DeserializeObject<csexsw_coustumer>(line);
            _context2.csexsw_coustumer.Add(l);
            var result = _context2.SaveChanges();
            return Json(result > 0 ? true : false);
        }

        [HttpGet]
        public IActionResult GetRmaInfo(int id)
        {
            var rma = _context2.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault();
            var lineas = _context2.csexsw_coustumer.OrderByDescending(s => s.Id).Where(r => r.RmaId == rma.Id).ToList();
            var attachments = _context2.CSEXSW_Attachmentrma.OrderByDescending(g => g.Id).Where(r => r.RmaId == rma.Id).ToList();

            return Json(new { rma = rma, lines = lineas, files = attachments });
        }



        [HttpGet]
        public IActionResult SubmitRMA(int id)
        {
            bool inicio = true;
            bool finalizado = false;
            var num = "1";
            try
            {
                int id2 = _contenedorTrabajo.CSEXSW_Rma.Releaserma();
                num = "8" + id2.ToString("D4");
            }
            catch (Exception)
            {

            }

            var rmaS = _context2.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault();

            var arcusfil_sql = new arcusfil_sql();

            double tipo_cambio = rmaS.Totalrmavalues;
            arcusfil_sql = _context.arcusfil_sql.Where(a => a.cus_no.Trim() == rmaS.Customer.Trim()).FirstOrDefault();

            var moneda = arcusfil_sql.curr_cd;
            var rate = _context.Rate.Where(a => a.DateL == _context.Rate.Max(a => a.DateL) && a.SourceCurrency == "USD").FirstOrDefault();


            if (moneda == "CNY")
            {
                tipo_cambio = Convert.ToDouble(rmaS.Totalrmavalues) / Convert.ToDouble(rate.RateExchange);
            }

            csexsw_coustumerVM rma = new csexsw_coustumerVM()
            {
                CSEXSW_Rma = rmaS
            };

            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];
            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());
            var fullname = _context2.humres.Where(s => s.res_id == int.Parse(var)).FirstOrDefault().fullname;
            rma.CSEXSW_Rma.Preparado = fullname;
            rma.CSEXSW_Rma.res_id = int.Parse(getResId());
            rma.CSEXSW_Rma.Status = "Pending";
            rma.CSEXSW_Rma.Sumbit = "Submitted";

            int idRMA = _contenedorTrabajo.CSEXSW_Rma.intRMA();


            if (inicio == true)
            {
                if (ModelState.IsValid)
                {
                    string rutaPrincipal = _hostingEnvironment.WebRootPath;
                    string QM = "";
                    humres empQM = null;
                    if (rma.CSEXSW_Rma.Wherebuilt == "Nogales")
                    {
                        //QM Quality Manager NOG)
                        var QMM10 = _context2.HRRoles.Where(s => s.RoleID == 100030).FirstOrDefault();
                        if (QMM10 != null)
                        {
                            empQM = _context2.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                            QM = empQM.fullname;
                        }
                    }
                    if (rma.CSEXSW_Rma.Wherebuilt == "Mesa")
                    {
                        //Mesa
                        //QM Quality Manager Mesa)
                        var QMM10 = _context2.HRRoles.Where(s => s.RoleID == 100062).FirstOrDefault();
                        if (QMM10 != null)
                        {
                            empQM = _context2.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                            QM = empQM.fullname;
                        }
                    }
                    if (rma.CSEXSW_Rma.Wherebuilt == "Endicott")
                    {
                        //Endicot
                        //QM Quality Manager END)
                        var QMM10 = _context2.HRRoles.Where(s => s.RoleID == 100039).FirstOrDefault();
                        if (QMM10 != null)
                        {
                            empQM = _context2.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                            QM = empQM.fullname;
                        }
                    }

                    //QD (Quality Director)
                    var QDM10 = _context2.HRRoles.Where(d => d.RoleID == 100031).FirstOrDefault();
                    string QD = "";
                    humres empQD = null;
                    if (QDM10 != null)
                    {
                        empQD = _context2.humres.Where(s => s.res_id == QDM10.EmpID).FirstOrDefault();
                        QD = empQD.fullname;
                    }
                    //GM (General Manager)
                    var GMM10 = _context2.HRRoles.Where(d => d.RoleID == 100032).FirstOrDefault();
                    humres empGM = null;
                    string GM = "";
                    if (GMM10 != null)
                    {
                        empGM = _context2.humres.Where(s => s.res_id == GMM10.EmpID).FirstOrDefault();
                        GM = empGM.fullname;
                    }
                    string mail1 = "";
                    string mail2 = "";
                    string mail3 = "";
                    string mail4 = "";
                    string mail5 = "";
                    string mail6 = "";
                    string mailp = "";


                    if (rma.CSEXSW_Rma.Wherebuilt == "Nogales" || rma.CSEXSW_Rma.Wherebuilt == "Endicott")
                    {
                        if (rma.CSEXSW_Rma.Rmatypeofrequest == "VALUE ADD RMA" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT & REPLACE" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT ONLY")
                        {
                            if (tipo_cambio <= 4999)
                            {
                                rma.CSEXSW_Rma.Approver = QM;
                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();


                            }

                            if (tipo_cambio >= 5000 && tipo_cambio <= 19999.99)
                            {
                                rma.CSEXSW_Rma.Approver = QD;
                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();
                            }

                            if (tipo_cambio >= 20000)
                            {
                                rma.CSEXSW_Rma.Approver = QD;

                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();
                            }
                        }
                        //actualizar res id de approver
                        if (!string.IsNullOrEmpty(rma.CSEXSW_Rma.Approver))
                        {
                            var residapprover = _context2.humres.Where(s => s.fullname == rma.CSEXSW_Rma.Approver).FirstOrDefault().res_id;
                            rma.CSEXSW_Rma.res_id_approver = residapprover;
                            _context2.Entry(rma.CSEXSW_Rma).State = EntityState.Modified;
                            _context2.SaveChanges();
                        }

                        if (rma.CSEXSW_Rma.Rmatypeofrequest == "DISTY SCRAP ALLOWANCE")
                        {
                            //if (tip)
                            //{

                            //}
                            rma.CSEXSW_Rma.Approver = QD;
                            _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                            _contenedorTrabajo.Save();
                        }
                    }

                    if (rma.CSEXSW_Rma.Wherebuilt == "ATZ" || rma.CSEXSW_Rma.Wherebuilt == "MAO")
                    {
                        if (rma.CSEXSW_Rma.Rmatypeofrequest == "VALUE ADD RMA" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT & REPLACE" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT ONLY")
                        {
                            if (tipo_cambio <= 4999)
                            {
                                rma.CSEXSW_Rma.Approver = QM;
                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();
                            }

                            if (tipo_cambio >= 5000 && tipo_cambio <= 19999.99)
                            {
                                rma.CSEXSW_Rma.Approver = QD;
                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();

                            }

                            if (tipo_cambio >= 20000)
                            {

                                rma.CSEXSW_Rma.Approver = QD;
                                _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                                _contenedorTrabajo.Save();
                            }
                        }

                        if (rma.CSEXSW_Rma.Rmatypeofrequest == "DISTY SCRAP ALLOWANCE")
                        {
                            rma.CSEXSW_Rma.Approver = QD;
                            _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
                            _contenedorTrabajo.Save();
                        }
                    }
                }

                return Json(new { data = "Primeras lineas" });
            }
            if (finalizado != true)
            {
                idRMA = _contenedorTrabajo.CSEXSW_Rma.Releaserma();
                return Json(new { data = "Lineas creadas" });
            }
            if (rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT & REPLACE")
            {

            }
            return Json(new { data = "Terminado" });
        }

        [HttpGet]
        public IActionResult OpenFileRMA(string rmano, string filename)
        {
            var directoryrma = Path.Combine(rootEC, $@"documents\RMA\{rmano}\Attachments\{filename}");



            var bytes = System.IO.File.ReadAllBytes(directoryrma);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            //string contentType;
            //if (Path.GetExtension(filePath) == ".txt")
            //{
            //    contentType = "text/plain";
            //}
            //else if (Path.GetExtension(filePath) == ".pdf")
            //{
            //    contentType = "application/pdf";
            //}
            //else
            //{
            //    contentType = "application/octet-stream";
            //} 
            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(bytes, "application/vnd.openxmlformats", filename);

            //using Process fileopener = new Process(); 
            //fileopener.StartInfo.FileName = "explorer";
            //fileopener.StartInfo.Arguments = "\"" + directoryrma + "\"";
            //fileopener.Start(); 
        }
        [HttpGet]
        public IActionResult LoadFilesByRMA(int rmaid)
        {
            var files = _context2.CSEXSW_Attachmentrma.Where(s => s.RmaId == rmaid).OrderByDescending(r => r.Id).ToList();
            return Json(files);
        }
        [HttpGet]
        public IActionResult DeleteFileRMA(int id, string rma, string filename)
        {
            var directoryfile = Path.Combine(rootEC, $@"documents\RMA\{rma}\Attachments\{filename}");
            if (_context2.CSEXSW_Attachmentrma.Any(s => s.Id == id))
            {
                var file = _context2.CSEXSW_Attachmentrma.Where(S => S.Id == id).FirstOrDefault();
                _context2.CSEXSW_Attachmentrma.Remove(file);
                var deleted = _context2.SaveChanges();
                if (deleted > 0)
                {
                    if (System.IO.File.Exists(directoryfile))
                    {
                        System.IO.File.Delete(directoryfile);
                    }
                }
            }

            return Json(null);
        }

        [HttpPost]
        public IActionResult SaveFileRMA()
        {
            bool inserted = false;
            //var files = HttpContext.Request.Form.Files;
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                string rmano = Request.Form["rma"];
                string fname = Request.Form["file"];
                var directoryrma = Path.Combine(rootEC, $@"documents\RMA\{rmano}\Attachments\");
                int idrma = _context2.CSEXSW_Rma.Where(s => s.Rmarequest.Trim() == rmano.Trim()).FirstOrDefault().Id;

                if (!Directory.Exists(directoryrma))
                {
                    Directory.CreateDirectory(directoryrma);
                }
                for (int i = 0; i < files.Count; i++)
                {
                    string nombrearchivo = Path.GetFileName(files[i].FileName);
                    using (var fs = new FileStream(Path.Combine(directoryrma, nombrearchivo), FileMode.Create))
                    {
                        files[i].CopyTo(fs);
                        _context2.CSEXSW_Attachmentrma.Add(new CSEXSW_Attachmentrma()
                        {
                            Documento = nombrearchivo,
                            RmaId = idrma
                        });
                        _context2.SaveChanges();
                    }
                }
                inserted = true;
            }
            return Json(inserted);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadReport(string rma)
        {
            if (string.IsNullOrWhiteSpace(rma))
            {
                return BadRequest("An RMA number is required.");
            }

            ReportDownloadDto report = await _reportService.GenerateReturnMaterialsAuthorizationPDF(rma);

            return File(report.Data, report.ContentType, report.FileName);
        }


        // [HttpPost]

        // public IActionResult Edit(int id, string Rmarequest, string rmastatus, string rmaapprover, string rmasumbit, string rmadata, string rmawherebuilt, double total, string client, string rmatypeofrequest, string description, string customercomplait, string rma500, string customerpo, string shipto, string contact, string phone, string ext, string fax, string contactemail, string companyemail, string comment,
        // bool finalizado, bool inicio, decimal[] acttion, string[] invoice, short[] seq, string[] coustumer, decimal[] qty, decimal[] unit, string[] code, string[] checkcar, string[] loc)
        // {
        //     var arcusfil_sql = new arcusfil_sql();

        //     double tipo_cambio = total;
        //     arcusfil_sql = _context.arcusfil_sql.Where(a => a.cus_no == client).FirstOrDefault();

        //     var moneda = arcusfil_sql.curr_cd;


        //     if (moneda == "CNY")
        //     {

        //         var rate = _context.Rate.Where(a => a.DateL == _context.Rate.Max(a => a.DateL) && a.SourceCurrency == "USD").FirstOrDefault();
        //         tipo_cambio = Convert.ToDouble(total) / Convert.ToDouble(rate.RateExchange);


        //     }


        //     csexsw_coustumerVM rma = new csexsw_coustumerVM()
        //     {
        //         CSEXSW_Rma = new Models.CSEXSW_Rma(),

        //     };
        //     rma.CSEXSW_Rma.Approver = rmaapprover;
        //     rma.CSEXSW_Rma.Contact = contact;
        //     rma.CSEXSW_Rma.Email = contactemail;

        //     rma.CSEXSW_Rma.Ext = ext;
        //     rma.CSEXSW_Rma.Company = companyemail;
        //     rma.CSEXSW_Rma.Fax = fax;
        //     rma.CSEXSW_Rma.Phone = phone;



        //     rma.CSEXSW_Rma.Wherebuilt = rmawherebuilt;
        //     rma.CSEXSW_Rma.Ship_To = shipto;
        //     rma.CSEXSW_Rma.Customercomplait = customercomplait;
        //     rma.CSEXSW_Rma.Customerpartno = customerpo;
        //     rma.CSEXSW_Rma.Customerpo = customerpo;
        //     rma.CSEXSW_Rma.Date = rmadata;
        //     rma.CSEXSW_Rma.Description = description;
        //     rma.CSEXSW_Rma.Preparado = "";
        //     rma.CSEXSW_Rma.RMA500 = rma500;
        //     rma.CSEXSW_Rma.Sumbit = rmasumbit;
        //     rma.CSEXSW_Rma.Rmarequest = Rmarequest;
        //     rma.CSEXSW_Rma.Id = id;
        //     rma.CSEXSW_Rma.Rmatypeofrequest = rmatypeofrequest;
        //     rma.CSEXSW_Rma.Customer = client;
        //     rma.CSEXSW_Rma.turno = "";
        //     rma.CSEXSW_Rma.Status = rmastatus;
        //     rma.CSEXSW_Rma.Comment = comment;
        //     rma.CSEXSW_Rma.Totalrmavalues = double.Parse(total.ToString("#.####"));

        //     String cadena = User.Identity.Name;
        //     string delimitador = @"\";
        //     string[] valores = cadena.Split(delimitador);
        //     string usuario = valores[1];


        //     string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());
        //     rma.CSEXSW_Rma.Preparado = var.Trim();
        //     rma.CSEXSW_Rma.Status = "Pending";
        //     rma.CSEXSW_Rma.Sumbit = "Submitted";
        //     string rutaPrincipal = _hostingEnvironment.WebRootPath;
        //     var archivos = HttpContext.Request.Form.Files;
        //     int idRMA = id;


        //     if (inicio == true)
        //     {

        //         if (ModelState.IsValid)
        //         {


        //             string QM = _context2.CSEXSW_Approver.Where(a => a.Rango == "QM").Select(a => a.Approver).FirstOrDefault();
        //             string QD = _context2.CSEXSW_Approver.Where(a => a.Rango == "QD").Select(a => a.Approver).FirstOrDefault();
        //             string GM = _context2.CSEXSW_Approver.Where(a => a.Rango == "GM").Select(a => a.Approver).FirstOrDefault();
        //             string Dee = _context2.CSEXSW_Approver.Where(a => a.Rango == "Dee").Select(a => a.Approver).FirstOrDefault();
        //             string Controller = _context2.CSEXSW_Approver.Where(a => a.Rango == "Controller").Select(a => a.Approver).FirstOrDefault();
        //             string CSM = _context2.CSEXSW_Approver.Where(a => a.Rango == "CSM").Select(a => a.Approver).FirstOrDefault();
        //             string mail1 = _context2.humres.Where(a => a.fullname == QM).Select(a => a.mail).FirstOrDefault();
        //             string mail2 = _context2.humres.Where(a => a.fullname == QD).Select(a => a.mail).FirstOrDefault();
        //             string mail3 = _context2.humres.Where(a => a.fullname == GM).Select(a => a.mail).FirstOrDefault();
        //             string mail4 = _context2.humres.Where(a => a.fullname == Dee).Select(a => a.mail).FirstOrDefault();
        //             string mail5 = _context2.humres.Where(a => a.fullname == Controller).Select(a => a.mail).FirstOrDefault();
        //             string mail6 = _context2.humres.Where(a => a.fullname == CSM).Select(a => a.mail).FirstOrDefault();
        //             string mailp = _context2.humres.Where(a => a.fullname == var).Select(a => a.mail).FirstOrDefault();



        //             if (rma.CSEXSW_Rma.Wherebuilt == "Nogales" || rma.CSEXSW_Rma.Wherebuilt == "Endicott")
        //             {
        //                 if (rma.CSEXSW_Rma.Rmatypeofrequest == "VALUE ADD RMA" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT & REPLACE" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT ONLY")
        //                 {
        //                     if (tipo_cambio <= 4999)
        //                     {
        //                         rma.CSEXSW_Rma.Approver = QM;

        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));



        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }




        //                         BackgroundJob.Schedule(() => ScheduleJob(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));




        //                     }

        //                     if (tipo_cambio >= 5000 && tipo_cambio <= 19999.99)
        //                     {

        //                         rma.CSEXSW_Rma.Approver = QD;

        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));





        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }



        //                         BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     }

        //                     if (tipo_cambio >= 20000)
        //                     {
        //                         rma.CSEXSW_Rma.Approver = QD;

        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));




        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }



        //                         BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     }
        //                 }

        //                 if (rma.CSEXSW_Rma.Rmatypeofrequest == "DISTY SCRAP ALLOWANCE")

        //                 {
        //                     //if (rma.CSEXSW_Rma.RMA500 == "Yes")
        //                     //{
        //                     rma.CSEXSW_Rma.Approver = QD;

        //                     BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                     _contenedorTrabajo.Save();
        //                     if (archivos.Count() > 0)
        //                     {
        //                         if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                         {


        //                             Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                         }
        //                         //Editamos imagen
        //                         for (int i = 0; i < archivos.Count(); i++)
        //                         {

        //                             string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                             var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                             var extension = Path.GetExtension(archivos[i].FileName);
        //                             var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                             string archivodocumento = nombreArchivo;
        //                             //subimos nuevamente el archivo
        //                             using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                             {
        //                                 archivos[i].CopyTo(fileStreams);
        //                                 _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                 _contenedorTrabajo.Save();
        //                             }






        //                         }
        //                     }



        //                     BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                        }
        //                     //                        else
        //                     //                        {
        //                     //                            if (tipo_cambio <= 4999)
        //                     //                            {
        //                     //                                var nextrma = (from c in _context.OERMACTL_SQL
        //                     //                                               where c.ID == 1
        //                     //                                               select c.ctl_next_order_no).First();


        //                     //                                int x = Int32.Parse(nextrma);

        //                     //                                string numString = x.ToString();
        //                     //                                x = x + 1;
        //                     //                                var numeroFormato = x.ToString("D8");
        //                     //                                _context.SaveChanges();


        //                     //                                var objDesdeDbz = _context.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);
        //                     //                                objDesdeDbz.ctl_next_order_no = numeroFormato;
        //                     //                                rma.CSEXSW_Rma.Approver = "";
        //                     //                                rma.CSEXSW_Rma.turno = numString;
        //                     //                                rma.CSEXSW_Rma.Status = "Approved";




        //                     //                                string cus = "";
        //                     //                                char pad = ' ';
        //                     //                                if (rma.CSEXSW_Rma.Customer != null)
        //                     //                                {
        //                     //                                    cus = rma.CSEXSW_Rma.Customer.PadRight(20, pad);
        //                     //                                }


        //                     //                                string retorno = _contenedorTrabajo.OERDTFIL_SQL.lineas(rma.CSEXSW_Rma, idRMA, numString, cus, acttion, invoice, seq, qty, coustumer, code, unit, checkcar, loc);


        //                     //                                _contenedorTrabajo.Save();

        //                     //                                if (retorno == "Approved")
        //                     //                                {
        //                     //                                    int idCAR = 0;

        //                     //                                    ArrayList objs = new ArrayList();
        //                     //                                    string connectionString = ConnectionM10.Connection;
        //                     //                                    var values = new List<Dictionary<string, object>>();
        //                     //                                    using (SqlConnection cn = new SqlConnection(connectionString))
        //                     //                                    {
        //                     //                                        cn.Open();
        //                     //                                        string query = @"SELECT IDENT_CURRENT('csexsw_car') as IdActual,
        //                     //IDENT_SEED('csexsw_car') as IdInicial,
        //                     //IDENT_INCR('csexsw_car') as Incremento";
        //                     //                                        SqlCommand cmd = new SqlCommand(query, cn);

        //                     //                                        SqlDataReader rdr = cmd.ExecuteReader();


        //                     //                                        //get the data reader, etc.
        //                     //                                        while (rdr.Read())
        //                     //                                        {
        //                     //                                            objs.Add(new
        //                     //                                            {
        //                     //                                                IdActual = rdr["IdActual"],
        //                     //                                                IdInicial = rdr["IdInicial"],
        //                     //                                                Incremento = rdr["Incremento"]


        //                     //                                            });


        //                     //                                            idCAR = Convert.ToInt32(rdr["IdActual"]);
        //                     //                                        }



        //                     //                                        cn.Close();
        //                     //                                    }

        //                     //                                    var objDesdeDblinea = _context2.csexsw_coustumer.Where(s => s.RmaId == idRMA).ToList();
        //                     //                                    var checkcars = _context2.csexsw_coustumer.Count(r => r.RmaId == idRMA && r.Car == true);
        //                     //                                    if (checkcars > 0)
        //                     //                                    {






        //                     //                                        idCAR = idCAR + 1;
        //                     //                                        var objDesdeDb = _context2.CSEXSW_Rma.FirstOrDefault(s => s.Id == idRMA);

        //                     //                                        string numString2 = idCAR.ToString();
        //                     //                                        objDesdeDb.Car = objDesdeDb.Car + "  " + numString2;
        //                     //                                        _context.SaveChanges();
        //                     //                                        string Client = (from c in _context.arcusfil_sql
        //                     //                                                         where c.cus_no.Contains(objDesdeDb.Customer)
        //                     //                                                         select c.cus_name).First();
        //                     //                                        var objDesdeDbt = new csexsw_car();
        //                     //                                        objDesdeDbt.customercode = objDesdeDb.Customer;
        //                     //                                        objDesdeDbt.Status = "In Process";
        //                     //                                        DateTime fecha = DateTime.Now;
        //                     //                                        objDesdeDbt.Issue_date = fecha;
        //                     //                                        objDesdeDbt.Owner = "8";
        //                     //                                        objDesdeDbt.owner_name = "Benjamin Cervantes";
        //                     //                                        objDesdeDbt.Internalduedate = fecha.AddDays(30);
        //                     //                                        objDesdeDbt.Responsabledate = fecha.AddDays(30);
        //                     //                                        objDesdeDbt.customername = Client;
        //                     //                                        objDesdeDbt.Defectcode = "";
        //                     //                                        objDesdeDbt.Partnumber = "";
        //                     //                                        objDesdeDbt.Rmanumber = objDesdeDb.turno;
        //                     //                                        objDesdeDbt.po = objDesdeDb.Customerpo;

        //                     //                                        objDesdeDbt.Notes = "RMA Requests No" + objDesdeDb.Rmarequest + "Description: " + objDesdeDb.Description + " Customer complait: " + objDesdeDb.Customercomplait + " Approver: " + objDesdeDb.Approver + " RMA type of request: " + objDesdeDb.Rmatypeofrequest + " Where built: " + objDesdeDb.Wherebuilt;
        //                     //                                        BackgroundJob.Enqueue(() => crearcar(objDesdeDbt, idCAR));



        //                     //                                    }
        //                     //                                }

        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                            }




        //                     //                            if (tipo_cambio >= 20000)
        //                     //                            {
        //                     //                                rma.CSEXSW_Rma.Approver = QD;

        //                     //                                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                     //                                _contenedorTrabajo.Save();
        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                            }

        //                     //                            if (tipo_cambio >= 5000 && tipo_cambio <= 19999.99)
        //                     //                            {
        //                     //                                rma.CSEXSW_Rma.Approver = QD;

        //                     //                                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                     //                                _contenedorTrabajo.Save();
        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                            }


        //                     //                        }
        //                 }

        //             }

        //             if (rma.CSEXSW_Rma.Wherebuilt == "ATZ" || rma.CSEXSW_Rma.Wherebuilt == "MAO")
        //             {
        //                 if (rma.CSEXSW_Rma.Rmatypeofrequest == "VALUE ADD RMA" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT & REPLACE" || rma.CSEXSW_Rma.Rmatypeofrequest == "CREDIT ONLY")
        //                 {
        //                     if (tipo_cambio <= 4999)
        //                     {
        //                         rma.CSEXSW_Rma.Approver = QM;
        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }



        //                         BackgroundJob.Schedule(() => ScheduleJob(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     }

        //                     if (tipo_cambio >= 5000 && tipo_cambio <= 19999.99)
        //                     {
        //                         rma.CSEXSW_Rma.Approver = QD;
        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }



        //                         BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));






        //                     }

        //                     if (tipo_cambio >= 20000)
        //                     {

        //                         rma.CSEXSW_Rma.Approver = QD;
        //                         BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                         _contenedorTrabajo.Save();
        //                         if (archivos.Count() > 0)
        //                         {
        //                             if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                             {


        //                                 Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                             }
        //                             //Editamos imagen
        //                             for (int i = 0; i < archivos.Count(); i++)
        //                             {

        //                                 string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                                 var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                                 var extension = Path.GetExtension(archivos[i].FileName);
        //                                 var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                                 string archivodocumento = nombreArchivo;
        //                                 //subimos nuevamente el archivo
        //                                 using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                                 {
        //                                     archivos[i].CopyTo(fileStreams);
        //                                     _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                     _contenedorTrabajo.Save();
        //                                 }






        //                             }
        //                         }



        //                         BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));



        //                     }
        //                 }

        //                 if (rma.CSEXSW_Rma.Rmatypeofrequest == "DISTY SCRAP ALLOWANCE")

        //                 {
        //                     //if (rma.CSEXSW_Rma.RMA500 == "Yes")

        //                     //{
        //                     rma.CSEXSW_Rma.Approver = QD;
        //                     BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                     _contenedorTrabajo.Save();
        //                     if (archivos.Count() > 0)
        //                     {
        //                         if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                         {


        //                             Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                         }
        //                         //Editamos imagen
        //                         for (int i = 0; i < archivos.Count(); i++)
        //                         {

        //                             string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                             var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                             var extension = Path.GetExtension(archivos[i].FileName);
        //                             var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                             string archivodocumento = nombreArchivo;
        //                             //subimos nuevamente el archivo
        //                             using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                             {
        //                                 archivos[i].CopyTo(fileStreams);
        //                                 _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                                 _contenedorTrabajo.Save();
        //                             }






        //                         }
        //                     }



        //                     BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                        }
        //                     //                        else
        //                     //                        {
        //                     //                            if (tipo_cambio <= 4999)
        //                     //                            {
        //                     //                                var nextrma = (from c in _context.OERMACTL_SQL
        //                     //                                               where c.ID == 1
        //                     //                                               select c.ctl_next_order_no).First();


        //                     //                                int x = Int32.Parse(nextrma);

        //                     //                                string numString = x.ToString();
        //                     //                                x = x + 1;
        //                     //                                var numeroFormato = x.ToString("D8");
        //                     //                                _context.SaveChanges();


        //                     //                                var objDesdeDbz = _context.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

        //                     //                                objDesdeDbz.ctl_next_order_no = numeroFormato;
        //                     //                                rma.CSEXSW_Rma.Approver = "";
        //                     //                                rma.CSEXSW_Rma.turno = numString;
        //                     //                                rma.CSEXSW_Rma.Status = "Approved";

        //                     //                                idRMA = rma.CSEXSW_Rma.Id;


        //                     //                                string cus = " ";
        //                     //                                char pad = ' ';
        //                     //                                if (rma.CSEXSW_Rma.Customer != null)
        //                     //                                {
        //                     //                                    cus = rma.CSEXSW_Rma.Customer.PadRight(20, pad);
        //                     //                                }





        //                     //                                string retorno = _contenedorTrabajo.OERDTFIL_SQL.lineas(rma.CSEXSW_Rma, idRMA, numString, cus, acttion, invoice, seq, qty, coustumer, code, unit, checkcar, loc);

        //                     //                                if (retorno == "Approved")
        //                     //                                {
        //                     //                                    int idCAR = 0;

        //                     //                                    ArrayList objs = new ArrayList();
        //                     //                                    string connectionString = _configuration.GetConnectionString("ConnectionM10").ToString();
        //                     //                                    var values = new List<Dictionary<string, object>>();
        //                     //                                    using (SqlConnection cn = new SqlConnection(connectionString))
        //                     //                                    {
        //                     //                                        cn.Open();
        //                     //                                        string query = @"SELECT IDENT_CURRENT('csexsw_car') as IdActual,
        //                     //IDENT_SEED('csexsw_car') as IdInicial,
        //                     //IDENT_INCR('csexsw_car') as Incremento";
        //                     //                                        SqlCommand cmd = new SqlCommand(query, cn);

        //                     //                                        SqlDataReader rdr = cmd.ExecuteReader();


        //                     //                                        //get the data reader, etc.
        //                     //                                        while (rdr.Read())
        //                     //                                        {
        //                     //                                            objs.Add(new
        //                     //                                            {
        //                     //                                                IdActual = rdr["IdActual"],
        //                     //                                                IdInicial = rdr["IdInicial"],
        //                     //                                                Incremento = rdr["Incremento"]


        //                     //                                            });


        //                     //                                            idCAR = Convert.ToInt32(rdr["IdActual"]);
        //                     //                                        }



        //                     //                                        cn.Close();
        //                     //                                    }

        //                     //                                    var objDesdeDblinea = _context2.csexsw_coustumer.Where(s => s.RmaId == idRMA).ToList();
        //                     //                                    var checkcars = _context2.csexsw_coustumer.Count(r => r.RmaId == idRMA && r.Car == true);
        //                     //                                    if (checkcars > 0)
        //                     //                                    {




        //                     //                                        idCAR = idCAR + 1;
        //                     //                                        var objDesdeDb = _context2.CSEXSW_Rma.FirstOrDefault(s => s.Id == idRMA);

        //                     //                                        string numString2 = idCAR.ToString();
        //                     //                                        objDesdeDb.Car = objDesdeDb.Car + "  " + numString2;
        //                     //                                        _context.SaveChanges();
        //                     //                                        string Client = (from c in _context.arcusfil_sql
        //                     //                                                         where c.cus_no.Contains(objDesdeDb.Customer)
        //                     //                                                         select c.cus_name).First();
        //                     //                                        var objDesdeDbt = new csexsw_car();
        //                     //                                        objDesdeDbt.customercode = objDesdeDb.Customer;
        //                     //                                        objDesdeDbt.Status = "In Process";
        //                     //                                        DateTime fecha = DateTime.Now;
        //                     //                                        objDesdeDbt.Issue_date = fecha;
        //                     //                                        objDesdeDbt.Owner = "8";
        //                     //                                        objDesdeDbt.owner_name = "Benjamin Cervantes";
        //                     //                                        objDesdeDbt.Internalduedate = fecha.AddDays(30);
        //                     //                                        objDesdeDbt.Responsabledate = fecha.AddDays(30);
        //                     //                                        objDesdeDbt.Partnumber = " ";
        //                     //                                        objDesdeDbt.customername = Client;
        //                     //                                        objDesdeDbt.Defectcode = "";
        //                     //                                        objDesdeDbt.Partnumber = "";
        //                     //                                        objDesdeDbt.Rmanumber = objDesdeDb.turno;
        //                     //                                        objDesdeDbt.po = objDesdeDb.Customerpo;

        //                     //                                        objDesdeDbt.Notes = "RMA Requests No" + objDesdeDb.Rmarequest + "Description: " + objDesdeDb.Description + " Customer complait: " + objDesdeDb.Customercomplait + " Approver: " + objDesdeDb.Approver + " RMA type of request: " + objDesdeDb.Rmatypeofrequest + " Where built: " + objDesdeDb.Wherebuilt;
        //                     //                                        BackgroundJob.Enqueue(() => crearcar(objDesdeDbt, idCAR));


        //                     //                                    }
        //                     //                                }


        //                     //                                _contenedorTrabajo.Save();
        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                            }




        //                     //                            if (tipo_cambio >= 20000)
        //                     //                            {
        //                     //                                rma.CSEXSW_Rma.Approver = QD;

        //                     //                                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));


        //                     //                                _contenedorTrabajo.Save();
        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));



        //                     //                            }


        //                     //                            if ((tipo_cambio >= 5000 && tipo_cambio <= 19999.99))

        //                     //                            {
        //                     //                                rma.CSEXSW_Rma.Approver = QD;


        //                     //                                BackgroundJob.Enqueue(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null));

        //                     //                                _contenedorTrabajo.Save();
        //                     //                                if (archivos.Count() > 0)
        //                     //                                {
        //                     //                                    if (!Directory.Exists(Path.Combine(rutaPrincipal, @"documents\documents\rma\")))
        //                     //                                    {


        //                     //                                        Directory.CreateDirectory(Path.Combine(rutaPrincipal, @"documents\documents\rma\"));

        //                     //                                    }
        //                     //                                    //Editamos imagen
        //                     //                                    for (int i = 0; i < archivos.Count(); i++)
        //                     //                                    {

        //                     //                                        string nombreArchivo = Path.GetFileName(archivos[i].FileName);
        //                     //                                        var subidas = Path.Combine(rutaPrincipal, @"documents\documents\rma\");
        //                     //                                        var extension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        var nuevaExtension = Path.GetExtension(archivos[i].FileName);
        //                     //                                        string archivodocumento = nombreArchivo;
        //                     //                                        //subimos nuevamente el archivo
        //                     //                                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo), FileMode.Create))
        //                     //                                        {
        //                     //                                            archivos[i].CopyTo(fileStreams);
        //                     //                                            _contenedorTrabajo.csexsw_coustumer.archivos(archivodocumento, idRMA);
        //                     //                                            _contenedorTrabajo.Save();
        //                     //                                        }






        //                     //                                    }
        //                     //                                }



        //                     //                                BackgroundJob.Schedule(() => ScheduleJob2(mail1, mail2, mail3, mail4, mail5, mail6, mailp, rma.CSEXSW_Rma.Rmarequest), TimeSpan.FromMinutes(5));


        //                     //                            }


        //                 }








        //             }







        //         }

        //         _contenedorTrabajo.CSEXSW_Rma.Update(rma.CSEXSW_Rma);
        //         _contenedorTrabajo.Save();

        //         return Json(new { data = "Primeras lineas" });


        //     }

        //     if (finalizado != true)
        //     {



        //         BackgroundJob.Schedule(() => _contenedorTrabajo.csexsw_coustumer.lineas(rma.CSEXSW_Rma, acttion, invoice, seq, qty, coustumer, idRMA, code, unit, checkcar, loc, inicio, null),
        //TimeSpan.FromSeconds(10));



        //         return Json(new { data = "Lineas creadas" });

        //     }


        //     return Json(new { data = "Terminado" });


        // }

        public string get_cus_name(string id)
        {
            string cli = string.Empty;
            if (_context.arcusfil_sql.Any(s => s.cus_no.Trim() == id.Trim()))
            {
                cli = _context.arcusfil_sql.Where(s => s.cus_no.Trim() == id.Trim()).FirstOrDefault().cus_name.Trim();
            }
            return cli;
        }
        public IActionResult ExportarExcel()
        {
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";



            //var productos = _context2.CSEXSW_Rma.Select(x => new { Rma_request = x.Rmarequest,Customer= x.Customer, x.Car,Approved=x.date_approved.Value.ToString("MM/dd/yy"), RMA = x.turno, Where_built = x.Wherebuilt, Total_rma_values = x.Totalrmavalues, x.Sumbit, x.Status, Rma_type_of_request = x.Rmatypeofrequest, x.Preparado, x.Date, x.Description, Customer_po = x.Customerpo, Customer_complait = x.Customercomplait, x.Approver }).AsNoTracking().ToList();
            //var productos = _context2.CSEXSW_Rma.Select(x => new { Rma_request = x.Rmarequest, x.Car, RMA = x.turno, Where_built = x.Wherebuilt, Total_rma_values = x.Totalrmavalues, x.Sumbit, x.Status, Rma_type_of_request = x.Rmatypeofrequest, x.Preparado, x.Date, x.Description, Customer_po = x.Customerpo, Customer_complait = x.Customercomplait, x.Customer, x.Approver }).AsNoTracking().ToList();
            var productos = _context2.csexsw_coustumer.Include(r => r.CSEXSW_Rma).Select(x => new { Rma_request = x.CSEXSW_Rma.Rmarequest, Part_Number = x.Coustumer, Qty = x.Qty, Valor = x.Qty * x.Unit, cus_no = x.CSEXSW_Rma.Customer, x.Car, Approved = x.CSEXSW_Rma.date_approved.Value.ToString("MM/dd/yy"), RMA = x.CSEXSW_Rma.turno, Where_built = x.CSEXSW_Rma.Wherebuilt, Total_rma_values = x.CSEXSW_Rma.Totalrmavalues, x.CSEXSW_Rma.Sumbit, x.CSEXSW_Rma.Status, Rma_type_of_request = x.CSEXSW_Rma.Rmatypeofrequest, x.CSEXSW_Rma.Preparado, x.CSEXSW_Rma.Date, x.CSEXSW_Rma.Description, Customer_po = x.CSEXSW_Rma.Customerpo, Customer_complait = x.CSEXSW_Rma.Customercomplait, x.CSEXSW_Rma.Approver }).ToList();
            productos = productos.OrderByDescending(r => r.Date).ToList();


            using (var libro = new ExcelPackage())
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var worksheet = libro.Workbook.Worksheets.Add("RMARequest");

                var excelImage = worksheet.Drawings.AddPicture("My Logo", Path.Combine(rutaPrincipal, @"aio2.jpg"));

                //add the image to row 20, column E
                excelImage.SetPosition(0, 0, 0, 0);

                worksheet.Cells["A4"].LoadFromCollection(productos, PrintHeaders: true);
                for (var col = 1; col < productos.Count + 1; col++)
                {

                    worksheet.Column(col).AutoFit();

                }

                // Agregar formato de tabla
                var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 4, fromCol: 1, toRow: productos.Count + 4, toColumn: 19), "CSEXSW_RMA");
                tabla.ShowHeader = true;
                tabla.TableStyle = TableStyles.Light8;


                return File(libro.GetAsByteArray(), excelContentType, "RMARequest.xlsx");
            }
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdateLinea(int id, string actionn, string loc, decimal qty, decimal price, decimal unitcost, string rcode, bool car)
        {
            var linea = _context2.csexsw_coustumer.Where(s => s.Id == id).FirstOrDefault();
            linea.Action = actionn;
            linea.Loc = loc;
            linea.Qty = qty;
            linea.Cost = price;
            linea.Unit = unitcost;
            linea.Retur = rcode;
            linea.Car = car;
            int result = 0;
            _context2.Entry(linea).State = EntityState.Modified;
            result = _context2.SaveChanges();
            return Json(result > 0 ? true : false);

        }
        [HttpGet]
        public IActionResult UpdateRMA(int id, string where, string desc, string phone, string ext, string fax, float totalrma, string po, string contact, string type, string email, string comments, string reason)
        {
            var rma = _context2.CSEXSW_Rma.Where(S => S.Id == id).FirstOrDefault();
            rma.Wherebuilt = where;
            rma.Description = desc;
            rma.Phone = phone;
            rma.Ext = ext;
            rma.Fax = fax;
            rma.Totalrmavalues = double.Parse(totalrma.ToString("#.####"));
            rma.Customerpo = po;
            rma.Contact = contact;
            rma.Rmatypeofrequest = type.ToString();
            rma.Email = email;
            rma.Comment = comments;
            rma.reason = reason;
            _context2.Entry(rma).State = EntityState.Modified;
            var result = _context2.SaveChanges();
            return Json(result > 0 ? true : false);

        }

        [HttpGet]
        public async Task<IActionResult> RmaModalAsync(int id, string mode)
        {
            var partialView = "RmaDetailsModalView";
            if (id < 1 || string.IsNullOrWhiteSpace(mode))
            {
                return PartialView(partialView, new RmaViewModel());
            }

            var rma = _context2.CSEXSW_Rma
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (rma == null)
            {
                return PartialView(partialView, new RmaViewModel());
            }

            string directory = Path.Combine(rootEC, "documents", "RMA", rma.Rmarequest.Trim(), "Attachments");


            var viewModel = new RmaViewModel
            {
                Attachments = await _context2.CSEXSW_Attachmentrma
                     .AsNoTracking()
                     .Where(x => x.RmaId == rma.Id)
                     .Select(x => new RmaAttachmentViewModel
                     {
                         Id = x.Id,
                         FileName = x.Documento
                     }).ToListAsync(),
                Comments = rma.Comment,
                CompanyEmail = rma.Company,
                Complaint = rma.Customercomplait,
                Contact = rma.Contact,
                ContactEmail = rma.Email,
                CustomerId = rma.Customer,
                Date = Convert.ToDateTime(rma.Date),
                Description = rma.Description,
                ExtensionNumber = rma.Ext,
                Fax = rma.Fax,
                Lines = await _context2.csexsw_coustumer
                     .AsNoTracking()
                     .Where(x => x.CSEXSW_Rma == rma)
                     .Select(x => new RmaLineViewModel
                     {
                         Id = x.Id,
                         AuthorizedQuantity = (int)x.Qty,
                         GenerateCAR = x.Car,
                         InvoiceNumber = x.Invoice,
                         PartNumber = x.Coustumer,
                         Price = x.Unit,
                         ReturnCode = x.Retur,
                         RmaRequestId = x.RmaId,
                         SelectedAction = x.Action,
                         SelectedLocation = x.Loc,
                         SequenceNumber = x.Seq,
                         UnitCost = x.Cost
                     }).ToListAsync(),
                PhoneNumber = rma.Phone,
                PoNumber = rma.Customerpo,
                RequestId = int.Parse(rma.Rmarequest),
                RequestStatus = RmaRequestStatusExtensions.FromDisplayString(rma.Status),
                SelectedBuildLocation = rma.Wherebuilt,
                SelectedReason = rma.reason,
                SelectedRequestType = rma.Rmatypeofrequest,
                ShipTo = rma.Ship_To,
                SubmitStatus = RmaSubmitStatusExtensions.FromDisplayString(rma.Sumbit)
            };


            return PartialView(partialView, viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadAttachment(int id)
        {
            var attachment = await _context2.CSEXSW_Attachmentrma
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (attachment == null)
            {
                return NotFound();
            }

            var rma = await _context2.CSEXSW_Rma
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == attachment.RmaId);

            if (rma == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(rootEC, "documents", "RMA", rma.Rmarequest.Trim(), "Attachments", attachment.Documento);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            return PhysicalFile(filePath, "application/octet-stream", attachment.Documento);
        }
        [HttpGet]
        public IActionResult AddLineRow()
        {
            ViewData["Index"] = "__INDEX__";

            return PartialView("RmaItemEditableLine", new RmaLineViewModel());
        }


        #region LLAMADAS A LA API
        [HttpGet]
        //RMA
        public IActionResult GetAllrma()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];


            string var = _contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim());

            usuario = var.Trim();

            //var rmas = _contenedorTrabajo.CSEXSW_Rma.GetAll().OrderByDescending(a => a.Date);
            List<CSEXSW_Rma_ViewModel> rmas = _contenedorTrabajo.CSEXSW_Rma.GetRMAListData();

            //var rmas = _context2.csexsw_coustumer.Include(r => r.CSEXSW_Rma).Where(a=>a.CSEXSW_Rma.Id >0).OrderByDescending(r => r.CSEXSW_Rma.Date );

            return Json(new { data = rmas, resid = usuario });
        }

        [HttpPut]
        public IActionResult Apruebo(int id)
        {

            _contenedorTrabajo.CSEXSW_Rma.UpdateAprove(id);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Approved RMA" });

        }

        [HttpPut]
        public IActionResult Rechazo(int id)
        {

            _contenedorTrabajo.CSEXSW_Rma.UpdateRechazo(id);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Approved RMA" });

        }
        //control rma
        [HttpGet]
        public IActionResult GetAllrma2()
        {
            String cadena = User.Identity.Name;
            string delimitador = @"\";
            string[] valores = cadena.Split(delimitador);
            string usuario = valores[1];

            int userId = int.Parse(_contenedorTrabajo.csexsw_dibujo.usuario(usuario.Trim()));

#if DEBUG
            var info = _contenedorTrabajo.CSEXSW_Rma
                .GetAll(a =>
                     a.Status == RmaRequestStatus.Pending.ToDisplayString() &&
                     a.Sumbit == RmaSubmitStatus.Submitted.ToDisplayString()
                ).OrderByDescending(a => a.Date);

#else
            var info = _contenedorTrabajo.CSEXSW_Rma
                .GetAll(a =>
                     a.Status == RmaRequestStatus.Pending.ToDisplayString() &&
                     a.Sumbit == RmaSubmitStatus.Submitted.ToDisplayString() &&
                     a.res_id_approver == userId
                ).OrderByDescending(a => a.Date);
#endif

            return Json(new { data = info });

        }
        [HttpGet]
        public IActionResult GetDetalles(int id, string fecha)
        {


            return Json(new { data = _contenedorTrabajo.CSEXSW_Rma.GetAll(a => a.Id == id).OrderByDescending(a => a.Date) });
        }

        [HttpGet]
        public IActionResult GetDocument(int id, string fecha)
        {
            string rmano = string.Empty;
            if (_context2.CSEXSW_Rma.Any(s => s.Id == id))
            {
                rmano = _context2.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault().Rmarequest;
            }

            return Json(new { data = _contenedorTrabajo.CSEXSW_Attachmentrma.GetAll(a => a.RmaId == id), rmano = rmano });
        }
        [HttpGet]
        public IActionResult comments(int id)
        {

            var data = new { data = _contenedorTrabajo.CSEXSW_Rma.GetAll(a => a.Id == id).Select(a => a.Comment) };
            return Json(data);
        }
        [HttpGet]
        public IActionResult GetAllitem()
        {


            return Json(new { data = _context.imitmidx_sql });
        }

        [HttpDelete]
        public IActionResult Deletedocument(int id)
        {


            var articuloDesdeDb = _contenedorTrabajo.CSEXSW_Attachmentrma.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;

            var ruta = @"\documents\documents\rma\" + articuloDesdeDb.Documento;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, ruta.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            var objFromDb = _contenedorTrabajo.CSEXSW_Attachmentrma.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting    document" });
            }

            _contenedorTrabajo.CSEXSW_Attachmentrma.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Document deleted successfully" });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.CSEXSW_Rma.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting RMA" });
            }

            _contenedorTrabajo.CSEXSW_Rma.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "RMA deleted successfully" });
        }

        [HttpPost]
        public IActionResult GetGeneratedOrderNumber(int id)
        {
            try
            {
                if (id <= 0 || !_context2.CSEXSW_Rma.Any(s => s.Id == id))
                {
                    return Json(new { message = "Could not find record", orderNumber = "", wasSuccessful = false });
                }
                var rmano = _context2.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault().turno;

                var existingOrder = _context.OEORDHDR_SQL
                    .AsNoTracking()
                    .Where(o => o.RmaNo.Trim() == rmano.Trim())
                    .FirstOrDefault();

                if (existingOrder is null) return Json(new { message = "Order has not been generated", orderNumber = "", wasSuccessful = true });

                return Json(new { message = "An order number has been already been generated for this RMA", orderNumber = existingOrder.OrdNo.Trim(), wasSuccessful = true });
            }
            catch (Exception)
            {
                return Json(new { message = "An unexpected error ocurred", orderNumber = "", wasSuccessful = false });
            }
        }
        #endregion

    }
}
