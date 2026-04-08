using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class CSEXSW_Rma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }


        [StringLength(8)]
        [Display(Name = "RMA Request")]
        public string Rmarequest { get; set; }
        [Display(Name = "Customer Complaint")]
        public string Comment { get; set; }

        [Display(Name = "Date")]
        [MaxLength(25)]
        public string Date { get; set; }
        [Display(Name = "Customer Part No")]
        [MaxLength(25)]
        public string Customerpartno { get; set; }
        [Display(Name = "Create CAR after approve?")]
        public bool Crearcar { get; set; }
        [MaxLength(25)]
        [Display(Name = "Customer PO Number")]
        public string Customerpo { get; set; }

        [Display(Name = "Customer Complaint")] 
        public string Customercomplait{ get; set; }


        [Display(Name = "Description")]
        [MaxLength(29)]
        public string Description { get; set; }


        [Required(ErrorMessage = "Select is required")]

        [Display(Name = "RMA Type of Request")]
        [MaxLength(64)]
        public string Rmatypeofrequest { get; set; }

        [Display(Name = "Total RMA Values")]

        public double Totalrmavalues { get; set; }
        [Required(ErrorMessage = "Select is required")]

        [Display(Name = "Where built")]
        [StringLength(8)]
        public string Wherebuilt { get; set; }

        [Display(Name = "RMA single line more than $500?")]

        [MaxLength(64)]
        public string RMA500 { get; set; }

        [MaxLength(64)]
        [Display(Name = "Ship_To")]
        public string Ship_To { get; set; }

        [MaxLength(64)]
        public string Preparado { get; set; }
        [MaxLength(20)]
        public string Sumbit { get; set; }
        [MaxLength(2000)]
        public string Car { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }
        [StringLength(8)]
        public string turno { get; set; }
        [MaxLength(64)]
        public string Approver { get; set; }
        [StringLength(8)]
        public string Customer { get; set; }
        [MaxLength(40)]
        public string Contact { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(4)]
        public string Ext { get; set; }
        [MaxLength(128)]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [EmailAddress]
        [MaxLength(128)]
        public string Company { get; set; } 
        public int res_id { get; set; }
        public int res_id_approver { get; set; }
        public string reason { get; set; }
        public DateTime? date_approved { get; set; }
        [NotMapped]
        public string cus_name { get; set; }
    }
}