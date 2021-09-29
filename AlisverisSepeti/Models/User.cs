using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class User
    {
        public User()
        {
            OzellikgrupEkleyenNavigations = new HashSet<Ozellikgrup>();
            OzellikgrupGuncelleyenNavigations = new HashSet<Ozellikgrup>();
            StokdurumEkleyenNavigations = new HashSet<Stokdurum>();
            StokdurumGuncelleyenNavigations = new HashSet<Stokdurum>();
            UrunlerEkleyenNavigations = new HashSet<Urunler>();
            UrunlerGuncelleyenNavigations = new HashSet<Urunler>();
        }

        public int UserId { get; set; }
        public string KullaniciIsim { get; set; }
        public string KullaniciTipi { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Durum { get; set; }
        public string DurumTxt { get; set; }

        public virtual ICollection<Ozellikgrup> OzellikgrupEkleyenNavigations { get; set; }
        public virtual ICollection<Ozellikgrup> OzellikgrupGuncelleyenNavigations { get; set; }
        public virtual ICollection<Stokdurum> StokdurumEkleyenNavigations { get; set; }
        public virtual ICollection<Stokdurum> StokdurumGuncelleyenNavigations { get; set; }
        public virtual ICollection<Urunler> UrunlerEkleyenNavigations { get; set; }
        public virtual ICollection<Urunler> UrunlerGuncelleyenNavigations { get; set; }
    }
}
