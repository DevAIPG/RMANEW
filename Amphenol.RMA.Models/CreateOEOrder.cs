using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    public class CreateOrder
    {
        public string rmaNo { get; set; }
        public string orderDate { get; set; }
        public bool  previewOnly { get; set; }
    }
}
