using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Poslar
    {
        public Poslar()
        {
            Kredikartlaris = new HashSet<Kredikartlari>();
        }

        public int PosId { get; set; }
        public string PosBankaAdi { get; set; }
        public string GecerliKartlar { get; set; }
        public int DizilisSira { get; set; }
        public string ApiUser { get; set; }
        public string ApiPassword { get; set; }
        public bool SifreliAlisverisVarmi { get; set; }
        public bool SifreliOlsunmu { get; set; }
        public bool Aktifmi { get; set; }
        public bool TaksitVarmi { get; set; }

        public virtual ICollection<Kredikartlari> Kredikartlaris { get; set; }
    }
}
