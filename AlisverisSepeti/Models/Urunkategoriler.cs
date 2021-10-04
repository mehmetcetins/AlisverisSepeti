using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunkategoriler
    {
        public Urunkategoriler()
        {
            UrunkategorilerDils = new HashSet<UrunkategorilerDil>();
        }

        public int KategoriId { get; set; }
        public int? PkategoriId { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }
        public int Lineage { get; set; }
        public int Depth { get; set; }
        public int? DizilisSira { get; set; }
        public string KategoriAdi { get; set; }
        public string PkategoriAdi { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public bool Silindimi { get; set; }
        public bool Aktifmi { get; set; }
        public string KategoriLogo { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual Urunkategoriler Pkategori { get; set; }
        public virtual Urunkategoriler InversePkategori { get; set; }
        public virtual ICollection<UrunkategorilerDil> UrunkategorilerDils { get; set; }
    }
}
