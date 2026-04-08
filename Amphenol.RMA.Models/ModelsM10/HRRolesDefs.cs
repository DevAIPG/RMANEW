using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models.ModelsM10
{
    public class HRRolesDefs
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public string JobGroup { get; set; }
        public Guid? Doc_id { get; set; }
        public string NTGroup { get; set; }
        public Int16? Type { get; set; }
        public Int16? Division { get; set; }

    }
}
