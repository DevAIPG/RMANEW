using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_d7_1
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Verification Method")]
        [MaxLength(64)]
        public string descripcion { get; set; }
        [Display(Name = "EFFECTIVE DATE")]
        [MaxLength(64)]
        public string when { get; set; }
        [Display(Name = "RESPONSIBILITY")]
        [MaxLength(64)]
        public string who { get; set; }
        [Display(Name = "STATUS")]
        [MaxLength(64)]
        public string status { get; set; }
      
        [Required]
        public int CarId { get; set; }

      
    }
}
