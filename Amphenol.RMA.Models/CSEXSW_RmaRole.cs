using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    [Table("CSEXSW_RmaRole")]
    public class CSEXSW_RmaRole
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public virtual ICollection<CSEXSW_RmaRoleAssignment> Assignments { get; set; } = [];
    }
}
