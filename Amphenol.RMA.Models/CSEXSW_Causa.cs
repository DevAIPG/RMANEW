using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_causa
    {
        [Key]
        public int Id { get; set; }

      
        public string Man { get; set; }
        public string Measurement { get; set; }
        public string Machine { get; set; }
        public string Material { get; set; }
        public string Method { get; set; }
        public string Environment { get; set; }

        [Required]
        public int CarId { get; set; }

       
    }
}
