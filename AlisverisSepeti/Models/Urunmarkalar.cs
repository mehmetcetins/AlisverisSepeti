using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunmarkalar
    {
        public int UrunMarkaId { get; set; }
        public int MarkaId { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }
        public int? SilenId { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public string Silen { get; set; }
        public int? DizilisSira { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public bool Silindimi { get; set; }
        public bool Aktifmi { get; set; }
        public int MarkalarId { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual Markalar Marka { get; set; }
        public virtual User SilenNavigation { get; set; }
    }
}
