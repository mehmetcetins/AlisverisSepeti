using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Uruntipleri
    {
        public Uruntipleri()
        {
            Urunlers = new HashSet<Urunler>();
        }

        public int UrunTipiId { get; set; }
        public string UrunTipi { get; set; }

        public virtual ICollection<Urunler> Urunlers { get; set; }
    }
}
