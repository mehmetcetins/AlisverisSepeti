using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunOpsiyonDegerleri",Name ="Urunopsiyondegerleri")]
    public class UrunOpsiyonDegerleriController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunOpsiyonDegerleri/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunOpsiyonDegerleri/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunOpsiyonDegerleri/UrunOpsiyonDegerleriForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers
                    .AsNoTracking()
                    .Include(urun => urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
                    .ToList();
                ViewBag.Opsiyonlar = context.Urunopsiyonlars
                    .AsNoTracking()
                    .ToList();
                
            }
        }
        #region Index

        public IActionResult Index()
        {
            using ( var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunOpsiyonDegerleri = context.Urunopsiyondegerleris
                    .AsNoTracking()
                    .Include(deger => deger.Urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
                    .Include(deger => deger.UrunOpsiyon)
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
                    ViewBag.UrunOpsiyonDegerleri = context.Urunopsiyondegerleris
                        .AsNoTracking()
                        .Where(deger => deger.UrunOdid == id)
                        .Include(deger => deger.Urun.Marka)
                        .Include(deger => deger.Urun.StokDurum)
                        .Include(deger => deger.Urun.EkleyenNavigation)
                        .Include(deger => deger.Urun.GuncelleyenNavigation)
                        .Include(deger => deger.Urun.UrunTipiNavigation)
                        .Include(deger => deger.Urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
                        .Include(deger => deger.UrunOpsiyon.OpsiyonTipiNavigation)
                        .Include(deger => deger.UrunOpsiyon.Degisken)
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
        [HttpGet("UrunOpsiyonDegerleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunOpsiyonDegerleri = new Models.Urunopsiyondegerleri();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        #endregion
        #region Post.Add
        [HttpPost("UrunOpsiyonDegerleriForm/Add")]
        public IActionResult Add(Models.Urunopsiyondegerleri urunopsiyondegerleri)
        {
            if (urunopsiyondegerleri == null)
            {
                TempData["error"] = "Beklenmedik bir hatayla karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunOpsiyonDegerleri = urunopsiyondegerleri;
                    extras();
                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunopsiyondegerleri.UrunId).First();
                        context.Urunopsiyonlars.AsNoTracking().Where(ops => ops.Id == urunopsiyondegerleri.UrunOpsiyonId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = "Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        context.Urunopsiyondegerleris.Add(urunopsiyondegerleri);
                        context.SaveChanges();
                    }
                    catch(DbUpdateException e)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu: " + e.InnerException.Message ;
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Get.Update
        [HttpGet("UrunOpsiyonDegerleriForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunOpsiyonDegerleri = context.Urunopsiyondegerleris.AsNoTracking().Where(deger => deger.UrunOdid == id).First();

                }
                catch(InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            extras();
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("UrunOpsiyonDegerleriForm/Update/{id:int}")]
        public IActionResult Update (int id, Models.Urunopsiyondegerleri urunopsiyondegerleri)
        {
            if (urunopsiyondegerleri == null)
            {
                TempData["error"] = "Beklenmedik bir hatayla karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunOpsiyonDegerleri = urunopsiyondegerleri;
                    extras();
                    try
                    {
                        context.Urunopsiyondegerleris.AsNoTracking().Where(deger => deger.UrunOdid == id).First();
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunopsiyondegerleri.UrunId).First();
                        context.Urunopsiyonlars.AsNoTracking().Where(ops => ops.Id == urunopsiyondegerleri.UrunOpsiyonId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = "Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        urunopsiyondegerleri.UrunOdid = id;
                        context.Urunopsiyondegerleris.Update(urunopsiyondegerleri);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu: " + e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
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
                        context.Urunopsiyondegerleris.Remove(context.Urunopsiyondegerleris.Where(deger => deger.UrunOdid == id).First());

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
