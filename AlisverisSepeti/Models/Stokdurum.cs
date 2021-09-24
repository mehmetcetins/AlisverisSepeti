using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Stokdurum
    {
        public int StokDurumId { get; set; }
        public string StokDurumKod { get; set; }
        public string StokDurumResim { get; set; }
        public bool SepetIzni { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
    }
}
