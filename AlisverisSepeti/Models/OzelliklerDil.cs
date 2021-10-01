using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class OzelliklerDil
    {
        public int OzellikDilId { get; set; }
        public string OzellikAdi { get; set; }
        public string OzellikAciklama { get; set; }
        public int DilId { get; set; }
        public int OzellikId { get; set; }

        public virtual Diller Dil { get; set; }
        public virtual Ozellikler Ozellik { get; set; }
    }
}
