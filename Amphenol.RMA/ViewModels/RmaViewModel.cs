using Amphenol.RMA.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Amphenol.RMA.ViewModels
{
    public class RmaViewModel
    {
        private const decimal PriceThreshold = 500m;
        //General Info
        [Range(1, int.MaxValue, ErrorMessage = "Request Id must be greater than 0.")]
        public int RequestId { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public List<string> BuildLocations { get; } = [];
        [Required]
        public string SelectedBuildLocation { get; set; }
        public decimal TotalValue { get; set; } = 0m;
        public List<string> RequestTypes { get; } = [];
        [Required]
        public string SelectedRequestType { get; set; }
        public List<SelectableStringOption> Reasons { get; } = [];
        [Required]
        public string SelectedReason { get; set; }
        public string Description { get; set; }
        [Required]
        public string Complaint { get; set; }
        public bool HasLineOverThreshold => Lines?.Any(x => x.Price > PriceThreshold) ?? false;

        //Customer info
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string PoNumber { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string ExtensionNumber { get; set; }
        public string Fax { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [EmailAddress]
        public string CompanyEmail { get; set; }
        [Required]
        public string ShipTo { get; set; }


        //RMA lines
        public List<RmaLineViewModel> Lines { get; set; } = [];


        //RMA Attatchments
        public List<RmaAttachmentViewModel> Attatchments { get; set; } = [];
        public List<IFormFile> UploadedFiles { get; set; } = [];
        public RmaViewModel()
        {
            InitializeLookups();
        }
        public RmaViewModel(int? requestId = null)
        {
            RequestId = requestId is not null ? (int)requestId : 0;
            InitializeLookups();
        }
        public void InitializeLookups()
        {
            BuildLocations.Clear();
            BuildLocations.AddRange(
            [
                "Nogales",
                "Endicott",
                "Mesa"
            ]);

            RequestTypes.Clear();
            RequestTypes.AddRange(
            [
                "VALUE ADD RMA",
                "CREDIT ONLY",
                "DISTY SCRAP ALLOWANCE"
            ]);

            Reasons.Clear();
            Reasons.AddRange(
            [
                new SelectableStringOption{ DisplayValue = "Billing Error", SelectedValue = "1" },
                new SelectableStringOption{ DisplayValue = "Buyback", SelectedValue = "2" },
                new SelectableStringOption{ DisplayValue = "Price Adjustment", SelectedValue = "3" },
                new SelectableStringOption{ DisplayValue = "Price Protection", SelectedValue = "4" },
                new SelectableStringOption{ DisplayValue = "Quality Issues - NG", SelectedValue = "5" },
                new SelectableStringOption{ DisplayValue = "Quality Issues - END", SelectedValue = "6" },
                new SelectableStringOption{ DisplayValue = "Shipping Error", SelectedValue = "7" },
                new SelectableStringOption{ DisplayValue = "Contractual Return/Stock", SelectedValue = "8" },
                new SelectableStringOption{ DisplayValue = "Ship and Debits", SelectedValue = "9" },
                new SelectableStringOption{ DisplayValue = "Scrap Allowance", SelectedValue = "10" },
                new SelectableStringOption{ DisplayValue = "Marcomco-op", SelectedValue = "11" },
            ]);
        }
    }
}
