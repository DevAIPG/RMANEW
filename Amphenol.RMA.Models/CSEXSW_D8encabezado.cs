using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_d8encabezado
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date problem reportd")]
        [MaxLength(64)]
        public string date { get; set; }
        [Display(Name = "Submitted For Closure By")]
        [MaxLength(64)]
        public string Submitted { get; set; }

        [Display(Name = "Quality Manager")]
        [MaxLength(64)]
        public string Manager { get; set; }
        [Display(Name = "Final Approval By")]
        [MaxLength(64)]
        public string Final { get; set; }
        [MaxLength(64)]
        [Display(Name = "Reviewed by")]
        public string Reviewed { get; set; }
        [MaxLength(64)]
        [Display(Name = "Closure Date")]

        public string aprovado { get; set; }
        [Display(Name = "Similar products/part Number affected (At Risk)?")]

        public string pregunta1 { get; set; }
        [Display(Name = "Mistake proofing?")]

        public string pregunta2 { get; set; }
        [Display(Name = "Similar processes affected (At Risk)? ")]

        public string pregunta3 { get; set; }
        public string Detallesd2 { get; set; }
        public string Detallesd3 { get; set; }
        public string Detallesd4 { get; set; }
        public string Detallesd5 { get; set; }
        public string Detallesd6 { get; set; }
        public string Detallesd7 { get; set; }
        public string Detallesd8 { get; set; }
        public bool option1 { get; set; }
        public bool option2 { get; set; }
        public bool option3 { get; set; }
        public bool option4 { get; set; }
        public bool option5 { get; set; }
        [Display(Name = "RC of why Problem Occurred")]
        public string Rc1 { get; set; }
        [Display(Name = "RC of why problem escaped")]
        public string Rc2 { get; set; }
        [Display(Name = "RC of why problem detected")]
        public string Rc3 { get; set; }
        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        [Display(Name = "CAR#")]
        public csexsw_car csexsw_car { get; set; }

    }
}
