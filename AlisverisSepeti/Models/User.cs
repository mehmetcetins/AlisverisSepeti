using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class User
    {
        public User()
        {
            StokdurumEkleyenNavigations = new HashSet<Stokdurum>();
            StokdurumGuncelleyenNavigations = new HashSet<Stokdurum>();
        }

        public int UserId { get; set; }
        public string KullaniciIsim { get; set; }
        public string KullaniciTipi { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Durum { get; set; }
        public string DurumTxt { get; set; }

        public virtual ICollection<Stokdurum> StokdurumEkleyenNavigations { get; set; }
        public virtual ICollection<Stokdurum> StokdurumGuncelleyenNavigations { get; set; }
    }
}
