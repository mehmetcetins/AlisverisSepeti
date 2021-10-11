using System;
using System.Collections.Generic;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class Siparisler
    {
        public int SiparisId { get; set; }
        public int UserId { get; set; }
        public int EkleyenId { get; set; }
        public int? GuncelleyenId { get; set; }
        public int GonderimSekliId { get; set; }
        public int OdemeSekliId { get; set; }
        public int? SevkEdenId { get; set; }
        public int? HavaleBankaId { get; set; }
        public int? OdemeDogrulayanId { get; set; }
        public int? SevkSekliId { get; set; }
        public string SiparisTarihi { get; set; }
        public string EklenmeTarihi { get; set; }
        public string GuncellemeTarihi { get; set; }
        public string SevkTarihi { get; set; }
        public string SiparisToplami { get; set; }
        public string SiparisKalemAdet { get; set; }
        public string UrunAdet { get; set; }
        public string Ekleyen { get; set; }
        public string Guncelleyen { get; set; }
        public string GonderimSekli { get; set; }
        public string OdemeSekli { get; set; }
        public string HavaleBankaAdi { get; set; }
        public string OdemeDogrulayan { get; set; }
        public string SevkEden { get; set; }
        public string SevkSekli { get; set; }
        public bool OdemeGeldimi { get; set; }

        public virtual User EkleyenNavigation { get; set; }
        public virtual Gonderimsekilleri GonderimSekliNavigation { get; set; }
        public virtual User GuncelleyenNavigation { get; set; }
        public virtual Havalebankalari HavaleBanka { get; set; }
        public virtual User OdemeDogrulayanNavigation { get; set; }
        public virtual Odemesecenekleri OdemeSekliNavigation { get; set; }
        public virtual User SevkEdenNavigation { get; set; }
        public virtual Gonderimsekilleri SevkSekliNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
