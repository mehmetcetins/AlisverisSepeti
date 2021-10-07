using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Uruntedarikcileri
    {
        public int UrunTedarikciId { get; set; }
        public int UserId { get; set; }
        public int UrunId { get; set; }
        public int UrunStokkodu { get; set; }
        public float Fiyat { get; set; }
        public bool Kdvdahilmi { get; set; }
        public float Kdvoran { get; set; }

        public virtual Urunler Urun { get; set; }
        public virtual User User { get; set; }
    }
}
