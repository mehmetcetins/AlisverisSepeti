using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class OzellikdegerleriDil
    {
        public int OzellikDegerDilId { get; set; }
        public string OzellikDeger { get; set; }
        public int DilId { get; set; }
        public int OzellikDegerId { get; set; }

        public virtual Diller Dil { get; set; }
        public virtual Ozellikdegerleri OzellikDegerNavigation { get; set; }
    }
}
