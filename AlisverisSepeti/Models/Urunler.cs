using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunler
    {
        public Urunler()
        {
            UrunlerDils = new HashSet<UrunlerDil>();
        }

        public int UrunId { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }
        public int MarkaId { get; set; }
        public int UrunTipi { get; set; }
        public int StokDurumId { get; set; }
        public int? DizilisSira { get; set; }
        public string UrunGosterimTarihiBas { get; set; }
        public string UrunGosterimTarihiBit { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public string UrunKodu { get; set; }
        public string UrunBarkodu { get; set; }
        public string UrunMuhasebeKodu { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public bool YorumYazilabilirmi { get; set; }
        public bool Aktifmi { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual Markalar Marka { get; set; }
        public virtual Stokdurum StokDurum { get; set; }
        public virtual Uruntipleri UrunTipiNavigation { get; set; }
        public virtual ICollection<UrunlerDil> UrunlerDils { get; set; }
    }
}
