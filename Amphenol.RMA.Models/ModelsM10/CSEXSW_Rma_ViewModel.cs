using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models.ModelsM10
{
    public class CSEXSW_Rma_ViewModel
    {
        public int Id { get; set; }
        public int qty { get; set; }
        public int parts { get; set; }
        public string Rmarequest { get; set; }
        public string Date { get; set; }
        public string Customerpartno { get; set; }
        public string Customerpo { get; set; }
        public string Customercomplait { get; set; }
        public string Description { get; set; }
        public string Rmatypeofrequest { get; set; }
        public double Totalrmavalues { get; set; }
        public string Wherebuilt { get; set; }
        public string Preparado { get; set; }
        public string Sumbit { get; set; }
        public string Status { get; set; }
        public string turno { get; set; }
        public string Approver { get; set; }
        public int res_id { get; set; }
        public string formated_date_approved { get; set; }
        public bool CanGenerateOrder { get; set; }
    }
}
