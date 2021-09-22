using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunOpsiyonlar",Name ="Urunopsiyonlar")]
    public class UrunOpsiyonlarController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/UrunOpsiyonlar/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/UrunOpsiyonlar/UrunOpsiyonlarForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunOpsiyonlar = context.Urunopsiyonlars.AsNoTracking().Include(opsiyonlar => opsiyonlar.OpsiyonTipiNavigation).ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("UrunOpsiyonlarForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.UrunOpsiyonlar = new Models.Urunopsiyonlar();
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().ToList();
            }
            return View(FormCS);
        }
        [HttpPost("UrunOpsiyonlarForm/Add")]
        public IActionResult Add(Models.Urunopsiyonlar urunopsiyonlar)
        {
            if (urunopsiyonlar == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunOpsiyonlar = urunopsiyonlar;
                    ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().ToList();
                    if (context.Urunopsiyonlars.AsNoTracking().Any(opsiyon => opsiyon.OpsiyonAdi == urunopsiyonlar.OpsiyonAdi))
                    {
                        ViewBag.error = "Aynı Opsiyon Adına Sahip Başka Bir Kayıt Var";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Urunopsiyonlars.Add(urunopsiyonlar);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Ekleme Sırasında Bir Hata İle Karşılaşıldı.";
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("UrunOpsiyonlarForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunOpsiyonlar = context.Urunopsiyonlars.AsNoTracking().Where(urun => urun.Id == id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().ToList();
            }
            return View(FormCS);
        }
        [HttpPost("UrunOpsiyonlarForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Urunopsiyonlar urunopsiyonlar)
        {
            if (urunopsiyonlar == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunOpsiyonlar = urunopsiyonlar;
                    ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().ToList();
                    if (context.Urunopsiyonlars.AsNoTracking().Any(opsiyon => opsiyon.OpsiyonAdi == urunopsiyonlar.OpsiyonAdi && opsiyon.Id != id))
                    {
                        ViewBag.error = "Aynı Opsiyon Adına Sahip Başka Bir Kayıt Var";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Urunopsiyonlars.Update(urunopsiyonlar);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Güncelleme Sırasında Bir Hata İle Karşılaşıldı.";
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
                        context.Urunopsiyonlars.Remove(context.Urunopsiyonlars.Where(urun => urun.Id == id).First());

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
