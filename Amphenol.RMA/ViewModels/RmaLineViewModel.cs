using Amphenol.RMA.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amphenol.RMA.ViewModels
{
    public class RmaLineViewModel : IValidatableObject
    {
        [Required]
        public int RmaRequestId { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        public int SequenceNumber { get; set; }
        [Required]
        public string PartNumber { get; set; }
        public List<SelectableStringOption> Actions { get; } = [];
        [Required]
        public string SelectedAction { get; set; }
        public List<SelectableStringOption> Locations { get; } = [];
        [Required]
        public string SelectedLocation { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Authorized Quantity must be greater than 0.")]
        public int AuthorizedQuantity { get; set; }
        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "Unit Cost must be greater than 0.")]
        public decimal UnitCost { get; set; }
        public string ReturnCode { get; set; }
        public bool GenerateCAR { get; set; }

        public RmaLineViewModel()
        {
            InitializeLookups();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price <= UnitCost)
            {
                yield return new ValidationResult("Price must be greater than Unit Cost.", [nameof(Price)]);
            }
        }
        public void InitializeLookups()
        {
            Actions.Clear();
            Actions.AddRange([
                new SelectableStringOption{ DisplayValue = "CREDIT ONLY", SelectedValue = "C" }
            ]);

            Locations.Clear();
            Locations.AddRange([
                new SelectableStringOption{ DisplayValue = "MRM - Mesa RMA", SelectedValue = "MRM" },
                new SelectableStringOption{ DisplayValue = "NRM - Nogales RMA", SelectedValue = "NRM" },
                new SelectableStringOption{ DisplayValue = "ERM - Endicott RMA", SelectedValue = "ERM" },
            ]);
        }
    }
}
