using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models.ViewModels100
{
    public class SquenceViewModel
    {
        public decimal ID { get; set; }
        public string ord_type { get; set; }
        public string ord_no { get; set; }
        public string inv_no { get; set; }
        public Int16 line_seq_no { get; set; }
        public string item_no { get; set; }
    }
}
