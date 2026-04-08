using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class OERMACTL_SQL
    {
       [Key]
       
        public decimal ID { get; set; }

        public string ctl_next_order_no { get; set; }
        
    }
}
