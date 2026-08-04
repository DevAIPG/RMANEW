using Amphenol.RMA.Models;
using System.Collections.Generic;

namespace Amphenol.RMA.ViewModels
{
    public class RmaLineViewModel
    {
        public int RmaRequestId { get; set; }
        public string InvoiceNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string PartNumber { get; set; }
        public List<SelectableStringOption> Actions { get; } = [];
        public string SelectedAction { get; set; }
        public List<SelectableStringOption> Locations { get; } = [];
        public string SelectedLocation { get; set; }
        public int AuthorizedQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitCost { get; set; }
        public string ReturnCode { get; set; }
        public bool GenerateCAR { get; set; }

        public RmaLineViewModel()
        {
            Actions.AddRange([
                new SelectableStringOption{ DisplayValue = "CREDIT ONLY", SelectedValue = "C" }
            ]);

            Locations.AddRange([
                new SelectableStringOption{ DisplayValue = "MRM - Mesa RMA", SelectedValue = "MRM" },
                new SelectableStringOption{ DisplayValue = "NRM - Nogales RMA", SelectedValue = "NRM" },
                new SelectableStringOption{ DisplayValue = "ERM - Endicott RMA", SelectedValue = "ERM" },
            ]);
        }
    }
}
