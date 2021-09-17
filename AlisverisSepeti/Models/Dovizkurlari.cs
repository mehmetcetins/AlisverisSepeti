using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Dovizkurlari
    {
        public int DovizKurId { get; set; }
        public string Tarih { get; set; }
        public int? DovizId { get; set; }
        public string DovizKodu { get; set; }
        public float Kur { get; set; }

        public virtual Dovizler Doviz { get; set; }
    }
}
