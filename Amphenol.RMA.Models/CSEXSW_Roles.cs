using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class CSEXSW_Roles
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a name for the filter")]
        [Display(Name = "Filter Name")]
        [MaxLength(64)]
        public string fullname { get; set; }
        [MaxLength(64)]

        [Display(Name = "Rol Name")]
        public string rol { get; set; }
    }
}
