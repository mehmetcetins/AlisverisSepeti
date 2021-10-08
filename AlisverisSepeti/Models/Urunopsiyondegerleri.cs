using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunopsiyondegerleri
    {
        public int UrunOdid { get; set; }
        public int UrunOpsiyonId { get; set; }
        public int UrunId { get; set; }
        public string OpsiyonDeger { get; set; }

        public virtual Urunler Urun { get; set; }
        public virtual Urunopsiyonlar UrunOpsiyon { get; set; }
    }
}
