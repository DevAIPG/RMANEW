using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_whys
    {
        [Key]
        public int Id { get; set; }

     public string problem { get; set; }

        public string Why1drc { get; set; }
        public string Why2drc { get; set; }
        public string Why3drc { get; set; }
        public string Why4drc { get; set; }
        public string Why5drc { get; set; }
        public string Why6drc { get; set; }

        public string Why1dc { get; set; }
        public string Why2dc { get; set; }
        public string Why3dc { get; set; }
        public string Why4dc { get; set; }
        public string Why5dc { get; set; }

        public string Why6dc { get; set; }

        public string Why1sc { get; set; }

        public string Why2sc { get; set; }
        public string Why3sc { get; set; }
        public string Why4sc { get; set; }
        public string Why5sc { get; set; }
        public string Why6sc { get; set; }
        public string Coutainment { get; set; }
        public string Measurement { get; set; }
        public string Notes { get; set; }
        public bool Humanfactor { get; set; }

        [Required]
        public int CarId { get; set; }

    }
}
