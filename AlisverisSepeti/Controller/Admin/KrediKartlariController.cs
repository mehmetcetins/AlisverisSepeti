using Microsoft.AspNetCore.Http;
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
        private string ImagesFolder = "KrediKartlari";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.KrediKartlari = context.Kredikartlaris.Include(kart => kart.Pos).ToList();
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
                    ViewBag.KrediKarti = context.Kredikartlaris.AsNoTracking().Where(kart => kart.KartId == id).Include(kart => kart.Pos).First();
                    
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
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
            }
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("KrediKartlariForm/Add")]
        public IActionResult Add(IFormFile KartLogo,Models.Kredikartlari kredikarti)
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
                        ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                        ViewBag.SubmitButtonValue = "Ekle";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            kredikarti.Pos = context.Poslars.Where(pos => pos.PosId == kredikarti.PosId).First();
                            kredikarti.PosBankaAdi = kredikarti.Pos.PosBankaAdi;
                        }
                        catch(InvalidOperationException)
                        {
                            ViewBag.error = "Seçilen Pos Kayıtlarda Bulunamadi.";
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        
                        try
                        {
                            context.Kredikartlaris.Add(kredikarti);
                            context.SaveChanges();
                            if (KartLogo != null)
                            {
                                kredikarti.KartLogo = Utils.Utils.ToFileName(KartLogo.FileName, kredikarti.KartId);
                                try
                                {
                                    Utils.ImageUtils.ResizeAndSave(
                                        KartLogo.OpenReadStream(),
                                        200,
                                        200,
                                        ImagesFolder,
                                        kredikarti.KartLogo
                                        );
                                }
                                catch (Exception e) {
                                    TempData["error"] = "Banka Bilgileri Kaydedildi Ama Logo Yüklenirken Bir Problem Oluştu. " + e.Message;
                                    return RedirectToAction("Index");
                                }
                                context.Kredikartlaris.Update(kredikarti);
                                context.SaveChanges();
                            }
                            
                        }
                        catch(DbUpdateException e)
                        {
                            ViewBag.error = "Ekleme Sirasında Bir Sorun Oluştu" + e.Message;
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        
                    }
                    TempData["success"] = "Başarıyla Eklendi.";
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
                    ViewBag.KrediKarti = context.Kredikartlaris.AsNoTracking().Where(kart=> kart.KartId == id).Include(kart => kart.Pos).First();
                    ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                ViewBag.SubmitButtonValue = "Güncelle";
               
            }
            return View(FormCS);
        }
        [HttpPost("KrediKartlariForm/Update/{id:int}")]
        public IActionResult Update(int id,IFormFile KartLogo, Models.Kredikartlari kredikarti)
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
                    Models.Kredikartlari oldKart;
                    try
                    {
                        oldKart = context.Kredikartlaris.AsNoTracking().Where(kart => kart.KartId == id).Include(kart => kart.Pos).First();
                    }
                    catch(InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }

                    if (context.Kredikartlaris.AsNoTracking().Any(kart => kart.KartAdi == kredikarti.KartAdi && kart.KartId != id))
                    {
                        ViewBag.error = "Aynı Kart Adinda Başka Bir Kayit Var";
                        ViewBag.KrediKarti = kredikarti;
                        ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                        ViewBag.SubmitButtonValue = "Güncelle";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            kredikarti.Pos = context.Poslars.Where(pos => pos.PosId == kredikarti.PosId).First();
                            kredikarti.PosBankaAdi = kredikarti.Pos.PosBankaAdi;
                        }
                        catch (InvalidOperationException)
                        {
                            ViewBag.error = "Seçilen Pos Kayıtlarda Bulunamadi.";
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                            ViewBag.SubmitButtonValue = "Güncelle";
                            return View(FormCS);
                        }
                        try
                        {
                            if(KartLogo == null)
                            {
                                kredikarti.KartLogo = oldKart.KartLogo;
                            }
                            else
                            {
                                kredikarti.KartLogo = Utils.Utils.ToFileName(KartLogo.FileName,id);
                                Utils.ImageUtils.ResizeAndSave(
                                    KartLogo.OpenReadStream(),
                                    200,
                                    200,
                                    ImagesFolder,
                                    kredikarti.KartLogo
                                );
                            }
                            kredikarti.KartId = id;
                            context.Kredikartlaris.Update(kredikarti);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = "Güncelleme Sirasında Bir Sorun Oluştu" + e.Message;
                            ViewBag.KrediKarti = kredikarti;
                            ViewBag.Poslar = context.Poslars.AsNoTracking().ToList();
                            ViewBag.SubmitButtonValue = "Güncelle";
                            return View(FormCS);
                        }

                    }
                    TempData["success"] = "Başarıyla Güncellendi.";
                    return RedirectToAction("Index");
                }
            }
        }
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
                        context.Kredikartlaris.Remove(context.Kredikartlaris.Where(kart => kart.KartId == id).First());
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return new EmptyResult();
                    }
                    
                    context.SaveChangesAsync();
                    
                }
                catch (DbUpdateException e)
                {
                    TempData["error"] = "Kayıt Silinirken Bir Hata Oluştu.";
                    return new EmptyResult();
                }
            }
            TempData["success"] = "Başarıyla Silindi.";
            return new EmptyResult();
        }
        #endregion
    }
}
