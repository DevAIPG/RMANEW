using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class humres
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter a name for the filter")]
        [Display(Name = "Filter Name")]
        public string fullname { get; set; }
        public string usr_id { get; set; }
        public string mail { get; set; }
        public int repto_id { get; set; }

        public int res_id { get; set; }


    }
}
