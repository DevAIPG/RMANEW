using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class CSEXSW_Approver
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(64)]
        public string Approver { get; set; }
        [MaxLength(64)]
        public string Rango { get; set; }

       
    }
}
