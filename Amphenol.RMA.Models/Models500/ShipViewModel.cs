using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphenol.RMA.Models.Models500
{
    public  class ShipViewModel
    {
        public int ID { get; set; }
        public string cus_alt_adr_cd { get; set; }
        public string user_def_fld_1 { get; set; }
        public string cmp_e_mail { get; set; }
        public string email_address { get; set; }
        public string phone_ext { get; set; }
        public string fax_no { get; set; }
        public string phone_no { get; set; }
        public string contact_1 { get; set; }
    }
}
