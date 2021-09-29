using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class StokdurumDil
    {
        public int StokDurumDilId { get; set; }
        public int StokDurumId { get; set; }
        public int DilId { get; set; }
        public string StokDurum { get; set; }
        public string StokDurumAciklama { get; set; }

        public virtual Diller Dil { get; set; }
        public virtual Stokdurum StokDurumNavigation { get; set; }
    }
}
