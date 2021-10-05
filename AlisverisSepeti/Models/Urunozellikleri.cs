using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunozellikleri
    {
        public int UrunOzellikId { get; set; }
        public int UrunId { get; set; }
        public int OzellikId { get; set; }
        public string OzellikAdi { get; set; }
        public string Deger { get; set; }
        public string OzellikAciklama { get; set; }
        public int? DizilisSira { get; set; }
        public string OzellikTipi { get; set; }
        public string DegiskenTipi { get; set; }

        public virtual Ozellikler Ozellik { get; set; }
        public virtual Urunler Urun { get; set; }
    }
}
