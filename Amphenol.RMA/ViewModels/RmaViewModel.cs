using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
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
        public decimal TotalValue => Lines?.Sum(x => x.AuthorizedQuantity * x.Price) ?? 0m;
        public List<string> RequestTypes { get; } = [];
        [Required]
        public string SelectedRequestType { get; set; }
        public List<string> Reasons { get; } = [];
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
        [Phone]
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
        public RmaSubmitStatus SubmitStatus { get; set; }
        public RmaRequestStatus RequestStatus { get; set; }
        public bool CanSubmit => SubmitStatus == RmaSubmitStatus.NotSubmitted;
        public bool CanResubmit => SubmitStatus == RmaSubmitStatus.Submitted && (
            RequestStatus == RmaRequestStatus.Remark || RequestStatus == RmaRequestStatus.Rejected);

        public string Comments { get; set; }

        //RMA lines
        public List<RmaLineViewModel> Lines { get; set; } = [];


        //RMA Attatchments
        public List<RmaAttachmentViewModel> Attachments { get; set; } = [];
        public List<IFormFile> UploadedFiles { get; set; } = [];



        public string ReturnUrl { get; set; }
        public string ReturnPageTitle { get; set; } = "RMA Requests";

        public RmaViewModel()
        {
            InitializeLookups();
        }
        public RmaViewModel(int? requestId = null)
        {
            RequestId = int.Parse($"8{(requestId ?? 0):0000}");

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
                "Billing Error",
                "Buyback",
                "Price Adjustment",
                "Price Protection",
                "Quality Issues - NG",
                "Quality Issues - END",
                "Shipping Error",
                "Contractual Return/Stock",
                "Ship and Debits",
                "Scrap Allowance",
                "Marcomco-op",
            ]);
        }
    }
}
