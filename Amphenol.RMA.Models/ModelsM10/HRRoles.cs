using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models.ModelsM10
{
    public class HRRoles
    {
        [Key]
        public Guid ID { get; set; }
    
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual HRRolesDefs HRRoleDefs { get; set; } 
        public int EmpID { get; set; }
        [ForeignKey("EmpID")]
        public virtual humres humres { get; set; }

        public int RoleLevel { get; set; }
        public Int16? Division { get; set; }

    }
}
