using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Ozellikdegerleri
    {
        public Ozellikdegerleri()
        {
            OzellikdegerleriDils = new HashSet<OzellikdegerleriDil>();
        }

        public int OzellikDegerId { get; set; }
        public int OzellikId { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }

        public virtual Ozellikler Ozellik { get; set; }
        public virtual ICollection<OzellikdegerleriDil> OzellikdegerleriDils { get; set; }
    }
}
