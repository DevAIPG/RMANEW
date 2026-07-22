using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    [Table("oeordhdr_sql")]
    public partial class Oeordhdr_sql
    {

        [Key]
        [Column("ID")]
        public decimal Id { get; set; }

        [Required]
        [Column("ord_type")]
        [StringLength(1)]
        public string OrdType { get; set; }

        [Required]
        [Column("ord_no")]
        [StringLength(8)]
        public string OrdNo { get; set; }

        [Required]
        [Column("status")]
        [StringLength(1)]
        public string Status { get; set; }

        [Column("entered_dt")]
        public DateTime? EnteredDate { get; set; }

        [Column("ord_dt")]
        public DateTime? OrderDate { get; set; }

        [Column("apply_to_no")]
        [StringLength(8)]
        public string? ApplyToNo { get; set; }

        [Required]
        [Column("oe_po_no")]
        [StringLength(25)]
        public string OePoNo { get; set; }

        [Required]
        [Column("cus_no")]
        [StringLength(20)]
        public string CustomerNumber { get; set; }

        [Column("tot_sls_amt", TypeName = "decimal(16,2)")]
        public decimal? TotalSalesAmount { get; set; }

        [Column("tot_tax_amt", TypeName = "decimal(16,2)")]
        public decimal? TotalTaxAmount { get; set; }

        [Column("tot_cost", TypeName = "decimal(16,2)")]
        public decimal? TotalCost { get; set; }

        [Column("shipping_dt")]
        public DateTime? ShippingDate { get; set; }

        [Column("ord_dt_shipped")]
        public DateTime? OrderDateShipped { get; set; }

        [Column("inv_no")]
        [StringLength(8)]
        public string InvoiceNumber { get; set; }

        [Column("inv_dt")]
        public DateTime? InvoiceDate { get; set; }

        [Column("bill_to_name")]
        [StringLength(40)]
        public string? BillToName { get; set; }

        [Column("ship_to_name")]
        [StringLength(40)]
        public string? ShipToName { get; set; }

        [Column("rma_no")]
        [StringLength(8)]
        public string RmaNo { get; set; }

        [Required]
        [Column("oe_cash_no")]
        [StringLength(8)]
        public string OeCashNo { get; set; }

        [Required]
        [Column("curr_cd")]
        [StringLength(3)]
        public string CurrencyCode { get; set; }

        [Required]
        [Column("Uid")]
        public Guid Uid { get; set; }

        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersion { get; set; }

    }
}
