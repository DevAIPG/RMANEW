using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class CSEXSW_Attachmentrma
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Documento { get; set; }
       
        [Required]
        public int RmaId { get; set; }

        [ForeignKey("RmaId")]
        public CSEXSW_Rma CSEXSW_Rma { get; set; }

    }
}
