using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Amphenol.RMA.Models
{
    public class csexsw_bomexcel

    {



        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Item_no { get; set; }
        [MaxLength(30)]
        public string Componente { get; set; }
        public double Sequence { get; set; }
        public double Quantify { get; set; }
        public double Scrap_quantify { get; set; }
        public double Shrink { get; set; }
        [MaxLength(30)]
        public string Componente_loc { get; set; }
        [MaxLength(30)]
        public string Manuf { get; set; }
        public int Attach { get; set; }
        [MaxLength(30)]
        public string Activity { get; set; }
        public DateTime Effective_date { get; set; }
        public DateTime Obsolete_date { get; set; }
        public string Note { get; set; }
        [MaxLength(30)]
        public string Bulkk_issue { get; set; }
        [MaxLength(30)]
        public string Bulkk_flush { get; set; }
        [MaxLength(128)]
        public string Excelid { get; set; }


    }
}
