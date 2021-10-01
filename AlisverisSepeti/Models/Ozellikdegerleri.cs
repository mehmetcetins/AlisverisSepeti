using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Ozellikdegerleri
    {
        public int OzellikDegerId { get; set; }
        public int OzellikId { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellenmeTarihi { get; set; }

        public virtual Ozellikler Ozellik { get; set; }
    }
}
