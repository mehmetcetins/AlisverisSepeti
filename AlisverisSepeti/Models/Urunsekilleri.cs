using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunsekilleri
    {
        public int UrunSekilId { get; set; }
        public string UrunSekilAdi { get; set; }
        public int? DizilisSira { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public int EkleyenId { get; set; }
        public string Ekleyen { get; set; }
        public int? GuncelleyenId { get; set; }
        public string Guncelleyen { get; set; }
        public bool Silindimi { get; set; }
        public bool Aktifmi { get; set; }
        public int? SilenId { get; set; }
        public string Silen { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual User SilenNavigation { get; set; }
    }
}
