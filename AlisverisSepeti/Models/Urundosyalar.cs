using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urundosyalar
    {
        public int UrunDosyaId { get; set; }
        public int UrunId { get; set; }
        public string FileName { get; set; }
        public string DosyaBaslik { get; set; }
        public string DosyaBilgi { get; set; }
        public string DosyaTipi { get; set; }
        public bool Varsayilanmi { get; set; }
        public int? DizilisSira { get; set; }

        public virtual Urunler Urun { get; set; }
    }
}
