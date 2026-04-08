using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Amphenol.RMA.Models{
    [Keyless]
    [Table("ARALTADR_SQL")]
    public partial class AraltadrSql
    {
        [Column("cus_no")]
        [StringLength(20)]
        public string CusNo { get; set; }
        [Column("cus_alt_adr_cd")]
        [StringLength(15)]

        public string CusAltAdrCd { get; set; }
        [Column("cus_name")]
        [StringLength(50)]
        public string CusName { get; set; }
        [Column("addr_1")]
        [StringLength(100)]
        public string Addr1 { get; set; }
        [Column("addr_2")]
        [StringLength(100)]
        public string Addr2 { get; set; }
        [Column("addr_3")]
        [StringLength(100)]
        public string Addr3 { get; set; }
        [Column("city")]
        [StringLength(100)]
        public string City { get; set; }
        [Column("state")]
        [StringLength(3)]
        public string State { get; set; }
        [Column("zip")]
        [StringLength(20)]
        public string Zip { get; set; }
        [Column("country")]
        [StringLength(3)]
        public string Country { get; set; }
        [Column("ups_zone")]
        [StringLength(4)]
        public string UpsZone { get; set; }
        [Column("tax_cd")]
        [StringLength(3)]
        public string TaxCd { get; set; }
        [Column("tax_cd_2")]
        [StringLength(3)]
        public string TaxCd2 { get; set; }
        [Column("tax_cd_3")]
        [StringLength(3)]
        public string TaxCd3 { get; set; }
        [Column("tax_sched")]
        [StringLength(5)]
        public string TaxSched { get; set; }
        [Column("slspsn_no")]
        public int? SlspsnNo { get; set; }
        [Column("phone_no")]
        [StringLength(25)]
        public string PhoneNo { get; set; }
        [Column("phone_ext")]
        [StringLength(4)]
        public string PhoneExt { get; set; }
        [Column("fax_no")]
        [StringLength(25)]
        public string FaxNo { get; set; }
        [Column("ship_via_cd")]
        [StringLength(3)]
        public string ShipViaCd { get; set; }
        [Column("loc")]
        [StringLength(4)]
        public string Loc { get; set; }
        [Column("user_def_fld_1")]
        [StringLength(80)]
        public string UserDefFld1 { get; set; }
        [Column("user_def_fld_2")]
        [StringLength(80)]
        public string UserDefFld2 { get; set; }
        [Column("user_def_fld_3")]
        [StringLength(80)]
        public string UserDefFld3 { get; set; }
        [Column("user_def_fld_4")]
        [StringLength(80)]
        public string UserDefFld4 { get; set; }
        [Column("user_def_fld_5")]
        [StringLength(80)]
        public string UserDefFld5 { get; set; }
        [Column("email_address")]
        [StringLength(128)]
        public string EmailAddress { get; set; }
        [Column("url")]
        [StringLength(128)]
        public string Url { get; set; }
        [Column("contact_1")]
        [StringLength(100)]
        public string Contact1 { get; set; }
        [Column("ID")]
        public int Id { get; set; }
    }
}
