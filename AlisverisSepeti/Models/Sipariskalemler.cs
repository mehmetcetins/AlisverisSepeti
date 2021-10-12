using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Sipariskalemler
    {
        public int SiparisKalemId { get; set; }
        public int SiparisId { get; set; }
        public int UrunId { get; set; }
        public int? PurunId { get; set; }
        public string PurunAdi { get; set; }
        public float BirimFiyat { get; set; }
        public int Adet { get; set; }
        public float VergiOran { get; set; }
        public float WeightDesi { get; set; }
        public float Vergi { get; set; }
        public float Toplam { get; set; }
        public string Id { get; set; }

        public virtual Urunler Purun { get; set; }
        public virtual Siparisler Siparis { get; set; }
        public virtual Urunler Urun { get; set; }
    }
}
