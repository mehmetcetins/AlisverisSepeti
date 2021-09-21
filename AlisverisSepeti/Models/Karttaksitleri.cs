using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Karttaksitleri
    {
        public int TaksitId { get; set; }
        public string Durum { get; set; }
        public byte TaksitSayisi { get; set; }
        public bool PesinFiyatinaTaksitlemi { get; set; }
        public string TaksitAciklama { get; set; }
        public float TaksitOran { get; set; }
        public bool Aktifmi { get; set; }
        public int? KartId { get; set; }

        public virtual Kredikartlari Kart { get; set; }
    }
}
