using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_documentos
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a name for the filter")]
        [Display(Name = "Filter Name")]

        [MaxLength(128)]
        public string nombredocumento { get; set; }
        [Required]
        public int CarId { get; set; }

    }
}
