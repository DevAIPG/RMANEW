using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models
{
    [Table("cicmpy")]
    [Index(nameof(CmpCode), Name = "AccountCode", IsUnique = true)]
    [Index(nameof(CmpName), nameof(CmpType), nameof(CmpStatus), Name = "AccountName")]
    [Index(nameof(Crdnr), nameof(CntId), nameof(ClassificationId), nameof(CmpWwn), nameof(CmpFax), nameof(Crdcode), nameof(CmpStatus), nameof(CmpName), nameof(BankAccountNumber), nameof(PaymentMethod), nameof(CreditLine), nameof(PostBankNumber), nameof(CmpTel), Name = "Aging_IDX_1")]
    [Index(nameof(Syscreator), nameof(CmpType), nameof(CmpStatus), Name = "CreatedByStatus")]
    [Index(nameof(DivisionCreditorId), Name = "DivisionCreditor")]
    [Index(nameof(DivisionDebtorId), Name = "DivisionDebtor")]
    [Index(nameof(CmpFctry), Name = "IXcountry_cicmpy")]
    [Index(nameof(CmpParent), nameof(CmpStatus), nameof(CmpType), Name = "Parent")]
    [Index(nameof(CmpReseller), nameof(CmpStatus), nameof(CmpType), Name = "Reseller")]
    [Index(nameof(TypeSince), nameof(CmpType), nameof(Administration), nameof(CmpCode), Name = "TypeSinceAdminEx")]
    [Index(nameof(WebAccessSince), nameof(ProcessedByBackgroundJob), Name = "WebAccess")]
    [Index(nameof(Administration), nameof(CmpType), nameof(CmpStatus), nameof(CmpCode), nameof(CmpName), Name = "admin_type_status_since")]
    [Index(nameof(ClassificationId), Name = "cicmpy_ClassificationID")]
    [Index(nameof(CmpFadd1), Name = "cicmpy_cmp_fadd1")]
    [Index(nameof(CmpFpc), Name = "cicmpy_cmp_fpc")]
    [Index(nameof(CmpOrigin), Name = "cicmpy_cmp_origin")]
    [Index(nameof(Numberfield1), Name = "cicmpy_numberfield1")]
    [Index(nameof(CmpType), nameof(CmpStatus), nameof(CmpRating), nameof(TypeSince), Name = "cicmpy_rating")]
    [Index(nameof(Textfield1), Name = "cicmpy_textfield1")]
    [Index(nameof(Textfield2), Name = "cicmpy_textfield2")]
    [Index(nameof(Textfield3), Name = "cicmpy_textfield3")]
    [Index(nameof(Textfield4), Name = "cicmpy_textfield4")]
    [Index(nameof(Textfield5), Name = "cicmpy_textfield5")]
    [Index(nameof(CmpType), nameof(CmpName), Name = "cicmpy_type_name")]
    [Index(nameof(CmpType), nameof(SctCode), nameof(SizCode), nameof(CmpName), Name = "cicmpy_type_sector_size_name")]
    [Index(nameof(CmpFcity), nameof(CmpName), nameof(Id), Name = "cmpix8", IsUnique = true)]
    [Index(nameof(CmpTel), nameof(Id), Name = "cpix10", IsUnique = true)]
    [Index(nameof(CmpParent), nameof(CmpName), nameof(Id), Name = "cpix11", IsUnique = true)]
    [Index(nameof(CmpAccMan), nameof(CmpName), nameof(Id), Name = "cpix12", IsUnique = true)]
    [Index(nameof(CmpReseller), nameof(CmpName), nameof(Id), Name = "cpix17", IsUnique = true)]
    [Index(nameof(InvoiceDebtor), nameof(Administration), Name = "ix_InvoiceDebtor")]
    [Index(nameof(SecurityLevel), nameof(CmpType), nameof(CmpStatus), Name = "ix_SecurityLevel_TypeStatus")]
    [Index(nameof(VatNumber), Name = "ix_VatNumber")]
    [Index(nameof(Crdcode), nameof(Crdnr), nameof(CmpName), nameof(CmpType), nameof(CmpStatus), Name = "ix_crdcode")]
    [Index(nameof(Crdnr), Name = "ix_crdnr")]
    [Index(nameof(Debcode), nameof(Debnr), nameof(CmpName), nameof(CmpType), nameof(CmpStatus), Name = "ix_debcode")]
    [Index(nameof(Debnr), Name = "ix_debnr")]
    [Index(nameof(Timestamp), nameof(Administration), nameof(CmpType), Name = "ix_timestamp")]
    [Index(nameof(CmpType), nameof(CmpReseller), nameof(TypeSince), Name = "reseller_since")]
    public partial class Cicmpy
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("cmp_wwn")]
        public Guid CmpWwn { get; set; }
        [Column("cmp_code")]
        [StringLength(20)]
        public string CmpCode { get; set; }
        [Column("cnt_id")]
        public Guid? CntId { get; set; }
        [Column("cmp_parent")]
        public Guid? CmpParent { get; set; }
        [Column("cmp_name")]
        [StringLength(50)]
        public string CmpName { get; set; }
        [Column("cmp_fadd1")]
        [StringLength(100)]
        public string CmpFadd1 { get; set; }
        [Column("cmp_fadd2")]
        [StringLength(100)]
        public string CmpFadd2 { get; set; }
        [Column("cmp_fadd3")]
        [StringLength(100)]
        public string CmpFadd3 { get; set; }
        [Column("cmp_fpc")]
        [StringLength(20)]
        public string CmpFpc { get; set; }
        [Column("cmp_fcity")]
        [StringLength(100)]
        public string CmpFcity { get; set; }
        [Column("cmp_fcounty")]
        [StringLength(100)]
        public string CmpFcounty { get; set; }
        [StringLength(3)]
        public string StateCode { get; set; }
        [Column("cmp_fctry")]
        [StringLength(3)]
        public string CmpFctry { get; set; }
        [Column("cmp_e_mail")]
        [StringLength(128)]
        public string CmpEMail { get; set; }
        [Column("cmp_web")]
        [StringLength(128)]
        public string CmpWeb { get; set; }
        [Column("cmp_fax")]
        [StringLength(25)]
        public string CmpFax { get; set; }
        [Column("cmp_tel")]
        [StringLength(25)]
        public string CmpTel { get; set; }
        [Column("sct_code")]
        [StringLength(10)]
        public string SctCode { get; set; }
        [StringLength(10)]
        public string SubSector { get; set; }
        [Column("siz_code")]
        [StringLength(10)]
        public string SizCode { get; set; }
        [Column("cmp_date_cust", TypeName = "datetime")]
        public DateTime? CmpDateCust { get; set; }
        [Column("cmp_note", TypeName = "text")]
        public string CmpNote { get; set; }
        [Column("cmp_acc_man")]
        public int? CmpAccMan { get; set; }
        [Column("cmp_reseller")]
        public Guid? CmpReseller { get; set; }
        [Required]
        [StringLength(3)]
        public string Administration { get; set; }
        [Column("cmp_type")]
        [StringLength(1)]
        public string CmpType { get; set; }
        [Column("cmp_status")]
        [StringLength(1)]
        public string CmpStatus { get; set; }
        [Column("DivisionDebtorID")]
        public Guid? DivisionDebtorId { get; set; }
        [Column("DivisionCreditorID")]
        public Guid? DivisionCreditorId { get; set; }
        [Column("datefield1", TypeName = "datetime")]
        public DateTime? Datefield1 { get; set; }
        [Column("datefield2", TypeName = "datetime")]
        public DateTime? Datefield2 { get; set; }
        [Column("datefield3", TypeName = "datetime")]
        public DateTime? Datefield3 { get; set; }
        [Column("datefield4", TypeName = "datetime")]
        public DateTime? Datefield4 { get; set; }
        [Column("datefield5", TypeName = "datetime")]
        public DateTime? Datefield5 { get; set; }
        [Column("numberfield1")]
        public double Numberfield1 { get; set; }
        [Column("numberfield2")]
        public double Numberfield2 { get; set; }
        [Column("numberfield3")]
        public double Numberfield3 { get; set; }
        [Column("numberfield4")]
        public double Numberfield4 { get; set; }
        [Column("numberfield5")]
        public double Numberfield5 { get; set; }
        public byte YesNofield1 { get; set; }
        public byte YesNofield2 { get; set; }
        public byte YesNofield3 { get; set; }
        public byte YesNofield4 { get; set; }
        public byte YesNofield5 { get; set; }
        [Column("textfield1")]
        [StringLength(80)]
        public string Textfield1 { get; set; }
        [Column("textfield2")]
        [StringLength(80)]
        public string Textfield2 { get; set; }
        [Column("textfield3")]
        [StringLength(80)]
        public string Textfield3 { get; set; }
        [Column("textfield4")]
        [StringLength(80)]
        public string Textfield4 { get; set; }
        [Column("textfield5")]
        [StringLength(80)]
        public string Textfield5 { get; set; }
        [Column("cmp_rating")]
        public int? CmpRating { get; set; }
        [Column("cmp_origin")]
        [StringLength(3)]
        public string CmpOrigin { get; set; }
        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }
        [StringLength(128)]
        public string LogoFileName { get; set; }
        [Column("document_id")]
        public Guid? DocumentId { get; set; }
        [StringLength(3)]
        public string ClassificationId { get; set; }
        [Column("type_since", TypeName = "datetime")]
        public DateTime TypeSince { get; set; }
        [Column("status_since", TypeName = "datetime")]
        public DateTime? StatusSince { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? WebAccessSince { get; set; }
        public bool? ProcessedByBackgroundJob { get; set; }
        [Column("debnr")]
        [StringLength(6)]
        public string Debnr { get; set; }
        [Column("crdnr")]
        [StringLength(6)]
        public string Crdnr { get; set; }
        [Column("debcode")]
        [StringLength(20)]
        public string Debcode { get; set; }
        [Column("crdcode")]
        [StringLength(20)]
        public string Crdcode { get; set; }
        [Column("ASPServer")]
        [StringLength(30)]
        public string Aspserver { get; set; }
        [Column("ASPDatabase")]
        [StringLength(30)]
        public string Aspdatabase { get; set; }
        [Column("ASPWebServer")]
        [StringLength(30)]
        public string AspwebServer { get; set; }
        [Column("ASPWebSite")]
        [StringLength(128)]
        public string AspwebSite { get; set; }
        [Column("syscreated", TypeName = "datetime")]
        public DateTime Syscreated { get; set; }
        [Column("syscreator")]
        public int Syscreator { get; set; }
        [Column("sysmodified", TypeName = "datetime")]
        public DateTime Sysmodified { get; set; }
        [Column("sysmodifier")]
        public int Sysmodifier { get; set; }
        [Column("sysguid")]
        public Guid Sysguid { get; set; }
        [Required]
        [Column("timestamp")]
        public byte[] Timestamp { get; set; }
        [StringLength(6)]
        public string SearchCode { get; set; }
        [StringLength(10)]
        public string Telex { get; set; }
        [StringLength(34)]
        public string PostBankNumber { get; set; }
        [Column("NoteID")]
        public int? NoteId { get; set; }
        public byte Blocked { get; set; }
        [StringLength(1)]
        public string LayoutCode { get; set; }
        public double BalanceDebit { get; set; }
        public double BalanceCredit { get; set; }
        public double SalesOrderAmount { get; set; }
        public short PageNumber { get; set; }
        public double AmountOpen { get; set; }
        public byte Factoring { get; set; }
        [Column("ISOCountry")]
        [StringLength(3)]
        public string Isocountry { get; set; }
        [Column("LiableToPayVAT")]
        public byte LiableToPayVat { get; set; }
        public byte BackOrders { get; set; }
        [StringLength(8)]
        public string CostCenter { get; set; }
        [StringLength(10)]
        public string AddressNumber { get; set; }
        [StringLength(6)]
        public string DeliveryAddress { get; set; }
        [StringLength(2)]
        public string RouteCode { get; set; }
        public double InvoiceDiscount { get; set; }
        [StringLength(10)]
        public string PaymentConditionSearchCode { get; set; }
        [StringLength(10)]
        public string SearchCodeGoods { get; set; }
        [StringLength(1)]
        public string ExpenseCode { get; set; }
        [Column("ICONumber")]
        [StringLength(8)]
        public string Iconumber { get; set; }
        [StringLength(34)]
        public string BankNumber2 { get; set; }
        [StringLength(30)]
        public string Area { get; set; }
        [StringLength(8)]
        public string InvoiceLayout { get; set; }
        [StringLength(75)]
        public string InvoiceName { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public double InvoiceThreshold { get; set; }
        [StringLength(10)]
        public string Location { get; set; }
        [Column("VATSource")]
        [StringLength(1)]
        public string Vatsource { get; set; }
        public byte CalculatePenaltyInterest { get; set; }
        [StringLength(30)]
        public string IntermediaryAssociate { get; set; }
        public byte SendPenaltyInvoices { get; set; }
        [StringLength(9)]
        public string CentralizationAccount { get; set; }
        [StringLength(3)]
        public string Currency { get; set; }
        [StringLength(34)]
        public string BankAccountNumber { get; set; }
        [StringLength(1)]
        public string PaymentMethod { get; set; }
        [StringLength(6)]
        public string InvoiceDebtor { get; set; }
        public double CreditLine { get; set; }
        public double Discount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateLastReminder { get; set; }
        [StringLength(20)]
        public string VatNumber { get; set; }
        [StringLength(1)]
        public string PurchaseReceipt { get; set; }
        [StringLength(9)]
        public string OffSetAccount { get; set; }
        [StringLength(3)]
        public string JournalCode { get; set; }
        public byte Reminder { get; set; }
        [StringLength(2)]
        public string PaymentCondition { get; set; }
        [StringLength(15)]
        public string PriceList { get; set; }
        [StringLength(2)]
        public string ItemCode { get; set; }
        [StringLength(3)]
        public string DeliveryMethod { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckDate { get; set; }
        [StringLength(3)]
        public string StatFactor { get; set; }
        [StringLength(3)]
        public string VatCode { get; set; }
        public byte ChangeVatCode { get; set; }
        [StringLength(10)]
        public string IntrastatSystem { get; set; }
        public byte ChangeIntraStatSystem { get; set; }
        [StringLength(10)]
        public string TransActionA { get; set; }
        public byte ChangeTransActionA { get; set; }
        [StringLength(10)]
        public string TransActionB { get; set; }
        public byte ChangeTransActionB { get; set; }
        [StringLength(3)]
        public string DestinationCountry { get; set; }
        public byte ChangeDestinationCountry { get; set; }
        [StringLength(10)]
        public string Transport { get; set; }
        public byte ChangeTransport { get; set; }
        [StringLength(10)]
        public string CityOfLoadUnload { get; set; }
        public byte ChangeCityOfLoadUnload { get; set; }
        [StringLength(10)]
        public string DeliveryTerms { get; set; }
        public byte ChangeDeliveryTerms { get; set; }
        [StringLength(10)]
        public string TransShipment { get; set; }
        public byte ChangeTransShipment { get; set; }
        [StringLength(3)]
        public string TriangularCountry { get; set; }
        public byte ChangeTriangularCountry { get; set; }
        [StringLength(10)]
        public string IntRegion { get; set; }
        public byte ChangeIntRegion { get; set; }
        [StringLength(1)]
        public string Collect { get; set; }
        public short InvoiceCopies { get; set; }
        public short PaymentDay1 { get; set; }
        public short PaymentDay2 { get; set; }
        public short PaymentDay3 { get; set; }
        public short PaymentDay4 { get; set; }
        public short PaymentDay5 { get; set; }
        [StringLength(30)]
        public string FiscalCode { get; set; }
        [StringLength(34)]
        public string CreditCard { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }
        [StringLength(80)]
        public string TextField6 { get; set; }
        [StringLength(80)]
        public string TextField7 { get; set; }
        [StringLength(80)]
        public string TextField8 { get; set; }
        [StringLength(80)]
        public string TextField9 { get; set; }
        [StringLength(80)]
        public string TextField10 { get; set; }
        public int AccountEmployeeId { get; set; }
        [StringLength(1)]
        public string CreditabilityScenario { get; set; }
        [StringLength(1)]
        public string VatLiability { get; set; }
        public byte Attention { get; set; }
        [StringLength(2)]
        public string Category { get; set; }
        [StringLength(10)]
        public string StatementAddress { get; set; }
        public byte StatementPrint { get; set; }
        public short StatementNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StatementDate { get; set; }
        [StringLength(2)]
        public string DefaultSelCode { get; set; }
        [StringLength(1)]
        public string GroupPayments { get; set; }
        [Column("BOELimitAmount")]
        public double BoelimitAmount { get; set; }
        [Column("BOEMaxAmount")]
        public double BoemaxAmount { get; set; }
        public byte ExtraDuty { get; set; }
        [Column("RegionCD")]
        [StringLength(2)]
        public string RegionCd { get; set; }
        [Column("region")]
        [StringLength(6)]
        public string Region { get; set; }
        [Column("IntermediaryAssociateID")]
        [StringLength(10)]
        public string IntermediaryAssociateId { get; set; }
        public short CompanyType { get; set; }
        public int? SalesPersonNumber { get; set; }
        [StringLength(5)]
        public string AccountTypeCode { get; set; }
        [StringLength(1)]
        public string StatementFrequency { get; set; }
        [StringLength(4)]
        public string AccountRating { get; set; }
        public byte FinanceCharge { get; set; }
        [StringLength(12)]
        public string ParentAccount { get; set; }
        public byte IsParentAccount { get; set; }
        [StringLength(3)]
        public string ShipVia { get; set; }
        [Column("UPSZone")]
        [StringLength(4)]
        public string Upszone { get; set; }
        [StringLength(2)]
        public string Terms { get; set; }
        public byte IsTaxable { get; set; }
        [StringLength(3)]
        public string TaxCode { get; set; }
        [StringLength(3)]
        public string TaxCode2 { get; set; }
        [StringLength(3)]
        public string TaxCode3 { get; set; }
        [StringLength(20)]
        public string ExemptNumber { get; set; }
        public byte AllowSubstituteItem { get; set; }
        public byte AllowBackOrders { get; set; }
        public byte AllowPartialShipment { get; set; }
        public byte DunningLetter { get; set; }
        [StringLength(30)]
        public string Comment1 { get; set; }
        [StringLength(30)]
        public string Comment2 { get; set; }
        [StringLength(5)]
        public string TaxSchedule { get; set; }
        [StringLength(15)]
        public string CreditCardDescription { get; set; }
        [StringLength(40)]
        public string CreditCardHolder { get; set; }
        public byte? DefaultInvoiceForm { get; set; }
        [StringLength(3)]
        public string LocationShort { get; set; }
        [Column("TaxID")]
        [StringLength(13)]
        public string TaxId { get; set; }
        public byte BillParent { get; set; }
        [Column(TypeName = "decimal(16, 2)")]
        public decimal? Balance { get; set; }
        [StringLength(4)]
        public string PhoneExtention { get; set; }
        public byte AutomaticPayment { get; set; }
        [StringLength(10)]
        public string CustomerCode { get; set; }
        public double PurchaseOrderAmount { get; set; }
        [StringLength(3)]
        public string CountryOfOrigin { get; set; }
        public byte ChangeCountryOfOrigin { get; set; }
        [StringLength(3)]
        public string CountryOfAssembly { get; set; }
        public byte ChangeCountryOfAssembly { get; set; }
        public byte PurchaseOrderConfirmation { get; set; }
        public byte RecepientOfCommissions { get; set; }
        [StringLength(1)]
        public string CreditorType { get; set; }
        public double PercentageToBePaid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SignDate { get; set; }
        [StringLength(3)]
        public string ImportOriginCode { get; set; }
        [StringLength(9)]
        public string ParticipantNumber { get; set; }
        public byte CertifiedSupplier { get; set; }
        [Column("FederalIDNumber")]
        public int? FederalIdnumber { get; set; }
        [Column("FederalIDType")]
        [StringLength(1)]
        public string FederalIdtype { get; set; }
        [StringLength(1)]
        public string FederalCategory { get; set; }
        public byte AutoDistribute { get; set; }
        [StringLength(1)]
        public string SupplierStatus { get; set; }
        [Column("FOBCode")]
        [StringLength(3)]
        public string Fobcode { get; set; }
        public byte PrintPrice { get; set; }
        public byte Acknowledge { get; set; }
        public byte Confirmed { get; set; }
        [StringLength(4)]
        public string LandedCostCode { get; set; }
        [StringLength(4)]
        public string LandedCostCode2 { get; set; }
        [StringLength(4)]
        public string LandedCostCode3 { get; set; }
        [StringLength(4)]
        public string LandedCostCode4 { get; set; }
        [StringLength(4)]
        public string LandedCostCode5 { get; set; }
        [StringLength(4)]
        public string LandedCostCode6 { get; set; }
        [StringLength(4)]
        public string LandedCostCode7 { get; set; }
        [StringLength(4)]
        public string LandedCostCode8 { get; set; }
        [StringLength(4)]
        public string LandedCostCode9 { get; set; }
        [StringLength(4)]
        public string LandedCostCode10 { get; set; }
        [Column("DefaultPOForm")]
        public byte? DefaultPoform { get; set; }
        [StringLength(40)]
        public string PayeeName { get; set; }
        [StringLength(4)]
        public string CommodityCode1 { get; set; }
        [StringLength(4)]
        public string CommodityCode2 { get; set; }
        [StringLength(4)]
        public string CommodityCode3 { get; set; }
        [StringLength(4)]
        public string CommodityCode4 { get; set; }
        [StringLength(4)]
        public string CommodityCode5 { get; set; }
        public int SecurityLevel { get; set; }
        [StringLength(20)]
        public string ChamberOfCommerce { get; set; }
        [StringLength(9)]
        public string DunsNumber { get; set; }
        [StringLength(80)]
        public string TextField11 { get; set; }
        [StringLength(80)]
        public string TextField12 { get; set; }
        [StringLength(80)]
        public string TextField13 { get; set; }
        [StringLength(80)]
        public string TextField14 { get; set; }
        [StringLength(80)]
        public string TextField15 { get; set; }
        [StringLength(80)]
        public string TextField16 { get; set; }
        [StringLength(80)]
        public string TextField17 { get; set; }
        [StringLength(80)]
        public string TextField18 { get; set; }
        [StringLength(80)]
        public string TextField19 { get; set; }
        [StringLength(80)]
        public string TextField20 { get; set; }
        [StringLength(80)]
        public string TextField21 { get; set; }
        [StringLength(80)]
        public string TextField22 { get; set; }
        [StringLength(80)]
        public string TextField23 { get; set; }
        [StringLength(80)]
        public string TextField24 { get; set; }
        [StringLength(80)]
        public string TextField25 { get; set; }
        [StringLength(80)]
        public string TextField26 { get; set; }
        [StringLength(80)]
        public string TextField27 { get; set; }
        [StringLength(80)]
        public string TextField28 { get; set; }
        [StringLength(80)]
        public string TextField29 { get; set; }
        [StringLength(80)]
        public string TextField30 { get; set; }
        [StringLength(15)]
        public string PhoneQueue { get; set; }
        [Column("cmp_Directory")]
        [StringLength(30)]
        public string CmpDirectory { get; set; }
        [Column("GUIDField1")]
        public Guid? Guidfield1 { get; set; }
        [Column("GUIDField2")]
        public Guid? Guidfield2 { get; set; }
        [Column("GUIDField3")]
        public Guid? Guidfield3 { get; set; }
        [Column("GUIDField4")]
        public Guid? Guidfield4 { get; set; }
        [Column("GUIDField5")]
        public Guid? Guidfield5 { get; set; }
        public int? NumIntField1 { get; set; }
        public int? NumIntField2 { get; set; }
        public int? NumIntField3 { get; set; }
        public int? NumIntField4 { get; set; }
        public int? NumIntField5 { get; set; }
        [StringLength(30)]
        public string RuleItem { get; set; }
        public bool? Substitute { get; set; }
        public bool? AutoAllocate { get; set; }
        [Column("FedIDNumber")]
        [StringLength(9)]
        public string FedIdnumber { get; set; }
        [Column("FedIDType")]
        [StringLength(1)]
        public string FedIdtype { get; set; }
        [StringLength(4)]
        public string FedCategory { get; set; }
        public short? Division { get; set; }
        [Column("Category_01")]
        [StringLength(30)]
        public string Category01 { get; set; }
        [Column("Category_02")]
        [StringLength(30)]
        public string Category02 { get; set; }
        [Column("Category_03")]
        [StringLength(30)]
        public string Category03 { get; set; }
        [Column("Category_04")]
        [StringLength(30)]
        public string Category04 { get; set; }
        [Column("Category_05")]
        [StringLength(30)]
        public string Category05 { get; set; }
        [Column("Category_06")]
        [StringLength(30)]
        public string Category06 { get; set; }
        [Column("Category_07")]
        [StringLength(30)]
        public string Category07 { get; set; }
        [Column("Category_08")]
        [StringLength(30)]
        public string Category08 { get; set; }
        [Column("Category_09")]
        [StringLength(30)]
        public string Category09 { get; set; }
        [Column("Category_10")]
        [StringLength(30)]
        public string Category10 { get; set; }
        [Column("Category_11")]
        [StringLength(30)]
        public string Category11 { get; set; }
        [Column("Category_12")]
        [StringLength(30)]
        public string Category12 { get; set; }
        [Column("Category_13")]
        [StringLength(30)]
        public string Category13 { get; set; }
        [Column("Category_14")]
        [StringLength(30)]
        public string Category14 { get; set; }
        [Column("Category_15")]
        [StringLength(30)]
        public string Category15 { get; set; }
        [MaxLength(34)]
        public byte[] EncryptedCreditCard { get; set; }
        [StringLength(15)]
        public string DefaultDeliveryAddress { get; set; }
        public bool? ExcludeMerging { get; set; }
    }
}
