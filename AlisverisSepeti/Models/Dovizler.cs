using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Dovizler
    {
        public int DovizId { get; set; }
        public string DovizAdi { get; set; }
        public string DovizAdiGlobal { get; set; }
        public string DovizKodu { get; set; }
        public string Kur { get; set; }
        public string Tarih { get; set; }
        public byte Aktifmi { get; set; }
        public string Sembol { get; set; }

        public virtual Dovizkurlari Dovizkurlari { get; set; }
    }
}
