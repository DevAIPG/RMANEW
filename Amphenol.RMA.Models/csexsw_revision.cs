using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_revision
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [Display(Name = "Reviewer name")]
        [MaxLength(128)]
        public string Nombre { get; set; }

        [Display(Name = "Description is Review")]
        [MaxLength(64)]
        public string Descripcion { get; set; }

        [MaxLength(128)]
        [Display(Name = "Description Drawing")]
        public string Drawing { get; set; }

        [Display(Name = "Status")]
        public bool Statust { get; set; }

        [Display(Name = "Drawing")]
        [MaxLength(128)]
        public string Dibujopdf { get; set; }

        [Display(Name = "Status Task")]
        public bool Status { get; set; }
        [Display(Name = "Status Rechazo")]
        public bool Statusrechazo { get; set; }
        [Required]
        public int Dibujoid { get; set; }
        public string assigned_to { get; set; }

        [ForeignKey("Dibujoid")]
        public csexsw_dibujo csexsw_dibujo { get; set; }

    }
}
