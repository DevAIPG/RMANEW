using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models
{
    [Table("CSEXSW_RmaRoleAssignment")]
    public class CSEXSW_RmaRoleAssignment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RmaRoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Location { get; set; }

        public int? EmployeeNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string DisplayName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(RmaRoleId))]
        public virtual CSEXSW_RmaRole RmaRole { get; set; }
    }
}
