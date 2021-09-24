using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunTipleri",Name = "Uruntipleri")]
    public class UrunTipleriController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/UrunTipleri/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/UrunTipleri/UrunTipleriForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunTipleri = context.Uruntipleris.AsNoTracking().ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("UrunTipleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunTipleri = new Models.Uruntipleri();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("UrunTipleriForm/Add")]
        public IActionResult Add(Models.Uruntipleri uruntipleri)
        {
            if (uruntipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UrunTipleri = uruntipleri;
                ViewBag.SubmitButtonValue = "Ekle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Uruntipleris.AsNoTracking().Any(uruntipi => uruntipi.UrunTipi== uruntipleri.UrunTipi))
                    {
                        ViewBag.error = "Aynı Urun Tipinde Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Uruntipleris.Add(uruntipleri);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("UrunTipleriForm/Update/{id:int}")]
        public IActionResult  Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunTipleri = context.Uruntipleris.AsNoTracking().Where(uruntipi => uruntipi.UrunTipiId== id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                
            }
                
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        [HttpPost("UrunTipleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Uruntipleri uruntipleri)
        {
            if (uruntipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UrunTipleri = uruntipleri;
                ViewBag.SubmitButtonValue = "Güncelle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        context.Uruntipleris.AsNoTracking().Where(uruntipi=> uruntipi.UrunTipiId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    if (context.Uruntipleris.AsNoTracking().Any(uruntipi => uruntipi.UrunTipi== uruntipleri.UrunTipi && uruntipi.UrunTipiId != id))
                    {
                        ViewBag.error = "Aynı İsimde Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            uruntipleri.UrunTipiId = id;
                            context.Uruntipleris.Update(uruntipleri);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Başarıyla Güncellendi.";
            return RedirectToAction("Index");
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
                        context.Uruntipleris.Remove(context.Uruntipleris.Where(uruntipi => uruntipi.UrunTipiId== id).First());

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
