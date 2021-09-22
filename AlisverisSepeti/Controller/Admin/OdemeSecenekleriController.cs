using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OdemeSecenekleri",Name = "Odemesecenekleri")]
    public class OdemeSecenekleriController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/OdemeSecenekleri/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/OdemeSecenekleri/OdemeSecenekleriForm.cshtml";
        #region Index

        
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OdemeSecenekleri = context.Odemesecenekleris.AsNoTracking().ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("OdemeSecenekleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.OdemeSecenekleri = new Models.Odemesecenekleri();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("OdemeSecenekleriForm/Add")]
        public IActionResult Add(Models.Odemesecenekleri odemesecenekleri)
        {
            if (odemesecenekleri == null)
            {
                TempData["error"] = "Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context= new Models.AlisverisSepetiContext())
                {
                    ViewBag.OdemeSecenekleri = odemesecenekleri;
                    ViewBag.SubmitButtonValue = "Ekle";
                    if (context.Odemesecenekleris.AsNoTracking().Any(odeme => odeme.OdemeSekli == odemesecenekleri.OdemeSekli))
                    {
                        ViewBag.error = "Aynı Odeme Sekli İçin Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Odemesecenekleris.Add(odemesecenekleri);
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
        [HttpGet("OdemeSecenekleriForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OdemeSecenekleri = context.Odemesecenekleris.AsNoTracking().Where(odeme => odeme.Id == id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                
                ViewBag.SubmitButtonValue = "Ekle";
            }

            return View(FormCS);
        }
        [HttpPost("OdemeSecenekleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Odemesecenekleri odemesecenekleri)
        {
            if (odemesecenekleri == null)
            {
                TempData["error"] = "Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.OdemeSecenekleri = odemesecenekleri;
                    ViewBag.SubmitButtonValue = "Güncelle";
                    if (context.Odemesecenekleris.AsNoTracking().Any(odeme => odeme.OdemeSekli == odemesecenekleri.OdemeSekli && odeme.Id != id))
                    {
                        ViewBag.error = "Aynı Odeme Sekli İçin Kayıt Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Odemesecenekleris.Update(odemesecenekleri);
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
                        context.Odemesecenekleris.Remove(context.Odemesecenekleris.Where(odeme => odeme.Id == id).First());

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
