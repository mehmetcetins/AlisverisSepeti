using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Kredikartlari
    {
        public int KartId { get; set; }
        public int? PosId { get; set; }
        public int? DizilisSira { get; set; }
        public string KartAdi { get; set; }
        public string PosBankaAdi { get; set; }
        public string GecerliKartlar { get; set; }
        public string KartLogo { get; set; }
        public bool TaksitVarmi { get; set; }
        public bool Yiaktifmi { get; set; }
        public bool Ydaktifmi { get; set; }

        public virtual Poslar Pos { get; set; }
    }
}
