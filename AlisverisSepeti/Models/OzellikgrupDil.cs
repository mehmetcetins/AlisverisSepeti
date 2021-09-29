using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class OzellikgrupDil
    {
        public int OzellikGrupDilId { get; set; }
        public int OzellikGrupId { get; set; }
        public int DilId { get; set; }
        public string OzellikGrupAdi { get; set; }
        public string OzellikGrupAciklama { get; set; }

        public virtual Diller Dil { get; set; }
        public virtual Ozellikgrup OzellikGrup { get; set; }
    }
}
