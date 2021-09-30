using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Ozelliktipleri
    {
        public Ozelliktipleri()
        {
            Ozelliklers = new HashSet<Ozellikler>();
        }

        public int OzellikTipiId { get; set; }
        public string OzellikTipi { get; set; }
        public int DegiskenTipi { get; set; }
        public string Durum { get; set; }
        public string Tanim { get; set; }
        public bool Liste { get; set; }

        public virtual Degiskentipleri DegiskenTipiNavigation { get; set; }
        public virtual ICollection<Ozellikler> Ozelliklers { get; set; }
    }
}
