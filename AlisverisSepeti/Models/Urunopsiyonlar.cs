using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunopsiyonlar
    {
        public int Id { get; set; }
        public string OpsiyonAdi { get; set; }
        public int OpsiyonTipi { get; set; }
        public bool Zorunlumu { get; set; }

        public virtual Opsiyontipleri OpsiyonTipiNavigation { get; set; }
    }
}
