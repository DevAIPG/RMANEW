using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class SYCDEFIL_SQL
    {
       [Key]
        public string sy_code { get; set; }

        public string cd_type { get; set; }
        public string code_desc { get; set; }

    }
}
