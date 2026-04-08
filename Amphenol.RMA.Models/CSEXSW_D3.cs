using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_d3
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Description")]
        [MaxLength(64)]
        public string descripcion { get; set; }
        [MaxLength(64)]
        public string when { get; set; }
        [MaxLength(64)]
        public string who { get; set; }
        [MaxLength(64)]
        public string status { get; set; }



        [Required]
        public int CarId { get; set; }

       
    }
}
