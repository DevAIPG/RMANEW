using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.IO;

namespace Amphenol.RMA.Models
{
    public class csexsw_dibujo
    {
        

        [Key]
        public int Id { get; set; }

        [MaxLength(128)]
        //[Required(ErrorMessage = "Documento requerido")]
        [Display(Name = "Document")]
        public string Dibujopdf { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Enter a name for the task ")]
        [Display(Name = "Description")]
        [MaxLength(64)]
        public string Descripcion { get; set; }
        [MaxLength(50)]
        public string Part { get; set; }
        [MaxLength(50)]
        public string Revision { get; set; } 
        [Required(ErrorMessage = "Enter a name for the document.")]
        [Display(Name = "Document Number")]
        [MaxLength(128)]
        public string Nombrepdf { get; set; }

        [Display(Name = "Design Engineer")]
        public bool Revision1 { get; set; }

        [Display(Name = "Drafting and Checking")]
        public bool Revision2 { get; set; }
        [Display(Name = "Engineer in Charge")]
        public bool Revision3 { get; set; }
        [Display(Name = "Finish spec")]
        public bool Revision4 { get; set; }
        [Display(Name = "Material spec")]
        public bool Revision5 { get; set; }
        [Display(Name = "Assembly spec")]
        public bool Revision6 { get; set; }

        [Display(Name = "High reliability drawing")]
        public bool Revision7 { get; set; }

        [Display(Name = "Molding compound spec")]
        public bool Revision8 { get; set; }

        [Display(Name = "Welding or special process")]
        public bool Revision9 { get; set; }


        [Display(Name = "Design Engineer Approval")]
        [MaxLength(64)]
        public string Revisior1 { get; set; }

        [Display(Name = "Drafting and Checking Approval")]
        [MaxLength(64)]
        public string Revisior2 { get; set; }
        [Display(Name = "Engineer in Charge Approval")]
        [MaxLength(64)]
        public string Revisior3 { get; set; }
        [Display(Name = "Finish spec Approval")]
        [MaxLength(64)]
        public string Revisior4 { get; set; }
        [Display(Name = "Material spec Approval")]
        [MaxLength(64)]
        public string Revisior5 { get; set; }
        [Display(Name = "Assembly spec Approval")]
        [MaxLength(64)]
        public string Revisior6 { get; set; }

        [Display(Name = "High reliability drawing Approval")]
        [MaxLength(64)]
        public string Revisior7 { get; set; }

        [Display(Name = "Molding compound spec Approval")]
        [MaxLength(64)]
        public string Revisior8 { get; set; }

        [Display(Name = "Welding or special process Approval")]
        [MaxLength(64)]
        public string Revisior9 { get; set; }

        [Display(Name = "Status Design Engineer")]
        public bool Status1 { get; set; }

        [Display(Name = "Status Drafting and Checking")]
        public bool Status2 { get; set; }
        [Display(Name = "StatusStatus Enginner in Charge")]
        public bool Status3 { get; set; }
        [Display(Name = "Status Finish spec")]
        public bool Status4 { get; set; }
        [Display(Name = "Status Material spec")]
        public bool Status5 { get; set; }
        [Display(Name = "Status Assembly spec")]
        public bool Status6 { get; set; }

        [Display(Name = "Status High reliability drawing")]
        public bool Status7 { get; set; }

        [Display(Name = "Status Molding compound spec")]
        public bool Status8 { get; set; }

        [Display(Name = "Status Welding or special process")]
        public bool Status9 { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }
        [Display(Name = "Rechazado")]
        public bool Statusrechazado { get; set; }
        [Display(Name = "Select: Document type")]

        public int  countAprove
        {
            get
            {
                //var arr = new bool[9] {Status1,Status2,Status3,Status4,Status5,Status6,Status7,Status8,Status9};
                var arrRev = new string[10] {Revisior1,Revisior2,Revisior3,Revisior4,Revisior5,Revisior6,Revisior7,Revisior8,Revisior9,Revisior10};
                return arrRev.Where(s => s != "0").Count();
            }
        }
        public int  countReject
        {
            get
            {
                //var arr = new bool[9] {Status1,Status2,Status3,Status4,Status5,Status6,Status7,Status8,Status9};
                var arrRev = new string[10] {Revisior1,Revisior2,Revisior3,Revisior4,Revisior5,Revisior6,Revisior7,Revisior8,Revisior9,Revisior10};
                return arrRev.Where(s => s != "0").Count();
            }
        }
        public int  countAprove_2
        {
            get
            {
                //var arr = new bool[9] {Status1,Status2,Status3,Status4,Status5,Status6,Status7,Status8,Status9};
                var arrRev = new string[4] {revisior_line_sup,revisior_line_lead,revisior_pe,revisior_quality};
                return arrRev.Where(s => s != "0").Count();
            }
        }
        public int  countReject_2
        {
            get
            {
                //var arr = new bool[9] {Status1,Status2,Status3,Status4,Status5,Status6,Status7,Status8,Status9};
                var arrRev = new string[4] { revisior_line_sup, revisior_line_lead, revisior_pe, revisior_quality };
                return arrRev.Where(s => s != "0").Count();
            }
        }
        public int Type { get; set; } 

        [Required]
        public int TareaId { get; set; }

        public string BOMFileName { get; set; }

        public string bomfile
        {
            get
            {
                return Path.GetFileNameWithoutExtension(BOMFileName);
            }
        }
        public string file
        {
            get
            {
                return string.Format("{0}/{1}", "100", BOMFileName);
            }
        }
        public bool isbomfile 
        {
            get
            {
                return !string.IsNullOrEmpty(BOMFileName) ? true : false;
            }
        
        }

        [ForeignKey("TareaId")]
        public csexsw_tarea csexsw_tarea { get; set; }
        public bool sync { get; set; }
        public bool approved { get; set; }

        public bool Revision10 { get; set; }
        public bool Status10 { get; set; }
        public string Revisior10 { get; set; }

        public bool revision_quality { get; set; }
        public bool revision_pe { get; set; }
        public bool revision_line_sup { get; set; }
        public bool revision_line_lead { get; set; }
        public string revisior_line_sup { get; set; }
        public string revisior_line_lead { get; set; }
        public string revisior_pe { get; set; }
        public string revisior_quality { get; set; }
        public bool status_line_lead { get; set; }
        public bool status_line_sup { get; set; }
        public bool status_pe { get; set; }
        public bool status_quality { get; set; }




        //public csexsw_tarea TaskDrawing
        //{
        //    get
        //    {
        //        return StoreProcedures.StoreProcedures.TaskById(TareaId);

        //    }
        //}
    }
}
