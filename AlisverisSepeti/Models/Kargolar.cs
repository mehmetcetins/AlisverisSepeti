using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Kargolar
    {
        public int KargoId { get; set; }
        public string KargoAdi { get; set; }
        public string KargoBedeliDovizKodu { get; set; }
        public string UcretsizKargoBedeliDovizKodu { get; set; }
        public string KargoLogo { get; set; }
        public float KargoBedeli { get; set; }
        public float? UcretsizKargoBedeli { get; set; }
        public int DizilisSira { get; set; }
        public bool KapidaNakitOdemeVarmi { get; set; }
        public bool KapidaKrediKartOdemeVarmi { get; set; }
        public bool YigonderimVarmi { get; set; }
        public bool YdgonderimVarmi { get; set; }
        public bool SehirDisiGonderimVarmi { get; set; }
        public bool Aktifmi { get; set; }
    }
}
