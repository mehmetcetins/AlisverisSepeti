using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Siparisler",Name ="Siparisler")]
    public class SiparislerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/Siparisler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/Siparisler/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/Siparisler/SiparislerForm.cshtml";
        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.GonderimSekilleri =context.Gonderimsekilleris.AsNoTracking().ToList();
                ViewBag.HavaleBankalari =context.Havalebankalaris.AsNoTracking().ToList();
                ViewBag.OdemeSekilleri =context.Odemesecenekleris.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Siparisler = context.Siparislers
                    .AsNoTracking()
                    .Include(siparis => siparis.SevkSekliNavigation)
                    .Include(siparis => siparis.OdemeSekliNavigation)
                    .ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Detail
        [HttpGet("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {

                    ViewBag.Siparisler = context.Siparislers
                        .AsNoTracking()
                        .Where(siparis => siparis.SiparisId == id)
                        .Include(siparis => siparis.EkleyenNavigation)
                        .Include(siparis => siparis.GuncelleyenNavigation)
                        .Include(siparis => siparis.SevkEdenNavigation)
                        .Include(siparis => siparis.SevkSekliNavigation)
                        .Include(siparis => siparis.GonderimSekliNavigation)
                        .Include(siparis => siparis.HavaleBanka)
                        .Include(siparis => siparis.OdemeDogrulayanNavigation)
                        .Include(siparis => siparis.OdemeSekliNavigation)
                        .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            return View(DetailCS);
        }
        #endregion
        #region Form
        #region Get.Add
        [HttpGet("SiparislerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.Siparisler = new Models.Siparisler();
            extras();
            return View(FormCS);
        }
        #endregion
        #region Post.Add
        [HttpPost("SiparislerForm/Add")]
        public IActionResult Add(Models.Siparisler siparisler)
        {
            if (siparisler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                extras();
                ViewBag.SubmitButtonValue = "Ekle";
                ViewBag.Siparisler = siparisler;
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Gonderimsekilleri gonderim;
                    Models.Havalebankalari havale;
                    Models.Odemesecenekleri odeme;
                    try
                    {
                        havale = context.Havalebankalaris.AsNoTracking().Where(banka => banka.HavaleBankaId == siparisler.HavaleBankaId).First();
                        odeme = context.Odemesecenekleris.AsNoTracking().Where(odeme => odeme.Id == siparisler.OdemeSekliId).First();
                        gonderim = context.Gonderimsekilleris.AsNoTracking().Where(gonderim => gonderim.GonderimId == siparisler.GonderimSekliId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Secenek Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        var firstUser = context.Users.First();
                        siparisler.EklenmeTarihi = DateTime.Now.ToString();
                        siparisler.SiparisTarihi = DateTime.Now.ToString();
                        siparisler.EkleyenId = firstUser.UserId;
                        siparisler.UserId = firstUser.UserId;
                        siparisler.Ekleyen = firstUser.KullaniciIsim;
                        siparisler.GonderimSekli = gonderim.GonderimSekli;
                        siparisler.HavaleBankaAdi = havale.BankaAdi;
                        siparisler.OdemeSekli = odeme.OdemeSekli;
                        siparisler.SiparisKalemAdet = "0";
                        siparisler.SiparisToplami = "0";
                        siparisler.UrunAdet = "0";
                        context.Siparislers.Add(siparisler);
                        context.SaveChanges();
                    }
                    catch(DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }

            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Get.Update
        [HttpGet("SiparislerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            using (var context= new Models.AlisverisSepetiContext())
            {
                try
                { 
                    ViewBag.Siparisler = context.Siparislers.AsNoTracking().Where(siparis => siparis.SiparisId == id).First();
                }
                catch(InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("SiparislerForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Siparisler siparisler)
        {
            if (siparisler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                extras();
                ViewBag.SubmitButtonValue = "Güncelle";
                ViewBag.Siparisler = siparisler;
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Gonderimsekilleri gonderim;
                    Models.Havalebankalari havale;
                    Models.Odemesecenekleri odeme;
                    Models.Siparisler previous;
                    try
                    {
                        try
                        {
                            previous = context.Siparislers.AsNoTracking().Where(siparis => siparis.SiparisId == id).First();
                        }
                        catch(InvalidOperationException)
                        {
                            TempData["error"] = "Kayıt Bulunamadi.";
                            return RedirectToAction("Index");
                        }
                        havale = context.Havalebankalaris.AsNoTracking().Where(banka => banka.HavaleBankaId == siparisler.HavaleBankaId).First();
                        odeme = context.Odemesecenekleris.AsNoTracking().Where(odeme => odeme.Id == siparisler.OdemeSekliId).First();
                        gonderim = context.Gonderimsekilleris.AsNoTracking().Where(gonderim => gonderim.GonderimId == siparisler.GonderimSekliId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Secenek Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        siparisler.EklenmeTarihi = previous.EklenmeTarihi;
                        siparisler.SiparisTarihi = previous.SiparisTarihi;
                        siparisler.EkleyenId = previous.UserId;
                        siparisler.UserId = previous.UserId;
                        siparisler.Ekleyen = previous.Ekleyen;
                        siparisler.GuncelleyenId = previous.UserId;
                        siparisler.Guncelleyen = previous.Ekleyen;
                        siparisler.GuncellemeTarihi = DateTime.Now.ToString();
                        siparisler.GonderimSekli = gonderim.GonderimSekli;
                        siparisler.HavaleBankaAdi = havale.BankaAdi;
                        siparisler.OdemeSekli = odeme.OdemeSekli;
                        siparisler.SiparisKalemAdet = "0";
                        siparisler.SiparisToplami = "0";
                        siparisler.UrunAdet = "0";
                        siparisler.SiparisId = id;
                        context.Siparislers.Update(siparisler);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }

            TempData["success"] = "Başarıyla Güncellendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #endregion
        #region Delete
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Siparislers.Remove(context.Siparislers.Where(siparis => siparis.SiparisId == id).First());

                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return new EmptyResult();
                    }
                    context.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    TempData["error"] = "Silme Sırasında Bir Hata Oluştu.";
                    return new EmptyResult();
                }
            }
            TempData["success"] = "Başarıyla Silindi.";
            return new EmptyResult();
        }
        #endregion
    }
}
