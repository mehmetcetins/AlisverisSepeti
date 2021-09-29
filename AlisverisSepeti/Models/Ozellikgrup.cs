using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Ozellikgrup
    {
        public Ozellikgrup()
        {
            OzellikgrupDils = new HashSet<OzellikgrupDil>();
        }

        public int OzellikGrupId { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public int? DizilisSira { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual ICollection<OzellikgrupDil> OzellikgrupDils { get; set; }
    }
}
