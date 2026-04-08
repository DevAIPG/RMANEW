using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    public class CSBOMAttachments
    {
        [Key]
        public int Id { get; set; }
        public string filename { get; set; }
        public bool isbom { get; set; }
        public int resid { get; set; }
        public string fullname { get; set; }
        public int tareaid { get; set; }

    }
}
