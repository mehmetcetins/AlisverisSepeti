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
            SiparislerEkleyenNavigations = new HashSet<Siparisler>();
            SiparislerGuncelleyenNavigations = new HashSet<Siparisler>();
            SiparislerOdemeDogrulayanNavigations = new HashSet<Siparisler>();
            SiparislerSevkEdenNavigations = new HashSet<Siparisler>();
            SiparislerUsers = new HashSet<Siparisler>();
            StokdurumEkleyenNavigations = new HashSet<Stokdurum>();
            StokdurumGuncelleyenNavigations = new HashSet<Stokdurum>();
            UrunkategorilerEkleyenNavigations = new HashSet<Urunkategoriler>();
            UrunkategorilerGuncelleyenNavigations = new HashSet<Urunkategoriler>();
            UrunlerEkleyenNavigations = new HashSet<Urunler>();
            UrunlerGuncelleyenNavigations = new HashSet<Urunler>();
            UrunsekilleriEkleyenNavigations = new HashSet<Urunsekilleri>();
            UrunsekilleriGuncelleyenNavigations = new HashSet<Urunsekilleri>();
            UrunsekilleriSilenNavigations = new HashSet<Urunsekilleri>();
            Uruntedarikcileris = new HashSet<Uruntedarikcileri>();
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
        public virtual ICollection<Siparisler> SiparislerEkleyenNavigations { get; set; }
        public virtual ICollection<Siparisler> SiparislerGuncelleyenNavigations { get; set; }
        public virtual ICollection<Siparisler> SiparislerOdemeDogrulayanNavigations { get; set; }
        public virtual ICollection<Siparisler> SiparislerSevkEdenNavigations { get; set; }
        public virtual ICollection<Siparisler> SiparislerUsers { get; set; }
        public virtual ICollection<Stokdurum> StokdurumEkleyenNavigations { get; set; }
        public virtual ICollection<Stokdurum> StokdurumGuncelleyenNavigations { get; set; }
        public virtual ICollection<Urunkategoriler> UrunkategorilerEkleyenNavigations { get; set; }
        public virtual ICollection<Urunkategoriler> UrunkategorilerGuncelleyenNavigations { get; set; }
        public virtual ICollection<Urunler> UrunlerEkleyenNavigations { get; set; }
        public virtual ICollection<Urunler> UrunlerGuncelleyenNavigations { get; set; }
        public virtual ICollection<Urunsekilleri> UrunsekilleriEkleyenNavigations { get; set; }
        public virtual ICollection<Urunsekilleri> UrunsekilleriGuncelleyenNavigations { get; set; }
        public virtual ICollection<Urunsekilleri> UrunsekilleriSilenNavigations { get; set; }
        public virtual ICollection<Uruntedarikcileri> Uruntedarikcileris { get; set; }
    }
}
