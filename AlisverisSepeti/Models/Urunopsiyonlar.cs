using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Urunopsiyonlar
    {
        public Urunopsiyonlar()
        {
            Urunopsiyondegerleris = new HashSet<Urunopsiyondegerleri>();
        }

        public int Id { get; set; }
        public string OpsiyonAdi { get; set; }
        public int OpsiyonTipi { get; set; }
        public bool Zorunlumu { get; set; }
        public int DegiskenId { get; set; }

        public virtual Degiskentipleri Degisken { get; set; }
        public virtual Opsiyontipleri OpsiyonTipiNavigation { get; set; }
        public virtual ICollection<Urunopsiyondegerleri> Urunopsiyondegerleris { get; set; }
    }
}
