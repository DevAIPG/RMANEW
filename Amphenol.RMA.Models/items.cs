using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amphenol.RMA.Models
{
   public class items
    {
        [Key]
        public int ID { get; set; }

        public string itemcode { get; set; }
        public string textdescription { get; set; }
        
    }
}
