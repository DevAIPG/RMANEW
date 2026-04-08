using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text; 
namespace Amphenol.RMA.Models
{
    public class csexsw_tarea
    {
         
        [Key]
        public int Id { get; set; } 
        [Required(ErrorMessage = "Enter a name for the task ")]
        [Display(Name = "Prepared By")]
        [MaxLength(64)]
        public string Encargado { get; set; }
        //return !string.IsNullOrEmpty( StoreProcedures.StoreProcedures.EmployeesByResId(Encargado)) ? StoreProcedures.StoreProcedures.EmployeesByResId(Encargado) : "No Employee";

        public string Employee
        {
            get
            {
                return "test"; 
            } 
        } 
        [Display(Name = "Project No. (Optional)")]
        [MaxLength(64)]
        public string Proyecto { get; set; }
        [Display(Name = "Project Description (Optional)")] 
        public string Descripcion { get; set; }
        [Display(Name = "Release Number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(8)]
        public string Version { get; set; }
        [Display(Name = "Date")]
        [MaxLength(64)]
        public string Fecha { get; set; } 
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        [Display(Name = "Statusaprobado")]
        public bool? Statusaprobado { get; set; }
        [Display(Name = "Statusrechazado")]
        public bool? Statusrechazado { get; set; }
        [MaxLength(128)]
        public string Category { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
        public string BOMFileName { get; set; }

        public bool IsBOMApprover { get; set; }
        public int BOMApproverId { get; set; }

        public string BOMApprover { get; set; }

    }
}
