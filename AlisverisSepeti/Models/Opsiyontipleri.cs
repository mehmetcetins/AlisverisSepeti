using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Opsiyontipleri
    {
        public Opsiyontipleri()
        {
            Urunopsiyonlars = new HashSet<Urunopsiyonlar>();
        }

        public string Ismi { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Urunopsiyonlar> Urunopsiyonlars { get; set; }
    }
}
