using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models
{
    [Table("rates")]
    [Index(nameof(DateL), nameof(SourceCurrency), nameof(TargetCurrency), Name = "ratdat", IsUnique = true)]
    [Index(nameof(TargetCurrency), nameof(SourceCurrency), nameof(DateL), Name = "rattrg", IsUnique = true)]
    public partial class Rate
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("source_currency")]
        [StringLength(3)]
        public string SourceCurrency { get; set; }
        [Column("target_currency")]
        [StringLength(3)]
        public string TargetCurrency { get; set; }
        [Column("date_l", TypeName = "datetime")]
        public DateTime? DateL { get; set; }
        [Column("rate_exchange")]
        public double RateExchange { get; set; }
        [Column("rate_buy")]
        public double RateBuy { get; set; }
        [Column("rate_sell")]
        public double RateSell { get; set; }
        [Column("rate_official")]
        public double RateOfficial { get; set; }
        public short? Division { get; set; }
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
    }
}
