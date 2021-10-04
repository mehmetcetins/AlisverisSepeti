using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class UrunkategorilerDil
    {
        public int KategoriDilId { get; set; }
        public int KategoriId { get; set; }
        public int DilId { get; set; }
        public string KategoriAdi { get; set; }
        public string KategoriAciklama { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string PageLabels { get; set; }

        public virtual Diller Dil { get; set; }
        public virtual Urunkategoriler Kategori { get; set; }
    }
}
