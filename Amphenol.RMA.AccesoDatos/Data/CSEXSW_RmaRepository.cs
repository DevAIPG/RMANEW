using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.ModelsM10;
using Amphenol.RMA.Models.ViewModels;
using DocumentFormat.OpenXml.InkML;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Organization.Client;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class CSEXSW_RmaRepository : Repository<CSEXSW_Rma>, ICSEXSW_RmaRepository
    {
        private readonly string _systemId = "-4";
        private readonly DbContextM10 _db;
        private readonly DbContext100 _db2;
        private Approver _autoApprover = new()
        {
            Id = -4,
            Name = "System"
        };
        private const double TwoStepAuthorizationThreshold = 20_000;
        private const double SmallRmaValueThreshold = 5_000;
        private readonly IConfiguration _configuration;

        public CSEXSW_RmaRepository(DbContextM10 db, DbContext100 db2, IConfiguration configuration) : base(db)
        {
            _db = db;
            _db2 = db2;
            _configuration = configuration;
        }



        public int Releaserma()
        {
            int id = 0;

            int cout = _db.CSEXSW_Rma.Count();

            if (cout != 0)
            {


                ArrayList objs = new ArrayList();
                string connectionString = _configuration.GetConnectionString("ConnectionM10").ToString();
                var values = new List<Dictionary<string, object>>();
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    string query = @"SELECT IDENT_CURRENT('CSEXSW_Rma') as IdActual,
    IDENT_SEED('CSEXSW_Rma') as IdInicial,
    IDENT_INCR('CSEXSW_Rma') as Incremento";
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


                        id = Convert.ToInt32(rdr["IdActual"]);
                    }



                    cn.Close();
                }





            }
            id = id + 1;
            return id;


        }

        public List<CSEXSW_Rma_ViewModel> GetRMAListData()
        {

            try
            {
                List<CSEXSW_Rma_ViewModel> result = new();

                string connectionString = _configuration.GetConnectionString("ConnectionM10");

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    string query = @"
                    Select
                        Sum(IsNull(csexsw_coustumer.Qty,0)) As qty,
                        Count(Distinct csexsw_coustumer.Coustumer) As parts,
                        CSEXSW_Rma.Id,
                        CSEXSW_Rma.Rmarequest,
                        CSEXSW_Rma.Date,
                        CSEXSW_Rma.Customerpartno,
                        CSEXSW_Rma.Customerpo,
                        CSEXSW_Rma.Customercomplait,
                        CSEXSW_Rma.Description,
                        CSEXSW_Rma.Rmatypeofrequest,
                        CSEXSW_Rma.Totalrmavalues,
                        CSEXSW_Rma.Wherebuilt,
                        CSEXSW_Rma.Preparado,
                        CSEXSW_Rma.Sumbit,
                        CSEXSW_Rma.Status,
                        CSEXSW_Rma.turno,
                        CSEXSW_Rma.Approver,
                        CSEXSW_Rma.res_id,
                        CONVERT(VARCHAR(10), CAST(CSEXSW_Rma.date_approved AS DATETIME), 101) As formated_date_approved
                    From CSEXSW_Rma With(NoLock)
                    Left Join csexsw_coustumer With(NoLock)
                        On CSEXSW_Rma.Id = csexsw_coustumer.RmaId
                    Group By
                        CSEXSW_Rma.Id,
                        CSEXSW_Rma.Rmarequest,
                        CSEXSW_Rma.Date,
                        CSEXSW_Rma.Customerpartno,
                        CSEXSW_Rma.Customerpo,
                        CSEXSW_Rma.Customercomplait,
                        CSEXSW_Rma.Description,
                        CSEXSW_Rma.Rmatypeofrequest,
                        CSEXSW_Rma.Totalrmavalues,
                        CSEXSW_Rma.Wherebuilt,
                        CSEXSW_Rma.Preparado,
                        CSEXSW_Rma.Sumbit,
                        CSEXSW_Rma.Status,
                        CSEXSW_Rma.turno,
                        CSEXSW_Rma.Approver,
                        CSEXSW_Rma.res_id,
                        CSEXSW_Rma.date_approved
                    Order By CSEXSW_Rma.Date";

                    using SqlCommand cmd = new(query, cn);
                    using SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        result.Add(new CSEXSW_Rma_ViewModel
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            qty = Convert.ToInt32(rdr["qty"]),
                            parts = Convert.ToInt32(rdr["parts"]),
                            Rmarequest = rdr["Rmarequest"]?.ToString(),
                            Date = rdr["Date"]?.ToString(),
                            Customerpartno = rdr["Customerpartno"]?.ToString(),
                            Customerpo = rdr["Customerpo"]?.ToString(),
                            Customercomplait = rdr["Customercomplait"]?.ToString(),
                            Description = rdr["Description"]?.ToString(),
                            Rmatypeofrequest = rdr["Rmatypeofrequest"]?.ToString(),
                            Totalrmavalues = rdr["Totalrmavalues"] == DBNull.Value
                                ? 0
                                : Convert.ToDouble(rdr["Totalrmavalues"]),
                            Wherebuilt = rdr["Wherebuilt"]?.ToString(),
                            Preparado = rdr["Preparado"]?.ToString(),
                            Sumbit = rdr["Sumbit"]?.ToString(),
                            Status = rdr["Status"]?.ToString(),
                            turno = rdr["turno"]?.ToString()?.Trim(),
                            Approver = rdr["Approver"]?.ToString(),
                            res_id = rdr["res_id"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(rdr["res_id"]),
                            formated_date_approved = rdr["formated_date_approved"]?.ToString()
                        });
                    }
                }

                if (!result.Any())
                    return result;

                var rmaNumbers = result
                    .Where(x => !string.IsNullOrWhiteSpace(x.turno))
                    .Select(x => x.turno)
                    .Distinct()
                    .ToList();

                //-----------------------------------------
                // Load all received lines ONE TIME
                //-----------------------------------------
                var receivedLinesLookup = _db2.OERDTFIL_SQL
                    .AsNoTracking()
                    .Where(x => rmaNumbers.Contains(x.rma_no.Trim()))
                    .ToList()
                    .GroupBy(x => x.rma_no.Trim())
                    .ToDictionary(g => g.Key, g => g.ToList());

                //-----------------------------------------
                // Load all orders ONE TIME
                //-----------------------------------------
                var orderLookup = _db2.OEORDHDR_SQL
                    .AsNoTracking()
                    .Where(x => rmaNumbers.Contains(x.RmaNo.Trim()))
                    .Select(x => new
                    {
                        RmaNo = x.RmaNo.Trim(),
                        x.OrdNo
                    })
                    .ToList()
                    .GroupBy(x => x.RmaNo)
                    .ToDictionary(
                        g => g.Key,
                        g => g.First().OrdNo
                    );

                //-----------------------------------------
                // Populate model properties
                //-----------------------------------------
                foreach (var model in result)
                {
                    if (string.IsNullOrWhiteSpace(model.turno))
                        continue;

                    if (receivedLinesLookup.TryGetValue(model.turno, out var lines))
                    {
                        model.CanGenerateOrder =
                            lines.Count > 0 &&
                            lines.All(x => x.RmaQtyRtnActual > 0) &&
                            model.Status == "Approved";
                    }
                    else
                    {
                        model.CanGenerateOrder = false;
                    }

                    if (orderLookup.TryGetValue(model.turno, out var orderNo))
                    {
                        model.OrderNumber = orderNo;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // log exception
                //_logger.LogError(ex, "Error loading RMA list");

                return [];
            }
        }

        //public async Task<bool> SendMailAsync5(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        //{




        //    var objDesdeDbs = _db.CSEXSW_Rma.Where(s => s.Rmarequest == rmarequest).FirstOrDefault();



        //    string coustumercorreo = "";
        //    var objDesdeDbL = _db.csexsw_coustumer.Where(s => s.RmaId == objDesdeDbs.Id).ToList();

        //    if (objDesdeDbL.Count() > 0)
        //    {

        //        foreach (var filtro in objDesdeDbL)
        //        {
        //            coustumercorreo = coustumercorreo + " " + filtro.Coustumer;
        //        }

        //    }
        //    string encabezado = "RMA Approval has been rejected: " + objDesdeDbs.Rmarequest + "";
        //    string mensaje = "<p> RMA has been rejected:  &nbsp; " + objDesdeDbs.Rmarequest + "<br />" + objDesdeDbs.Preparado + " at  &nbsp; " + objDesdeDbs.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbs.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbs.Date + "<br />Customer: &nbsp; " + objDesdeDbs.Customer + "<br />Customer Part #: &nbsp; " + coustumercorreo + "<br />Customer PO #: &nbsp; " + objDesdeDbs.Customerpo + " <br />Description: &nbsp;" + objDesdeDbs.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbs.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbs.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbs.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbs.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbs.Approver + "<br />Reject Comments: &nbsp;" + objDesdeDbs.Comment + "</p>";
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage();
        //        oMailMessagep.IsBodyHtml = true;


        //        // Rellenamos el mensaje
        //        oMailMessagep.From = new MailAddress(EmailOrigen);  // Remitente
        //        oMailMessagep.To.Add(new MailAddress(mail1));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail2));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail3));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail4));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail5));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail6));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mailp));    // Destino

        //        //oMailMessagep.CC.Add(new MailAddress();  // Destino (en Carbon Copy)
        //        oMailMessagep.Bcc.Add(new MailAddress("ccorona@amphenol-aio.com"));    // Destino (en Blind Carbon Copy, oculto)

        //        // Asunto y cuerpo
        //        oMailMessagep.Subject = encabezado;
        //        oMailMessagep.Body = mensaje;


        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbs.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    await Task.Delay(10000);
        //    return true;
        //}

        //public async Task<bool> SendMailAsync4(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        //{
        //    var objDesdeDbR = _db.CSEXSW_Rma.Where(s => s.Rmarequest == rmarequest).FirstOrDefault();


        //    string result = "";
        //    var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //    if (objDesdeDbT.Count() > 0)
        //    {

        //        foreach (var filtro in objDesdeDbT)
        //        {
        //            result = result + " " + filtro.Coustumer;
        //        }

        //    }


        //    string EmailDestino = mailp;
        //    string encabezadoaprobador = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensajeaprobador = "<p>Review by your Global Marketing Dir role</p><br/><p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";

        //    string mailaprobador = "";
        //    string connectionString = _configuration.GetConnectionString("Connection100").ToString();
        //    using (SqlConnection cn2 = new SqlConnection(connectionString))
        //    {
        //        cn2.Open();
        //        string query2 = @"select mail from humres where fullname = '" + objDesdeDbR.Approver + "'";
        //        SqlCommand cmd2 = new SqlCommand(query2, cn2);

        //        SqlDataReader rdr2 = cmd2.ExecuteReader();

        //        //get the data reader, etc.
        //        while (rdr2.Read())
        //        {


        //            mailaprobador = rdr2["mail"].ToString();



        //        }



        //        cn2.Close();
        //    }
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage(EmailOrigen, mailaprobador, encabezadoaprobador, mensajeaprobador);

        //        oMailMessagep.IsBodyHtml = true;
        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    string encabezado = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensaje = "<p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";

        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage();
        //        oMailMessagep.IsBodyHtml = true;


        //        // Rellenamos el mensaje
        //        oMailMessagep.From = new MailAddress(EmailOrigen);  // Remitente
        //        oMailMessagep.To.Add(new MailAddress(mail1));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail2));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail3));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail4));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail5));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail6));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mailp));    // Destino

        //        //oMailMessagep.CC.Add(new MailAddress();  // Destino (en Carbon Copy)
        //        oMailMessagep.Bcc.Add(new MailAddress("ccorona@amphenol-aio.com"));    // Destino (en Blind Carbon Copy, oculto)

        //        // Asunto y cuerpo
        //        oMailMessagep.Subject = encabezado;
        //        oMailMessagep.Body = mensaje;


        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    await Task.Delay(10000);
        //    return true;
        //}

        //public async Task<bool> SendMail(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        //{
        //    var objDesdeDbR = _db.CSEXSW_Rma.Where(s => s.Rmarequest == rmarequest).FirstOrDefault();

        //    string result = "";
        //    try
        //    {
        //        var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbT.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbT)
        //            {
        //                result = result + " " + filtro.Coustumer;
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    string rango = _db.CSEXSW_Approver.Where(a => a.Approver == objDesdeDbR.Approver).Select(a => a.Rango).FirstOrDefault();
        //    string encabezado = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensaje = "<p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";
        //    string encabezadoaprobador = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensajeaprobador = "<p>Review by your " + rango + " role</p><br/><p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";

        //    string mailaprobador = "";
        //    string connectionString = _configuration.GetConnectionString("ConnectionM10").ToString();
        //    using (SqlConnection cn2 = new SqlConnection(connectionString))
        //    {
        //        cn2.Open();
        //        string query2 = @"select mail from humres where fullname = '" + objDesdeDbR.Approver + "'";
        //        SqlCommand cmd2 = new SqlCommand(query2, cn2);

        //        SqlDataReader rdr2 = cmd2.ExecuteReader();

        //        //get the data reader, etc.
        //        while (rdr2.Read())
        //        {


        //            mailaprobador = rdr2["mail"].ToString();



        //        }



        //        cn2.Close();
        //    }
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage(EmailOrigen, mailaprobador, encabezadoaprobador, mensajeaprobador);

        //        oMailMessagep.IsBodyHtml = true;
        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();
        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    { }

        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage();
        //        oMailMessagep.IsBodyHtml = true;


        //        // Rellenamos el mensaje
        //        oMailMessagep.From = new MailAddress(EmailOrigen);  // Remitente
        //        oMailMessagep.To.Add(new MailAddress(mail1));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail2));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail3));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail4));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail5));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail6));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mailp));    // Destino

        //        //oMailMessagep.CC.Add(new MailAddress();  // Destino (en Carbon Copy)
        //        oMailMessagep.Bcc.Add(new MailAddress("ccorona@amphenol-aio.com"));    // Destino (en Blind Carbon Copy, oculto)

        //        // Asunto y cuerpo
        //        oMailMessagep.Subject = encabezado;
        //        oMailMessagep.Body = mensaje;


        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    await Task.Delay(10000);

        //    return true;
        //}

        //public async Task<bool> SendMailAsync2(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        //{

        //    int id = (from c in _db.CSEXSW_Rma
        //              where c.Rmarequest == rmarequest
        //              select c.Id).First();

        //    string result = "";
        //    var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == id).ToList();

        //    if (objDesdeDbT.Count() > 0)
        //    {

        //        foreach (var filtro in objDesdeDbT)
        //        {
        //            result = result + " " + filtro.Coustumer;
        //        }

        //    }
        //    var objDesdeDbR = _db.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault();
        //    string rango = _db.CSEXSW_Approver.Where(a => a.Approver == objDesdeDbR.Approver).Select(a => a.Rango).FirstOrDefault();
        //    string encabezado = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensaje = "<p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";
        //    string encabezadoaprobador = "New RMA Approval has been created: " + objDesdeDbR.Rmarequest + "";
        //    string mensajeaprobador = "<p>Review by your " + rango + " role</p><br/><p>Please review this new RMA request:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "</p>";

        //    string mailaprobador = "";
        //    string connectionString = _configuration.GetConnectionString("Connection100").ToString();
        //    using (SqlConnection cn2 = new SqlConnection(connectionString))
        //    {
        //        cn2.Open();
        //        string query2 = @"select mail from humres where fullname = '" + objDesdeDbR.Approver + "'";
        //        SqlCommand cmd2 = new SqlCommand(query2, cn2);

        //        SqlDataReader rdr2 = cmd2.ExecuteReader();

        //        //get the data reader, etc.
        //        while (rdr2.Read())
        //        {


        //            mailaprobador = rdr2["mail"].ToString();



        //        }



        //        cn2.Close();
        //    }
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage(EmailOrigen, mailaprobador, encabezadoaprobador, mensajeaprobador);

        //        oMailMessagep.IsBodyHtml = true;
        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();
        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage();
        //        oMailMessagep.IsBodyHtml = true;


        //        // Rellenamos el mensaje
        //        oMailMessagep.From = new MailAddress(EmailOrigen);  // Remitente
        //        oMailMessagep.To.Add(new MailAddress(mail1));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail2));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail3));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail4));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail5));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail6));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mailp));    // Destino

        //        //oMailMessagep.CC.Add(new MailAddress();  // Destino (en Carbon Copy)
        //        oMailMessagep.Bcc.Add(new MailAddress("ccorona@amphenol-aio.com"));    // Destino (en Blind Carbon Copy, oculto)

        //        // Asunto y cuerpo
        //        oMailMessagep.Subject = encabezado;
        //        oMailMessagep.Body = mensaje;


        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    await Task.Delay(10000);
        //    return true;
        //}


        //public async Task<bool> SendMailAsync3(string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp, string rmarequest)
        //{


        //    int id = (from c in _db.CSEXSW_Rma
        //              where c.Rmarequest == rmarequest
        //              select c.Id).First();

        //    string result = "";
        //    var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == id).ToList();

        //    if (objDesdeDbT.Count() > 0)
        //    {

        //        foreach (var filtro in objDesdeDbT)
        //        {
        //            result = result + " " + filtro.Coustumer;
        //        }

        //    }



        //    var objDesdeDbR = _db.CSEXSW_Rma.Where(s => s.Id == id).FirstOrDefault();



        //    string encabezado = "New RMA Approval has been approved: " + objDesdeDbR.Rmarequest + "";
        //    string mensaje = "<p>RMA has been approved:  &nbsp; " + objDesdeDbR.Rmarequest + "<br />" + objDesdeDbR.Preparado + " at  &nbsp; " + objDesdeDbR.Date + "<br />RMA Request #: &nbsp; " + objDesdeDbR.Rmarequest + "<br />Date : &nbsp; " + objDesdeDbR.Date + "<br />Customer: &nbsp; " + objDesdeDbR.Customer + "<br />Customer Part #: &nbsp; " + result + "<br />Customer PO #: &nbsp; " + objDesdeDbR.Customerpo + " <br />Description: &nbsp;" + objDesdeDbR.Description + "<br />Customer Complaint: &nbsp;" + objDesdeDbR.Customercomplait + "<br />RMA Type of Request: &nbsp;" + objDesdeDbR.Rmatypeofrequest + "<br />Total RMA Value: &nbsp;" + objDesdeDbR.Totalrmavalues + "<br />Where Built: &nbsp;" + objDesdeDbR.Wherebuilt + "<br />Approver: &nbsp;" + objDesdeDbR.Approver + "<br />Approval Comments: &nbsp;" + objDesdeDbR.Comment + "</p>";
        //    try
        //    {


        //        string EmailOrigen = _configuration.GetConnectionString("correo").ToString();
        //        string Contraseña = _configuration.GetConnectionString("contrasennia").ToString();
        //        string path;
        //        MailMessage oMailMessagep = new MailMessage();
        //        oMailMessagep.IsBodyHtml = true;


        //        // Rellenamos el mensaje
        //        oMailMessagep.From = new MailAddress(EmailOrigen);  // Remitente
        //        oMailMessagep.To.Add(new MailAddress(mail1));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail2));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail3));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail4));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail5));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mail6));    // Destino
        //        oMailMessagep.To.Add(new MailAddress(mailp));    // Destino

        //        //oMailMessagep.CC.Add(new MailAddress();  // Destino (en Carbon Copy)
        //        oMailMessagep.Bcc.Add(new MailAddress("ccorona@amphenol-aio.com"));    // Destino (en Blind Carbon Copy, oculto)

        //        // Asunto y cuerpo
        //        oMailMessagep.Subject = encabezado;
        //        oMailMessagep.Body = mensaje;


        //        var objDesdeDbA = _db.CSEXSW_Attachmentrma.Where(s => s.RmaId == objDesdeDbR.Id).ToList();

        //        if (objDesdeDbA.Count() > 0)
        //        {

        //            foreach (var filtro in objDesdeDbA)
        //            {
        //                path = @"wwwroot\documents\documents\rma\" + filtro.Documento;


        //                oMailMessagep.Attachments.Add(new Attachment(path));


        //            }

        //        }
        //        SmtpClient oSmtpClientp = new SmtpClient();

        //        oSmtpClientp.EnableSsl = Convert.ToBoolean(_configuration.GetConnectionString("EnableSsl"));
        //        oSmtpClientp.Host = _configuration.GetConnectionString("hostcorreo");
        //        oSmtpClientp.Port = Convert.ToInt32(_configuration.GetConnectionString("puerto"));
        //        oSmtpClientp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);
        //        oSmtpClientp.Send(oMailMessagep);
        //        oSmtpClientp.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    await Task.Delay(10000);
        //    return true;
        //}

        public void UpdateRema(int idrema, string commentrema, string var)
        {

            var rma = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == idrema);
            var gm = _db.HRRoles.Where(S => S.RoleID == 100032).FirstOrDefault().EmpID;
            var qd = _db.HRRoles.Where(S => S.RoleID == 100031).FirstOrDefault().EmpID;



            if (rma.Totalrmavalues >= 20000)
            {
                //2 aprobadores
                if (rma.res_id_approver == gm)
                {
                    var emp = _db.humres.Where(s => s.res_id == qd).FirstOrDefault().fullname;
                    rma.res_id_approver = qd;
                    rma.Approver = emp.Trim();

                }

            }

            rma.Status = "Remark";
            rma.Comment = "Remark: " + commentrema;

            _db.SaveChanges();


        }
        public async Task<bool> falloRMA(string rma, int requests, string reason)
        {
            rma = rma.Remove(0, 2).Trim();
            rma = string.Format("  {0}", rma);
            var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == requests);
            string retorno;
            objDesdeDb.Status = "Approved";
            retorno = "Approved";
            objDesdeDb.turno = rma;

            string cus = objDesdeDb.Customer;
            string po = objDesdeDb.Customerpo;
            string comment = objDesdeDb.Description;
            _db.SaveChanges();


            Int16 seqq = 1;

            var objDesdeDbt = new OERHDFIL_SQL();




            var AccountTypeCode = "";
            var cicmpy = new Cicmpy();
            cicmpy = _db2.Cicmpy.Where(a => a.CmpCode.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();
            AccountTypeCode = cicmpy.AccountTypeCode;




            var ArtypfilSql = new ArtypfilSql();
            ArtypfilSql = _db2.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
            objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

            objDesdeDbt.dept = ArtypfilSql.SlsDpNo;


            var moneda = "";

            objDesdeDbt.orig_trx_rt = 1;

            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
            objDesdeDbt.UserDefFld3 = reason;
            objDesdeDbt.curr_trx_rt = 1;

            var arcusfil_sql = new arcusfil_sql();
            var oehdrhst_sql = new OEHDRHST_SQL();
            int oehdrhst_sqlcuantos = 0;
            var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == requests).ToList();
            try
            {
                if (objDesdeDbT.Count() > 0)
                {

                    foreach (var filtro in objDesdeDbT)
                    {
                        if (filtro.Invoice != "0" && filtro.Invoice != "" && filtro.Invoice != null)
                        {


                            oehdrhst_sqlcuantos = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).Count();

                            oehdrhst_sql = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).FirstOrDefault();
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex, Color.Red);
            }

            if (oehdrhst_sqlcuantos == 0)
            {


                arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();

                moneda = arcusfil_sql.curr_cd;
                objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;


                objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
                objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;





                objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
                objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
                objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;




                var AraltadrSql = _db2.AraltadrSql.Where(a => a.CusNo.Trim() == objDesdeDb.Customer.Trim() && a.CusAltAdrCd.Contains(objDesdeDb.Ship_To)).FirstOrDefault();
                objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();

                objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;

                objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;

                objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;

                objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;

                objDesdeDbt.tax_cd = AraltadrSql.TaxCd;

                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                objDesdeDbt.UserDefFld3 = reason;
                objDesdeDbt.ship_to_country = AraltadrSql.Country;


                objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                objDesdeDbt.bill_to_country = AraltadrSql.Country;
                objDesdeDbt.ship_to_name = AraltadrSql.CusName;

                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();


                if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
                {
                    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }

            }
            else
            {


                objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                moneda = oehdrhst_sql.CurrCd;
                objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;



                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                objDesdeDbt.UserDefFld3 = reason;


                if (moneda != "CNY")
                {
                    objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
                }

                var imctlfil_sql = new ImctlfilSql();
                imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();
                if (oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "")
                {
                    objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc;
                }
                else
                {
                    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                }



                objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;

                objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
                objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;

                objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;

                objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
                objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;





                objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
                objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
                objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
                objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;


                objDesdeDbt.bill_to_name = oehdrhst_sql.BillToName;
                objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
                objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

            }

            //if (_configuration.GetConnectionString("server") != "m10testna01")
            //{
            //    if (moneda != "CNY")
            //    {

            //        var rate = _db2.Rate.Where(a => a.DateL == _db2.Rate.Max(a => a.DateL) && a.SourceCurrency == moneda).FirstOrDefault();
            //        objDesdeDbt.orig_trx_rt = (decimal?)rate.RateExchange;

            //        objDesdeDbt.curr_trx_rt = (decimal?)rate.RateExchange;
            //    }
            //}


            objDesdeDbt.contact = objDesdeDb.Contact;
            objDesdeDbt.phone_no = objDesdeDb.Phone;
            objDesdeDbt.fax_no = objDesdeDb.Fax;
            objDesdeDbt.phone_ext = objDesdeDb.Ext;
            objDesdeDbt.contact_email = objDesdeDb.Email;
            objDesdeDbt.user_def_fld_5 = "Normal                        ";
            objDesdeDbt.deter_rate_by = "O";
            objDesdeDbt.form_no = 1;
            objDesdeDbt.rma_no = rma.Trim().PadLeft(8);
            objDesdeDbt.cus_no = cus.Trim().PadLeft(20);
            objDesdeDbt.slspsn_pct_comm = 100;

            objDesdeDbt.cus_ship_to = objDesdeDb.Ship_To;
            objDesdeDbt.oe_po_no = po;
            objDesdeDbt.rma_cmt = comment;
            objDesdeDbt.status = "O";
            objDesdeDbt.SlspsnCommAmt = 0;
            objDesdeDbt.SlspsnNo2 = 0;
            objDesdeDbt.SlspsnPctComm2 = 0;
            objDesdeDbt.SlspsnCommAmt2 = 0;
            objDesdeDbt.SlspsnPctComm3 = 0;
            objDesdeDbt.SlspsnCommAmt3 = 0;
            objDesdeDbt.SlspsnNo3 = 0;
            objDesdeDbt.Extra10 = 0;
            objDesdeDbt.Extra11 = 0;
            objDesdeDbt.Extra12 = 0;
            objDesdeDbt.Extra13 = 0;
            objDesdeDbt.Extra14 = 0;
            objDesdeDbt.Extra15 = 0;
            objDesdeDbt.TaxPct = 0;
            objDesdeDbt.TaxPct2 = 0;
            objDesdeDbt.TaxPct3 = 0;
            objDesdeDbt.DiscountPct = 0;
            objDesdeDbt.TotSlsAmt = 0;
            objDesdeDbt.TotSlsDisc = 0;
            objDesdeDbt.TotTaxAmt = 0;
            objDesdeDbt.TotCost = 0;
            objDesdeDbt.TotWeight = 0;
            objDesdeDbt.SlsTaxAmt1 = 0;
            objDesdeDbt.SlsTaxAmt2 = 0;
            objDesdeDbt.SlsTaxAmt3 = 0;
            objDesdeDbt.CommPct = 0;
            objDesdeDbt.CommAmt = 0;
            objDesdeDbt.AccumMiscAmt = 0;
            objDesdeDbt.AccumFrtAmt = 0;
            objDesdeDbt.AccumTotTaxAmt = 0;
            objDesdeDbt.AccumSlsTaxAmt = 0;
            objDesdeDbt.AccumTotSlsAmt = 0;
            objDesdeDbt.TotTaxCost = 0;
            objDesdeDbt.TotDollars = 0;

            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
            objDesdeDbt.UserDefFld3 = reason;
            objDesdeDbt.TaxFg = "N";

            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
            objDesdeDbt.rma_dt_entered = date;
            objDesdeDbt.LastActDt = date;
            objDesdeDbt.ExpRecDate = date;
            _db2.OERHDFIL_SQL.Add(objDesdeDbt);


            _db2.SaveChanges();


            var up = _db2.OERHDFIL_SQL.Where(s => s.Id == objDesdeDbt.Id).FirstOrDefault();

            up.UserDefFld1 = "FOB SOURCE";
            up.UserDefFld3 = reason;
            _db2.Entry(up).State = EntityState.Modified;

            _db2.SaveChanges();


            try
            {
                var objDesdeDbTr = _db.csexsw_coustumer.Where(s => s.RmaId == requests).ToList();

                if (objDesdeDbTr.Count() > 0)
                {

                    foreach (var filtro in objDesdeDbTr)
                    {






                        int total2 = _db2.iminvloc_sql.Where(a => a.ItemNo == filtro.Coustumer && a.Loc == filtro.Loc).Count();
                        if (total2 == 0)
                        {
                            var locprincipal = _db2.imitmidx_sql.Where(a => a.item_no == filtro.Coustumer).Select(s => s.loc).FirstOrDefault().ToString();

                            var result = _db2.iminvloc_sql.FirstOrDefault(a => a.ItemNo == filtro.Coustumer && a.Loc == locprincipal);

                            var objDesdeDb5 = new iminvloc_sql();


                            objDesdeDb5.ActiveOrds = 0;
                            objDesdeDb5.AvgCost = 0;
                            objDesdeDb5.AvgFrcstError = 0;
                            objDesdeDb5.AvgUsage = 0;
                            objDesdeDb5.InvClass = result.InvClass;
                            objDesdeDb5.ByrPlnr = result.ByrPlnr;
                            objDesdeDb5.CostLastYr = 0;
                            objDesdeDb5.CostPtd = 0;
                            objDesdeDb5.CostYtd = 0;
                            objDesdeDb5.CubeHeight = 0;
                            objDesdeDb5.CubeLength = 0;
                            objDesdeDb5.CubeQtyPer = 0;
                            objDesdeDb5.CubeWidth = 0;
                            //objDesdeDb5.DocField1 = 0;
                            //objDesdeDb5.DocField2 = 0;
                            //objDesdeDb5.DocField3 = 0;
                            objDesdeDb5.DocToStkLdTm = 0;
                            objDesdeDb5.EconomicOrdQty = 0;

                            objDesdeDb5.Extra10 = 0;
                            objDesdeDb5.Extra11 = 0;
                            objDesdeDb5.Extra12 = 0;
                            objDesdeDb5.Extra13 = 0;
                            objDesdeDb5.Extra14 = 0;
                            objDesdeDb5.Extra15 = 0;

                            objDesdeDb5.FrzCost = 0;
                            objDesdeDb5.FrzQty = 0;
                            objDesdeDb5.IncludeParCost = 0;
                            objDesdeDb5.InvLocReturnCostLyr = 0;

                            objDesdeDb5.InvLocReturnCostPtd = 0;

                            objDesdeDb5.InvLocReturnCostYtd = 0;
                            objDesdeDb5.InvLocReturnSalesLyr = 0;
                            objDesdeDb5.InvLocReturnSalesPtd = 0;
                            objDesdeDb5.InvLocReturnSalesYtd = 0;

                            objDesdeDb5.LastCost = 0;
                            objDesdeDb5.LocQtyFld = 0;

                            objDesdeDb5.OrdUpToLvl = 0;
                            objDesdeDb5.PctErrLastCnt = 0;
                            objDesdeDb5.PoLeadTm = 0;
                            objDesdeDb5.PoMax = 0;
                            objDesdeDb5.PoMin = 0;
                            //objDesdeDb5.PoMult = 0;
                            objDesdeDb5.PriorYearSls = 0;
                            objDesdeDb5.PriorYearUsage = 0;
                            objDesdeDb5.QtyAllocated = 0;

                            objDesdeDb5.QtyBkord = 0;
                            objDesdeDb5.QtyLastSold = 0;
                            objDesdeDb5.QtyOnHand = 0;
                            objDesdeDb5.QtyOnOrd = 0;
                            objDesdeDb5.QtyRejectLastYr = 0;
                            objDesdeDb5.QtyRejectPtd = 0;
                            objDesdeDb5.QtyRejectYtd = 0;

                            objDesdeDb5.QtyReturnedYtd = 0;
                            objDesdeDb5.QtyRtnLyr = 0;
                            objDesdeDb5.QtyRtnPtd = 0;
                            objDesdeDb5.QtyScrpLastYr = 0;
                            objDesdeDb5.QtyScrpPtd = 0;
                            objDesdeDb5.QtyScrpYtd = 0;
                            objDesdeDb5.QtySldPtd = 0;
                            objDesdeDb5.QtySoldLastYr = 0;
                            objDesdeDb5.QtySoldYtd = 0;

                            objDesdeDb5.RecomMinOrd = 0;
                            objDesdeDb5.ReorderLvl = 0;
                            objDesdeDb5.SafetyFctr = 0;
                            objDesdeDb5.SafetyStk = 0;
                            objDesdeDb5.SlsPrice = 0;
                            objDesdeDb5.SlsPtd = 0;
                            objDesdeDb5.SlsYtd = 0;
                            objDesdeDb5.SumOfErrors = 0;
                            objDesdeDb5.TagCost = 0;
                            objDesdeDb5.TagQty = 0;
                            objDesdeDb5.TargetMargin = 0;
                            objDesdeDb5.UsageFilter = 0;
                            objDesdeDb5.TmsCntdYtd = 0;
                            objDesdeDb5.UsagePtd = 0;
                            objDesdeDb5.UsageYtd = 0;

                            objDesdeDb5.UserFld8 = 0;
                            objDesdeDb5.UserFld9 = 0;
                            objDesdeDb5.UserFld10 = 0;
                            objDesdeDb5.UserFld11 = 0;
                            objDesdeDb5.UserFld12 = 0;
                            objDesdeDb5.UserFld13 = 0;

                            objDesdeDb5.UserFld17 = 0;
                            objDesdeDb5.UserFld18 = 0;
                            objDesdeDb5.UserFld19 = 0;
                            objDesdeDb5.UserFld20 = 0;

                            objDesdeDb5.UsgWghtFctr = 0;
                            objDesdeDb5.PricesApplyFlag = "N";
                            objDesdeDb5.DiscsApplyFg = "N";
                            objDesdeDb5.price = result.price;
                            objDesdeDb5.std_cost = result.std_cost;
                            objDesdeDb5.Status = result.Status;
                            objDesdeDb5.ProdCat = result.ProdCat;
                            objDesdeDb5.MultBinFg = "Y";
                            objDesdeDb5.ItemNo = filtro.Coustumer;
                            objDesdeDb5.Loc = filtro.Loc;


                            objDesdeDb5.Id = 0;
                            _db2.iminvloc_sql.Add(objDesdeDb5);
                            _db2.SaveChanges();






                        }

                        var objDesdeDby = new OERDTFIL_SQL();
                        var objDesdeDbv = _db2.imitmidx_sql.FirstOrDefault(s => s.item_no == filtro.Coustumer);

                        var objDesdeDbvL = _db2.iminvloc_sql.FirstOrDefault(s => s.ItemNo == filtro.Coustumer && s.Loc == filtro.Loc);
                        objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;

                        objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;

                        objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
                        objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
                        objDesdeDby.uom = objDesdeDbv.uom;
                        objDesdeDby.rma_no = rma;
                        objDesdeDby.oe_cus_no = cus.PadLeft(20);
                        objDesdeDby.apply_to_invc_no = filtro.Invoice;
                        objDesdeDby.apply_to_seq_no = filtro.Seq;
                        objDesdeDby.rma_seq_no = seqq;
                        objDesdeDby.item_no = filtro.Coustumer;
                        objDesdeDby.reason_cd = filtro.Retur;
                        objDesdeDby.pick_seq_no = " ";
                        objDesdeDby.oe_ord_no = " ";
                        objDesdeDby.oe_unique_seq_no = 0;
                        objDesdeDby.oe_unique_seq = 0;
                        objDesdeDby.action = filtro.Action;
                        objDesdeDby.oe_unit_cost = filtro.Cost;
                        objDesdeDby.oe_unit_price = filtro.Unit;
                        objDesdeDby.rma_qty_rtn_auth = filtro.Qty;

                        objDesdeDby.loc = filtro.Loc;
                        objDesdeDby.status = "O";
                        objDesdeDby.DiscountPct = 0;
                        objDesdeDby.UomRatio = 1;
                        objDesdeDby.RmaQtyRtnActual = 0;

                        objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
                        objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
                        objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
                        objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
                        objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;

                        objDesdeDby.Extra10 = 0;
                        objDesdeDby.Extra11 = 0;
                        objDesdeDby.Extra12 = 0;
                        objDesdeDby.Extra13 = 0;
                        objDesdeDby.Extra14 = 0;
                        objDesdeDby.Extra15 = 0;
                        _db2.OERDTFIL_SQL.Add(objDesdeDby);


                        _db2.SaveChanges();
                        seqq++;


                    }

                }
            }
            catch (Exception)
            {



            }

            await Task.Delay(10000);
            return true;
        }

        public void Updatedesaprobar(int ids, string comment, string var /* string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp*/)
        {
            var currentDate = DateTime.Now;

            var rma = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == ids);


            rma.Status = "Rejected";

            string newComment = $"• ({currentDate:MM/dd/yyyy hh:mm:ss tt}) {rma.Approver}: {comment.Trim()}";

            rma.Comment = string.IsNullOrWhiteSpace(rma.Comment) ? newComment : $"{rma.Comment}{Environment.NewLine}{newComment}";

            _db.SaveChanges();
            //BackgroundJob.Enqueue(() => SendMailAsync5(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

        }
        public string Updateaprobar(int rmaId, string comment, string userId)
        {
            var rma = _db.CSEXSW_Rma.FirstOrDefault(x => x.Id == rmaId);

            if (rma is null)
            {
                return string.Empty;
            }

            var currentDate = DateTime.Now;

            AddComment(rma, currentDate, comment);

            rma.date_approved = currentDate;

            if (!int.TryParse(userId, out var currentUserId))
            {
                return string.Empty;
            }

            var exchangeRate = GetRmaExchangeRate(rma);

            if (exchangeRate >= TwoStepAuthorizationThreshold)
            {
                return ProcessTwoStepApproval(rma, currentUserId, currentDate);
            }

            return ApproveRma(rma, currentDate);
        }

        private void AddComment(CSEXSW_Rma rma, DateTime currentDate, string comment)
        {
            var newComment =
                $"• ({currentDate:MM/dd/yyyy hh:mm:ss tt}) {rma.Approver}: {comment.Trim()}";

            rma.Comment = string.IsNullOrWhiteSpace(rma.Comment)
                ? newComment
                : $"{rma.Comment}{Environment.NewLine}{newComment}";
        }

        private double GetRmaExchangeRate(CSEXSW_Rma rma)
        {
            var customer = _db2.arcusfil_sql
                .FirstOrDefault(x =>
                    x.cus_no.Trim() == rma.Customer.Trim());

            if (customer is null)
            {
                throw new InvalidOperationException(
                    $"Customer '{rma.Customer}' was not found.");
            }

            if (!string.Equals(
                    customer.curr_cd,
                    "CNY",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToDouble(rma.Totalrmavalues);
            }

            var latestRate = _db2.Rate
                .Where(x => x.SourceCurrency == "USD")
                .OrderByDescending(x => x.DateL)
                .FirstOrDefault();

            if (latestRate is null || latestRate.RateExchange == 0)
            {
                throw new InvalidOperationException(
                    "A valid USD exchange rate could not be found.");
            }

            return Convert.ToDouble(rma.Totalrmavalues) / Convert.ToDouble(latestRate.RateExchange);
        }

        private string ProcessTwoStepApproval(CSEXSW_Rma rma, int currentUserId, DateTime currentDate)
        {
            var qualityDirector = GetEmployeeByRole(100031);
            var generalManager = GetEmployeeByRole(100032);

            if (qualityDirector is null || generalManager is null)
            {
                throw new InvalidOperationException(
                    "Required approval roles could not be found.");
            }

            if (rma.res_id_approver == qualityDirector.Id)
            {
                if (currentUserId != qualityDirector.Id &&
                    currentUserId != _autoApprover.Id)
                {
                    return string.Empty;
                }

                rma.Status = "Pending";
                rma.Approver = generalManager.Name;
                rma.res_id_approver = generalManager.Id;

                _db.SaveChanges();

                return "Pending";
            }

            if (rma.res_id_approver == generalManager.Id)
            {
                return ApproveRma(rma, currentDate);
            }

            return string.Empty;
        }

        private string ApproveRma(CSEXSW_Rma rma, DateTime currentDate)
        {
            var control = _db2.OERMACTL_SQL
                .FirstOrDefault(x => x.ID == 1);

            if (control is null ||
                !int.TryParse(control.ctl_next_order_no, out var rmaNumber))
            {
                return string.Empty;
            }

            var rmaNo = FormatRmaNumber(control.ctl_next_order_no);

            // Increment control number using the original D8 behavior.
            control.ctl_next_order_no = (rmaNumber + 1).ToString("D8");

            rma.Status = "Approved";
            rma.turno = rmaNo;

            _db2.SaveChanges();
            _db.SaveChanges();

            var rmaLines = _db.csexsw_coustumer
                .Where(x => x.RmaId == rma.Id)
                .ToList();

            /*
             * HEADER
             */
            var orderHeader = CreateOrderHeader(
                rma,
                rmaNo,
                rmaLines,
                currentDate);

            _db2.OERHDFIL_SQL.Add(orderHeader);

            _db2.SaveChanges();

            /*
             * DETAILS
             */
            CreateOrderDetails(
                rma,
                rmaNo,
                rmaLines);

            _db2.SaveChanges();


            var headerExists = _db2.OERHDFIL_SQL
                .Any(x => x.rma_no == rmaNo);

            if (!headerExists)
            {
                BackgroundJob.Enqueue(() => falloRMA(rmaNo, rma.Id, rma.reason));
            }

            return "Approved";
        }

        private string FormatRmaNumber(string rmaNumber)
        {
            if (string.IsNullOrWhiteSpace(rmaNumber))
            {
                throw new ArgumentException(
                    "RMA number cannot be empty.",
                    nameof(rmaNumber));
            }

            if (!int.TryParse(rmaNumber.Trim(), out var number))
            {
                throw new FormatException(
                    $"Invalid RMA number: '{rmaNumber}'.");
            }

            if (number < 0 || number > 99_999_999)
            {
                throw new FormatException(
                    $"RMA number '{rmaNumber}' cannot be represented using 8 characters.");
            }


            return number.ToString().PadLeft(8, ' ');
        }

        private OERHDFIL_SQL CreateOrderHeader(
            CSEXSW_Rma rma,
            string rmaNo,
            List<csexsw_coustumer> rmaLines,
            DateTime currentDate)
        {
            var header = new OERHDFIL_SQL();

            PopulateAccountInformation(header, rma);

            PopulateShippingInformation(header, rma, rmaLines);

            header.contact = rma.Contact;
            header.phone_no = rma.Phone;
            header.fax_no = rma.Fax;
            header.phone_ext = rma.Ext;
            header.contact_email = rma.Email;

            header.user_def_fld_5 = "Normal                        ";
            header.deter_rate_by = "O";
            header.form_no = 1;

            header.rma_no = rmaNo;

            header.cus_no = rma.Customer
                .Trim()
                .PadLeft(20);

            header.slspsn_pct_comm = 100;

            header.cus_ship_to = rma.Ship_To;
            header.oe_po_no = rma.Customerpo;
            header.rma_cmt = rma.Description;

            header.status = "O";

            PopulateHeaderDefaults(header);

            header.UserDefFld1 = "FOB SOURCE";
            header.UserDefFld3 = rma.reason;

            header.rma_dt_entered = currentDate;
            header.LastActDt = currentDate;
            header.ExpRecDate = currentDate;

            return header;
        }

        private void PopulateHeaderDefaults(OERHDFIL_SQL header)
        {
            header.orig_trx_rt = 1;
            header.curr_trx_rt = 1;

            header.UserDefFld1 = "FOB SOURCE";

            header.user_def_fld_5 = "Normal                        ";
            header.deter_rate_by = "O";
            header.form_no = 1;

            header.slspsn_pct_comm = 100;

            header.status = "O";

            header.SlspsnCommAmt = 0;
            header.SlspsnNo2 = 0;
            header.SlspsnPctComm2 = 0;
            header.SlspsnCommAmt2 = 0;
            header.SlspsnPctComm3 = 0;
            header.SlspsnCommAmt3 = 0;
            header.SlspsnNo3 = 0;

            header.Extra10 = 0;
            header.Extra11 = 0;
            header.Extra12 = 0;
            header.Extra13 = 0;
            header.Extra14 = 0;
            header.Extra15 = 0;

            header.TaxPct = 0;
            header.TaxPct2 = 0;
            header.TaxPct3 = 0;

            header.DiscountPct = 0;

            header.TotSlsAmt = 0;
            header.TotSlsDisc = 0;
            header.TotTaxAmt = 0;
            header.TotCost = 0;
            header.TotWeight = 0;

            header.SlsTaxAmt1 = 0;
            header.SlsTaxAmt2 = 0;
            header.SlsTaxAmt3 = 0;

            header.CommPct = 0;
            header.CommAmt = 0;

            header.AccumMiscAmt = 0;
            header.AccumFrtAmt = 0;
            header.AccumTotTaxAmt = 0;
            header.AccumSlsTaxAmt = 0;
            header.AccumTotSlsAmt = 0;

            header.TotTaxCost = 0;
            header.TotDollars = 0;

            header.TaxFg = "N";
        }

        private void PopulateAccountInformation(OERHDFIL_SQL header, CSEXSW_Rma rma)
        {
            var company = _db2.Cicmpy
                .FirstOrDefault(x =>
                    x.CmpCode.Trim() == rma.Customer.Trim());

            if (company is null)
            {
                throw new InvalidOperationException(
                    $"Customer '{rma.Customer}' was not found in Cicmpy.");
            }

            var accountType = _db2.ArtypfilSql
                .FirstOrDefault(x =>
                    x.CusTypeCd == company.AccountTypeCode);

            if (accountType is null)
            {
                throw new InvalidOperationException(
                    $"Account type '{company.AccountTypeCode}' was not found " +
                    $"for customer '{rma.Customer}'.");
            }

            header.profit_center = accountType.SlsSbNo;
            header.dept = accountType.SlsDpNo;
        }

        private void PopulateShippingInformation(OERHDFIL_SQL header, CSEXSW_Rma rma, List<csexsw_coustumer> rmaLines)
        {
            OEHDRHST_SQL? orderHistory = null;

            foreach (var line in rmaLines)
            {
                if (string.IsNullOrWhiteSpace(line.Invoice) ||
                    line.Invoice == "0")
                {
                    continue;
                }

                var historyQuery = _db2.OEHDRHST_SQL
                    .Where(x =>
                        x.CusAltAdrCd.Contains(rma.Ship_To.ToString()) &&
                        x.InvNo == line.Invoice);

                orderHistory = historyQuery.FirstOrDefault();
            }

            if (orderHistory is null)
            {
                PopulateShippingFromCustomer(header, rma);

                return;
            }

            PopulateShippingFromHistory(header, rma, orderHistory);
        }

        private void PopulateShippingFromCustomer(OERHDFIL_SQL header, CSEXSW_Rma rma)
        {
            header.UserDefFld1 = "FOB SOURCE";
            header.UserDefFld3 = rma.reason;

            var customer = _db2.arcusfil_sql
                .FirstOrDefault(x =>
                    x.cus_no.Trim() == rma.Customer.Trim());

            if (customer is null)
            {
                throw new InvalidOperationException(
                    $"Customer '{rma.Customer}' was not found.");
            }

            header.curr_cd = customer.curr_cd;

            header.bill_to_addr_4 =
                $"{customer.City?.Trim()}, " +
                $"{customer.State?.Trim()} " +
                $"{customer.Zip?.Trim()}";

            header.ar_terms_cd = customer.ArTermsCd;

            header.bill_to_addr_1 = customer.Addr1;
            header.bill_to_addr_2 = customer.Addr2;
            header.bill_to_addr_3 = customer.Addr3;

            var alternateAddress = _db2.AraltadrSql
                .FirstOrDefault(x =>
                    x.CusNo.Trim() == rma.Customer.Trim() &&
                    x.CusAltAdrCd.Contains(rma.Ship_To));

            if (alternateAddress is null)
            {
                throw new InvalidOperationException(
                    $"Shipping address '{rma.Ship_To}' was not found " +
                    $"for customer '{rma.Customer}'.");
            }

            header.ship_to_addr_4 =
                $"{alternateAddress.City?.Trim()}, " +
                $"{alternateAddress.State?.Trim()} " +
                $"{alternateAddress.Zip?.Trim()}";

            header.ship_via_cd = alternateAddress.ShipViaCd;
            header.slspsn_no = alternateAddress.SlspsnNo;

            header.ship_to_addr_1 = alternateAddress.Addr1;
            header.ship_to_addr_2 = alternateAddress.Addr2;
            header.ship_to_addr_3 = alternateAddress.Addr3;

            header.tax_cd = alternateAddress.TaxCd;
            header.ship_to_country = alternateAddress.Country;

            header.bill_to_name = alternateAddress.CusName;
            header.bill_to_country = alternateAddress.Country;
            header.ship_to_name = alternateAddress.CusName;

            var inventoryControl = _db2.ImctlfilSql
                .FirstOrDefault();

            header.mfg_loc =
                !string.IsNullOrEmpty(alternateAddress.Loc)
                    ? alternateAddress.Loc
                    : inventoryControl?.Loc;
        }

        private void PopulateShippingFromHistory(OERHDFIL_SQL header, CSEXSW_Rma rma, OEHDRHST_SQL history)
        {
            header.UserDefFld1 = "FOB SOURCE";
            header.UserDefFld3 = rma.reason;

            header.ar_terms_cd = history.ArTermsCd;
            header.tax_cd = history.TaxCd;

            header.curr_cd = history.CurrCd;
            header.ship_to_country = history.ShipToCountry;

            if (history.CurrCd != "CNY")
            {
                header.curr_trx_rt = history.CurrTrxRt;
            }

            var inventoryControl = _db2.ImctlfilSql
                .FirstOrDefault();

            header.mfg_loc =
                !string.IsNullOrEmpty(history.MfgLoc)
                    ? history.MfgLoc
                    : inventoryControl?.Loc;

            header.ship_via_cd = history.ShipViaCd;
            header.slspsn_no = history.SlspsnNo;

            header.ship_to_addr_1 = history.ShipToAddr1;
            header.ship_to_addr_2 = history.ShipToAddr2;
            header.ship_to_addr_3 = history.ShipToAddr3;
            header.ship_to_addr_4 = history.ShipToAddr4;

            header.bill_to_addr_1 = history.BillToAddr1;
            header.bill_to_addr_2 = history.BillToAddr2;
            header.bill_to_addr_3 = history.BillToAddr3;
            header.bill_to_addr_4 = history.BillToAddr4;

            header.bill_to_name = history.BillToName;
            header.bill_to_country = history.BillToCountry;
            header.ship_to_name = history.BillToName;
        }

        private void CreateOrderDetails(CSEXSW_Rma rma, string rmaNo, List<csexsw_coustumer> rmaLines)
        {
            short sequence = 1;

            foreach (var line in rmaLines)
            {
                EnsureInventoryLocationExists(line);

                var item = _db2.imitmidx_sql
                    .FirstOrDefault(x =>
                        x.item_no == line.Coustumer);

                if (item is null)
                {
                    throw new InvalidOperationException(
                        $"Item '{line.Coustumer}' was not found.");
                }

                var inventory = _db2.iminvloc_sql
                    .FirstOrDefault(x =>
                        x.ItemNo == line.Coustumer &&
                        x.Loc == line.Loc);

                if (inventory is null)
                {
                    throw new InvalidOperationException(
                        $"Inventory location for item '{line.Coustumer}' " +
                        $"at location '{line.Loc}' was not found.");
                }

                var detail = new OERDTFIL_SQL
                {
                    OeBinFg = inventory.MultBinFg,
                    OeMfgMethod = item.MfgMethod,

                    item_desc_1 = item.item_desc_1,
                    item_desc_2 = item.item_desc_2,
                    uom = item.uom,

                    rma_no = FormatDetailRmaNumber(rmaNo),

                    oe_cus_no = rma.Customer.PadLeft(20),

                    apply_to_invc_no = line.Invoice,
                    apply_to_seq_no = line.Seq,
                    rma_seq_no = sequence,

                    item_no = line.Coustumer,
                    reason_cd = line.Retur,

                    pick_seq_no = " ",
                    oe_ord_no = " ",

                    oe_unique_seq_no = 0,
                    oe_unique_seq = 0,

                    action = line.Action,

                    oe_unit_cost = line.Cost,
                    oe_unit_price = line.Unit,
                    rma_qty_rtn_auth = line.Qty,

                    loc = line.Loc,
                    status = "O",

                    DiscountPct = 0,
                    UomRatio = 1,
                    RmaQtyRtnActual = 0,

                    OeUnitWeight = item.ItemWeight,
                    CommCalcType = item.CalcCommTp,
                    tax_fg = item.TaxFg,
                    OeSerLotCd = item.SerLotFg,
                    OeProdCat = item.prod_cat,

                    Extra10 = 0,
                    Extra11 = 0,
                    Extra12 = 0,
                    Extra13 = 0,
                    Extra14 = 0,
                    Extra15 = 0
                };

                _db2.OERDTFIL_SQL.Add(detail);

                sequence++;
            }
        }

        private void EnsureInventoryLocationExists(csexsw_coustumer line)
        {
            var exists = _db2.iminvloc_sql
                .Any(x =>
                    x.ItemNo == line.Coustumer &&
                    x.Loc == line.Loc);

            if (exists)
            {
                return;
            }

            var principalLocation = _db2.imitmidx_sql
                .Where(x =>
                    x.item_no == line.Coustumer)
                .Select(x => x.loc)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(principalLocation))
            {
                throw new InvalidOperationException(
                    $"No principal location found for item '{line.Coustumer}'.");
            }

            var sourceInventory = _db2.iminvloc_sql
                .FirstOrDefault(x =>
                    x.ItemNo == line.Coustumer &&
                    x.Loc == principalLocation);

            if (sourceInventory is null)
            {
                throw new InvalidOperationException(
                    $"No source inventory record found for item " +
                    $"'{line.Coustumer}' at location '{principalLocation}'.");
            }

            var inventoryLocation =
                CreateInventoryLocation(
                    line,
                    sourceInventory);

            _db2.iminvloc_sql.Add(inventoryLocation);
        }

        private iminvloc_sql CreateInventoryLocation(csexsw_coustumer line, iminvloc_sql source)
        {
            return new iminvloc_sql
            {
                InvClass = source.InvClass,
                ByrPlnr = source.ByrPlnr,

                price = source.price,
                std_cost = source.std_cost,
                Status = source.Status,
                ProdCat = source.ProdCat,

                ItemNo = line.Coustumer,
                Loc = line.Loc,
                Id = 0,

                MultBinFg = "Y",
                PricesApplyFlag = "N",
                DiscsApplyFg = "N",

                ActiveOrds = 0,
                AvgCost = 0,
                AvgFrcstError = 0,
                AvgUsage = 0,

                CostLastYr = 0,
                CostPtd = 0,
                CostYtd = 0,

                CubeHeight = 0,
                CubeLength = 0,
                CubeQtyPer = 0,
                CubeWidth = 0,

                DocToStkLdTm = 0,
                EconomicOrdQty = 0,

                Extra10 = 0,
                Extra11 = 0,
                Extra12 = 0,
                Extra13 = 0,
                Extra14 = 0,
                Extra15 = 0,

                FrzCost = 0,
                FrzQty = 0,
                IncludeParCost = 0,

                InvLocReturnCostLyr = 0,
                InvLocReturnCostPtd = 0,
                InvLocReturnCostYtd = 0,

                InvLocReturnSalesLyr = 0,
                InvLocReturnSalesPtd = 0,
                InvLocReturnSalesYtd = 0,

                LastCost = 0,
                LocQtyFld = 0,

                OrdUpToLvl = 0,
                PctErrLastCnt = 0,
                PoLeadTm = 0,
                PoMax = 0,
                PoMin = 0,

                PriorYearSls = 0,
                PriorYearUsage = 0,
                QtyAllocated = 0,

                QtyBkord = 0,
                QtyLastSold = 0,
                QtyOnHand = 0,
                QtyOnOrd = 0,

                QtyRejectLastYr = 0,
                QtyRejectPtd = 0,
                QtyRejectYtd = 0,

                QtyReturnedYtd = 0,
                QtyRtnLyr = 0,
                QtyRtnPtd = 0,

                QtyScrpLastYr = 0,
                QtyScrpPtd = 0,
                QtyScrpYtd = 0,

                QtySldPtd = 0,
                QtySoldLastYr = 0,
                QtySoldYtd = 0,

                RecomMinOrd = 0,
                ReorderLvl = 0,

                SafetyFctr = 0,
                SafetyStk = 0,

                SlsPrice = 0,
                SlsPtd = 0,
                SlsYtd = 0,

                SumOfErrors = 0,
                TagCost = 0,
                TagQty = 0,

                TargetMargin = 0,
                UsageFilter = 0,

                TmsCntdYtd = 0,
                UsagePtd = 0,
                UsageYtd = 0,

                UserFld8 = 0,
                UserFld9 = 0,
                UserFld10 = 0,
                UserFld11 = 0,
                UserFld12 = 0,
                UserFld13 = 0,

                UserFld17 = 0,
                UserFld18 = 0,
                UserFld19 = 0,
                UserFld20 = 0,

                UsgWghtFctr = 0
            };
        }

        private string FormatDetailRmaNumber(string rmaNumber)
        {
            var numrma = rmaNumber.PadLeft(8, ' ');
            var remove = numrma.Remove(0, 2).Trim();

            return $"  {remove}";
        }

        //public string Updateaprobar(int idsa, string commentt, string userId)
        //{
        //    var nextrma = "";
        //    var rma = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == idsa);

        //    var qualityManager = GetQualityManager(rma.Wherebuilt);
        //    var qualityDirector = GetEmployeeByRole(100031);
        //    var generalManager = GetEmployeeByRole(100032);

        //    var currentDate = DateTime.Now;

        //    string newComment = $"({currentDate:MM/dd/yyyy hh:mm:ss tt}) {rma.Approver}: {commentt.Trim()}";

        //    rma.Comment = string.IsNullOrWhiteSpace(rma.Comment) ? newComment : $"{rma.Comment}{Environment.NewLine}{newComment}";

        //    string retorno = string.Empty;

        //    rma.date_approved = currentDate;

        //    int id = idsa;

        //    var total = rma.Totalrmavalues;

        //    var arcusfil_sql = new arcusfil_sql();

        //    double tipo_cambio = total;

        //    arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == rma.Customer.Trim()).FirstOrDefault();

        //    var moneda = arcusfil_sql.curr_cd;


        //    if (moneda == "CNY")
        //    {

        //        var rate = _db2.Rate.Where(a => a.DateL == _db2.Rate.Max(a => a.DateL) && a.SourceCurrency == "USD").FirstOrDefault();
        //        tipo_cambio = Convert.ToDouble(total) / Convert.ToDouble(rate.RateExchange);


        //    }
        //    var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

        //    var formmatedUserId = int.Parse(userId.Trim());

        //    if (tipo_cambio >= TwoStepAuthorizationThreshold)
        //    {

        //        if (rma.res_id_approver == qualityDirector.Id)
        //        {
        //            if (formmatedUserId == qualityDirector.Id || formmatedUserId == _autoApprover.Id)
        //            {
        //                objDesdeDb.Status = "Pending";
        //                objDesdeDb.Approver = generalManager.Name;
        //                objDesdeDb.res_id_approver = generalManager.Id;
        //                retorno = "Pending";
        //                _db.Entry(objDesdeDb).State = EntityState.Modified;
        //                _db.SaveChanges();
        //            }
        //        }
        //        else if (rma.res_id_approver == generalManager.Id)
        //        {
        //            var oERMACTL_SQL = (from c in _db2.OERMACTL_SQL where c.ID == 1 select c).First();
        //            if (oERMACTL_SQL != null)
        //            {
        //                nextrma = oERMACTL_SQL.ctl_next_order_no;
        //            }

        //            int x = Int32.Parse(nextrma);
        //            char pad = ' ';
        //            string numString = x.ToString().PadLeft(8, pad);

        //            x = x + 1;
        //            var numeroFormato = x.ToString("D8");


        //            var objDesdeDbz = _db2.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

        //            objDesdeDbz.ctl_next_order_no = numeroFormato;
        //            _db2.SaveChanges();
        //            //var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);
        //            objDesdeDb.Status = "Approved";
        //            retorno = "Approved";
        //            objDesdeDb.turno = numString;
        //            string cus = objDesdeDb.Customer;
        //            string po = objDesdeDb.Customerpo;
        //            string comment = objDesdeDb.Description;

        //            _db.SaveChanges();

        //            Int16 seqq = 1;
        //            /*RMA HEADER*/
        //            var objDesdeDbt = new OERHDFIL_SQL();
        //            objDesdeDbt.UserDefFld3 = rma.reason;
        //            var AccountTypeCode = "";
        //            var cicmpy = new Cicmpy();
        //            cicmpy = _db2.Cicmpy.Where(a => a.CmpCode.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();
        //            AccountTypeCode = cicmpy.AccountTypeCode;
        //            var ArtypfilSql = new ArtypfilSql();
        //            ArtypfilSql = _db2.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
        //            objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;
        //            objDesdeDbt.dept = ArtypfilSql.SlsDpNo;
        //            objDesdeDbt.orig_trx_rt = 1;
        //            objDesdeDbt.curr_trx_rt = 1;
        //            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //            var oehdrhst_sql = new OEHDRHST_SQL();
        //            int oehdrhst_sqlcuantos = 0;
        //            var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();
        //            try
        //            {
        //                if (objDesdeDbT.Count() > 0)
        //                {
        //                    foreach (var filtro in objDesdeDbT)
        //                    {
        //                        if (filtro.Invoice != "0" && filtro.Invoice != "" && filtro.Invoice != null)
        //                        {

        //                            oehdrhst_sqlcuantos = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).Count();
        //                            Console.WriteLine("" + oehdrhst_sqlcuantos, Color.Red);
        //                            oehdrhst_sql = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).FirstOrDefault();

        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("" + ex, Color.Red);
        //            }

        //            if (oehdrhst_sqlcuantos == 0)
        //            {
        //                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //                arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();
        //                moneda = arcusfil_sql.curr_cd;
        //                objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;
        //                objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
        //                objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;
        //                objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
        //                objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
        //                objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;

        //                var AraltadrSql = _db2.AraltadrSql.Where(a => a.CusNo.Trim() == objDesdeDb.Customer.Trim() && a.CusAltAdrCd.Contains(objDesdeDb.Ship_To)).FirstOrDefault();
        //                objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();
        //                objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;
        //                objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
        //                objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;
        //                objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;
        //                objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;
        //                objDesdeDbt.tax_cd = AraltadrSql.TaxCd;
        //                objDesdeDbt.ship_to_country = AraltadrSql.Country;
        //                objDesdeDbt.bill_to_name = AraltadrSql.CusName;
        //                objDesdeDbt.bill_to_country = AraltadrSql.Country;
        //                objDesdeDbt.ship_to_name = AraltadrSql.CusName;
        //                objDesdeDbt.UserDefFld3 = rma.reason;
        //                var imctlfil_sql = new ImctlfilSql();
        //                imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();
        //                objDesdeDbt.mfg_loc = AraltadrSql.Loc != null && AraltadrSql.Loc != "" ? AraltadrSql.Loc : imctlfil_sql.Loc;
        //            }
        //            else
        //            {

        //                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //                objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
        //                objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
        //                moneda = oehdrhst_sql.CurrCd;
        //                objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
        //                objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;
        //                objDesdeDbt.UserDefFld3 = rma.reason;

        //                if (moneda != "CNY")
        //                {
        //                    objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
        //                }
        //                var imctlfil_sql = new ImctlfilSql();
        //                imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();
        //                objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "" ? oehdrhst_sql.MfgLoc : imctlfil_sql.Loc;
        //                objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;
        //                objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
        //                objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;
        //                objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;
        //                objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
        //                objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;
        //                objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
        //                objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
        //                objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
        //                objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;
        //                objDesdeDbt.bill_to_name = oehdrhst_sql.BillToName;
        //                objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
        //                objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

        //            }

        //            objDesdeDbt.contact = objDesdeDb.Contact;
        //            objDesdeDbt.phone_no = objDesdeDb.Phone;
        //            objDesdeDbt.fax_no = objDesdeDb.Fax;
        //            objDesdeDbt.phone_ext = objDesdeDb.Ext;
        //            objDesdeDbt.UserDefFld3 = rma.reason;
        //            objDesdeDbt.contact_email = objDesdeDb.Email;
        //            objDesdeDbt.user_def_fld_5 = "Normal                        ";
        //            objDesdeDbt.deter_rate_by = "O";
        //            objDesdeDbt.form_no = 1;
        //            objDesdeDbt.rma_no = numString;
        //            objDesdeDbt.cus_no = cus.Trim().PadLeft(20);
        //            objDesdeDbt.slspsn_pct_comm = 100;
        //            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //            objDesdeDbt.cus_ship_to = objDesdeDb.Ship_To;
        //            objDesdeDbt.oe_po_no = po;
        //            objDesdeDbt.rma_cmt = comment;
        //            objDesdeDbt.status = "O";
        //            objDesdeDbt.SlspsnCommAmt = 0;
        //            objDesdeDbt.SlspsnNo2 = 0;
        //            objDesdeDbt.SlspsnPctComm2 = 0;
        //            objDesdeDbt.SlspsnCommAmt2 = 0;
        //            objDesdeDbt.SlspsnPctComm3 = 0;
        //            objDesdeDbt.SlspsnCommAmt3 = 0;
        //            objDesdeDbt.SlspsnNo3 = 0;
        //            objDesdeDbt.Extra10 = 0;
        //            objDesdeDbt.Extra11 = 0;
        //            objDesdeDbt.Extra12 = 0;
        //            objDesdeDbt.Extra13 = 0;
        //            objDesdeDbt.Extra14 = 0;
        //            objDesdeDbt.Extra15 = 0;
        //            objDesdeDbt.TaxPct = 0;
        //            objDesdeDbt.TaxPct2 = 0;
        //            objDesdeDbt.TaxPct3 = 0;
        //            objDesdeDbt.DiscountPct = 0;
        //            objDesdeDbt.TotSlsAmt = 0;
        //            objDesdeDbt.TotSlsDisc = 0;
        //            objDesdeDbt.TotTaxAmt = 0;
        //            objDesdeDbt.TotCost = 0;
        //            objDesdeDbt.TotWeight = 0;
        //            objDesdeDbt.SlsTaxAmt1 = 0;
        //            objDesdeDbt.SlsTaxAmt2 = 0;
        //            objDesdeDbt.SlsTaxAmt3 = 0;
        //            objDesdeDbt.CommPct = 0;
        //            objDesdeDbt.CommAmt = 0;
        //            objDesdeDbt.AccumMiscAmt = 0;
        //            objDesdeDbt.AccumFrtAmt = 0;
        //            objDesdeDbt.AccumTotTaxAmt = 0;
        //            objDesdeDbt.AccumSlsTaxAmt = 0;
        //            objDesdeDbt.AccumTotSlsAmt = 0;
        //            objDesdeDbt.TotTaxCost = 0;
        //            objDesdeDbt.TotDollars = 0;
        //            objDesdeDbt.TaxFg = "N";
        //            objDesdeDbt.rma_dt_entered = currentDate;
        //            objDesdeDbt.LastActDt = currentDate;
        //            objDesdeDbt.ExpRecDate = currentDate;

        //            try
        //            {
        //                var objDesdeDbTr = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();

        //                if (objDesdeDbTr.Count() > 0)
        //                {

        //                    foreach (var filtro in objDesdeDbTr)
        //                    {
        //                        int total2 = _db2.iminvloc_sql.Where(a => a.ItemNo == filtro.Coustumer && a.Loc == filtro.Loc).Count();
        //                        if (total2 == 0)
        //                        {
        //                            var locprincipal = _db2.imitmidx_sql.Where(a => a.item_no == filtro.Coustumer).Select(s => s.loc).FirstOrDefault().ToString();

        //                            var result = _db2.iminvloc_sql.FirstOrDefault(a => a.ItemNo == filtro.Coustumer && a.Loc == locprincipal);

        //                            var objDesdeDb5 = new iminvloc_sql();


        //                            objDesdeDb5.ActiveOrds = 0;
        //                            objDesdeDb5.AvgCost = 0;
        //                            objDesdeDb5.AvgFrcstError = 0;
        //                            objDesdeDb5.AvgUsage = 0;
        //                            objDesdeDb5.InvClass = result.InvClass;
        //                            objDesdeDb5.ByrPlnr = result.ByrPlnr;
        //                            objDesdeDb5.CostLastYr = 0;
        //                            objDesdeDb5.CostPtd = 0;
        //                            objDesdeDb5.CostYtd = 0;
        //                            objDesdeDb5.CubeHeight = 0;
        //                            objDesdeDb5.CubeLength = 0;
        //                            objDesdeDb5.CubeQtyPer = 0;
        //                            objDesdeDb5.CubeWidth = 0;
        //                            //objDesdeDb5.DocField1 = 0;
        //                            //objDesdeDb5.DocField2 = 0;
        //                            //objDesdeDb5.DocField3 = 0;
        //                            objDesdeDb5.DocToStkLdTm = 0;
        //                            objDesdeDb5.EconomicOrdQty = 0;
        //                            objDesdeDb5.Extra10 = 0;
        //                            objDesdeDb5.Extra11 = 0;
        //                            objDesdeDb5.Extra12 = 0;
        //                            objDesdeDb5.Extra13 = 0;
        //                            objDesdeDb5.Extra14 = 0;
        //                            objDesdeDb5.Extra15 = 0;
        //                            objDesdeDb5.FrzCost = 0;
        //                            objDesdeDb5.FrzQty = 0;
        //                            objDesdeDb5.IncludeParCost = 0;
        //                            objDesdeDb5.InvLocReturnCostLyr = 0;
        //                            objDesdeDb5.InvLocReturnCostPtd = 0;
        //                            objDesdeDb5.InvLocReturnCostYtd = 0;
        //                            objDesdeDb5.InvLocReturnSalesLyr = 0;
        //                            objDesdeDb5.InvLocReturnSalesPtd = 0;
        //                            objDesdeDb5.InvLocReturnSalesYtd = 0;
        //                            objDesdeDb5.LastCost = 0;
        //                            objDesdeDb5.LocQtyFld = 0;

        //                            objDesdeDb5.OrdUpToLvl = 0;
        //                            objDesdeDb5.PctErrLastCnt = 0;
        //                            objDesdeDb5.PoLeadTm = 0;
        //                            objDesdeDb5.PoMax = 0;
        //                            objDesdeDb5.PoMin = 0;
        //                            objDesdeDb5.PriorYearSls = 0;
        //                            objDesdeDb5.PriorYearUsage = 0;
        //                            objDesdeDb5.QtyAllocated = 0;

        //                            objDesdeDb5.QtyBkord = 0;
        //                            objDesdeDb5.QtyLastSold = 0;
        //                            objDesdeDb5.QtyOnHand = 0;
        //                            objDesdeDb5.QtyOnOrd = 0;
        //                            objDesdeDb5.QtyRejectLastYr = 0;
        //                            objDesdeDb5.QtyRejectPtd = 0;
        //                            objDesdeDb5.QtyRejectYtd = 0;

        //                            objDesdeDb5.QtyReturnedYtd = 0;
        //                            objDesdeDb5.QtyRtnLyr = 0;
        //                            objDesdeDb5.QtyRtnPtd = 0;
        //                            objDesdeDb5.QtyScrpLastYr = 0;
        //                            objDesdeDb5.QtyScrpPtd = 0;
        //                            objDesdeDb5.QtyScrpYtd = 0;
        //                            objDesdeDb5.QtySldPtd = 0;
        //                            objDesdeDb5.QtySoldLastYr = 0;
        //                            objDesdeDb5.QtySoldYtd = 0;

        //                            objDesdeDb5.RecomMinOrd = 0;
        //                            objDesdeDb5.ReorderLvl = 0;
        //                            objDesdeDb5.SafetyFctr = 0;
        //                            objDesdeDb5.SafetyStk = 0;
        //                            objDesdeDb5.SlsPrice = 0;
        //                            objDesdeDb5.SlsPtd = 0;
        //                            objDesdeDb5.SlsYtd = 0;
        //                            objDesdeDb5.SumOfErrors = 0;
        //                            objDesdeDb5.TagCost = 0;
        //                            objDesdeDb5.TagQty = 0;
        //                            objDesdeDb5.TargetMargin = 0;
        //                            objDesdeDb5.UsageFilter = 0;
        //                            objDesdeDb5.TmsCntdYtd = 0;
        //                            objDesdeDb5.UsagePtd = 0;
        //                            objDesdeDb5.UsageYtd = 0;

        //                            objDesdeDb5.UserFld8 = 0;
        //                            objDesdeDb5.UserFld9 = 0;
        //                            objDesdeDb5.UserFld10 = 0;
        //                            objDesdeDb5.UserFld11 = 0;
        //                            objDesdeDb5.UserFld12 = 0;
        //                            objDesdeDb5.UserFld13 = 0;

        //                            objDesdeDb5.UserFld17 = 0;
        //                            objDesdeDb5.UserFld18 = 0;
        //                            objDesdeDb5.UserFld19 = 0;
        //                            objDesdeDb5.UserFld20 = 0;

        //                            objDesdeDb5.UsgWghtFctr = 0;
        //                            objDesdeDb5.PricesApplyFlag = "N";
        //                            objDesdeDb5.DiscsApplyFg = "N";
        //                            objDesdeDb5.price = result.price;
        //                            objDesdeDb5.std_cost = result.std_cost;
        //                            objDesdeDb5.Status = result.Status;
        //                            objDesdeDb5.ProdCat = result.ProdCat;
        //                            objDesdeDb5.MultBinFg = "Y";
        //                            objDesdeDb5.ItemNo = filtro.Coustumer;
        //                            objDesdeDb5.Loc = filtro.Loc;


        //                            objDesdeDb5.Id = 0;
        //                            _db2.iminvloc_sql.Add(objDesdeDb5);
        //                            _db2.SaveChanges();

        //                        }
        //                        var objDesdeDby = new OERDTFIL_SQL();
        //                        var objDesdeDbv = _db2.imitmidx_sql.FirstOrDefault(s => s.item_no == filtro.Coustumer);

        //                        var objDesdeDbvL = _db2.iminvloc_sql.FirstOrDefault(s => s.ItemNo == filtro.Coustumer && s.Loc == filtro.Loc);
        //                        objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;

        //                        objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;

        //                        objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
        //                        objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
        //                        objDesdeDby.uom = objDesdeDbv.uom;
        //                        char space = ' ';
        //                        var numrma = numString.PadLeft(8, space);
        //                        var remove = numrma.Remove(0, 2).Trim();
        //                        var n = string.Format("  {0}", remove);
        //                        objDesdeDby.rma_no = n;
        //                        objDesdeDby.oe_cus_no = cus.PadLeft(20);
        //                        objDesdeDby.apply_to_invc_no = filtro.Invoice;
        //                        objDesdeDby.apply_to_seq_no = filtro.Seq;
        //                        objDesdeDby.rma_seq_no = seqq;
        //                        objDesdeDby.item_no = filtro.Coustumer;
        //                        objDesdeDby.reason_cd = filtro.Retur;
        //                        objDesdeDby.pick_seq_no = " ";
        //                        objDesdeDby.oe_ord_no = " ";
        //                        objDesdeDby.oe_unique_seq_no = 0;
        //                        objDesdeDby.oe_unique_seq = 0;
        //                        objDesdeDby.action = filtro.Action;
        //                        objDesdeDby.oe_unit_cost = filtro.Cost;
        //                        objDesdeDby.oe_unit_price = filtro.Unit;
        //                        objDesdeDby.rma_qty_rtn_auth = filtro.Qty;

        //                        objDesdeDby.loc = filtro.Loc;
        //                        objDesdeDby.status = "O";
        //                        objDesdeDby.DiscountPct = 0;
        //                        objDesdeDby.UomRatio = 1;
        //                        objDesdeDby.RmaQtyRtnActual = 0;

        //                        objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
        //                        objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
        //                        objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
        //                        objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
        //                        objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;

        //                        objDesdeDby.Extra10 = 0;
        //                        objDesdeDby.Extra11 = 0;
        //                        objDesdeDby.Extra12 = 0;
        //                        objDesdeDby.Extra13 = 0;
        //                        objDesdeDby.Extra14 = 0;
        //                        objDesdeDby.Extra15 = 0;

        //                        using (var transaction = _db2.Database.BeginTransaction())
        //                        {
        //                            try
        //                            {
        //                                _db2.OERDTFIL_SQL.Add(objDesdeDby);
        //                                _db2.SaveChanges();
        //                                transaction.Commit();
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                if (!string.IsNullOrEmpty(ex.Message))
        //                                {
        //                                    transaction.Rollback();
        //                                }
        //                            }

        //                        }

        //                        seqq++;


        //                    }

        //                }
        //            }
        //            catch (Exception)
        //            {



        //            }
        //            int secreo = _db2.OERHDFIL_SQL.Where(a => a.rma_no == nextrma).Count();


        //            if (secreo > 0)
        //            {


        //                //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));



        //            }
        //            else
        //            {
        //                BackgroundJob.Enqueue(() => falloRMA(nextrma, rma.Id, rma.reason));

        //                //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

        //            }
        //        }
        //    }
        //    else
        //    {
        //        var oERMACTL_SQL = (from c in _db2.OERMACTL_SQL where c.ID == 1 select c).First();

        //        if (oERMACTL_SQL != null)
        //        {
        //            nextrma = oERMACTL_SQL.ctl_next_order_no;
        //        }
        //        int x = Int32.Parse(nextrma);
        //        char pad = ' ';
        //        string numString = x.ToString().PadLeft(8, pad);

        //        x = x + 1;
        //        var numeroFormato = x.ToString("D8");


        //        var objDesdeDbz = _db2.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

        //        objDesdeDbz.ctl_next_order_no = numeroFormato;
        //        _db2.SaveChanges();
        //        //var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

        //        objDesdeDb.Status = "Approved";
        //        retorno = "Approved";
        //        objDesdeDb.turno = numString;

        //        string cus = objDesdeDb.Customer;
        //        string po = objDesdeDb.Customerpo;
        //        string comment = objDesdeDb.Description;

        //        _db.SaveChanges();


        //        Int16 seqq = 1;

        //        var objDesdeDbt = new OERHDFIL_SQL();




        //        var AccountTypeCode = "";
        //        var cicmpy = new Cicmpy();
        //        cicmpy = _db2.Cicmpy.Where(a => a.CmpCode.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();
        //        AccountTypeCode = cicmpy.AccountTypeCode;




        //        var ArtypfilSql = new ArtypfilSql();
        //        ArtypfilSql = _db2.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
        //        objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

        //        objDesdeDbt.dept = ArtypfilSql.SlsDpNo;



        //        objDesdeDbt.orig_trx_rt = 1;

        //        objDesdeDbt.curr_trx_rt = 1;
        //        objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //        objDesdeDbt.UserDefFld3 = objDesdeDb.reason;

        //        var oehdrhst_sql = new OEHDRHST_SQL();
        //        int oehdrhst_sqlcuantos = 0;
        //        var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();
        //        try
        //        {
        //            if (objDesdeDbT.Count() > 0)
        //            {

        //                foreach (var filtro in objDesdeDbT)
        //                {
        //                    if (filtro.Invoice != "0" && filtro.Invoice != "" && filtro.Invoice != null)
        //                    {

        //                        oehdrhst_sqlcuantos = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).Count();
        //                        Console.WriteLine("" + oehdrhst_sqlcuantos, Color.Red);
        //                        oehdrhst_sql = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).FirstOrDefault();

        //                    }

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("" + ex, Color.Red);
        //        }

        //        if (oehdrhst_sqlcuantos == 0)
        //        {


        //            arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();

        //            moneda = arcusfil_sql.curr_cd;
        //            objDesdeDbt.curr_cd = arcusfil_sql.curr_cd;


        //            objDesdeDbt.bill_to_addr_4 = (arcusfil_sql.City)?.Trim() + ", " + (arcusfil_sql.State)?.Trim() + " " + (arcusfil_sql.Zip)?.Trim();
        //            objDesdeDbt.ar_terms_cd = arcusfil_sql.ArTermsCd;





        //            objDesdeDbt.bill_to_addr_1 = arcusfil_sql.Addr1;
        //            objDesdeDbt.bill_to_addr_2 = arcusfil_sql.Addr2;
        //            objDesdeDbt.bill_to_addr_3 = arcusfil_sql.Addr3;




        //            var AraltadrSql = _db2.AraltadrSql.Where(a => a.CusNo.Trim() == objDesdeDb.Customer.Trim() && a.CusAltAdrCd.Contains(objDesdeDb.Ship_To)).FirstOrDefault();
        //            objDesdeDbt.ship_to_addr_4 = (AraltadrSql.City)?.Trim() + ", " + (AraltadrSql.State)?.Trim() + " " + (AraltadrSql.Zip)?.Trim();

        //            objDesdeDbt.ship_via_cd = AraltadrSql.ShipViaCd;

        //            objDesdeDbt.slspsn_no = AraltadrSql.SlspsnNo;
        //            objDesdeDbt.ship_to_addr_1 = AraltadrSql.Addr1;

        //            objDesdeDbt.ship_to_addr_2 = AraltadrSql.Addr2;

        //            objDesdeDbt.ship_to_addr_3 = AraltadrSql.Addr3;

        //            objDesdeDbt.tax_cd = AraltadrSql.TaxCd;

        //            objDesdeDbt.ship_to_country = AraltadrSql.Country;
        //            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //            objDesdeDbt.UserDefFld3 = objDesdeDb.reason;

        //            objDesdeDbt.bill_to_name = AraltadrSql.CusName;
        //            objDesdeDbt.bill_to_country = AraltadrSql.Country;
        //            objDesdeDbt.ship_to_name = AraltadrSql.CusName;

        //            //var imctlfil_sql = new ImctlfilSql();
        //            //imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();


        //            //if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
        //            //{
        //            //    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
        //            //}
        //            //else
        //            //{
        //            //    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
        //            //}

        //        }
        //        else
        //        {

        //            objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
        //            objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
        //            moneda = oehdrhst_sql.CurrCd;
        //            objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
        //            objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;




        //            objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //            objDesdeDbt.UserDefFld3 = objDesdeDb.reason;
        //            if (moneda != "CNY")
        //            {
        //                objDesdeDbt.curr_trx_rt = oehdrhst_sql.CurrTrxRt;
        //            }

        //            var imctlfil_sql = new ImctlfilSql();
        //            imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();
        //            if (oehdrhst_sql.MfgLoc != null && oehdrhst_sql.MfgLoc != "")
        //            {
        //                objDesdeDbt.mfg_loc = oehdrhst_sql.MfgLoc;
        //            }
        //            else
        //            {
        //                objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
        //            }



        //            objDesdeDbt.ship_via_cd = oehdrhst_sql.ShipViaCd;

        //            objDesdeDbt.slspsn_no = oehdrhst_sql.SlspsnNo;
        //            objDesdeDbt.ship_to_addr_1 = oehdrhst_sql.ShipToAddr1;

        //            objDesdeDbt.ship_to_addr_2 = oehdrhst_sql.ShipToAddr2;

        //            objDesdeDbt.ship_to_addr_3 = oehdrhst_sql.ShipToAddr3;
        //            objDesdeDbt.ship_to_addr_4 = oehdrhst_sql.ShipToAddr4;





        //            objDesdeDbt.bill_to_addr_4 = oehdrhst_sql.BillToAddr4;
        //            objDesdeDbt.bill_to_addr_1 = oehdrhst_sql.BillToAddr1;
        //            objDesdeDbt.bill_to_addr_2 = oehdrhst_sql.BillToAddr2;
        //            objDesdeDbt.bill_to_addr_3 = oehdrhst_sql.BillToAddr3;


        //            objDesdeDbt.bill_to_name = oehdrhst_sql.BillToName;
        //            objDesdeDbt.bill_to_country = oehdrhst_sql.BillToCountry;
        //            objDesdeDbt.ship_to_name = oehdrhst_sql.BillToName;

        //        }

        //        objDesdeDbt.contact = objDesdeDb.Contact;
        //        objDesdeDbt.phone_no = objDesdeDb.Phone;
        //        objDesdeDbt.fax_no = objDesdeDb.Fax;
        //        objDesdeDbt.phone_ext = objDesdeDb.Ext;
        //        objDesdeDbt.contact_email = objDesdeDb.Email;
        //        objDesdeDbt.user_def_fld_5 = "Normal                 ";
        //        objDesdeDbt.deter_rate_by = "O";
        //        objDesdeDbt.form_no = 1;
        //        objDesdeDbt.rma_no = numString;
        //        objDesdeDbt.cus_no = cus.Trim().PadLeft(20);
        //        objDesdeDbt.slspsn_pct_comm = 100;

        //        objDesdeDbt.cus_ship_to = objDesdeDb.Ship_To;
        //        objDesdeDbt.oe_po_no = po;
        //        objDesdeDbt.rma_cmt = comment;
        //        objDesdeDbt.status = "O";
        //        objDesdeDbt.SlspsnCommAmt = 0;
        //        objDesdeDbt.SlspsnNo2 = 0;
        //        objDesdeDbt.SlspsnPctComm2 = 0;
        //        objDesdeDbt.SlspsnCommAmt2 = 0;
        //        objDesdeDbt.SlspsnPctComm3 = 0;
        //        objDesdeDbt.SlspsnCommAmt3 = 0;
        //        objDesdeDbt.SlspsnNo3 = 0;
        //        objDesdeDbt.Extra10 = 0;
        //        objDesdeDbt.Extra11 = 0;
        //        objDesdeDbt.Extra12 = 0;
        //        objDesdeDbt.Extra13 = 0;
        //        objDesdeDbt.Extra14 = 0;

        //        objDesdeDbt.UserDefFld1 = "FOB SOURCE";
        //        objDesdeDbt.UserDefFld3 = objDesdeDb.reason;
        //        objDesdeDbt.Extra15 = 0;
        //        objDesdeDbt.TaxPct = 0;
        //        objDesdeDbt.TaxPct2 = 0;
        //        objDesdeDbt.TaxPct3 = 0;
        //        objDesdeDbt.DiscountPct = 0;
        //        objDesdeDbt.TotSlsAmt = 0;
        //        objDesdeDbt.TotSlsDisc = 0;
        //        objDesdeDbt.TotTaxAmt = 0;
        //        objDesdeDbt.TotCost = 0;
        //        objDesdeDbt.TotWeight = 0;
        //        objDesdeDbt.SlsTaxAmt1 = 0;
        //        objDesdeDbt.SlsTaxAmt2 = 0;
        //        objDesdeDbt.SlsTaxAmt3 = 0;
        //        objDesdeDbt.CommPct = 0;
        //        objDesdeDbt.CommAmt = 0;
        //        objDesdeDbt.AccumMiscAmt = 0;
        //        objDesdeDbt.AccumFrtAmt = 0;
        //        objDesdeDbt.AccumTotTaxAmt = 0;
        //        objDesdeDbt.AccumSlsTaxAmt = 0;
        //        objDesdeDbt.AccumTotSlsAmt = 0;
        //        objDesdeDbt.TotTaxCost = 0;
        //        objDesdeDbt.TotDollars = 0;
        //        objDesdeDbt.TaxFg = "N";

        //        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        //        DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
        //        objDesdeDbt.rma_dt_entered = date;
        //        objDesdeDbt.LastActDt = date;
        //        objDesdeDbt.ExpRecDate = date;

        //        //_db2.OERHDFIL_SQL.Add(objDesdeDbt);

        //        //_db2.SaveChanges();
        //        try
        //        {
        //            var objDesdeDbTr = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();

        //            if (objDesdeDbTr.Count() > 0)
        //            {

        //                foreach (var filtro in objDesdeDbTr)
        //                {
        //                    int total2 = _db2.iminvloc_sql.Where(a => a.ItemNo == filtro.Coustumer && a.Loc == filtro.Loc).Count();
        //                    if (total2 == 0)
        //                    {
        //                        var locprincipal = _db2.imitmidx_sql.Where(a => a.item_no == filtro.Coustumer).Select(s => s.loc).FirstOrDefault().ToString();

        //                        var result = _db2.iminvloc_sql.FirstOrDefault(a => a.ItemNo == filtro.Coustumer && a.Loc == locprincipal);

        //                        var objDesdeDb5 = new iminvloc_sql();


        //                        objDesdeDb5.ActiveOrds = 0;
        //                        objDesdeDb5.AvgCost = 0;
        //                        objDesdeDb5.AvgFrcstError = 0;
        //                        objDesdeDb5.AvgUsage = 0;
        //                        objDesdeDb5.InvClass = result.InvClass;
        //                        objDesdeDb5.ByrPlnr = result.ByrPlnr;
        //                        objDesdeDb5.CostLastYr = 0;
        //                        objDesdeDb5.CostPtd = 0;
        //                        objDesdeDb5.CostYtd = 0;
        //                        objDesdeDb5.CubeHeight = 0;
        //                        objDesdeDb5.CubeLength = 0;
        //                        objDesdeDb5.CubeQtyPer = 0;
        //                        objDesdeDb5.CubeWidth = 0;
        //                        //objDesdeDb5.DocField1 = 0;
        //                        //objDesdeDb5.DocField2 = 0;
        //                        //objDesdeDb5.DocField3 = 0;
        //                        objDesdeDb5.DocToStkLdTm = 0;
        //                        objDesdeDb5.EconomicOrdQty = 0;

        //                        objDesdeDb5.Extra10 = 0;
        //                        objDesdeDb5.Extra11 = 0;
        //                        objDesdeDb5.Extra12 = 0;
        //                        objDesdeDb5.Extra13 = 0;
        //                        objDesdeDb5.Extra14 = 0;
        //                        objDesdeDb5.Extra15 = 0;

        //                        objDesdeDb5.FrzCost = 0;
        //                        objDesdeDb5.FrzQty = 0;
        //                        objDesdeDb5.IncludeParCost = 0;
        //                        objDesdeDb5.InvLocReturnCostLyr = 0;

        //                        objDesdeDb5.InvLocReturnCostPtd = 0;

        //                        objDesdeDb5.InvLocReturnCostYtd = 0;
        //                        objDesdeDb5.InvLocReturnSalesLyr = 0;
        //                        objDesdeDb5.InvLocReturnSalesPtd = 0;
        //                        objDesdeDb5.InvLocReturnSalesYtd = 0;

        //                        objDesdeDb5.LastCost = 0;
        //                        objDesdeDb5.LocQtyFld = 0;

        //                        objDesdeDb5.OrdUpToLvl = 0;
        //                        objDesdeDb5.PctErrLastCnt = 0;
        //                        objDesdeDb5.PoLeadTm = 0;
        //                        objDesdeDb5.PoMax = 0;
        //                        objDesdeDb5.PoMin = 0;
        //                        //objDesdeDb5.PoMult = 0;
        //                        objDesdeDb5.PriorYearSls = 0;
        //                        objDesdeDb5.PriorYearUsage = 0;
        //                        objDesdeDb5.QtyAllocated = 0;

        //                        objDesdeDb5.QtyBkord = 0;
        //                        objDesdeDb5.QtyLastSold = 0;
        //                        objDesdeDb5.QtyOnHand = 0;
        //                        objDesdeDb5.QtyOnOrd = 0;
        //                        objDesdeDb5.QtyRejectLastYr = 0;
        //                        objDesdeDb5.QtyRejectPtd = 0;
        //                        objDesdeDb5.QtyRejectYtd = 0;

        //                        objDesdeDb5.QtyReturnedYtd = 0;
        //                        objDesdeDb5.QtyRtnLyr = 0;
        //                        objDesdeDb5.QtyRtnPtd = 0;
        //                        objDesdeDb5.QtyScrpLastYr = 0;
        //                        objDesdeDb5.QtyScrpPtd = 0;
        //                        objDesdeDb5.QtyScrpYtd = 0;
        //                        objDesdeDb5.QtySldPtd = 0;
        //                        objDesdeDb5.QtySoldLastYr = 0;
        //                        objDesdeDb5.QtySoldYtd = 0;

        //                        objDesdeDb5.RecomMinOrd = 0;
        //                        objDesdeDb5.ReorderLvl = 0;
        //                        objDesdeDb5.SafetyFctr = 0;
        //                        objDesdeDb5.SafetyStk = 0;
        //                        objDesdeDb5.SlsPrice = 0;
        //                        objDesdeDb5.SlsPtd = 0;
        //                        objDesdeDb5.SlsYtd = 0;
        //                        objDesdeDb5.SumOfErrors = 0;
        //                        objDesdeDb5.TagCost = 0;
        //                        objDesdeDb5.TagQty = 0;
        //                        objDesdeDb5.TargetMargin = 0;
        //                        objDesdeDb5.UsageFilter = 0;
        //                        objDesdeDb5.TmsCntdYtd = 0;
        //                        objDesdeDb5.UsagePtd = 0;
        //                        objDesdeDb5.UsageYtd = 0;

        //                        objDesdeDb5.UserFld8 = 0;
        //                        objDesdeDb5.UserFld9 = 0;
        //                        objDesdeDb5.UserFld10 = 0;
        //                        objDesdeDb5.UserFld11 = 0;
        //                        objDesdeDb5.UserFld12 = 0;
        //                        objDesdeDb5.UserFld13 = 0;

        //                        objDesdeDb5.UserFld17 = 0;
        //                        objDesdeDb5.UserFld18 = 0;
        //                        objDesdeDb5.UserFld19 = 0;
        //                        objDesdeDb5.UserFld20 = 0;

        //                        objDesdeDb5.UsgWghtFctr = 0;
        //                        objDesdeDb5.PricesApplyFlag = "N";
        //                        objDesdeDb5.DiscsApplyFg = "N";
        //                        objDesdeDb5.price = result.price;
        //                        objDesdeDb5.std_cost = result.std_cost;
        //                        objDesdeDb5.Status = result.Status;
        //                        objDesdeDb5.ProdCat = result.ProdCat;
        //                        objDesdeDb5.MultBinFg = "Y";
        //                        objDesdeDb5.ItemNo = filtro.Coustumer;
        //                        objDesdeDb5.Loc = filtro.Loc;


        //                        objDesdeDb5.Id = 0;
        //                        _db2.iminvloc_sql.Add(objDesdeDb5);
        //                        _db2.SaveChanges();

        //                    }
        //                    var objDesdeDby = new OERDTFIL_SQL();
        //                    var objDesdeDbv = _db2.imitmidx_sql.FirstOrDefault(s => s.item_no == filtro.Coustumer);

        //                    var objDesdeDbvL = _db2.iminvloc_sql.FirstOrDefault(s => s.ItemNo == filtro.Coustumer && s.Loc == filtro.Loc);
        //                    objDesdeDby.OeBinFg = objDesdeDbvL.MultBinFg;

        //                    objDesdeDby.OeMfgMethod = objDesdeDbv.MfgMethod;

        //                    objDesdeDby.item_desc_1 = objDesdeDbv.item_desc_1;
        //                    objDesdeDby.item_desc_2 = objDesdeDbv.item_desc_2;
        //                    objDesdeDby.uom = objDesdeDbv.uom;
        //                    char space = ' ';
        //                    var numrma = numString.PadLeft(8, space);
        //                    var remove = numrma.Remove(0, 2).Trim();
        //                    var n = string.Format("  {0}", remove);
        //                    objDesdeDby.rma_no = n;
        //                    objDesdeDby.oe_cus_no = cus.PadLeft(20);
        //                    objDesdeDby.apply_to_invc_no = filtro.Invoice;
        //                    objDesdeDby.apply_to_seq_no = filtro.Seq;
        //                    objDesdeDby.rma_seq_no = seqq;
        //                    objDesdeDby.item_no = filtro.Coustumer;
        //                    objDesdeDby.reason_cd = filtro.Retur;
        //                    objDesdeDby.pick_seq_no = " ";
        //                    objDesdeDby.oe_ord_no = " ";
        //                    objDesdeDby.oe_unique_seq_no = 0;
        //                    objDesdeDby.oe_unique_seq = 0;
        //                    objDesdeDby.action = filtro.Action;
        //                    objDesdeDby.oe_unit_cost = filtro.Cost;
        //                    objDesdeDby.oe_unit_price = filtro.Unit;
        //                    objDesdeDby.rma_qty_rtn_auth = filtro.Qty;

        //                    objDesdeDby.loc = filtro.Loc;
        //                    objDesdeDby.status = "O";
        //                    objDesdeDby.DiscountPct = 0;
        //                    objDesdeDby.UomRatio = 1;
        //                    objDesdeDby.RmaQtyRtnActual = 0;

        //                    objDesdeDby.OeUnitWeight = objDesdeDbv.ItemWeight;
        //                    objDesdeDby.CommCalcType = objDesdeDbv.CalcCommTp;
        //                    objDesdeDby.tax_fg = objDesdeDbv.TaxFg;
        //                    objDesdeDby.OeSerLotCd = objDesdeDbv.SerLotFg;
        //                    objDesdeDby.OeProdCat = objDesdeDbv.prod_cat;

        //                    objDesdeDby.Extra10 = 0;
        //                    objDesdeDby.Extra11 = 0;
        //                    objDesdeDby.Extra12 = 0;
        //                    objDesdeDby.Extra13 = 0;
        //                    objDesdeDby.Extra14 = 0;
        //                    objDesdeDby.Extra15 = 0;

        //                    using (var transaction = _db2.Database.BeginTransaction())
        //                    {
        //                        try
        //                        {
        //                            _db2.OERDTFIL_SQL.Add(objDesdeDby);
        //                            _db2.SaveChanges();
        //                            transaction.Commit();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            if (!string.IsNullOrEmpty(ex.Message))
        //                            {
        //                                transaction.Rollback();
        //                            }
        //                        }

        //                    }

        //                    seqq++;


        //                }

        //            }
        //        }
        //        catch (Exception)
        //        {



        //        }
        //        int secreo = _db2.OERHDFIL_SQL.Where(a => a.rma_no == nextrma).Count();


        //        if (secreo > 0)
        //        {


        //            //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));



        //        }
        //        else
        //        {
        //            BackgroundJob.Enqueue(() => falloRMA(nextrma, rma.Id, rma.reason));

        //            //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

        //        }
        //    }

        //    return retorno;

        //}


        public void UpdateRechazo(int id)
        {

            var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status = "Reject";


            _db.SaveChanges();

        }
        public void UpdateAprove(int id)
        {
            var revisor = (from c in _db.CSEXSW_Approver
                           where c.Rango == "GM"
                           select c.Approver).First();
            var total = (from c in _db.CSEXSW_Rma
                         where c.Id == id
                         select c.Totalrmavalues).First();
            var client = (from c in _db.CSEXSW_Rma
                          where c.Id == id
                          select c.Customer).First();
            var arcusfil_sql = new arcusfil_sql();

            double tipo_cambio = total;
            arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == client.Trim()).FirstOrDefault();

            var moneda = arcusfil_sql.curr_cd;


            if (moneda == "CNY")
            {

                var rate = _db2.Rate.Where(a => a.DateL == _db2.Rate.Max(a => a.DateL) && a.SourceCurrency == "USD").FirstOrDefault();
                tipo_cambio = Convert.ToDouble(total) / Convert.ToDouble(rate.RateExchange);


            }

            if (tipo_cambio >= 20000)
            {
                var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

                objDesdeDb.Status = "Pending";
                objDesdeDb.Approver = revisor;

                _db.SaveChanges();
            }
            else
            {
                var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

                objDesdeDb.Status = "Approved";
                objDesdeDb.date_approved = DateTime.Now;

                _db.SaveChanges();

            }
        }


        public int intRMA()
        {
            int id = 0;

            id = (from c in _db.CSEXSW_Rma
                  orderby c.Id descending
                  select c.Id).FirstOrDefault();





            return id;


        }
        public void Update(CSEXSW_Rma rma)
        {
            var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == rma.Id);
            objDesdeDb.Approver = rma.Approver;
            objDesdeDb.Contact = rma.Contact;
            objDesdeDb.Email = rma.Email;

            objDesdeDb.Ext = rma.Ext;
            objDesdeDb.Company = rma.Company;
            objDesdeDb.Fax = rma.Fax;
            objDesdeDb.Phone = rma.Phone;



            objDesdeDb.Wherebuilt = rma.Wherebuilt;
            objDesdeDb.Ship_To = rma.Ship_To;
            objDesdeDb.Customercomplait = rma.Customercomplait;
            objDesdeDb.Customerpartno = rma.Customerpartno;
            objDesdeDb.Customerpo = rma.Customerpo;
            objDesdeDb.Date = rma.Date;
            objDesdeDb.Description = rma.Description;
            objDesdeDb.Preparado = rma.Preparado;
            objDesdeDb.RMA500 = rma.RMA500;
            objDesdeDb.Sumbit = rma.Sumbit;
            objDesdeDb.Rmarequest = rma.Rmarequest;
            objDesdeDb.Rmatypeofrequest = rma.Rmatypeofrequest;
            objDesdeDb.Customer = rma.Customer;
            objDesdeDb.turno = rma.turno;
            objDesdeDb.Status = rma.Status;
            objDesdeDb.Comment = rma.Comment;
            objDesdeDb.Totalrmavalues = rma.Totalrmavalues;

            _db.SaveChanges();

        }

        private Approver GetEmployeeByRole(int roleId)
        {
            var role = _db.HRRoles.FirstOrDefault(x => x.RoleID == roleId);

            if (role == null) return null;

            var employee = _db.humres.FirstOrDefault(x => x.res_id == role.EmpID);
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
    }
}
