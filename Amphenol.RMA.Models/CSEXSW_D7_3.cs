using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_d7_3
    {
        [Key]
       
        public int Id { get; set; }

        [MaxLength(64)]

        public string name { get; set; }
        [MaxLength(64)]
        public string funtion { get; set; }

        [Required]
        public int CarId { get; set; }



    }
}
