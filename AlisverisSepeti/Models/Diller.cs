using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Diller
    {
        public int DilId { get; set; }
        public string DilAdi { get; set; }
        public string DilKodu { get; set; }
        public string BolgeDilAdi { get; set; }
        public string DovizKodu { get; set; }
        public string DilLogo { get; set; }
        public bool Aktifmi { get; set; }
        public bool? Varsayilanmi { get; set; }
    }
}
