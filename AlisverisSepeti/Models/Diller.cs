using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Diller
    {
        public Diller()
        {
            OzellikdegerleriDils = new HashSet<OzellikdegerleriDil>();
            OzellikgrupDils = new HashSet<OzellikgrupDil>();
            OzelliklerDils = new HashSet<OzelliklerDil>();
            StokdurumDils = new HashSet<StokdurumDil>();
            UrunkategorilerDils = new HashSet<UrunkategorilerDil>();
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

        public virtual ICollection<OzellikdegerleriDil> OzellikdegerleriDils { get; set; }
        public virtual ICollection<OzellikgrupDil> OzellikgrupDils { get; set; }
        public virtual ICollection<OzelliklerDil> OzelliklerDils { get; set; }
        public virtual ICollection<StokdurumDil> StokdurumDils { get; set; }
        public virtual ICollection<UrunkategorilerDil> UrunkategorilerDils { get; set; }
        public virtual ICollection<UrunlerDil> UrunlerDils { get; set; }
    }
}
