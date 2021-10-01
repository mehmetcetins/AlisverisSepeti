using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Ozellikler
    {
        public Ozellikler()
        {
            Ozellikdegerleris = new HashSet<Ozellikdegerleri>();
            OzelliklerDils = new HashSet<OzelliklerDil>();
        }

        public int OzellikId { get; set; }
        public int OzellikTipiId { get; set; }
        public int OzellikGrupId { get; set; }
        public string OzellikTipi { get; set; }
        public string DegiskenTipi { get; set; }
        public string Birim { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }
        public int? DizilisSira { get; set; }

        public virtual Ozellikgrup OzellikGrup { get; set; }
        public virtual Ozelliktipleri OzellikTipiNavigation { get; set; }
        public virtual ICollection<Ozellikdegerleri> Ozellikdegerleris { get; set; }
        public virtual ICollection<OzelliklerDil> OzelliklerDils { get; set; }
    }
}
