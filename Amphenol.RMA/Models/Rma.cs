using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace Amphenol.RMA.Models
{
    public class Rma
    {
        public CSEXSW_Rma Details { get; set; }
        public List<csexsw_coustumer> ItemLines { get; set; } = [];
        public List<CSEXSW_Attachmentrma> Attachments { get; set; } = [];
    }
}
