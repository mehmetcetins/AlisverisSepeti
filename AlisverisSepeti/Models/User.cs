using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string KullaniciIsim { get; set; }
        public string KullaniciTipi { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Durum { get; set; }
        public string DurumTxt { get; set; }
    }
}
