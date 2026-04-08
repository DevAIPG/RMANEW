using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class BacoDiscussions
    {
        [Key] 
        public Guid ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int HID { get; set; }
        public Guid? ParentID { get; set; }
        public int GroupID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ViewDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByFullName { get; set; }
        public string Subject { get; set; }
        public string Class_02_1 { get; set; }
        public string Class_02_2 { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int Type { get; set; }
        public string FileName { get; set; }
        public string Body { get; set; }
        public byte[] Document { get; set; }
        public string Company { get; set; }
        public int JobLevel { get; set; }
        public int Status { get; set; }
        public string ItemCode { get; set; }
        public int? HumResID{ get; set; }
        public Guid? CmpWwn { get; set; }
        public int? ModifiedBy { get; set; }
        public byte NewsType { get; set; }
        public string LanguageID { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsMailMerge { get; set; }
        public string StatusText { get; set; }
        public string ProjectNr { get; set; }
        public Guid? EntryKey { get; set; }
        public int? Owner { get; set; }
        public Int16? OwnerType  { get; set; }
        public string OwnerTypeValue { get; set; }
        public int? VoteCount { get; set; }
        public int? VoteSum { get; set; }
        public Guid? ItemNumberID { get; set; }
        public string FPIntroText { get; set; }
        public int? Assortment { get; set; }
        public Guid? cnt_id { get; set; } 
        public DateTime? ExpiryDate { get; set; }
        public string OrderNumber { get; set; }
        public string ShipmentMethod { get; set; }
        public string OurRef { get; set; }
        public string YourRef { get; set; }
        public string PaymentReference { get; set; }
        public string Warehouse { get; set; }
        public string Source { get; set; }
        public string Note { get; set; }
        public string Version { get; set; }
        public DateTime? CheckedOut { get; set; }
        public int? CheckedOutBy  { get; set; }
        public Guid? ReportID { get; set; }
        public int? OpportunityID { get; set; }
        public string AttachmentType { get; set; }
        public Int16? Division { get; set; }
      
        [Timestamp]

        public byte[] timestamp { get; set; } 
        public byte[] CompressBody { get; set; }
        public int OwnerTypeRoleLevel { get; set; }
        public string OwnerTypeRoleRefPoint { get; set; }
        public int? ViewRightsOwner { get; set; }
        public Int16? ViewRightsType { get; set; }
        public string ViewRightsTypeValue { get; set; }
        public int? ViewRightTypeRoleLevel { get; set; }
        public string ViewRightTypeRoleRefPoint { get; set; }
        public int? LayoutReference { get; set; }
        public Guid? SMSContractID { get; set; }
        public Guid? SMSConfigurationID { get; set; }
    }
}
