using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class UrunlerDil
    {
        public int UrunDilId { get; set; }
        public string UrunAdi { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string PageLabels { get; set; }
        public string UrunBilgisi { get; set; }
        public string UrunBilgisiText { get; set; }
        public string UrunKisaAciklama { get; set; }
        public int DilId { get; set; }
        public int UrunId { get; set; }
    }
}
