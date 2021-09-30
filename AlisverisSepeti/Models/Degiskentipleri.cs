using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Degiskentipleri
    {
        public Degiskentipleri()
        {
            Ozelliktipleris = new HashSet<Ozelliktipleri>();
            Urunopsiyonlars = new HashSet<Urunopsiyonlar>();
        }

        public int Id { get; set; }
        public string DegiskenAdi { get; set; }

        public virtual ICollection<Ozelliktipleri> Ozelliktipleris { get; set; }
        public virtual ICollection<Urunopsiyonlar> Urunopsiyonlars { get; set; }
    }
}
