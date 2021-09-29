using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Diller
    {
        public Diller()
        {
            OzellikgrupDils = new HashSet<OzellikgrupDil>();
            StokdurumDils = new HashSet<StokdurumDil>();
            UrunlerDils = new HashSet<UrunlerDil>();
        }

        public int DilId { get; set; }
        public string DilAdi { get; set; }
        public string DilKodu { get; set; }
        public string BolgeDilAdi { get; set; }
        public string DovizKodu { get; set; }
        public string DilLogo { get; set; }
        public bool Aktifmi { get; set; }
        public bool? Varsayilanmi { get; set; }

        public virtual ICollection<OzellikgrupDil> OzellikgrupDils { get; set; }
        public virtual ICollection<StokdurumDil> StokdurumDils { get; set; }
        public virtual ICollection<UrunlerDil> UrunlerDils { get; set; }
    }
}
