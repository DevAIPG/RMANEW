using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_car
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Part Number")]
        [MaxLength(30)]
        public string Partnumber { get; set; }

        [MaxLength(128)]
        public string Department { get; set; }
        [Display(Name = "Internal Notes")]
        public string Notes { get; set; }
        [Display(Name = "Discrepancy ")]
        public string Discrepancy { get; set; }
        [MaxLength(64)]
        public string Owner { get; set; }
        [MaxLength(64)]
        public string Status { get; set; }
        [Display(Name = "Category")]
        [MaxLength(64)]
        public string Category { get; set; }
        [Display(Name = "Issue Date")]
        public DateTime Issue_date { get; set; }
        [Display(Name = "RMA Number")]
        [MaxLength(8)]
        public string Rmanumber { get; set; }
        [MaxLength(64)]
        [Display(Name = "Defect Code")]
        public string Defectcode { get; set; }


        [Display(Name = "Internal Due Date")]
        public DateTime Internalduedate { get; set; }


        [Display(Name = "Customer due date")]
        public DateTime Responsabledate { get; set; }
        [Display(Name = "Answer Accepted?")]
        [MaxLength(64)]
        public string Answeraccepted { get; set; }

        [Display(Name = "Cell")]
        [MaxLength(64)]
        public string Cell { get; set; }
        [Display(Name = "Assigned to")]
        [MaxLength(64)]
        public string Assignedto { get; set; }
        [MaxLength(64)]
        [Display(Name = "Layout Operation")]
        public string Layoutoperation { get; set; }

        [MaxLength(64)]
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Display(Name = "Customer CAR")]
        [MaxLength(64)]
        public string customercar { get; set; }
        [MaxLength(64)]
        [Display(Name = "Requester")]
        public string Requester { get; set; }
        [MaxLength(64)]
        [Display(Name = "STO (internal) #")]
        public string sto { get; set; }
        [MaxLength(64)]
        [Display(Name = "Customer Part")]
        public string customerpart { get; set; }

        [MaxLength(64)]
        [Display(Name = "Customer P/N")]
        public string customerpn { get; set; }
        [Display(Name = "Customer Name")]
        [MaxLength(64)]
        public string customername { get; set; }
        [MaxLength(64)]
        [Display(Name = "Customer Code")]
        public string customercode { get; set; }
        [MaxLength(64)]

        [Display(Name = "P.O.")]
        public string po { get; set; }

        public string reject_comments { get; set; }
        public string owner_name { get; set; }

        [Display(Name = "Submit for approval")]
        public bool sumbit { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_causa> csexsw_causa { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_whys> csexsw_whys { get; set; }

        [ForeignKey("CarId")]

        public ICollection<csexsw_documentos> csexsw_documentos { get; set; }

        [ForeignKey("CarId")]

        public ICollection<csexsw_d3> csexsw_d3 { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_d6> csexsw_d6 { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_d7_1> csexsw_d7_1 { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_d7_2> csexsw_d7_2 { get; set; }
        [ForeignKey("CarId")]

        public ICollection<csexsw_d7_3> csexsw_d7_3 { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? approval_date { get; set; }

    }
}
