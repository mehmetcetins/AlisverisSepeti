using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Gonderimsekilleri
    {
        public Gonderimsekilleri()
        {
            SiparislerGonderimSekliNavigations = new HashSet<Siparisler>();
            SiparislerSevkSekliNavigations = new HashSet<Siparisler>();
        }

        public string GonderimSekli { get; set; }
        public bool KapidaOdemeVarmi { get; set; }
        public float? MinTutar { get; set; }
        public float? MaxTutar { get; set; }
        public bool Aktifmi { get; set; }
        public int? DizilisSira { get; set; }
        public bool YurtIciGonderimVarmi { get; set; }
        public bool YurtDisiGonderimVarmi { get; set; }
        public int GonderimId { get; set; }

        public virtual ICollection<Siparisler> SiparislerGonderimSekliNavigations { get; set; }
        public virtual ICollection<Siparisler> SiparislerSevkSekliNavigations { get; set; }
    }
}
