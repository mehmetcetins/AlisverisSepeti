using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OpsiyonTipleri",Name = "Opsiyontipleri")]
    public class OpsiyonTipleriController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/OpsiyonTipleri/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/OpsiyonTipleri/OpsiyonTipleriForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("OpsiyonTipleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.OpsiyonTipleri = new Models.Opsiyontipleri();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("OpsiyonTipleriForm/Add")]
        public IActionResult Add(Models.Opsiyontipleri opsiyontipleri)
        {
            if (opsiyontipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.OpsiyonTipleri = opsiyontipleri;
                ViewBag.SubmitButtonValue = "Ekle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Opsiyontipleris.AsNoTracking().Any(tip => tip.Ismi == opsiyontipleri.Ismi))
                    {
                        ViewBag.error = "Aynı İsimde Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Opsiyontipleris.Add(opsiyontipleri);
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
        [HttpGet("OpsiyonTipleriForm/Update/{id:int}")]
        public IActionResult  Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OpsiyonTipleri = context.Opsiyontipleris.AsNoTracking().Where(tip => tip.Id == id).First();
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
        [HttpPost("OpsiyonTipleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Opsiyontipleri opsiyontipleri)
        {
            if (opsiyontipleri == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.OpsiyonTipleri = opsiyontipleri;
                ViewBag.SubmitButtonValue = "Güncelle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        context.Opsiyontipleris.AsNoTracking().Where(tip=> tip.Id == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    if (context.Opsiyontipleris.AsNoTracking().Any(tip => tip.Ismi == opsiyontipleri.Ismi && tip.Id != id))
                    {
                        ViewBag.error = "Aynı İsimde Başka Bir Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Opsiyontipleris.Update(opsiyontipleri);
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
                        context.Opsiyontipleris.Remove(context.Opsiyontipleris.Where(tip => tip.Id== id).First());

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
                    TempData["error"] = "Silme Sırasında Bir Hata Oluştu. " + e.InnerException.Message;
                    return new EmptyResult();
                }
            }
            TempData["success"] = "Başarıyla Silindi.";
            return new EmptyResult();
        }
        #endregion
    }
}
