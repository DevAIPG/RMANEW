using Amphenol.RMA.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amphenol.RMA.ViewModels
{
    public class RmaLineViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public int RmaRequestId { get; set; }
        public bool NoInvoice { get; set; }
        [StringLength(8)]
        public string InvoiceNumber { get; set; }
        public int? SequenceNumber { get; set; }
        public string StoredInvoiceNumber => NoInvoice ? "0" : InvoiceNumber?.Trim();
        public short GetStoredSequenceNumber() => NoInvoice ? (short)0 : checked((short)(SequenceNumber ?? 0));
        private string _partNumber;
        [Required]
        [StringLength(20)]
        public string PartNumber
        {
            get => _partNumber;
            set => _partNumber = value?.TrimEnd();
        }
        public List<SelectableStringOption> Actions { get; } = [];
        [Required]
        public string SelectedAction { get; set; }
        public List<SelectableStringOption> Locations { get; } = [];
        [Required]
        public string SelectedLocation { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Authorized Quantity must be greater than 0.")]
        public int AuthorizedQuantity { get; set; }
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Price must not be negative.")]
        public decimal Price { get; set; }
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Unit Cost must not be negative.")]
        public decimal UnitCost { get; set; }
        [Required]
        public string ReturnCode { get; set; }
        public bool GenerateCAR { get; set; }

        public RmaLineViewModel()
        {
            InitializeLookups();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NoInvoice)
            {
                if (string.IsNullOrWhiteSpace(InvoiceNumber) || InvoiceNumber.Trim() == "0")
                    yield return new ValidationResult("Select an invoice or choose No invoice.", [nameof(InvoiceNumber)]);
                if (!SequenceNumber.HasValue || SequenceNumber <= 0 || SequenceNumber > short.MaxValue)
                    yield return new ValidationResult("Select a valid invoice sequence.", [nameof(SequenceNumber)]);
            }
            if (Price < 0.01m && !(NoInvoice && Price == 0))
                yield return new ValidationResult("Price must be greater than 0 unless No invoice is selected.", [nameof(Price)]);
            if (UnitCost < 0.01m && !(NoInvoice && UnitCost == 0))
                yield return new ValidationResult("Unit Cost must be greater than 0 unless No invoice is selected.", [nameof(UnitCost)]);
            if (Price <= UnitCost && !(NoInvoice && Price == 0 && UnitCost == 0))
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
