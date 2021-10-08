using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Markalar
    {
        public Markalar()
        {
            Urunlers = new HashSet<Urunler>();
            Urunmarkalars = new HashSet<Urunmarkalar>();
        }

        public int MarkaId { get; set; }
        public string MarkaAdi { get; set; }
        public string MarkaHakkinda { get; set; }
        public bool Aktifmi { get; set; }
        public int? DizilisSira { get; set; }
        public string MarkaLogo { get; set; }
        public string MarkaBanner { get; set; }

        public virtual ICollection<Urunler> Urunlers { get; set; }
        public virtual ICollection<Urunmarkalar> Urunmarkalars { get; set; }
    }
}
