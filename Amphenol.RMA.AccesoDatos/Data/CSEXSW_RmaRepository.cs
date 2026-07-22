using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.ModelsM10;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Organization.Client;
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
        private readonly DbContextM10 _db;
        private readonly DbContext100 _db2;

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
                List<CSEXSW_Rma_ViewModel> result = new List<CSEXSW_Rma_ViewModel>();

                string connectionString = _configuration.GetConnectionString("ConnectionM10").ToString();
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    string query = @"Select 
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
                                Left Join csexsw_coustumer With(NoLock) On CSEXSW_Rma.Id = csexsw_coustumer.RmaId
                                Group By CSEXSW_Rma.Id,CSEXSW_Rma.Rmarequest,CSEXSW_Rma.Date,CSEXSW_Rma.Customerpartno,CSEXSW_Rma.Customerpo,CSEXSW_Rma.Customercomplait,CSEXSW_Rma.Description,CSEXSW_Rma.Rmatypeofrequest,
                                CSEXSW_Rma.Totalrmavalues,CSEXSW_Rma.Wherebuilt,CSEXSW_Rma.Preparado,CSEXSW_Rma.Sumbit,CSEXSW_Rma.Status,CSEXSW_Rma.turno,CSEXSW_Rma.Approver,CSEXSW_Rma.res_id,CSEXSW_Rma.date_approved
                                Order by CSEXSW_Rma.Date";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var rmaNumber = rdr["turno"].ToString();

                        CSEXSW_Rma_ViewModel model = new CSEXSW_Rma_ViewModel
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            qty = Convert.ToInt32(rdr["qty"]),
                            parts = Convert.ToInt32(rdr["parts"]),
                            Rmarequest = rdr["Rmarequest"].ToString(),
                            Date = rdr["Date"].ToString(),
                            Customerpartno = rdr["Customerpartno"].ToString(),
                            Customerpo = rdr["Customerpo"].ToString(),
                            Customercomplait = rdr["Customercomplait"].ToString(),
                            Description = rdr["Description"].ToString(),
                            Rmatypeofrequest = rdr["Rmatypeofrequest"].ToString(),
                            Totalrmavalues = Convert.ToDouble(rdr["Totalrmavalues"]),
                            Wherebuilt = rdr["Wherebuilt"].ToString(),
                            Preparado = rdr["Preparado"].ToString(),
                            Sumbit = rdr["Sumbit"].ToString(),
                            Status = rdr["Status"].ToString(),
                            turno = rmaNumber,
                            Approver = rdr["Approver"].ToString(),
                            res_id = Convert.ToInt32(rdr["res_id"]),
                            formated_date_approved = rdr["formated_date_approved"].ToString()
                        };

                        var receivedlines = _db2.OERDTFIL_SQL
                            .AsNoTracking()
                            .Where(o => o.rma_no.Trim() == rmaNumber.Trim());

                        model.CanGenerateOrder =
                            receivedlines.Any() &&
                            receivedlines.All(o => o.RmaQtyRtnActual > 0) &&
                            model.Status == "Approved";

                        model.OrderNumber = _db2.OEORDHDR_SQL
                        .AsNoTracking()
                        .Where(o => o.RmaNo.Trim() == rmaNumber.Trim())
                        .Select(o => o.OrdNo)
                        .FirstOrDefault();

                        result.Add(model);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
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
                if (rma.res_id_approver==gm)
                {
                    var emp = _db.humres.Where(s => s.res_id == qd).FirstOrDefault().fullname;
                    rma.res_id_approver =qd;
                    rma.Approver = emp.Trim();

                }

            } 

            rma.Status = "Remark";
            rma.Comment ="Remark: "+ commentrema;

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
                Console.WriteLine("" + ex,
     Color.Red);
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
            //string QM = _db.CSEXSW_Approver.Where(a => a.Rango == "QM").Select(a => a.Approver).FirstOrDefault();
            //string QD = _db.CSEXSW_Approver.Where(a => a.Rango == "QD").Select(a => a.Approver).FirstOrDefault();
            //string GM = _db.CSEXSW_Approver.Where(a => a.Rango == "GM").Select(a => a.Approver).FirstOrDefault();
            //string Dee = _db.CSEXSW_Approver.Where(a => a.Rango == "Dee").Select(a => a.Approver).FirstOrDefault();
            //string Controller = _db.CSEXSW_Approver.Where(a => a.Rango == "Controller").Select(a => a.Approver).FirstOrDefault();
            //string CSM = _db.CSEXSW_Approver.Where(a => a.Rango == "CSM").Select(a => a.Approver).FirstOrDefault();


            var objDesdeDbs = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == ids);

            objDesdeDbs.Status = "Rejected";
            objDesdeDbs.Comment = comment;
            _db.SaveChanges();
            //BackgroundJob.Enqueue(() => SendMailAsync5(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

        }
        public string Updateaprobar(int idsa, string commentt, string var, string mail1, string mail2, string mail3, string mail4, string mail5, string mail6, string mailp)
        {
            var nextrma = "";
            var objDesdeDbs = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == idsa);


            string QM = "";
            humres empQM = null;
            if (objDesdeDbs.Wherebuilt == "Nogales")
            {
                //QM Quality Manager NOG)
                var QMM10 = _db.HRRoles.Where(s => s.RoleID == 100030).FirstOrDefault();
                if (QMM10 != null)
                {
                    empQM = _db.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                    QM = empQM.fullname;
                }
            }
            if (objDesdeDbs.Wherebuilt== "Mesa")
            {  
                //QM Quality Manager NOG)
                var QMM10 = _db.HRRoles.Where(s => s.RoleID == 100062).FirstOrDefault();
                if (QMM10 != null)
                {
                    empQM = _db.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                    QM = empQM.fullname;
                }

            }
            if(objDesdeDbs.Wherebuilt == "Endicott")
            {
                //Endicot
                //QM Quality Manager END)
                var QMM10 = _db.HRRoles.Where(s => s.RoleID == 100039).FirstOrDefault();
                if (QMM10 != null)
                {
                    empQM = _db.humres.Where(s => s.res_id == QMM10.EmpID).FirstOrDefault();
                    QM = empQM.fullname;
                }
            }



            //QD (Quality Director)
            var QDM10 = _db.HRRoles.Where(d => d.RoleID == 100031).FirstOrDefault();
            string QD = "";
            humres empQD = null;
            int residQD = 0;
            if (QDM10 != null)
            {
                empQD = _db.humres.Where(s => s.res_id == QDM10.EmpID).FirstOrDefault();
                QD = empQD.fullname;
                residQD = empQD.res_id;
            }
            //GM (General Manager)
            var GMM10 = _db.HRRoles.Where(d => d.RoleID == 100032).FirstOrDefault();
            humres empGM = null;
            string GM = "";
            int residGM = 0;
            if (GMM10 != null)
            {
                empGM = _db.humres.Where(s => s.res_id == GMM10.EmpID).FirstOrDefault();
                GM = empGM.fullname;
                residGM = empGM.res_id;
            }
            //string QM = _db.CSEXSW_Approver.Where(a => a.Rango == "QM").Select(a => a.Approver).FirstOrDefault();
            //string QD = _db.CSEXSW_Approver.Where(a => a.Rango == "QD").Select(a => a.Approver).FirstOrDefault();
            //string GM = _db.CSEXSW_Approver.Where(a => a.Rango == "GM").Select(a => a.Approver).FirstOrDefault();
            //string Dee = _db.CSEXSW_Approver.Where(a => a.Rango == "Dee").Select(a => a.Approver).FirstOrDefault();
            //string Controller = _db.CSEXSW_Approver.Where(a => a.Rango == "Controller").Select(a => a.Approver).FirstOrDefault();
            //string CSM = _db.CSEXSW_Approver.Where(a => a.Rango == "CSM").Select(a => a.Approver).FirstOrDefault();


            objDesdeDbs.Comment = objDesdeDbs.Comment + "<br />" + var + ":" + commentt;
            string retorno=string.Empty;

            objDesdeDbs.date_approved = DateTime.Now;

            _db.SaveChanges();
            int id = idsa;
            //var revisor = (from c in _db.CSEXSW_Approver
            //               where c.Rango == "GM"
            //               select c.Approver).First();

            var total = (from c in _db.CSEXSW_Rma
                         where c.Id == id
                         select c.Totalrmavalues).First();

            var arcusfil_sql = new arcusfil_sql();

            double tipo_cambio = total;
            arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim() == objDesdeDbs.Customer.Trim()).FirstOrDefault();

            var moneda = arcusfil_sql.curr_cd;


            if (moneda == "CNY")
            {

                var rate = _db2.Rate.Where(a => a.DateL == _db2.Rate.Max(a => a.DateL) && a.SourceCurrency == "USD").FirstOrDefault();
                tipo_cambio = Convert.ToDouble(total) / Convert.ToDouble(rate.RateExchange);


            }
            var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

            if (tipo_cambio >= 20000 && var.Trim() == residQD.ToString().Trim())
            {
                objDesdeDb.Status = "Pending";
                objDesdeDb.Approver = GM;
                objDesdeDb.res_id_approver = residGM;
                retorno = "Pending";
                _db.Entry(objDesdeDb).State = EntityState.Modified;
                _db.SaveChanges();
            }
            if (tipo_cambio >= 20000 && var.Trim() == residGM.ToString().Trim())
            {
                var oERMACTL_SQL = (from c in _db2.OERMACTL_SQL where c.ID == 1 select c).First();
                //siguiente numero de rma 00800042
                //nextrma = (from c in _db2.OERMACTL_SQL
                //           where c.ID == 1
                //           select c.ctl_next_order_no).First();

                if(oERMACTL_SQL != null)
                {
                    nextrma = oERMACTL_SQL.ctl_next_order_no;
                }

                int x = Int32.Parse(nextrma);
                char pad = ' ';
                string numString = x.ToString().PadLeft(8, pad);

                x = x + 1;
                var numeroFormato = x.ToString("D8");


                var objDesdeDbz = _db2.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

                objDesdeDbz.ctl_next_order_no = numeroFormato;
                _db2.SaveChanges();
                //var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

                objDesdeDb.Status = "Approved";
                retorno = "Approved";
                objDesdeDb.turno = numString;

                string cus = objDesdeDb.Customer;
                string po = objDesdeDb.Customerpo;
                string comment = objDesdeDb.Description;

                _db.SaveChanges();


                Int16 seqq = 1;
                /*RMA HEADER*/
                var objDesdeDbt = new OERHDFIL_SQL();


                objDesdeDbt.UserDefFld3 = objDesdeDbs.reason;


                var AccountTypeCode = "";
                var cicmpy = new Cicmpy();
                cicmpy = _db2.Cicmpy.Where(a => a.CmpCode.Trim() == objDesdeDb.Customer.Trim()).FirstOrDefault();
                AccountTypeCode = cicmpy.AccountTypeCode;




                var ArtypfilSql = new ArtypfilSql();
                ArtypfilSql = _db2.ArtypfilSql.Where(a => a.CusTypeCd == AccountTypeCode).FirstOrDefault();
                objDesdeDbt.profit_center = ArtypfilSql.SlsSbNo;

                objDesdeDbt.dept = ArtypfilSql.SlsDpNo;



                objDesdeDbt.orig_trx_rt = 1;

                objDesdeDbt.curr_trx_rt = 1;
                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
               


                var oehdrhst_sql = new OEHDRHST_SQL();
                int oehdrhst_sqlcuantos = 0;
                var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();
                try
                {
                    if (objDesdeDbT.Count() > 0)
                    {

                        foreach (var filtro in objDesdeDbT)
                        {
                            if (filtro.Invoice != "0" && filtro.Invoice != "" && filtro.Invoice != null)
                            {

                                oehdrhst_sqlcuantos = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).Count();
                                Console.WriteLine("" + oehdrhst_sqlcuantos, Color.Red);
                                oehdrhst_sql = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).FirstOrDefault();

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex,
         Color.Red);
                }

                if (oehdrhst_sqlcuantos == 0)
                {
                    objDesdeDbt.UserDefFld1 = "FOB SOURCE"; 

                    arcusfil_sql = _db2.arcusfil_sql.Where(a => a.cus_no.Trim()  == objDesdeDb.Customer.Trim()).FirstOrDefault();

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

                    objDesdeDbt.ship_to_country = AraltadrSql.Country;


                    objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                    objDesdeDbt.bill_to_country = AraltadrSql.Country;
                    objDesdeDbt.ship_to_name = AraltadrSql.CusName;
                    objDesdeDbt.UserDefFld3 = objDesdeDbs.reason;

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

                    objDesdeDbt.UserDefFld1 ="FOB SOURCE";
                    objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                    objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                    moneda = oehdrhst_sql.CurrCd;
                    objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                    objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;
                    objDesdeDbt.UserDefFld3 = objDesdeDbs.reason;





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

                objDesdeDbt.UserDefFld3 = objDesdeDbs.reason;

                objDesdeDbt.contact_email = objDesdeDb.Email;
                objDesdeDbt.user_def_fld_5 = "Normal                        ";
                objDesdeDbt.deter_rate_by = "O";
                objDesdeDbt.form_no = 1;
                objDesdeDbt.rma_no = numString;
                objDesdeDbt.cus_no = cus.Trim().PadLeft(20);
                objDesdeDbt.slspsn_pct_comm = 100;
                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
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
                objDesdeDbt.TaxFg = "N";

                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
                objDesdeDbt.rma_dt_entered = date;
                objDesdeDbt.LastActDt = date;
                objDesdeDbt.ExpRecDate = date;

                //_db2.OERHDFIL_SQL.Add(objDesdeDbt);

                //_db2.SaveChanges();
                try
                {
                    var objDesdeDbTr = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();

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
                            char space = ' ';
                            var numrma = numString.PadLeft(8, space);
                            var remove = numrma.Remove(0, 2).Trim();
                            var n = string.Format("  {0}", remove);
                            objDesdeDby.rma_no = n;
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

                            using (var transaction = _db2.Database.BeginTransaction())
                            {
                                try
                                {
                                    _db2.OERDTFIL_SQL.Add(objDesdeDby);
                                    _db2.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    if (!string.IsNullOrEmpty(ex.Message))
                                    {
                                        transaction.Rollback();
                                    }
                                }

                            }

                            seqq++;


                        }

                    }
                }
                catch (Exception)
                {



                }
                int secreo = _db2.OERHDFIL_SQL.Where(a => a.rma_no == nextrma).Count();


                if (secreo > 0)
                {


                    //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));



                }
                else
                {
                    BackgroundJob.Enqueue(() => falloRMA(nextrma, objDesdeDbs.Id, objDesdeDbs.reason));

                    //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

                }


            }


            else if (tipo_cambio < 20000)
            {
                var oERMACTL_SQL = (from c in _db2.OERMACTL_SQL where c.ID == 1 select c).First();
                //siguiente numero de rma 00800042
                //nextrma = (from c in _db2.OERMACTL_SQL
                //           where c.ID == 1
                //           select c.ctl_next_order_no).First();

                if (oERMACTL_SQL != null)
                {
                    nextrma = oERMACTL_SQL.ctl_next_order_no;
                }

                //siguiente numero de rma 00800042
                //nextrma = (from c in _db2.OERMACTL_SQL
                //           where c.ID == 1
                //           select c.ctl_next_order_no).First();

                int x = Int32.Parse(nextrma);
                char pad = ' ';
                string numString = x.ToString().PadLeft(8, pad);

                x = x + 1;
                var numeroFormato = x.ToString("D8");


                var objDesdeDbz = _db2.OERMACTL_SQL.FirstOrDefault(s => s.ID == 1);

                objDesdeDbz.ctl_next_order_no = numeroFormato;
                _db2.SaveChanges();
                //var objDesdeDb = _db.CSEXSW_Rma.FirstOrDefault(s => s.Id == id);

                objDesdeDb.Status = "Approved";
                retorno = "Approved";
                objDesdeDb.turno = numString;

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



                objDesdeDbt.orig_trx_rt = 1;

                objDesdeDbt.curr_trx_rt = 1;
                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                objDesdeDbt.UserDefFld3 = objDesdeDb.reason;

                var oehdrhst_sql = new OEHDRHST_SQL();
                int oehdrhst_sqlcuantos = 0;
                var objDesdeDbT = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();
                try
                {
                    if (objDesdeDbT.Count() > 0)
                    {

                        foreach (var filtro in objDesdeDbT)
                        {
                            if (filtro.Invoice != "0" && filtro.Invoice != "" && filtro.Invoice != null)
                            {

                                oehdrhst_sqlcuantos = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).Count();
                                Console.WriteLine("" + oehdrhst_sqlcuantos, Color.Red);
                                oehdrhst_sql = _db2.OEHDRHST_SQL.Where(a => a.CusAltAdrCd.Contains(objDesdeDb.Ship_To.ToString()) && a.InvNo == filtro.Invoice).FirstOrDefault();

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex,
         Color.Red);
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

                    objDesdeDbt.ship_to_country = AraltadrSql.Country;
                    objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                    objDesdeDbt.UserDefFld3 = objDesdeDb.reason;

                    objDesdeDbt.bill_to_name = AraltadrSql.CusName;
                    objDesdeDbt.bill_to_country = AraltadrSql.Country;
                    objDesdeDbt.ship_to_name = AraltadrSql.CusName;

                    //var imctlfil_sql = new ImctlfilSql();
                    //imctlfil_sql = _db2.ImctlfilSql.FirstOrDefault();


                    //if (AraltadrSql.Loc != null && AraltadrSql.Loc != "")
                    //{
                    //    objDesdeDbt.mfg_loc = AraltadrSql.Loc;
                    //}
                    //else
                    //{
                    //    objDesdeDbt.mfg_loc = imctlfil_sql.Loc;
                    //}

                }
                else
                {

                    objDesdeDbt.ar_terms_cd = oehdrhst_sql.ArTermsCd;
                    objDesdeDbt.tax_cd = oehdrhst_sql.TaxCd;
                    moneda = oehdrhst_sql.CurrCd;
                    objDesdeDbt.curr_cd = oehdrhst_sql.CurrCd;
                    objDesdeDbt.ship_to_country = oehdrhst_sql.ShipToCountry;




                    objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                    objDesdeDbt.UserDefFld3 = objDesdeDb.reason;
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
                objDesdeDbt.user_def_fld_5 = "Normal                 "; 
                objDesdeDbt.deter_rate_by = "O";
                objDesdeDbt.form_no = 1;
                objDesdeDbt.rma_no = numString;
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

                objDesdeDbt.UserDefFld1 = "FOB SOURCE";
                objDesdeDbt.UserDefFld3 = objDesdeDb.reason;
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
                objDesdeDbt.TaxFg = "N";

                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
                objDesdeDbt.rma_dt_entered = date;
                objDesdeDbt.LastActDt = date;
                objDesdeDbt.ExpRecDate = date;

                //_db2.OERHDFIL_SQL.Add(objDesdeDbt);

                //_db2.SaveChanges();
                try
                {
                    var objDesdeDbTr = _db.csexsw_coustumer.Where(s => s.RmaId == idsa).ToList();

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
                            char space = ' ';
                            var numrma = numString.PadLeft(8, space);
                            var remove = numrma.Remove(0, 2).Trim();
                            var n = string.Format("  {0}", remove);
                            objDesdeDby.rma_no = n;
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

                            using (var transaction = _db2.Database.BeginTransaction())
                            {
                                try
                                {
                                    _db2.OERDTFIL_SQL.Add(objDesdeDby);
                                    _db2.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    if (!string.IsNullOrEmpty(ex.Message))
                                    {
                                        transaction.Rollback();
                                    }
                                }

                            }

                            seqq++;


                        }

                    }
                }
                catch (Exception)
                {



                }
                int secreo = _db2.OERHDFIL_SQL.Where(a => a.rma_no == nextrma).Count();
              

                if (secreo > 0)
                {


                    //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));



                }
                else
                {
                    BackgroundJob.Enqueue(() => falloRMA(nextrma, objDesdeDbs.Id, objDesdeDbs.reason));

                    //BackgroundJob.Enqueue(() => SendMailAsync3(mail1, mail2, mail3, mail4, mail5, mail6, mailp, objDesdeDbs.Rmarequest));

                }


            } 
             
            return retorno;

        }


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
    }
}
