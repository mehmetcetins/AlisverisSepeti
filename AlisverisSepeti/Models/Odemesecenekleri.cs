using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Odemesecenekleri
    {
        public Odemesecenekleri()
        {
            Siparislers = new HashSet<Siparisler>();
        }

        public int Id { get; set; }
        public string OdemeSekli { get; set; }
        public int? DizilisSira { get; set; }
        public bool Aktifmi { get; set; }

        public virtual ICollection<Siparisler> Siparislers { get; set; }
    }
}
