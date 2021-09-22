using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/DegiskenTipleri",Name = "Degiskentipleri")]
    public class DegiskenTipleriController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/DegiskenTipleri/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/DegiskenTipleri/DegiskenTipleriForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.DegiskenTipleri = context.Degiskentipleris.AsNoTracking().ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("DegiskenTipleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.DegiskenTipleri = new Models.Degiskentipleri();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("DegiskenTipleriForm/Add")]
        public IActionResult Add(Models.Degiskentipleri degiskentipleri)
        {
            if (degiskentipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.DegiskenTipleri = degiskentipleri;
                ViewBag.SubmitButtonValue = "Ekle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Degiskentipleris.AsNoTracking().Any(degisken => degisken.DegiskenAdi == degiskentipleri.DegiskenAdi))
                    {
                        ViewBag.error = "Aynı Degisken Adinda Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Degiskentipleris.Add(degiskentipleri);
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
        [HttpGet("DegiskenTipleriForm/Update/{id:int}")]
        public IActionResult  Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.DegiskenTipleri = context.Degiskentipleris.AsNoTracking().Where(degisken => degisken.Id == id).First();
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
        [HttpPost("DegiskenTipleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Degiskentipleri degiskentipleri)
        {
            if (degiskentipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.DegiskenTipleri = degiskentipleri;
                ViewBag.SubmitButtonValue = "Güncelle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        context.Degiskentipleris.AsNoTracking().Where(degisken=> degisken.Id == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    if (context.Degiskentipleris.AsNoTracking().Any(degisken => degisken.DegiskenAdi == degiskentipleri.DegiskenAdi && degisken.Id != id))
                    {
                        ViewBag.error = "Aynı İsimde Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Degiskentipleris.Update(degiskentipleri);
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
                        context.Degiskentipleris.Remove(context.Degiskentipleris.Where(degisken => degisken.Id== id).First());

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
