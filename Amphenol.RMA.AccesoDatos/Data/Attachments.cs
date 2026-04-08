using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class Attachments
    {
        [Key]
        public Guid ID { get; set; }
        public Guid  Entity { get; set; }
        public int EntityType { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileExtension { get; set; }
        public string VersionID { get; set; }
        public int? AttachmentSize { get; set; }
        public Int16? Division { get; set; }
    }
}
