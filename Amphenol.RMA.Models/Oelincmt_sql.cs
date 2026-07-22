using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    [Table("Oelincmt_sql")]
    public class Oelincmt_sql
    {

        [Required]
        [Column("ord_type", TypeName = "char(1)")]
        public string OrdType { get; set; }

        [Required]
        [Column("ord_no", TypeName = "char(8)")]
        public string OrdNo { get; set; }

        [Required]
        [Column("line_seq_no")]
        public short LineSeqNo { get; set; }

        [Required]
        [Column("lvl_no", TypeName = "char(1)")]
        public string LvlNo { get; set; }

        [Required]
        [Column("cmt_type", TypeName = "char(1)")]
        public string CmtType { get; set; }

        [Required]
        [Column("cmt_seq_no")]
        public short CmtSeqNo { get; set; }

        [Column("cmt")]
        public string Cmt { get; set; }

        [Column("cmt_doc_type", TypeName = "char(1)")]
        public string CmtDocType { get; set; }

        [Column("extra_1", TypeName = "char(1)")]
        public string Extra1 { get; set; }

        [Column("extra_2", TypeName = "char(1)")]
        public string Extra2 { get; set; }

        [Column("extra_3", TypeName = "char(1)")]
        public string Extra3 { get; set; }

        [Column("extra_4", TypeName = "char(1)")]
        public string Extra4 { get; set; }

        [Column("extra_5", TypeName = "char(1)")]
        public string Extra5 { get; set; }

        [Column("extra_6", TypeName = "char(8)")]
        public string Extra6 { get; set; }

        [Column("extra_7", TypeName = "char(8)")]
        public string Extra7 { get; set; }

        [Column("extra_8", TypeName = "char(12)")]
        public string Extra8 { get; set; }

        [Column("extra_9", TypeName = "char(12)")]
        public string Extra9 { get; set; }

        [Column("extra_10", TypeName = "decimal(16,6)")]
        public decimal? Extra10 { get; set; }

        [Column("extra_11", TypeName = "decimal(16,6)")]
        public decimal? Extra11 { get; set; }

        [Column("extra_12", TypeName = "decimal(16,2)")]
        public decimal? Extra12 { get; set; }

        [Column("extra_13", TypeName = "decimal(16,2)")]
        public decimal? Extra13 { get; set; }

        [Column("extra_14")]
        public int? Extra14 { get; set; }

        [Column("extra_15")]
        public int? Extra15 { get; set; }

        [Column("is_ext", TypeName = "char(1)")]
        public string IsExt { get; set; }

        [Column("filler_0001", TypeName = "char(149)")]
        public string Filler0001 { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public decimal Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
