using Amphenol.RMA.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace Amphenol.RMA.ViewModels
{
    public class RmaViewModel
    {

        public List<RmaLineViewModel> Lines { get; set; } = [];
        public List<RmaAttachment> Attachments { get; set; } = [];
    }
}
