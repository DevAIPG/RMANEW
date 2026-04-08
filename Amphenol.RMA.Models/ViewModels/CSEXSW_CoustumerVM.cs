
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amphenol.RMA.Models.ViewModels
{
    public class csexsw_coustumerVM
    {
        public CSEXSW_Rma CSEXSW_Rma { get; set; }
        public csexsw_coustumer csexsw_costumber { get; set; }
        public IEnumerable<csexsw_coustumer> Lista { get; set; }
        public IEnumerable<CSEXSW_Attachmentrma> Lista2 { get; set; }

    }
}
