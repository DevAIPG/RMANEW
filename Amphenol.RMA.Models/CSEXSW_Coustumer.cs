using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Amphenol.RMA.Models
{
    public class csexsw_coustumer { 
        [Key]
        public int Id { get; set; }
        [Display(Name = "Invoice Number")]
        [StringLength(8)]
        [MaxLength(8)]
        public string Invoice { get; set; }
        [Display(Name = "Coustumer Part Number")]
        [StringLength(20)]
        [MaxLength(20)]

        public string Coustumer { get; set; }
        [StringLength(3)]

        public string Loc { get; set; }

        [Display(Name = "Authorized Qty")]
        [Column(TypeName = "decimal (13,4)")]
        public decimal Qty { get; set; }

        [Display(Name = " Unit Cost")]

        [Column(TypeName = "decimal (16,6)")]
        public decimal Cost { get; set; }
     

       [Display(Name = " Seq No.")]


        public short Seq { get; set; }
        [MaxLength(1)]
        [StringLength(1)]
        public string Action { get; set; }


        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal (16,6)")]
        public decimal Unit { get; set; }

        [Display(Name = "Return Code")]
        [StringLength(3)]
        public string Retur { get; set; }
        [Required]
        public int RmaId { get; set; }

        public bool Car { get; set; }

        public int rma_seq_no { get; set; }
        [ForeignKey("RmaId")]
        public CSEXSW_Rma CSEXSW_Rma { get; set; }

    }
}
