using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Lang",Name = "Lang")]
    public class LangController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/Lang/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/Lang/LangForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Lang = context.Langs.AsNoTracking().ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("LangForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Lang = new Models.Lang();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("LangForm/Add")]
        public IActionResult Add(Models.Lang lang)
        {
            if (lang == null)
            {
                TempData["error"] = "Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context= new Models.AlisverisSepetiContext())
                {
                    ViewBag.Lang = lang;
                    ViewBag.SubmitButtonValue = "Ekle";
                    if (context.Langs.AsNoTracking().Any(l => l.Title2 == lang.Title2))
                    {
                        ViewBag.error = "Aynı Lang İçin Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Langs.Add(lang);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                        }

                    }
                }

            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("LangForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Lang = context.Langs.AsNoTracking().Where(l => l.Id == id).First();
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
        [HttpPost("LangForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Lang lang)
        {
            if (lang == null)
            {
                TempData["error"] = "Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.Lang = lang;
                    ViewBag.SubmitButtonValue = "Güncelle";
                    if (context.Langs.AsNoTracking().Any(l => l.Title2 == lang.Title2 && l.Id != id))
                    {
                        ViewBag.error = "Aynı Lang İçin Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Langs.Update(lang);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Güncelleme Sırasında Bir Hata Oluştu.";
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
                        context.Langs.Remove(context.Langs.Where(l => l.Id == id).First());

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
