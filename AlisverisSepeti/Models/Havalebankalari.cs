using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Havalebankalari
    {
        public int HavaleBankaId { get; set; }
        public string BankaAdi { get; set; }
        public string SubeKodu { get; set; }
        public string SubeAdi { get; set; }
        public string HesapNo { get; set; }
        public string Iban { get; set; }
        public int DizilisSira { get; set; }
        public bool Yiaktifmi { get; set; }
        public string DovizKodu { get; set; }
        public bool Ydaktifmi { get; set; }
        public string BankaLogo { get; set; }
    }
}
