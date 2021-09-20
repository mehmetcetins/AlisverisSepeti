using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/KrediKartlari",Name="Kredikartlari")]
    public class KrediKartlariController : Controller
    {
        // Form Dosya Geleceğini ve Posların listeleneceğini unuttum.
        private string IndexCS = "~/Views/AdminPanel/KrediKartlari/Index.cshtml";
        private string FormCS= "~/Views/AdminPanel/KrediKartlari/KrediKartlariForm.cshtml";
        private string DetailCS= "~/Views/AdminPanel/KrediKartlari/Detail.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.KrediKartlari = context.Kredikartlaris.ToList();
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
                    ViewBag.KrediKarti = context.Kredikartlaris.AsNoTracking().Where(kart => kart.KartId == id).First();
                    
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
        #region Add
        [HttpGet("KrediKartlariForm/Add")]
        public IActionResult Add()
        {
            ViewBag.KrediKarti = new Models.Kredikartlari();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("KrediKartlariForm/Add")]
        public IActionResult Add(Models.Kredikartlari kredikarti)
        {
            if(kredikarti == null)
            {
                TempData["error"] = "Bir Hata İle Karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Kredikartlaris.AsNoTracking().Any(kart => kart.KartAdi == kredikarti.KartAdi))
                    {
                        ViewBag.error = "Aynı Kart Adinda Başka Bir Kayit Var";
                        ViewBag.KrediKarti = kredikarti;
                        ViewBag.SubmitButtonValue = "Ekle";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            kredikarti.Pos = context.Poslars.Where(pos => pos.PosId == kredikarti.PosId).First();
                        }
                        catch(InvalidOperationException)
                        {
                            ViewBag.error = "Seçilen Pos Kayıtlarda Bulunamadi.";
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        try
                        {
                            context.Kredikartlaris.Add(kredikarti);
                            context.SaveChanges();
                        }
                        catch(DbUpdateException e)
                        {
                            ViewBag.error = "Ekleme Sirasında Bir Sorun Oluştu" + e.Message;
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        
                    }
                    TempData["error"] = "Başarıyla Eklendi.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
        #region Update
        [HttpGet("KrediKartlariForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.KrediKarti = context.Kredikartlaris.AsNoTracking().Where(kart=> kart.KartId == id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                ViewBag.SubmitButtonValue = "Guncelle";
               
            }
            return View(FormCS);
        }
        [HttpPost("KrediKartlariForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Kredikartlari kredikarti)
        {
            if (kredikarti == null) 
            {
                TempData["error"] = "Bir Hata İle Karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Kredikartlaris.AsNoTracking().Any(kart => kart.KartAdi == kredikarti.KartAdi && kart.KartId != id))
                    {
                        ViewBag.error = "Aynı Kart Adinda Başka Bir Kayit Var";
                        ViewBag.KrediKarti = kredikarti;
                        ViewBag.SubmitButtonValue = "Guncelle";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            kredikarti.Pos = context.Poslars.Where(pos => pos.PosId == kredikarti.PosId).First();
                        }
                        catch (InvalidOperationException)
                        {
                            ViewBag.error = "Seçilen Pos Kayıtlarda Bulunamadi.";
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        try
                        {
                            context.Kredikartlaris.Add(kredikarti);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = "Ekleme Sirasında Bir Sorun Oluştu" + e.Message;
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }

                    }
                    TempData["error"] = "Başarıyla Eklendi.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
    }
}
