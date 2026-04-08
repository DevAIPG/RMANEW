using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Amphenol.RMA.AccesoDatos.Data
{

    public class DibujoRepository : Repository<csexsw_dibujo>, IDibujoRepository
    {
        private readonly DbContextM10 _db;
        private readonly DbContext100 _db100;
        private readonly IConfiguration _configuration;
        public DibujoRepository(DbContextM10 db, DbContext100 db100, IConfiguration configuration) : base(db)
        {
            _db = db;
            _db100 = db100;
            _configuration = configuration;
        }

        public IEnumerable<SelectListItem> GetListadrawings()
        {
            return _db.csexsw_dibujo.Select(i => new SelectListItem()
            {
                Text = i.Descripcion,
                Value = i.Id.ToString()
            });
        }
        public void Updatecomment(int commentid, string comment, string navegador, string usuario)
        {
            if (navegador == "explore")
            {
                var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == commentid);
                objDesdeDb.Comment = usuario + ": " + comment;
                _db.SaveChanges();
            }
            else
            {
                var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == commentid);
                objDesdeDb.Comment = usuario + ": " + comment;
                _db.SaveChanges();
            }
        }

        public void Updaterechazo(int dib)
        {
            var objDesdeDb = _db.csexsw_revision.FirstOrDefault(s => s.Id == dib);
            objDesdeDb.Statusrechazo = true;
            _db.SaveChanges();

        }
        public void Updatecomment2(int idd, string comment, string usuario)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == idd);
                objDesdeDb.Comment = $"({usuario})-{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")}: {comment}";
                _db.SaveChanges();
            }
        }
        public void Update(csexsw_dibujo dibujo)
        {
            var us = dibujo.Id;
            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == dibujo.Id);
            objDesdeDb.Dibujopdf = dibujo.Dibujopdf;
            objDesdeDb.Nombrepdf = dibujo.Nombrepdf;
            objDesdeDb.Descripcion = !string.IsNullOrEmpty(dibujo.Descripcion) ? dibujo.Descripcion : "";
            objDesdeDb.Comment = dibujo.Comment;
            objDesdeDb.Revision1 = dibujo.Revision1;
            objDesdeDb.Revision = dibujo.Revision;
            objDesdeDb.Part = dibujo.Part;
            objDesdeDb.Revision2 = dibujo.Revision2;
            objDesdeDb.Revision3 = dibujo.Revision3;
            objDesdeDb.Revision4 = dibujo.Revision4;
            objDesdeDb.Revision5 = dibujo.Revision5;
            objDesdeDb.Revision6 = dibujo.Revision6;
            objDesdeDb.Revision7 = dibujo.Revision7;
            objDesdeDb.Revision8 = dibujo.Revision8;
            objDesdeDb.Revision9 = dibujo.Revision9;
            objDesdeDb.revision_line_lead = dibujo.revision_line_lead;
            objDesdeDb.revision_line_sup = dibujo.revision_line_sup;
            objDesdeDb.revision_pe = dibujo.revision_pe;
            objDesdeDb.revision_quality = dibujo.revision_quality;
            objDesdeDb.Revisior1 = dibujo.Revisior1;
            objDesdeDb.Revisior2 = dibujo.Revisior2;
            objDesdeDb.Revisior3 = dibujo.Revisior3;
            objDesdeDb.Revisior4 = dibujo.Revisior4;
            objDesdeDb.Revisior5 = dibujo.Revisior5;
            objDesdeDb.Revisior6 = dibujo.Revisior6;
            objDesdeDb.Revisior7 = dibujo.Revisior7;
            objDesdeDb.Revisior8 = dibujo.Revisior8;
            objDesdeDb.Revisior9 = dibujo.Revisior9;
            objDesdeDb.revisior_line_lead = dibujo.revisior_line_lead;
            objDesdeDb.revisior_line_sup = dibujo.revisior_line_sup;
            objDesdeDb.revisior_pe = dibujo.revisior_pe;
            objDesdeDb.revisior_quality = dibujo.revisior_quality;
            objDesdeDb.Status1 = dibujo.Status1;
            objDesdeDb.Status2 = dibujo.Status2;
            objDesdeDb.Status3 = dibujo.Status3;
            objDesdeDb.Status4 = dibujo.Status4;
            objDesdeDb.Status5 = dibujo.Status5;
            objDesdeDb.Status6 = dibujo.Status6;
            objDesdeDb.Status7 = dibujo.Status7;
            objDesdeDb.Status8 = dibujo.Status8;
            objDesdeDb.Status9 = dibujo.Status9;
            objDesdeDb.status_line_lead = dibujo.status_line_lead;
            objDesdeDb.status_line_sup = dibujo.status_line_sup;
            objDesdeDb.status_pe = dibujo.status_pe;
            objDesdeDb.status_quality = dibujo.status_quality;
            objDesdeDb.Status = dibujo.Status == true ? true : false;
            objDesdeDb.Type = dibujo.Type;
            _db.SaveChanges();

            var revisor1 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior1).FirstOrDefault();

            var revisor2 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior2).FirstOrDefault();

            var revisor3 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior3).FirstOrDefault();
            var revisor4 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior4).FirstOrDefault();
            var revisor5 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior5).FirstOrDefault();
            var revisor6 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior6).FirstOrDefault();
            var revisor7 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior7).FirstOrDefault();
            var revisor8 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior8).FirstOrDefault();
            var revisor9 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior9).FirstOrDefault();

            //var status1 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status1).FirstOrDefault();
            //var status2 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status2).FirstOrDefault();
            //var status3 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status3).FirstOrDefault();
            //var status4 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status4).FirstOrDefault();
            //var status5 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status5).FirstOrDefault();
            //var status9 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status9).FirstOrDefault();
            //var status6 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status6).FirstOrDefault();
            //var status7 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status7).FirstOrDefault();
            //var status8 = (from c in _db.csexsw_dibujo
            //               where c.Id == us
            //               select c.Status8).FirstOrDefault();




            var revision1 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision1).FirstOrDefault();
            var revision2 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision2).FirstOrDefault();
            var revision3 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision3).FirstOrDefault();
            var revision4 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision4).FirstOrDefault();
            var revision5 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision5).FirstOrDefault();
            var revision9 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision9).FirstOrDefault();
            var revision6 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision6).FirstOrDefault();
            var revision7 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision7).FirstOrDefault();
            var revision8 = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Revision8).FirstOrDefault();

            var dibujopdf = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Dibujopdf).FirstOrDefault();

            var descripcion = (from c in _db.csexsw_dibujo
                               where c.Id == us
                               select c.Descripcion).FirstOrDefault();

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us).Count() == 0)
            {
                reviewers2(us);
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Design Engineer").Count() > 0)
            {
                var objDesdeDb1 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Design Engineer").FirstOrDefault();
                objDesdeDb1.Nombre = !string.IsNullOrEmpty(revisor1) && revisor1 != "0" ? EmployeeNameByResId(revisor1) : "Not Reviewer";


                objDesdeDb1.Status = objDesdeDb1.Status;
                objDesdeDb1.Dibujopdf = dibujopdf;
                objDesdeDb1.Drawing = descripcion;
                _db.SaveChanges();

            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Drafting and Checking").Count() > 0)
            {
                var objDesdeDb2 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Drafting and Checking").FirstOrDefault();
                objDesdeDb2.Nombre = !string.IsNullOrEmpty(revisor2) && revisor2 != "0" ? EmployeeNameByResId(revisor2) : "Not Reviewer"; ;


                objDesdeDb2.Status = objDesdeDb2.Status;
                objDesdeDb2.Dibujopdf = dibujopdf;
                objDesdeDb2.Drawing = descripcion;
                _db.SaveChanges();
            }



            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Engineer in Charge").Count() > 0)
            {


                var objDesdeDb3 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Engineer in Charge").FirstOrDefault();
                objDesdeDb3.Nombre = !string.IsNullOrEmpty(revisor3) && revisor3 != "0" ? EmployeeNameByResId(revisor3) : "Not Reviewer"; ;


                objDesdeDb3.Status = objDesdeDb3.Status;
                objDesdeDb3.Dibujopdf = dibujopdf;
                objDesdeDb3.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Finish spec").Count() > 0)
            {
                var objDesdeDb4 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Finish spec").FirstOrDefault();
                objDesdeDb4.Nombre = !string.IsNullOrEmpty(revisor4) && revisor4 != "0" ? EmployeeNameByResId(revisor4) : "Not Reviewer"; ;


                objDesdeDb4.Status = objDesdeDb4.Status;
                objDesdeDb4.Dibujopdf = dibujopdf;
                objDesdeDb4.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Material spec").Count() > 0)
            {
                var objDesdeDb5 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Material spec").FirstOrDefault();
                objDesdeDb5.Nombre = !string.IsNullOrEmpty(revisor5) && revisor5 != "0" ? EmployeeNameByResId(revisor5) : "Not Reviewer"; ;


                objDesdeDb5.Status = objDesdeDb5.Status;
                objDesdeDb5.Dibujopdf = dibujopdf;
                objDesdeDb5.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Assembly spec").Count() > 0)
            {
                var objDesdeDb6 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Assembly spec").FirstOrDefault();
                objDesdeDb6.Nombre = !string.IsNullOrEmpty(revisor6) && revisor6 != "0" ? EmployeeNameByResId(revisor6) : "Not Reviewer"; ;


                objDesdeDb6.Status = objDesdeDb6.Status;
                objDesdeDb6.Dibujopdf = dibujopdf;
                objDesdeDb6.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "High reliability drawing").Count() > 0)
            {
                var objDesdeDb7 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "High reliability drawing").FirstOrDefault();
                objDesdeDb7.Nombre = !string.IsNullOrEmpty(revisor7) && revisor7 != "0" ? EmployeeNameByResId(revisor7) : "Not Reviewer"; ;


                objDesdeDb7.Status = objDesdeDb7.Status;
                objDesdeDb7.Dibujopdf = dibujopdf;
                objDesdeDb7.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Molding compound spec").Count() > 0)
            {
                var objDesdeDb8 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Molding compound spec").FirstOrDefault();
                objDesdeDb8.Nombre = !string.IsNullOrEmpty(revisor8) && revisor8 != "0" ? EmployeeNameByResId(revisor8) : "Not Reviewer"; ;


                objDesdeDb8.Status = objDesdeDb8.Status;
                objDesdeDb8.Dibujopdf = dibujopdf;
                objDesdeDb8.Drawing = descripcion;
                _db.SaveChanges();
            }

            if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Welding or special process").Count() > 0)
            {
                var objDesdeDb9 = _db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Welding or special process").FirstOrDefault();
                objDesdeDb9.Nombre = !string.IsNullOrEmpty(revisor9) && revisor9 != "0" ? EmployeeNameByResId(revisor9) : "Not Reviewer"; ;

                objDesdeDb9.Status = objDesdeDb9.Status;
                objDesdeDb9.Dibujopdf = dibujopdf;
                objDesdeDb9.Drawing = descripcion;
                _db.SaveChanges();
            }




            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Design Engineer").Count() > 0)
            //{

            //    var Statust1 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Design Engineer").FirstOrDefault();
            //    Statust1.Statusrechazo = false;
            //    Statust1.Status = false;
            //    Statust1.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Drafting and Checking").Count() > 0)
            //{
            //    var Statust2 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Drafting and Checking").FirstOrDefault();
            //    Statust2.Statusrechazo = false;
            //    Statust2.Status = false;
            //    Statust2.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Enginner in Charge").Count() > 0)
            //{
            //    var Statust3 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Enginner in Charge").FirstOrDefault();
            //    Statust3.Statusrechazo = false;
            //    Statust3.Status = false;
            //    Statust3.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Finish spec").Count() > 0)
            //{
            //    var Statust4 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Finish spec").FirstOrDefault();
            //    Statust4.Statusrechazo = false;
            //    Statust4.Status = false;
            //    Statust4.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Material spec").Count() > 0)
            //{
            //    var Statust5 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Material spec").FirstOrDefault();
            //    Statust5.Statusrechazo = false;
            //    Statust5.Status = false;
            //    Statust5.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Assembly spec").Count() > 0)
            //{
            //    var Statust6 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Assembly spec").FirstOrDefault();
            //    Statust6.Statusrechazo = false;
            //    Statust6.Status = false;
            //    Statust6.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "High reliability drawing").Count() > 0)
            //{
            //    var Statust7 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "High reliability drawing").FirstOrDefault();
            //    Statust7.Statusrechazo = false;
            //    Statust7.Status = false;
            //    Statust7.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Molding compound spec").Count() > 0)
            //{
            //    var Statust8 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Molding compound spec").FirstOrDefault();
            //    Statust8.Statusrechazo = false;
            //    Statust8.Status = false;
            //    Statust8.Statust = false;
            //    _db.SaveChanges();
            //}

            //if (_db.csexsw_revision.Where(s => s.Dibujoid == us && s.Descripcion == "Welding or special process").Count() > 0)
            //{
            //    var Statust9 = _db.csexsw_revision.Where(s => s.Dibujoid == dibujo.Id && s.Descripcion == "Welding or special process").FirstOrDefault();
            //    Statust9.Statusrechazo = false;
            //    Statust9.Status = false;
            //    Statust9.Statust = false;
            //    _db.SaveChanges();
            //}


            //var revisiones = _db.csexsw_revision.Where(s=>s.Dibujoid==dibujo.Id).ToList();
            //if (revisiones.Count>0)
            //{
            //    foreach (var rev in revisiones)
            //    {
            //        rev.Status = false;
            //        rev.Statust = false;
            //        rev.Statusrechazo = false;
            //        _db.Entry(rev).State = EntityState.Modified;
            //        _db.SaveChanges();
            //    }
            //}

        }

        public string usuario2(string id)
        {


            var LastRecord = (from c in _db.humres
                              where c.fullname == id
                              select c.usr_id).FirstOrDefault();



            return LastRecord;


        }
        public void stampar(string documento, string rele)
        {
            DateTime hoy = DateTime.Now;
            DateTime dt = new DateTime();
            string cErr = "";
            string hora = DateTime.Now.ToString("hh:mm:ss:tt");

            string fecha = hoy.ToString("MMM dd yyyy");

            string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(fecha);

            string aprovado = "RELEASED/ " + resul + " " + hora;

            using (var reader = new iTextSharp.text.pdf.PdfReader(System.IO.Path.Combine(rootEC, @"documents\drawings\" + rele + @"\" + documento)))
            {
                using (var fileStream = new FileStream(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + rele + @"\" + documento), FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(reader.GetPageSizeWithRotation(1));
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fileStream);

                    document.Open();

                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                        var importedPage = writer.GetImportedPage(reader, i);

                        var contentByte = writer.DirectContent;
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(baseFont, 12);

                        contentByte.SetColorFill(BaseColor.BLUE);


                        var multiLineString = aprovado.Split('\n');

                        foreach (var line in multiLineString)
                        {
                            contentByte.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, line, 1, 1, 0);
                        }

                        contentByte.EndText();
                        contentByte.AddTemplate(importedPage, 0, 0);
                    }

                    document.Close();
                    writer.Close();
                }
            }




        }

        public string Release(int id)
        {


            var LastRecord = (from c in _db.csexsw_tarea
                              where c.Id == id
                              select c.Version).FirstOrDefault();



            return LastRecord;


        }

        public string usuario(string id)
        {
            var LastRecord = (from c in _db.humres
                              select c.res_id).FirstOrDefault().ToString();

            //var LastRecord = (from c in _db.humres
            //                  where c.usr_id == id
            //                  select c.res_id).FirstOrDefault().ToString();



            return LastRecord;
        }



        public void Bomrelease(string release)

        {
            var objDesdeDb = _db.csexsw_bomexcel.Where(s => s.Excelid == release).FirstOrDefault();

            ArrayList objs = new ArrayList();
            var item_no = "";
            var seq_no = "";
            var comp_item_no = "";
            var alt_item_no = "";
            var qty_per_par = "";
            var attaching_oper_no = "0";
            var scrap_factor = "0";
            var activity_fg = "";
            var user_def_fld = "";
            var effectivity_dt = "";
            var obsolete_dt = "";
            var mfg_uom = "";
            var filler_0001 = "";
            var loc = "";
            var backflush_fg = "N";
            var bulk_issue_fg = "N";
            var scrap_qty = "0";
            var extra_1 = "";
            var extra_2 = "";
            var extra_3 = "";
            var extra_4 = "";
            var extra_5 = "";
            var extra_6 = "";
            var extra_7 = "";
            var extra_8 = "";
            var extra_9 = "";
            var extra_10 = "0";
            var extra_11 = "0";
            var extra_12 = "0";
            var extra_13 = "0";
            var extra_14 = "0";
            var extra_15 = "0";
            var filler_0002 = "";
            string connectionString = _configuration.GetConnectionString("Connection100").ToString();
            var values = new List<Dictionary<string, object>>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"select * from bmprdstr_sql where item_no = '" + objDesdeDb.Item_no + "'";
                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rdr = cmd.ExecuteReader();


                //get the data reader, etc.
                while (rdr.Read())
                {
                    item_no = Convert.ToString(rdr["item_no"]);
                    seq_no = Convert.ToString(rdr["seq_no"]);
                    comp_item_no = Convert.ToString(rdr["comp_item_no"]);
                    alt_item_no = Convert.ToString(rdr["alt_item_no"]);
                    qty_per_par = Convert.ToString(rdr["qty_per_par"]);
                    attaching_oper_no = Convert.ToString(rdr["attaching_oper_no"]);
                    loc = Convert.ToString(rdr["loc"]);
                    mfg_uom = Convert.ToString(rdr["mfg_uom"]);
                    scrap_factor = Convert.ToString(rdr["scrap_factor"]);
                    activity_fg = Convert.ToString(rdr["activity_fg"]);
                    user_def_fld = Convert.ToString(rdr["user_def_fld"]);
                    effectivity_dt = Convert.ToString(rdr["effectivity_dt"]);
                    obsolete_dt = Convert.ToString(rdr["obsolete_dt"]);


                    filler_0001 = Convert.ToString(rdr["filler_0001"]);


                    backflush_fg = Convert.ToString(rdr["backflush_fg"]);
                    bulk_issue_fg = Convert.ToString(rdr["bulk_issue_fg"]);
                    scrap_qty = Convert.ToString(rdr["scrap_qty"]);
                    extra_1 = Convert.ToString(rdr["extra_1"]);
                    extra_2 = Convert.ToString(rdr["extra_2"]);
                    extra_3 = Convert.ToString(rdr["extra_3"]);
                    extra_4 = Convert.ToString(rdr["extra_4"]);
                    extra_5 = Convert.ToString(rdr["extra_5"]);
                    extra_6 = Convert.ToString(rdr["extra_6"]);
                    extra_7 = Convert.ToString(rdr["extra_7"]);
                    extra_8 = Convert.ToString(rdr["extra_8"]);
                    extra_9 = Convert.ToString(rdr["extra_9"]);
                    extra_10 = Convert.ToString(rdr["extra_10"]);
                    extra_11 = Convert.ToString(rdr["extra_11"]);
                    extra_12 = Convert.ToString(rdr["extra_12"]);
                    extra_13 = Convert.ToString(rdr["extra_13"]);
                    extra_14 = Convert.ToString(rdr["extra_14"]);
                    extra_15 = Convert.ToString(rdr["extra_15"]);
                    filler_0002 = Convert.ToString(rdr["filler_0002"]);
                    objs.Add(new
                    {

                        item_no = rdr["item_no"],
                        seq_no = rdr["seq_no"],
                        comp_item_no = rdr["comp_item_no"],
                        alt_item_no = rdr["alt_item_no"],
                        qty_per_par = rdr["qty_per_par"],
                        attaching_oper_no = rdr["attaching_oper_no"],
                        loc = rdr["loc"],
                        mfg_uom = rdr["mfg_uom"],
                        scrap_factor = rdr["scrap_factor"],
                        activity_fg = rdr["activity_fg"],
                        user_def_fld = rdr["user_def_fld"],
                        effectivity_dt = rdr["effectivity_dt"],
                        obsolete_dt = rdr["obsolete_dt"],


                        filler_0001 = rdr["filler_0001"],


                        backflush_fg = rdr["backflush_fg"],
                        bulk_issue_fg = rdr["bulk_issue_fg"],
                        scrap_qty = rdr["scrap_qty"],
                        extra_1 = rdr["extra_1"],
                        extra_2 = rdr["extra_2"],
                        extra_3 = rdr["extra_3"],
                        extra_4 = rdr["extra_4"],
                        extra_5 = rdr["extra_5"],
                        extra_6 = rdr["extra_6"],
                        extra_7 = rdr["extra_7"],
                        extra_8 = rdr["extra_8"],
                        extra_9 = rdr["extra_9"],
                        extra_10 = rdr["extra_10"],
                        extra_11 = rdr["extra_11"],
                        extra_12 = rdr["extra_12"],
                        extra_13 = rdr["extra_13"],
                        extra_14 = rdr["extra_14"],
                        extra_15 = rdr["extra_15"],
                        filler_0002 = rdr["filler_0002"]





                    });
                }



                cn.Close();
            }


            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"Delete  from bmprdstr_sql where item_no = '" + objDesdeDb.Item_no + "'";
                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rdr = cmd.ExecuteReader();


                //get the data reader, etc.
                while (rdr.Read())
                {
                    objs.Add(new
                    {

                        item_no = rdr["item_no"],
                        seq_no = rdr["seq_no"],
                        comp_item_no = rdr["comp_item_no"],
                        alt_item_no = rdr["alt_item_no"],
                        qty_per_par = rdr["qty_per_par"],
                        attaching_oper_no = rdr["attaching_oper_no"],
                        loc = rdr["loc"],
                        mfg_uom = rdr["mfg_uom"]
                    });
                }



                cn.Close();
            }

            var objDesdeDbL = _db.csexsw_bomexcel.Where(s => s.Excelid == release).ToList();

            if (objDesdeDbL.Count() > 0)
            {

                foreach (var filtro in objDesdeDbL)
                {

                    string insertQuery = @"INSERT INTO [dbo].[bmprdstr_sql]
                  ([item_no]
                  ,[seq_no]
                  ,[comp_item_no]
                  ,[alt_item_no]
                  ,[qty_per_par]
                  ,[attaching_oper_no]
                  ,[scrap_factor]
                  ,[activity_fg]
                  ,[user_def_fld]
                  ,[effectivity_dt]
                  ,[obsolete_dt]
                  ,[mfg_uom]
                  ,[filler_0001]
                  ,[loc]
                  ,[backflush_fg]
                  ,[bulk_issue_fg]
                  ,[scrap_qty]
                  ,[extra_1]
                  ,[extra_2]
                  ,[extra_3]
                  ,[extra_4]
                  ,[extra_5]
                  ,[extra_6]
                  ,[extra_7]
                  ,[extra_8]
                  ,[extra_9]
                  ,[extra_10]
                  ,[extra_11]
                  ,[extra_12]
                  ,[extra_13]
                  ,[extra_14]
                  ,[extra_15]
                  ,[filler_0002])
            VALUES
                  (@item_no
                  ,@seq_no
                  ,@comp_item_no
                  ,@alt_item_no
                  ,@qty_per_par
                  ,@attaching_oper_no
                  ,@scrap_factor
                  ,@activity_fg
                  ,@user_def_fld
                  ,@effectivity_dt
                  ,@obsolete_dt
                  ,@mfg_uom
                  ,@filler_0001
                  ,@loc
                  ,@backflush_fg
                  ,@bulk_issue_fg
                  ,@scrap_qty
                  ,@extra_1
                  ,@extra_2
                  ,@extra_3
                  ,@extra_4
                  ,@extra_5
                  ,@extra_6
                  ,@extra_7
                  ,@extra_8
                  ,@extra_9
                  ,@extra_10
                  ,@extra_11
                  ,@extra_12
                  ,@extra_13
                  ,@extra_14
                  ,@extra_15
                  ,@filler_0002)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // define your parameters ONCE outside the loop, and use EXPLICIT typing
                        command.Parameters.Add("@item_no", SqlDbType.Char);
                        command.Parameters.Add("@seq_no", SqlDbType.SmallInt);
                        command.Parameters.Add("@comp_item_no", SqlDbType.Char);
                        command.Parameters.Add("@alt_item_no", SqlDbType.Char);
                        command.Parameters.Add("@qty_per_par", SqlDbType.Decimal);
                        command.Parameters.Add("@attaching_oper_no", SqlDbType.SmallInt);
                        command.Parameters.Add("@scrap_factor", SqlDbType.Decimal);
                        command.Parameters.Add("@activity_fg", SqlDbType.Char);
                        command.Parameters.Add("@user_def_fld", SqlDbType.Char);
                        command.Parameters.Add("@effectivity_dt", SqlDbType.DateTime);
                        command.Parameters.Add("@obsolete_dt", SqlDbType.DateTime);
                        command.Parameters.Add("@mfg_uom", SqlDbType.Char);
                        command.Parameters.Add("@filler_0001", SqlDbType.Char);
                        command.Parameters.Add("@loc", SqlDbType.Char);
                        command.Parameters.Add("@backflush_fg", SqlDbType.Char);
                        command.Parameters.Add("@bulk_issue_fg", SqlDbType.Char);
                        command.Parameters.Add("@scrap_qty", SqlDbType.Decimal);
                        command.Parameters.Add("@extra_1", SqlDbType.Char);
                        command.Parameters.Add("@extra_2", SqlDbType.Char);
                        command.Parameters.Add("@extra_3", SqlDbType.Char);
                        command.Parameters.Add("@extra_4", SqlDbType.Char);
                        command.Parameters.Add("@extra_5", SqlDbType.Char);
                        command.Parameters.Add("@extra_6", SqlDbType.Char);
                        command.Parameters.Add("@extra_7", SqlDbType.Char);
                        command.Parameters.Add("@extra_8", SqlDbType.Char);
                        command.Parameters.Add("@extra_9", SqlDbType.Char);
                        command.Parameters.Add("@extra_10", SqlDbType.Decimal);
                        command.Parameters.Add("@extra_11", SqlDbType.Decimal);
                        command.Parameters.Add("@extra_12", SqlDbType.Decimal);
                        command.Parameters.Add("@extra_13", SqlDbType.Decimal);
                        command.Parameters.Add("@extra_14", SqlDbType.Int);
                        command.Parameters.Add("@extra_15", SqlDbType.Int);
                        command.Parameters.Add("@filler_0002", SqlDbType.Char);
                        connection.Open();



                        command.Parameters["@item_no"].Value = filtro.Item_no;
                        command.Parameters["@seq_no"].Value = Convert.ToInt16(filtro.Sequence);
                        command.Parameters["@comp_item_no"].Value = filtro.Componente;
                        command.Parameters["@alt_item_no"].Value = alt_item_no;
                        command.Parameters["@qty_per_par"].Value = Convert.ToDecimal(filtro.Quantify);
                        command.Parameters["@attaching_oper_no"].Value = filtro.Attach;
                        command.Parameters["@scrap_factor"].Value = Convert.ToDecimal(scrap_factor);
                        command.Parameters["@activity_fg"].Value = filtro.Activity;
                        command.Parameters["@user_def_fld"].Value = user_def_fld;
                        DateTime fecha = DateTime.Now;
                        if (effectivity_dt == null)
                        {
                            command.Parameters["@effectivity_dt"].Value = fecha;
                        }
                        else
                        {
                            command.Parameters["@effectivity_dt"].Value = fecha;
                        }
                        if (obsolete_dt == null)
                        {
                            command.Parameters["@obsolete_dt"].Value = fecha;
                        }
                        else
                        {
                            command.Parameters["@obsolete_dt"].Value = fecha.AddYears(25);
                        }
                        command.Parameters["@mfg_uom"].Value = filtro.Manuf;
                        command.Parameters["@filler_0001"].Value = filler_0001;
                        command.Parameters["@loc"].Value = filtro.Componente_loc;
                        command.Parameters["@backflush_fg"].Value = filtro.Bulkk_flush;
                        command.Parameters["@bulk_issue_fg"].Value = filtro.Bulkk_issue;
                        command.Parameters["@scrap_qty"].Value = Convert.ToDecimal(scrap_qty);
                        command.Parameters["@extra_1"].Value = extra_1;
                        command.Parameters["@extra_2"].Value = extra_2;
                        command.Parameters["@extra_3"].Value = extra_3;
                        command.Parameters["@extra_4"].Value = extra_4;
                        command.Parameters["@extra_5"].Value = extra_5;
                        command.Parameters["@extra_6"].Value = extra_6;
                        command.Parameters["@extra_7"].Value = extra_7;
                        command.Parameters["@extra_8"].Value = extra_8;
                        command.Parameters["@extra_9"].Value = extra_9;
                        command.Parameters["@extra_10"].Value = Convert.ToDecimal(extra_10);
                        command.Parameters["@extra_11"].Value = Convert.ToDecimal(extra_11);
                        command.Parameters["@extra_12"].Value = Convert.ToDecimal(extra_12);
                        command.Parameters["@extra_13"].Value = Convert.ToDecimal(extra_13);
                        command.Parameters["@extra_14"].Value = Convert.ToInt16(extra_14);
                        command.Parameters["@extra_15"].Value = Convert.ToInt16(extra_15);
                        command.Parameters["@filler_0002"].Value = filler_0002;

                        command.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine(filtro.Componente);
                    }



                }



            }

        }
        public void StamparRelease(List<csexsw_dibujo> dibujos, string version, string hid_compuesto)
        {
            foreach (var filtro in dibujos)
            {
                DateTime hoy = DateTime.Now;
                DateTime dt = new DateTime();
                string cErr = "";
                string hora = DateTime.Now.ToString("hh:mm:ss:tt");

                string fecha = hoy.ToString("MMM dd yyyy");

                string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(fecha);

                string aprovado = "";
                if (!string.IsNullOrEmpty(hid_compuesto))
                {
                    aprovado = hid_compuesto;
                }
                else
                {
                    aprovado = "RELEASED/ " + resul + " " + hora;
                }

                using (var reader = new iTextSharp.text.pdf.PdfReader(System.IO.Path.Combine(rootEC, @"documents\drawings\" + version + @"\" + filtro.Dibujopdf)))
                {
                    using (var fileStream = new FileStream(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf), FileMode.Create, FileAccess.Write))
                    {
                        var document = new Document(reader.GetPageSizeWithRotation(1));
                        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fileStream);

                        document.Open();

                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            document.NewPage();

                            var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                            var importedPage = writer.GetImportedPage(reader, i);

                            var contentByte = writer.DirectContent;
                            contentByte.BeginText();
                            contentByte.SetFontAndSize(baseFont, 12);

                            contentByte.SetColorFill(BaseColor.BLUE);


                            var multiLineString = aprovado.Split('\n');

                            foreach (var line in multiLineString)
                            {
                                contentByte.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, line, 1, 1, 0);
                            }

                            contentByte.EndText();
                            contentByte.AddTemplate(importedPage, 0, 0);
                        }

                        document.Close();
                        writer.Close();
                    }
                    reader.Close();

                    PdfReader reader2 = new PdfReader(Path.Combine(rootEC, @"documents\drawings\" + version + @"\" + filtro.Dibujopdf));
                    int n = reader.NumberOfPages;
                    PdfDictionary page;
                    PdfNumber rotate;
                    var fs = new FileStream(Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf), FileMode.Open, FileAccess.ReadWrite);
                    PdfStamper stamper = new PdfStamper(reader2, fs);


                    for (int i = 1; i <= n; i++)
                    {
                        page = reader.GetPageN(i);
                        rotate = page.GetAsNumber(PdfName.ROTATE);
                        if (rotate == null)
                        {
                            page.Put(PdfName.ROTATE, new PdfNumber(0));
                        }
                        else
                        {
                            page.Put(PdfName.ROTATE, new PdfNumber((rotate.IntValue + 0) % 360));
                        }
                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfContentByte canvas = stamper.GetOverContent(i);
                        canvas.SetFontAndSize(baseFont, 12);
                        canvas.SetColorFill(BaseColor.BLUE);
                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, new Phrase(aprovado), 5, 5, 0);


                    }
                    stamper.Close();
                    reader.Close();

                }


            }
        }
        public void stamparfinal(int id)

        {

            try
            {
                //string ruta = @"\DCC-Macola\Drawings\Internal_Drawings\";

                var version = (from c in _db.csexsw_tarea
                               where c.Id == id
                               select c.Version).First();
                var objDesdeDb = _db.csexsw_dibujo.Where(s => s.TareaId == id).ToList();
                string connectionString = _configuration.GetConnectionString("Connection100");

                if (objDesdeDb.Count() > 0)
                {

                    StamparRelease(objDesdeDb, version, "");

                    foreach (var filtro in objDesdeDb)
                    {
                        try
                        {
                            int count = _db.items.Where(s => s.itemcode == filtro.Nombrepdf).Count();
                            if (count != 0 & filtro.Type == 2)
                            {
                                //string updateQuery = @"Update imitmidx_sql set item_note_4 = @item_note_4 ,last_item_revision = @last_item_revision where item_no = @item_no  ";

                                //using (SqlConnection connection = new SqlConnection(connectionString))
                                //using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                //{
                                //    command.Parameters.Add("@item_note_4", SqlDbType.VarChar);
                                //    command.Parameters.Add("@last_item_revision", SqlDbType.VarChar);
                                //    command.Parameters.Add("@item_no", SqlDbType.VarChar); 
                                //    connection.Open(); 
                                //    command.Parameters["@item_note_4"].Value = filtro.Nombrepdf;
                                //    command.Parameters["@last_item_revision"].Value = filtro.Revision;
                                //    command.Parameters["@item_no"].Value = filtro.Nombrepdf;
                                //    command.ExecuteNonQuery();
                                //    connection.Close();
                                //    Console.WriteLine("\t" + filtro.Nombrepdf);

                                //} 
                                //if (_db.csexsw_documents.Where(s => s.Document == filtro.Dibujopdf).Count() == 0)
                                //{
                                //} 
                                var varFilePath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf);

                                byte[] file;
                                using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);
                                    }
                                }

                                //var objDesdeDbD = new csexsw_documents();
                                //objDesdeDbD.Document = filtro.Dibujopdf;
                                //objDesdeDbD.Category = "";
                                //objDesdeDbD.Revision = filtro.Revision;
                                //objDesdeDbD.Part = filtro.Part;
                                //objDesdeDbD.Name = filtro.Nombrepdf;
                                //objDesdeDbD.Archivo = file;
                                //_db.csexsw_documents.Add(objDesdeDbD);

                                //_db.SaveChanges();


                            }
                            else
                            {
                                //if (_db.csexsw_documents.Where(s => s.Category + s.Document == ruta + filtro.Dibujopdf).Count() == 0)
                                //{ }

                                var varFilePath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf);


                                byte[] file;
                                using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);
                                    }
                                }
                                //var objDesdeDbD = new csexsw_documents();
                                //objDesdeDbD.Document = filtro.Dibujopdf;
                                //objDesdeDbD.Category = "";
                                //objDesdeDbD.Revision = filtro.Revision;
                                //objDesdeDbD.Part = filtro.Part;
                                //objDesdeDbD.Name = filtro.Nombrepdf;

                                //_db.csexsw_documents.Add(objDesdeDbD);
                                //objDesdeDbD.Archivo = file;
                                //_db.SaveChanges();

                            }

                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            //int count = _db.items.Where(s => s.itemcode == filtro.Part).Count();
                            //if (count != 0 & filtro.Type == 2)
                            //{ 
                            //var Update = _db.items.Where(s => s.itemcode == filtro.Part).First();
                            //Update.textdescription = "<a href='http://m10ATZ:8080/documents/approved_stamped/" + version + "/" + filtro.Dibujopdf + "'>" + filtro.Nombrepdf + "</a>";
                            //_db.SaveChanges();


                            //string updateQuery = @"Update items set textdescription = @textdescription where itemcode = @itemcode  ";

                            //using (SqlConnection connection = new SqlConnection(connectionString))
                            //using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            //{
                            //    command.Parameters.Add("@textdescription", SqlDbType.VarChar);
                            //    command.Parameters.Add("@itemcode", SqlDbType.VarChar);
                            //    connection.Open();



                            //    command.Parameters["@textdescription"].Value = "<a href='http://" + _configuration.GetConnectionString("server").ToString() + ":8080/documents/AIO-Release/DCC-Macola/Drawings/Internal_Drawings/" + filtro.Dibujopdf + "'>" + filtro.Nombrepdf + "</a>";
                            //    command.Parameters["@itemcode"].Value = filtro.Part;

                            //    command.ExecuteNonQuery();
                            //    connection.Close();
                            //    Console.WriteLine("\t" + filtro.Part);

                            //}
                            //}
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                //string startPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version);
                //if (System.IO.File.Exists(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip")))
                //{
                //    System.IO.File.Delete(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip"));
                //}
                //string zipPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip");
                //if (!File.Exists(zipPath))
                //{
                //    ZipFile.CreateFromDirectory(startPath, zipPath);
                //}

                var release = _db.csexsw_tarea.Where(S => S.Id == id).FirstOrDefault();
                //dibujos por release
                var dibujosportarea = _db.csexsw_dibujo.Where(s => s.TareaId == id).ToList();
                if (dibujosportarea.Count > 0)
                {
                    foreach (var drawing in dibujosportarea)
                    {
                        //get doc in M10 BacoDiscussions by Document Number
                        var bacoid = _db.BacoDiscussions.Where(S => S.Subject == drawing.Nombrepdf).Select(r => r.ID).FirstOrDefault();
                        if (bacoid != Guid.Empty)
                        {
                            //file by drawing
                            var attachment = _db.Attachments.Where(f => f.Entity == bacoid).FirstOrDefault();
                            if (attachment.ID != Guid.Empty)
                            {
                                //get file approved stamped
                                var pathfileApproved = @$"E:\CSFiles\documents\approved_stamped\{release.Version}\{drawing.Dibujopdf}";
                                var exists = System.IO.File.Exists(pathfileApproved);
                                if (exists)
                                {
                                    FileStream fs = new FileStream(pathfileApproved, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] pdf = br.ReadBytes((int)fs.Length);
                                    br.Close();
                                    fs.Close();
                                    //update attachment for approved document and stamped
                                    attachment.Attachment = pdf;
                                    _db.Entry(attachment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    var result = _db.SaveChanges();

                                    if (result > 0)
                                    {
                                        //update drawing by approved true
                                        drawing.approved = true;
                                        _db.Entry(drawing).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                        _db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void stamparfinalOneDrawing(int id, string stamphid)

        {

            try
            {
                //string ruta = @"\DCC-Macola\Drawings\Internal_Drawings\";
                var objDesdeDb = _db.csexsw_dibujo.Where(s => s.Id == id).FirstOrDefault();


                var version = (from c in _db.csexsw_tarea
                               where c.Id == objDesdeDb.TareaId
                               select c.Version).First();
                string connectionString = _configuration.GetConnectionString("Connection100");

                if (objDesdeDb.Id > 0)
                {

                    StamparRelease(new List<csexsw_dibujo>() { objDesdeDb }, version, stamphid);

                    var filtro = objDesdeDb;

                    try
                    {
                        int count = _db.items.Where(s => s.itemcode == filtro.Nombrepdf).Count();
                        if (count != 0 & filtro.Type == 2)
                        {
                            //string updateQuery = @"Update imitmidx_sql set item_note_4 = @item_note_4 ,last_item_revision = @last_item_revision where item_no = @item_no  ";

                            //using (SqlConnection connection = new SqlConnection(connectionString))
                            //using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            //{
                            //    command.Parameters.Add("@item_note_4", SqlDbType.VarChar);
                            //    command.Parameters.Add("@last_item_revision", SqlDbType.VarChar);
                            //    command.Parameters.Add("@item_no", SqlDbType.VarChar); 
                            //    connection.Open(); 
                            //    command.Parameters["@item_note_4"].Value = filtro.Nombrepdf;
                            //    command.Parameters["@last_item_revision"].Value = filtro.Revision;
                            //    command.Parameters["@item_no"].Value = filtro.Nombrepdf;
                            //    command.ExecuteNonQuery();
                            //    connection.Close();
                            //    Console.WriteLine("\t" + filtro.Nombrepdf);

                            //} 
                            //if (_db.csexsw_documents.Where(s => s.Document == filtro.Dibujopdf).Count() == 0)
                            //{
                            //} 
                            var varFilePath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf);

                            byte[] file;
                            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    file = reader.ReadBytes((int)stream.Length);
                                }
                            }

                            //var objDesdeDbD = new csexsw_documents();
                            //objDesdeDbD.Document = filtro.Dibujopdf;
                            //objDesdeDbD.Category = "";
                            //objDesdeDbD.Revision = filtro.Revision;
                            //objDesdeDbD.Part = filtro.Part;
                            //objDesdeDbD.Name = filtro.Nombrepdf;
                            //objDesdeDbD.Archivo = file;
                            //_db.csexsw_documents.Add(objDesdeDbD);

                            //_db.SaveChanges();


                        }
                        else
                        {
                            //if (_db.csexsw_documents.Where(s => s.Category + s.Document == ruta + filtro.Dibujopdf).Count() == 0)
                            //{ }

                            var varFilePath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + @"\" + filtro.Dibujopdf);


                            byte[] file;
                            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    file = reader.ReadBytes((int)stream.Length);
                                }
                            }
                            //var objDesdeDbD = new csexsw_documents();
                            //objDesdeDbD.Document = filtro.Dibujopdf;
                            //objDesdeDbD.Category = "";
                            //objDesdeDbD.Revision = filtro.Revision;
                            //objDesdeDbD.Part = filtro.Part;
                            //objDesdeDbD.Name = filtro.Nombrepdf;

                            //_db.csexsw_documents.Add(objDesdeDbD);
                            //objDesdeDbD.Archivo = file;
                            //_db.SaveChanges();

                        }

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        //int count = _db.items.Where(s => s.itemcode == filtro.Part).Count();
                        //if (count != 0 & filtro.Type == 2)
                        //{ 
                        //var Update = _db.items.Where(s => s.itemcode == filtro.Part).First();
                        //Update.textdescription = "<a href='http://m10ATZ:8080/documents/approved_stamped/" + version + "/" + filtro.Dibujopdf + "'>" + filtro.Nombrepdf + "</a>";
                        //_db.SaveChanges();


                        //string updateQuery = @"Update items set textdescription = @textdescription where itemcode = @itemcode  ";

                        //using (SqlConnection connection = new SqlConnection(connectionString))
                        //using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        //{
                        //    command.Parameters.Add("@textdescription", SqlDbType.VarChar);
                        //    command.Parameters.Add("@itemcode", SqlDbType.VarChar);
                        //    connection.Open();



                        //    command.Parameters["@textdescription"].Value = "<a href='http://" + _configuration.GetConnectionString("server").ToString() + ":8080/documents/AIO-Release/DCC-Macola/Drawings/Internal_Drawings/" + filtro.Dibujopdf + "'>" + filtro.Nombrepdf + "</a>";
                        //    command.Parameters["@itemcode"].Value = filtro.Part;

                        //    command.ExecuteNonQuery();
                        //    connection.Close();
                        //    Console.WriteLine("\t" + filtro.Part);

                        //}
                        //}
                    }
                    catch (Exception)
                    {

                    }
                }
                //string startPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version);
                //if (System.IO.File.Exists(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip")))
                //{
                //    System.IO.File.Delete(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip"));
                //}
                //string zipPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + version + ".zip");
                //if (!File.Exists(zipPath))
                //{
                //    ZipFile.CreateFromDirectory(startPath, zipPath);
                //}

                var release = _db.csexsw_tarea.Where(S => S.Id == objDesdeDb.TareaId).FirstOrDefault();
                //dibujos por release
                var dibujosportarea = _db.csexsw_dibujo.Where(s => s.Id == id).ToList();
                if (dibujosportarea.Count > 0)
                {
                    foreach (var drawing in dibujosportarea)
                    {
                        //get doc in M10 BacoDiscussions by Document Number
                        var bacoid = _db.BacoDiscussions.Where(S => S.Subject == drawing.Nombrepdf.Trim()).Select(r => r.ID).FirstOrDefault();
                        if (bacoid != Guid.Empty)
                        {
                            //file by drawing
                            var attachment = _db.Attachments.Where(f => f.Entity == bacoid).FirstOrDefault();
                            if (attachment != null && attachment.ID != Guid.Empty)
                            {
                                //get file approved stamped
                                var pathfileApproved = @$"E:\CSFiles\documents\approved_stamped\{release.Version}\{drawing.Dibujopdf}";
                                var exists = System.IO.File.Exists(pathfileApproved);
                                if (exists)
                                {
                                    FileStream fs = new FileStream(pathfileApproved, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] pdf = br.ReadBytes((int)fs.Length);
                                    br.Close();
                                    fs.Close();
                                    //update attachment for approved document and stamped
                                    attachment.Attachment = pdf;
                                    _db.Entry(attachment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    var result = _db.SaveChanges();

                                    if (result > 0)
                                    {
                                        //update drawing by approved true
                                        drawing.approved = true;
                                        _db.Entry(drawing).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                        _db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void statustarea(int id, int idrev, bool apruebo, bool isUpdate)
        {
            var drawing = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);


            var drawings = (from dd in _db.csexsw_dibujo
                            where dd.TareaId == drawing.TareaId
                            select dd).ToList();
            int debeaprobar = 0;
            int aprobadas = 0;
            int rechazadas = 0;
            if (drawings.Count > 0)
            {
                foreach (var d in drawings)
                {
                    debeaprobar += _db.csexsw_revision.Where(s => s.Nombre != "Not Reviewer" && s.Dibujoid == d.Id).Count();
                    aprobadas += _db.csexsw_revision.Where(s => s.Status == true && s.Dibujoid == d.Id).Count();
                    rechazadas += _db.csexsw_revision.Where(s => s.Statusrechazo == true && s.Dibujoid == d.Id).Count();
                }
            }

            var release = _db.csexsw_tarea.Where(S => S.Id == drawing.TareaId).FirstOrDefault();

            if (aprobadas == debeaprobar)
            {


                foreach (var dw in drawings)
                {
                    if (apruebo)
                    {
                        release.Status = true;
                        dw.Status = true;
                    }
                    else
                    {
                        release.Status = false;
                        release.Statusaprobado = false;
                        release.Statusrechazado = true;
                        dw.Statusrechazado = false;

                    }
                    _db.Entry(dw).State = EntityState.Modified;
                    _db.SaveChanges();
                }

            }
            else
            {
                release.Status = false;
                if (apruebo == true)
                {

                    if (rechazadas > 0)
                    {
                        drawing.Status = false;
                        drawing.Statusrechazado = true;
                        _db.Entry(drawing).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                    else
                    {
                        drawing.Statusrechazado = false;
                        var totaltareas = _db.csexsw_revision.Where(S => S.Dibujoid == drawing.Id && S.Nombre != "Not Reviewer").Count();
                        var totaltareasaprobadas = _db.csexsw_revision.Where(S => S.Dibujoid == drawing.Id && S.Nombre != "Not Reviewer" && S.Status == true).Count();
                        if (totaltareasaprobadas == totaltareas)
                        {
                            drawing.Status = true;
                            _db.Entry(drawing).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }


                }
            }
            if (rechazadas == debeaprobar)
            {
                release.Status = false;
                release.Statusaprobado = false;
                release.Statusrechazado = false;
                foreach (var dw in drawings)
                {
                    dw.Statusrechazado = true;
                    dw.Status = false;

                    _db.Entry(dw).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            else
            {
                if (apruebo == false)
                {
                    drawing.Statusrechazado = true;
                    drawing.Status = false;
                    drawing.Status = false;
                    _db.Entry(drawing).State = EntityState.Modified;
                    _db.SaveChanges();
                }

            }
            _db.Entry(release).State = EntityState.Modified;
            _db.SaveChanges();

            //if (!isUpdate)
            //{
            //    if (objDesdeDb.countAprove == 1)
            //    {

            //        var release = _db.csexsw_tarea.Where(s => s.Id == objDesdeDb.TareaId).FirstOrDefault();
            //        release.Statusaprobado = false;
            //        release.Statusrechazado = false;
            //        release.Status = true;
            //        _db.Entry(release).State = EntityState.Modified;
            //        _db.SaveChanges();

            //        objDesdeDb.Statusrechazado = false;
            //        objDesdeDb.Status = true;
            //        _db.Entry(objDesdeDb).State = EntityState.Modified;
            //        _db.SaveChanges();

            //    }
            //    else
            //    {

            //        if (apruebo == true)
            //        {
            //            var release = _db.csexsw_tarea.Where(s => s.Id == objDesdeDb.TareaId).FirstOrDefault();
            //            release.Statusaprobado = false;
            //            release.Statusrechazado = false;
            //            release.Status = false;
            //            _db.Entry(release).State = EntityState.Modified;
            //            _db.SaveChanges();
            //            //aprobar dibujo 
            //            objDesdeDb.Statusrechazado = false;
            //            objDesdeDb.Status = true;
            //            _db.Entry(objDesdeDb).State = EntityState.Modified;
            //            _db.SaveChanges();
            //        }
            //        else
            //        {
            //            bool rech = false;
            //            if (_db.csexsw_revision.Where(s => s.Dibujoid == id && s.Id == idrev).Count() > 0)
            //            {
            //                rech = (from c in _db.csexsw_revision
            //                        where c.Dibujoid == id
            //                        where c.Id == idrev
            //                        select c.Statusrechazo).First();
            //            }
            //            if (rech == true)
            //            {
            //                var Statustrechazo = _db.csexsw_dibujo.Where(s => s.Id == id).First();
            //                Statustrechazo.Statusrechazado = true;
            //                _db.SaveChanges();
            //            }
            //            if (objDesdeDb.countReject == 1)
            //            {
            //                //rechazar ultimo
            //                var release = _db.csexsw_tarea.Where(s => s.Id == objDesdeDb.TareaId).FirstOrDefault();
            //                release.Statusaprobado = false;
            //                release.Statusrechazado = false;
            //                release.Status = false;
            //                _db.Entry(release).State = EntityState.Modified;
            //                _db.SaveChanges();

            //                //dibujo
            //                objDesdeDb.Status = false;
            //                objDesdeDb.Statusrechazado = true;
            //                _db.Entry(objDesdeDb).State = EntityState.Modified;
            //                _db.SaveChanges();
            //            }
            //            else
            //            {

            //                var release = _db.csexsw_tarea.Where(s => s.Id == objDesdeDb.TareaId).FirstOrDefault();
            //                release.Statusrechazado = false;
            //                release.Statusaprobado = false;
            //                release.Status = false;
            //                _db.Entry(release).State = EntityState.Modified;
            //                _db.SaveChanges();
            //                //rechazar dibujo individual
            //                objDesdeDb.Status = false;
            //                objDesdeDb.Statusrechazado = true;
            //                _db.Entry(objDesdeDb).State = EntityState.Modified;
            //                _db.SaveChanges();
            //            }
            //        }

            //    }

            //}


        }
        public void comprimir(string rele)
        {

            string startPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + rele);
            if (System.IO.File.Exists(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + rele + ".zip")))
            {
                System.IO.File.Delete(System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + rele + ".zip"));
            }
            string zipPath = System.IO.Path.Combine(rootEC, @"documents\approved_stamped\" + rele + ".zip");

            ZipFile.CreateFromDirectory(startPath, zipPath);
        }

        public void statustarea2(int id)
        {


            var count = _db.csexsw_dibujo.Count(t => t.TareaId == id);

            var count2 = _db.csexsw_dibujo.Count(t => t.TareaId == id && t.Status == true);


            if (count == count2)
            {
                var objDesdeDb2 = _db.csexsw_tarea.FirstOrDefault(s => s.Id == id);

                objDesdeDb2.Status = true;

                _db.SaveChanges();
            }
            else
            {
                var objDesdeDb = _db.csexsw_tarea.FirstOrDefault(s => s.Id == id);

                objDesdeDb.Status = false;
                objDesdeDb.Statusaprobado = false;
                objDesdeDb.Statusrechazado = false;
                _db.SaveChanges();
            }
        }
        public void cambiopdf(int id)
        {
            var Statustrechazo = _db.csexsw_dibujo.Where(s => s.Id == id).First();
            Statustrechazo.Statusrechazado = false;
            _db.SaveChanges();

            var Statust1 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Design Engineer").First();
            Statust1.Statusrechazo = false;
            Statust1.Status = false;
            Statust1.Statust = false;
            _db.SaveChanges();

            var Statust2 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Drafting and Checking").First();
            Statust2.Statusrechazo = false;
            Statust2.Status = false;
            Statust2.Statust = false;
            _db.SaveChanges();

            var Statust3 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Engineer in Charge").First();
            Statust3.Statusrechazo = false;
            Statust3.Status = false;
            Statust3.Statust = false;
            _db.SaveChanges();

            var Statust4 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Finish spec").First();
            Statust4.Statusrechazo = false;
            Statust4.Status = false;
            Statust4.Statust = false;
            _db.SaveChanges();

            var Statust5 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Material spec").First();
            Statust5.Statusrechazo = false;
            Statust5.Status = false;
            Statust5.Statust = false;
            _db.SaveChanges();

            var Statust6 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Assembly spec").First();
            Statust6.Statusrechazo = false;
            Statust6.Status = false;
            Statust6.Statust = false;
            _db.SaveChanges();

            var Statust7 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "High reliability drawing").First();
            Statust7.Statusrechazo = false;
            Statust7.Status = false;
            Statust7.Statust = false;
            _db.SaveChanges();

            var Statust8 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Molding compound spec").First();
            Statust8.Statusrechazo = false;
            Statust8.Status = false;
            Statust8.Statust = false;
            _db.SaveChanges();

            var Statust9 = _db.csexsw_revision.Where(s => s.Dibujoid == id && s.Descripcion == "Welding or special process").First();
            Statust9.Statusrechazo = false;
            Statust9.Status = false;
            Statust9.Statust = false;
            _db.SaveChanges();

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status = false;
            objDesdeDb.Status1 = false;
            objDesdeDb.Status2 = false;
            objDesdeDb.Status3 = false;
            objDesdeDb.Status4 = false;
            objDesdeDb.Status5 = false;
            objDesdeDb.Status6 = false;
            objDesdeDb.Status7 = false;
            objDesdeDb.Status8 = false;
            objDesdeDb.Status9 = false;

            _db.SaveChanges();




        }


        public iTextSharp.text.pdf.PdfWriter PdfWriter_GetInstance(iTextSharp.text.Document document, System.IO.FileStream FS)
        {
            iTextSharp.text.pdf.PdfWriter writer = null;

            for (int Times = 0; Times < 6; Times++)
            {
                try
                {
                    writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, FS); // sometime rise exception on first call
                    break; //created, then exit loop
                }
                catch
                {
                    System.Threading.Thread.Sleep(250); // wait for a while...
                }
            }
            if (writer == null) // check if instantiated
            {
                throw new Exception("iTextSharp PdfWriter is null");
            }

            return writer;
        }

        public void manipulatePdf(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            int n = reader.NumberOfPages;
            PdfDictionary page;
            PdfNumber rotate;
            var fs = new FileStream(dest, FileMode.Open, FileAccess.ReadWrite);
            PdfStamper stamper = new PdfStamper(reader, fs);

            DateTime hoy = DateTime.Now;
            string hora = DateTime.Now.ToString("hh:mm:ss:tt");
            string fecha = hoy.ToString("MMM dd yyyy");
            string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(fecha);
            string aprovado = "";
            aprovado = "PRELIMINARY DESIGN/ " + resul + " " + hora;

            for (int i = 1; i <= n; i++)
            {
                page = reader.GetPageN(i);
                rotate = page.GetAsNumber(PdfName.ROTATE);
                if (rotate == null)
                {
                    page.Put(PdfName.ROTATE, new PdfNumber(0));
                }
                else
                {
                    page.Put(PdfName.ROTATE, new PdfNumber((rotate.IntValue + 0) % 360));
                }
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                PdfContentByte canvas = stamper.GetOverContent(i);
                canvas.SetFontAndSize(baseFont, 12);
                canvas.SetColorFill(BaseColor.RED);
                ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, new Phrase(aprovado), 5, 5, 0);


            }
            stamper.Close();
            reader.Close();

        }
        const string rootE = @"E:\CSFiles\documents\";
        const string rootEC = @"E:\CSFiles\";
        public void nuevostampado(string dibujopdf, string rele, bool sysc)
        {
            using (var reader = new iTextSharp.text.pdf.PdfReader(Path.Combine(rootEC, @"documents\drawings\" + rele + @"\" + dibujopdf)))
            {
                DateTime hoy = DateTime.Now;
                string hora = DateTime.Now.ToString("hh:mm:ss:tt");
                string fecha = hoy.ToString("MMM dd yyyy");
                string resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(fecha);
                string aprovado = "";
                aprovado = "PRELIMINARY DESIGN/ " + resul + " " + hora;
                string statusPath = "";
                if (sysc)
                {
                    statusPath = "approved_stamped";
                }
                else
                {
                    statusPath = "pending";
                }
                using (var fileStream = new FileStream(Path.Combine(rootEC, @"documents\" + statusPath + @"\" + rele + @"\" + dibujopdf), FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(reader.GetPageSize(1));
                    var writer = PdfWriter.GetInstance(document, fileStream);
                    document.Open();

                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                        var importedPage = writer.GetImportedPage(reader, i);
                        var contentByte = writer.DirectContent;
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(baseFont, 10);
                        contentByte.SetColorFill(BaseColor.RED);
                        var multiLineString = aprovado.Split('\n');

                        foreach (var line in multiLineString)
                        {
                            contentByte.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, line, 1, 1, 0);
                        }
                        contentByte.EndText();
                        contentByte.AddTemplate(importedPage, 0, 0);

                    }
                    document.Close();
                    writer.Close();
                    manipulatePdf(Path.Combine(rootEC, @"documents\drawings\" + rele + @"\" + dibujopdf), Path.Combine(rootEC, @"documents\pending\" + rele + @"\" + dibujopdf));

                }
            }
        }

        //public void nuevostampado(string dibujopdf, string rele)
        //{
        //    try
        //    {
        //        string sourcePath = Path.Combine(rootEC, @"documents\drawings", rele, dibujopdf);
        //        string destPath = Path.Combine(rootEC, @"documents\pending", rele, dibujopdf);

        //        // Ensure the destination directory exists
        //        string destDir = Path.GetDirectoryName(destPath);
        //        if (!Directory.Exists(destDir))
        //        {
        //            Directory.CreateDirectory(destDir);
        //        }

        //        // Check if source file exists
        //        if (!File.Exists(sourcePath))
        //        {
        //            Console.WriteLine("Source file does not exist: " + sourcePath);
        //            return;
        //        }

        //        using (PdfReader reader = new PdfReader(sourcePath))
        //        {
        //            using (FileStream fs = new FileStream(destPath, FileMode.Create, FileAccess.Write))
        //            {
        //                using (PdfStamper stamper = new PdfStamper(reader, fs))
        //                {
        //                    DateTime now = DateTime.Now;
        //                    string time = now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        //                    string date = now.ToString("MMM dd yyyy", CultureInfo.InvariantCulture);
        //                    string formattedDate = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(date);
        //                    string stampText = "PRELIMINARY DESIGN/ " + formattedDate + " " + time;

        //                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //                    for (int i = 1; i <= reader.NumberOfPages; i++)
        //                    {
        //                        PdfContentByte content = stamper.GetOverContent(i);
        //                        content.BeginText();
        //                        content.SetFontAndSize(baseFont, 10);
        //                        content.SetColorFill(BaseColor.RED);
        //                        content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, stampText, 10, 10, 0);
        //                        content.EndText();
        //                    }
        //                }
        //            }
        //        }

        //        // Optional: further manipulation if needed
        //        manipulatePdf(sourcePath, destPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in nuevostampado: " + ex.ToString());
        //    }
        //}

        public void txt(string fecha, string usuario)
        {

            string path = @"E:\CSFiles\documents\pending";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(usuario + "RELEASED" + fecha);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }
        public void UpdateAprobado(int us, string id)
        {

            var revisor1 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior1).First();

            var revisor2 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior2).First();

            var revisor3 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior3).First();
            var revisor4 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior4).First();
            var revisor5 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior5).First();
            var revisor6 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior6).First();
            var revisor7 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior7).First();
            var revisor8 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior8).First();
            var revisor9 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior9).First();


            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == us);


            if (revisor1 == id)
            {
                objDesdeDb.Status1 = true;
                _db.SaveChanges();
            }
            if (revisor2 == id)
            {
                objDesdeDb.Status2 = true;
                _db.SaveChanges();
            }
            if (revisor3 == id)
            {
                objDesdeDb.Status3 = true;
                _db.SaveChanges();
            }
            if (revisor4 == id)
            {
                objDesdeDb.Status4 = true;
                _db.SaveChanges();
            }
            if (revisor5 == id)
            {
                objDesdeDb.Status5 = true;
                _db.SaveChanges();
            }
            if (revisor6 == id)
            {
                objDesdeDb.Status6 = true;
                _db.SaveChanges();
            }
            if (revisor7 == id)
            {
                objDesdeDb.Status7 = true;
                _db.SaveChanges();
            }
            if (revisor8 == id)
            {
                objDesdeDb.Status8 = true;
                _db.SaveChanges();
            }

            if (revisor9 == id)
            {
                objDesdeDb.Status9 = true;

                _db.SaveChanges();
            }








        }


        public void reviewers2(int us)
        {

            var revisor1 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior1).FirstOrDefault();

            var revisor2 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior2).FirstOrDefault();

            var revisor3 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior3).FirstOrDefault();
            var revisor4 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior4).FirstOrDefault();
            var revisor5 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior5).FirstOrDefault();
            var revisor6 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior6).FirstOrDefault();
            var revisor7 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior7).FirstOrDefault();
            var revisor8 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior8).FirstOrDefault();
            var revisor9 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior9).FirstOrDefault();

            var dibujopdf = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Dibujopdf).FirstOrDefault();

            var descripcion = (from c in _db.csexsw_dibujo
                               where c.Id == us
                               select c.Descripcion).FirstOrDefault();



            var objDesdeDb = new csexsw_revision();
            objDesdeDb.Nombre = !string.IsNullOrEmpty(revisor1) && revisor1 != "0" ? EmployeeNameByResId(revisor1) : "Not Reviewer"; ;
            objDesdeDb.Descripcion = "Design Engineer";
            objDesdeDb.Statust = false;
            objDesdeDb.Status = false;
            objDesdeDb.Dibujoid = us;
            objDesdeDb.Dibujopdf = dibujopdf;
            objDesdeDb.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb);

            _db.SaveChanges();


            var objDesdeDb2 = new csexsw_revision();


            objDesdeDb2.Nombre = !string.IsNullOrEmpty(revisor2) && revisor2 != "0" ? EmployeeNameByResId(revisor2) : "Not Reviewer"; ;
            objDesdeDb2.Descripcion = "Drafting and Checking";
            objDesdeDb2.Status = false;
            objDesdeDb2.Dibujoid = us;
            objDesdeDb2.Statust = false;
            objDesdeDb2.Dibujopdf = dibujopdf;
            objDesdeDb2.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb2);

            _db.SaveChanges();


            var objDesdeDb3 = new csexsw_revision();

            objDesdeDb3.Nombre = !string.IsNullOrEmpty(revisor3) && revisor3 != "0" ? EmployeeNameByResId(revisor3) : "Not Reviewer"; ;
            objDesdeDb3.Descripcion = "Enginner in Charge";
            objDesdeDb3.Status = false;
            objDesdeDb3.Dibujoid = us;
            objDesdeDb3.Statust = false;
            objDesdeDb3.Dibujopdf = dibujopdf;
            objDesdeDb3.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb3);

            _db.SaveChanges();


            var objDesdeDb4 = new csexsw_revision();

            objDesdeDb4.Nombre = !string.IsNullOrEmpty(revisor4) && revisor4 != "0" ? EmployeeNameByResId(revisor4) : "Not Reviewer"; ;
            objDesdeDb4.Descripcion = "Finish spec ";
            objDesdeDb4.Status = false;
            objDesdeDb4.Statust = false;
            objDesdeDb4.Dibujoid = us;
            objDesdeDb4.Dibujopdf = dibujopdf;
            objDesdeDb4.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb4);

            _db.SaveChanges();

            var objDesdeDb5 = new csexsw_revision();

            objDesdeDb5.Nombre = !string.IsNullOrEmpty(revisor5) && revisor5 != "0" ? EmployeeNameByResId(revisor5) : "Not Reviewer"; ;
            objDesdeDb5.Descripcion = "Material spec";
            objDesdeDb5.Status = false;
            objDesdeDb5.Dibujoid = us;
            objDesdeDb5.Statust = false;
            objDesdeDb5.Dibujopdf = dibujopdf;
            objDesdeDb5.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb5);

            _db.SaveChanges();



            var objDesdeDb6 = new csexsw_revision();

            objDesdeDb6.Nombre = !string.IsNullOrEmpty(revisor6) && revisor6 != "0" ? EmployeeNameByResId(revisor6) : "Not Reviewer"; ;
            objDesdeDb6.Statust = false;
            objDesdeDb6.Descripcion = "Assembly spec";
            objDesdeDb6.Status = false;
            objDesdeDb6.Dibujoid = us;
            objDesdeDb6.Dibujopdf = dibujopdf;
            objDesdeDb6.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb6);

            _db.SaveChanges();
            var objDesdeDb7 = new csexsw_revision();

            objDesdeDb7.Nombre = !string.IsNullOrEmpty(revisor7) && revisor7 != "0" ? EmployeeNameByResId(revisor7) : "Not Reviewer"; ;
            objDesdeDb7.Descripcion = "High reliability drawing";
            objDesdeDb7.Status = false;
            objDesdeDb7.Dibujoid = us;
            objDesdeDb7.Statust = false;
            objDesdeDb7.Dibujopdf = dibujopdf;
            objDesdeDb7.Drawing = descripcion;

            _db.csexsw_revision.Add(objDesdeDb7);

            _db.SaveChanges();
            var objDesdeDb8 = new csexsw_revision();

            objDesdeDb8.Nombre = !string.IsNullOrEmpty(revisor8) && revisor8 != "0" ? EmployeeNameByResId(revisor8) : "Not Reviewer"; ;
            objDesdeDb8.Statust = false;
            objDesdeDb8.Descripcion = "Molding compound spec";
            objDesdeDb8.Status = false;
            objDesdeDb8.Dibujoid = us;
            objDesdeDb8.Dibujopdf = dibujopdf;
            objDesdeDb8.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb8);

            _db.SaveChanges();

            var objDesdeDb9 = new csexsw_revision();

            objDesdeDb9.Nombre = !string.IsNullOrEmpty(revisor9) && revisor9 != "0" ? EmployeeNameByResId(revisor9) : "Not Reviewer"; ;
            objDesdeDb9.Descripcion = "Welding or special process";
            objDesdeDb9.Status = false;
            objDesdeDb9.Dibujoid = us;
            objDesdeDb9.Statust = false;
            objDesdeDb9.Dibujopdf = dibujopdf;
            objDesdeDb9.Drawing = descripcion;
            _db.csexsw_revision.Add(objDesdeDb9);

            _db.SaveChanges();



        }



        public void reviewers(string type = "")
        {
            var us = (from c in _db.csexsw_dibujo
                      orderby c.Id descending
                      select c.Id).FirstOrDefault();



            var revisor1 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior1).FirstOrDefault();

            var revisor2 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior2).FirstOrDefault();

            var revisor3 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior3).FirstOrDefault();
            var revisor4 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior4).FirstOrDefault();
            var revisor5 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior5).FirstOrDefault();
            var revisor6 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior6).FirstOrDefault();
            var revisor7 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior7).FirstOrDefault();
            var revisor8 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior8).FirstOrDefault();
            var revisor9 = (from c in _db.csexsw_dibujo
                            where c.Id == us
                            select c.Revisior9).FirstOrDefault();

            var rev_qc = (from c in _db.csexsw_dibujo
                          where c.Id == us
                          select c.revisior_quality).FirstOrDefault();

            var rev_linesup = (from c in _db.csexsw_dibujo
                               where c.Id == us
                               select c.revisior_line_sup).FirstOrDefault();

            var rev_linelead = (from c in _db.csexsw_dibujo
                                where c.Id == us
                                select c.revisior_line_lead).FirstOrDefault();

            var rev_pe = (from c in _db.csexsw_dibujo
                          where c.Id == us
                          select c.revisior_pe).FirstOrDefault();



            var dibujopdf = (from c in _db.csexsw_dibujo
                             where c.Id == us
                             select c.Dibujopdf).FirstOrDefault();

            var descripcion = (from c in _db.csexsw_dibujo
                               where c.Id == us
                               select c.Descripcion).FirstOrDefault();
            //es release
            if (string.IsNullOrEmpty(type) || type == "")
            {



                var objDesdeDb = new csexsw_revision();
                objDesdeDb.Nombre = !string.IsNullOrEmpty(revisor1) && revisor1 != "0" ? EmployeeNameByResId(revisor1) : "Not Reviewer";
                objDesdeDb.Descripcion = "Design Engineer";
                objDesdeDb.Statust = false;
                objDesdeDb.Status = false;
                objDesdeDb.Dibujoid = us;
                objDesdeDb.Dibujopdf = dibujopdf;
                objDesdeDb.Drawing = descripcion;
                objDesdeDb.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb);

                _db.SaveChanges();


                var objDesdeDb2 = new csexsw_revision();


                objDesdeDb2.Nombre = !string.IsNullOrEmpty(revisor2) && revisor2 != "0" ? EmployeeNameByResId(revisor2) : "Not Reviewer";
                objDesdeDb2.Descripcion = "Drafting and Checking";
                objDesdeDb2.Status = false;
                objDesdeDb2.Dibujoid = us;
                objDesdeDb2.Statust = false;
                objDesdeDb2.Dibujopdf = dibujopdf;
                objDesdeDb2.Drawing = descripcion;
                objDesdeDb2.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb2);

                _db.SaveChanges();


                var objDesdeDb3 = new csexsw_revision();

                objDesdeDb3.Nombre = !string.IsNullOrEmpty(revisor3) && revisor3 != "0" ? EmployeeNameByResId(revisor3) : "Not Reviewer";
                objDesdeDb3.Descripcion = "Engineer in Charge";
                objDesdeDb3.Status = false;
                objDesdeDb3.Dibujoid = us;
                objDesdeDb3.Statust = false;
                objDesdeDb3.Dibujopdf = dibujopdf;
                objDesdeDb3.Drawing = descripcion;
                objDesdeDb3.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb3);

                _db.SaveChanges();


                var objDesdeDb4 = new csexsw_revision();

                objDesdeDb4.Nombre = !string.IsNullOrEmpty(revisor4) && revisor4 != "0" ? EmployeeNameByResId(revisor4) : "Not Reviewer";
                objDesdeDb4.Descripcion = "Finish spec";
                objDesdeDb4.Status = false;
                objDesdeDb4.Statust = false;
                objDesdeDb4.Dibujoid = us;
                objDesdeDb4.Dibujopdf = dibujopdf;
                objDesdeDb4.Drawing = descripcion;
                objDesdeDb4.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb4);

                _db.SaveChanges();

                var objDesdeDb5 = new csexsw_revision();

                objDesdeDb5.Nombre = !string.IsNullOrEmpty(revisor5) && revisor5 != "0" ? EmployeeNameByResId(revisor5) : "Not Reviewer";
                objDesdeDb5.Descripcion = "Material spec";
                objDesdeDb5.Status = false;
                objDesdeDb5.Dibujoid = us;
                objDesdeDb5.Statust = false;
                objDesdeDb5.Dibujopdf = dibujopdf;
                objDesdeDb5.Drawing = descripcion;
                objDesdeDb5.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb5);

                _db.SaveChanges();



                var objDesdeDb6 = new csexsw_revision();

                objDesdeDb6.Nombre = !string.IsNullOrEmpty(revisor6) && revisor5 != "0" ? EmployeeNameByResId(revisor6) : "Not Reviewer";
                objDesdeDb6.Statust = false;
                objDesdeDb6.Descripcion = "Assembly spec";
                objDesdeDb6.Status = false;
                objDesdeDb6.Dibujoid = us;
                objDesdeDb6.Dibujopdf = dibujopdf;
                objDesdeDb6.Drawing = descripcion;
                objDesdeDb6.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb6);

                _db.SaveChanges();
                var objDesdeDb7 = new csexsw_revision();

                objDesdeDb7.Nombre = !string.IsNullOrEmpty(revisor7) && revisor7 != "0" ? EmployeeNameByResId(revisor7) : "Not Reviewer";
                objDesdeDb7.Descripcion = "High reliability drawing";
                objDesdeDb7.Status = false;
                objDesdeDb7.Dibujoid = us;
                objDesdeDb7.Statust = false;
                objDesdeDb7.Dibujopdf = dibujopdf;
                objDesdeDb7.Drawing = descripcion;
                objDesdeDb7.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb7);

                _db.SaveChanges();
                var objDesdeDb8 = new csexsw_revision();

                objDesdeDb8.Nombre = !string.IsNullOrEmpty(revisor8) && revisor8 != "0" ? EmployeeNameByResId(revisor8) : "Not Reviewer";
                objDesdeDb8.Statust = false;
                objDesdeDb8.Descripcion = "Molding compound spec";
                objDesdeDb8.Status = false;
                objDesdeDb8.Dibujoid = us;
                objDesdeDb8.Dibujopdf = dibujopdf;
                objDesdeDb8.Drawing = descripcion;
                objDesdeDb8.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb8);

                _db.SaveChanges();

                var objDesdeDb9 = new csexsw_revision();

                objDesdeDb9.Nombre = !string.IsNullOrEmpty(revisor9) && revisor9 != "0" ? EmployeeNameByResId(revisor9) : "Not Reviewer";
                objDesdeDb9.Descripcion = "Welding or special process";
                objDesdeDb9.Status = false;
                objDesdeDb9.Dibujoid = us;
                objDesdeDb9.Statust = false;
                objDesdeDb9.Dibujopdf = dibujopdf;
                objDesdeDb9.Drawing = descripcion;
                objDesdeDb9.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(objDesdeDb9);

                _db.SaveChanges();
            }
            else
            {
                //dc quality, line sup etc
                var rev_quality = new csexsw_revision();
                rev_quality.Nombre = !string.IsNullOrEmpty(rev_qc) && rev_qc != "0" ? EmployeeNameByResId(rev_qc) : "Not Reviewer";
                rev_quality.Descripcion = "DC Quality";
                rev_quality.Statust = false;
                rev_quality.Status = false;
                rev_quality.Dibujoid = us;
                rev_quality.Dibujopdf = dibujopdf;
                rev_quality.Drawing = descripcion;
                rev_quality.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(rev_quality);
                _db.SaveChanges();

                var rev_line_sup = new csexsw_revision();
                rev_line_sup.Nombre = !string.IsNullOrEmpty(rev_linesup) && rev_linesup != "0" ? EmployeeNameByResId(rev_linesup) : "Not Reviewer";
                rev_line_sup.Descripcion = "DC - Line Supervisor";
                rev_line_sup.Statust = false;
                rev_line_sup.Status = false;
                rev_line_sup.Dibujoid = us;
                rev_line_sup.Dibujopdf = dibujopdf;
                rev_line_sup.Drawing = descripcion;
                rev_line_sup.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(rev_line_sup);
                _db.SaveChanges();

                var rev_line_lead = new csexsw_revision();
                rev_line_lead.Nombre = !string.IsNullOrEmpty(rev_linelead) && rev_linelead != "0" ? EmployeeNameByResId(rev_linelead) : "Not Reviewer";
                rev_line_lead.Descripcion = "DC - Line Lead";
                rev_line_lead.Statust = false;
                rev_line_lead.Status = false;
                rev_line_lead.Dibujoid = us;
                rev_line_lead.Dibujopdf = dibujopdf;
                rev_line_lead.Drawing = descripcion;
                rev_line_lead.assigned_to = "Not assigned";
                _db.csexsw_revision.Add(rev_line_lead);
                _db.SaveChanges();

                var revpe = new csexsw_revision();
                revpe.Nombre = !string.IsNullOrEmpty(rev_pe) && rev_pe != "0" ? EmployeeNameByResId(rev_pe) : "Not Reviewer";
                revpe.Descripcion = "DC - Process Engineer";
                revpe.Statust = false;
                revpe.Status = false;
                revpe.Dibujoid = us;
                revpe.assigned_to = "Not assigned";
                revpe.Dibujopdf = dibujopdf;
                revpe.Drawing = descripcion;
                _db.csexsw_revision.Add(revpe);
                _db.SaveChanges();


            }





        }


        public void borrarTasks(int id)
        {

            _db.csexsw_revision.RemoveRange(_db.csexsw_revision.Where(c => c.Dibujoid == id));
        }


        public void UpdateStatus11(int id, int dib, string usuario)
        {

            var result = _db.csexsw_dibujo.SingleOrDefault(b => b.Id == id);
            result.Revisior1 = usuario;
            result.Status1 = false;
            _db.SaveChanges();

            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(result);
        }

        public void UpdateStatus22(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);
            objDesdeDb.Status2 = false;
            objDesdeDb.Revisior2 = usuario;




            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus33(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status3 = false;
            objDesdeDb.Revisior3 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus44(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status4 = false;

            objDesdeDb.Revisior4 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus55(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status5 = false;

            objDesdeDb.Revisior5 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(objDesdeDb);
        }

        public void UpdateStatus66(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status6 = false;

            objDesdeDb.Revisior6 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();

            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus77(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status7 = false;
            objDesdeDb.Revisior7 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();

            DisaprroveDrawing(objDesdeDb);
        }



        public void UpdateStatus88(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status8 = false;

            objDesdeDb.Revisior8 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();
            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus99(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status9 = false;
            objDesdeDb.Revisior9 = usuario;

            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();

            DisaprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus1010(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);
            objDesdeDb.Status10 = false;
            objDesdeDb.Revisior10 = usuario;

            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = false;
            result2.Statusrechazo = true;
            _db.SaveChanges();

            DisaprroveDrawing(objDesdeDb);
        }

        public void UpdateStatus10(int id, int dib, string usuario)
        {
            var result = _db.csexsw_dibujo.Include(s => s.csexsw_tarea).SingleOrDefault(b => b.Id == id);
            result.Revisior10 = usuario;
            result.Status10 = true;
            _db.SaveChanges();

            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            //result.csexsw_tarea.Status = true;
            _db.Entry(result.csexsw_tarea).State = EntityState.Modified;

            _db.SaveChanges();
            AprroveDrawing(result);
        }
        public void UpdateStatus1(int id, int dib, string usuario)
        {

            var result = _db.csexsw_dibujo.SingleOrDefault(b => b.Id == id);
            result.Revisior1 = usuario;
            result.Status1 = true;
            _db.SaveChanges();

            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(result);

        }
        public void UpdateStatus2(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);
            objDesdeDb.Status2 = true;
            objDesdeDb.Revisior2 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);


        }
        public void AprroveDrawing(csexsw_dibujo dibujo)
        {
            //debe aprobar
            int debeaprobar = 0;
            if (dibujo.Descripcion == "BOM Approver")
            {
                debeaprobar = _db.CSBOMAttachments.Count(r => !string.IsNullOrEmpty(r.filename) && r.resid > 0 && r.tareaid == dibujo.TareaId);
            }
            else
            {
                debeaprobar = dibujo.countAprove;
            }
            int aprobados = 0;
            if (dibujo.Descripcion == "BOM Approver")
            {
                //aprobados 
                aprobados = _db.csexsw_revision.Include(r => r.csexsw_dibujo).Count(s => s.csexsw_dibujo.TareaId == dibujo.TareaId && s.Status == true);
            }
            else
            {
                //aprobados 
                aprobados = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Status == true);
            }

            //si aprobados == debeaprobar
            if (aprobados == debeaprobar)
            {
                //actualizar dibujo a true 
                dibujo.Status = true;
                _db.Entry(dibujo).State = EntityState.Modified;
                _db.SaveChanges();

                if (dibujo.Descripcion == "BOM Approver")
                {
                    var tarea = _db.csexsw_tarea.Where(s => s.Id == dibujo.TareaId).FirstOrDefault();
                    tarea.Status = true;
                    _db.Entry(tarea).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
        }
        public void DisaprroveDrawing(csexsw_dibujo dibujo)
        {
            //debe aprobar
            int deberechazar = 0;
            if (dibujo.Descripcion == "BOM Approver")
            {
                deberechazar = _db.CSBOMAttachments.Count(r => !string.IsNullOrEmpty(r.filename) && r.resid > 0 && r.tareaid == dibujo.TareaId);
            }
            else
            {
                deberechazar = dibujo.countReject;
            }
            //aprobados 
            int rechazados = 0;
            if (dibujo.Descripcion == "BOM Approver")
            {
                rechazados = _db.csexsw_revision.Include(r => r.csexsw_dibujo).Count(s => s.csexsw_dibujo.TareaId == dibujo.TareaId && s.Statusrechazo == true);
            }
            else
            {
                rechazados = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Statusrechazo == true);

            }
            //si rechazados == deberechazar
            if (rechazados == deberechazar)
            {
                //actualizar dibujo a true 
                dibujo.Status = false;
                dibujo.Statusrechazado = true;
                _db.Entry(dibujo).State = EntityState.Modified;
                _db.SaveChanges();

                //if (dibujo.Descripcion=="BOM Approver")
                //{
                //    var tarea = _db.csexsw_tarea.Where(s => s.Id == dibujo.TareaId).FirstOrDefault();
                //    tarea.Status = false;
                //    tarea.Statusaprobado = false;
                //    tarea.Statusrechazado = true;
                //    _db.Entry(tarea).State = EntityState.Modified;
                //    _db.SaveChanges();

                //}
            }
        }
        public void UpdateStatus3(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status3 = true;
            objDesdeDb.Revisior3 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus4(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status4 = true;
            objDesdeDb.Revisior4 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;
            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus5(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status5 = true;
            objDesdeDb.Revisior5 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;
            _db.SaveChanges();

            AprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus6(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status6 = true;

            objDesdeDb.Revisior6 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus7(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status7 = true;

            objDesdeDb.Revisior7 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }



        public void UpdateStatus8(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status8 = true;
            objDesdeDb.Revisior8 = usuario;



            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }
        public void UpdateStatus9(int id, int dib, string usuario)
        {

            var objDesdeDb = _db.csexsw_dibujo.FirstOrDefault(s => s.Id == id);

            objDesdeDb.Status9 = true;

            objDesdeDb.Revisior9 = usuario;


            _db.SaveChanges();
            var result2 = _db.csexsw_revision.SingleOrDefault(b => b.Id == dib);
            result2.Nombre = !string.IsNullOrEmpty(usuario) && usuario != "0" ? EmployeeNameByResId(usuario) : "Not Reviewer";
            result2.Status = true;
            result2.Statusrechazo = false;

            _db.SaveChanges();
            AprroveDrawing(objDesdeDb);
        }

        public IEnumerable<csexsw_dibujo> All(int id)
        {
            return _db.csexsw_dibujo.Include(s => s.csexsw_tarea).Where(s => s.csexsw_tarea.Id == id).OrderByDescending(s => s.Id).ToList();
        }

        public void AddDrawing(csexsw_dibujo drawing)
        {
            drawing.Descripcion = !string.IsNullOrEmpty(drawing.Descripcion) ? drawing.Descripcion : "";
            drawing.BOMFileName = "";
            _db.csexsw_dibujo.Add(drawing);
            _db.SaveChanges();
        }

        public humres EmployeeByUsername(string resid)
        {
            var emp = (from c in _db.humres
                       where c.res_id.ToString() == resid
                       select c).FirstOrDefault();
            return emp;
        }

        public string EmployeeNameByResId(string resid)
        {
            if (!string.IsNullOrEmpty(resid) && resid != "0")
            {
                return _db.humres.Where(s => s.res_id.ToString() == resid).FirstOrDefault().fullname;
            }
            else
            {
                return "Not Reviewer";
            }
        }

        public void UpdateStatusNoDrawing(int dibujo_id, int revision_id, string username)
        {
            var rev = _db.csexsw_revision.Where(S => S.Id == revision_id).FirstOrDefault();
            var dibujo = _db.csexsw_dibujo.Where(s => s.Id == dibujo_id).FirstOrDefault();
            switch (rev.Descripcion)
            {
                case "DC - Line Supervisor":
                    dibujo.status_line_sup = true;
                    dibujo.revisior_line_sup = username;
                    break;
                case "DC - Line Lead":
                    dibujo.status_line_lead = true;
                    dibujo.revisior_line_lead = username;
                    break;
                case "DC - Process Engineer":
                    dibujo.status_pe = true;
                    dibujo.revisior_pe = username;
                    break;
                case "DC Quality":
                    dibujo.status_quality = true;
                    dibujo.revisior_quality = username;
                    break;
            }
            _db.SaveChanges();

            rev.Nombre = !string.IsNullOrEmpty(username) && username != "0" ? EmployeeNameByResId(username) : "Not Reviewer";
            rev.Status = true;
            rev.Statusrechazo = false;
            _db.SaveChanges();
            AprroveNoDrawingRelease(dibujo);

        }

        public void AprroveNoDrawingRelease(csexsw_dibujo dibujo)
        {

            int debeaprobar = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Nombre != "Not Reviewer");
            int aprobados = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Status == true);
            if (aprobados == debeaprobar)
            {
                dibujo.Status = true;
                _db.Entry(dibujo).State = EntityState.Modified;
                _db.SaveChanges();
            }

            int total_dibujos = _db.csexsw_dibujo.Count(r => r.TareaId == dibujo.TareaId);
            int aprobados_dibujos = _db.csexsw_dibujo.Count(f => f.TareaId == dibujo.TareaId && f.Status == true);
            if (aprobados_dibujos == total_dibujos)
            {
                var tarea = _db.csexsw_tarea.Where(s => s.Id == dibujo.TareaId).FirstOrDefault();
                tarea.Status = true;
                tarea.Statusrechazado = false;
                _db.Entry(tarea).State = EntityState.Modified;
                _db.SaveChanges();
            }

        }

        public void RejectNoDrawing(int dibujo_id, int revision_id, string username)
        {
            var rev = _db.csexsw_revision.Where(S => S.Id == revision_id).FirstOrDefault();
            var dibujo = _db.csexsw_dibujo.Where(s => s.Id == dibujo_id).FirstOrDefault();
            switch (rev.Descripcion)
            {
                case "DC - Line Supervisor":
                    dibujo.status_line_sup = false;
                    dibujo.revisior_line_sup = username;
                    break;
                case "DC - Line Lead":
                    dibujo.status_line_lead = false;
                    dibujo.revisior_line_lead = username;
                    break;
                case "DC - Process Engineer":
                    dibujo.status_pe = false;
                    dibujo.revisior_pe = username;
                    break;
                case "DC Quality":
                    dibujo.status_quality = false;
                    dibujo.revisior_quality = username;
                    break;
            }
            _db.SaveChanges();

            rev.Nombre = !string.IsNullOrEmpty(username) && username != "0" ? EmployeeNameByResId(username) : "Not Reviewer";
            rev.Status = false;
            rev.Statusrechazo = true;
            _db.SaveChanges();
            RejectNoDrawingRelease(dibujo);
        }

        public void RejectNoDrawingRelease(csexsw_dibujo dibujo)
        {
            int deberechazar = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Nombre != "Not Reviewer");
            int rechazados = _db.csexsw_revision.Count(s => s.Dibujoid == dibujo.Id && s.Statusrechazo == true);
            if (rechazados == deberechazar)
            {

                dibujo.Status = false;
                dibujo.Statusrechazado = true;
                _db.Entry(dibujo).State = EntityState.Modified;
                _db.SaveChanges();

            }
            else
            {
                if (rechazados > 0)
                {
                    var tarea = _db.csexsw_tarea.Where(r => r.Id == dibujo.TareaId).FirstOrDefault();
                    tarea.Status = true;
                    tarea.Statusrechazado = false;
                    tarea.Statusaprobado = false;
                    _db.Entry(tarea).State = EntityState.Modified;
                    _db.SaveChanges();

                    dibujo.Status = false;
                    dibujo.Statusrechazado = false;
                    _db.Entry(dibujo).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }

            int total_dibujos = _db.csexsw_dibujo.Count(s => s.TareaId == dibujo.TareaId);
            int dibujos_rechazados = _db.csexsw_dibujo.Count(r => r.TareaId == dibujo.TareaId && r.Statusrechazado == true);
            if (dibujos_rechazados == total_dibujos)
            {
                var tarea = _db.csexsw_tarea.Where(s => s.Id == dibujo.TareaId).FirstOrDefault();
                tarea.Status = false;
                tarea.Statusrechazado = true;
                _db.Entry(tarea).State = EntityState.Modified;
                _db.SaveChanges();
            }

        }
    }




    public class BeerValidations
    {
        public static readonly Predicate<csexsw_dibujo>[] validations =
            {



                        (d) => (d.Status1 == true && d.Revision1 == true) || (d.Status1 == false && d.Revision1 == false) ,
                        (d) => (d.Status2 == true && d.Revision2 == true) || (d.Status2 == false && d.Revision2 == false) ,
                        (d) => (d.Status3 == true && d.Revision3 == true) || (d.Status3 == false && d.Revision3 == false) ,
                        (d) => (d.Status4 == true && d.Revision4 == true) || (d.Status4 == false && d.Revision4 == false) ,
                        (d) => (d.Status5 == true && d.Revision5 == true) || (d.Status5 == false && d.Revision5 == false) ,
                        (d) => (d.Status6 == true && d.Revision6 == true) || (d.Status6 == false && d.Revision6 == false) ,
                        (d) => (d.Status7 == true && d.Revision7 == true) || (d.Status7 == false && d.Revision7 == false) ,
                        (d) => (d.Status8 == true && d.Revision8 == true) || (d.Status8 == false && d.Revision8 == false) ,
                        (d) => (d.Status9 == true && d.Revision9 == true) || (d.Status9 == false && d.Revision9 == false) ,

            };
    }

    public class Validator
    {
        public static bool Validate<T>(T data, params Predicate<T>[] validations) =>
            validations.ToList().Where(d =>
            {
                return !d(data);
            }).Count() == 0;

    }


}
