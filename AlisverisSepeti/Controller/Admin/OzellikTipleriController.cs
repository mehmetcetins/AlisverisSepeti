using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzellikTipleri",Name ="Ozelliktipleri")]
    public class OzellikTipleriController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzellikTipleri/Index.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzellikTipleri/OzellikTipleriForm.cshtml";
        
        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.DegiskenTipleri = context.Degiskentipleris.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikTipleri = context.Ozelliktipleris
                    .AsNoTracking()
                    .Include(tip => tip.DegiskenTipiNavigation)
                    .ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("OzellikTipleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.OzellikTipleri = new Models.Ozelliktipleri();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("OzellikTipleriForm/Add")]
        public IActionResult Add(Models.Ozelliktipleri ozelliktipleri)
        {
            if (ozelliktipleri == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errorList = new List<string>();
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.OzellikTipleri = ozelliktipleri;
                    extras();
                    Models.Degiskentipleri degisken;
                    try
                    {
                        degisken = context.Degiskentipleris.AsNoTracking().Where(tip => tip.Id == ozelliktipleri.DegiskenTipi).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Degisken Tipi Bulunamadi.";
                        errorList.Add("Degisken Tipi Bulunamadi.");
                        
                    }
                    if(context.Ozelliktipleris.AsNoTracking().Any(tip => tip.OzellikTipi == ozelliktipleri.OzellikTipi))
                    {
                        ViewBag.error = "Aynı Özellik Tipinde Kayıt Bulunuyor.";
                        errorList.Add("Aynı Özellik Tipinde Kayıt Bulunuyor.");
                        
                    }
                    else
                    {
                        try
                        {
                            context.Ozelliktipleris.Add(ozelliktipleri);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Eklendi.";
                            return RedirectToAction("Index");
                        }
                        catch(DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
                            errorList.Add(e.InnerException.Message);
                            
                        }
                    }
                }
                ViewBag.errorList = errorList;
            }
            
            return View(FormCS);
        }
        #endregion
        #region Update
        [HttpGet("OzellikTipleriForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikTipleri = context.Ozelliktipleris
                    .AsNoTracking()
                    .Where(tip => tip.OzellikTipiId == id)
                    .Include(tip => tip.DegiskenTipiNavigation)
                    .First();
            }
            extras();
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        [HttpPost("OzellikTipleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Ozelliktipleri ozelliktipleri)
        {
            if (ozelliktipleri == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errorList = new List<string>();
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.OzellikTipleri = ozelliktipleri;
                    extras();
                    Models.Degiskentipleri degisken;
                    Models.Ozelliktipleri oldTip;
                    try
                    {
                        try
                        {
                            oldTip = context.Ozelliktipleris.AsNoTracking().Where(tip => tip.OzellikTipiId == id).First();
                        }
                        catch (InvalidOperationException)
                        {
                            TempData["error"] = "Kayıt Bulunamadi.";
                            return View(FormCS);
                        }
                        
                        degisken = context.Degiskentipleris.AsNoTracking().Where(tip => tip.Id == ozelliktipleri.DegiskenTipi).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Degisken Tipi Bulunamadi.";
                        errorList.Add("Degisken Tipi Bulunamadi.");
                    }
                    if (context.Ozelliktipleris.AsNoTracking().Any(tip => tip.OzellikTipi == ozelliktipleri.OzellikTipi && ozelliktipleri.OzellikTipiId != id))
                    {
                        ViewBag.error = "Aynı Özellik Tipinde Kayıt Bulunuyor.";
                        errorList.Add("Aynı Özellik Tipinde Kayıt Bulunuyor.");
                    }
                    else
                    {
                        try
                        {
                            ozelliktipleri.OzellikTipiId = id;
                            context.Ozelliktipleris.Update(ozelliktipleri);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Güncellendi.";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
                            errorList.Add(e.InnerException.Message);
                        }
                    }
                }
                ViewBag.errorList = errorList;
            }
            return View(FormCS);
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
                        context.Ozelliktipleris.Remove(context.Ozelliktipleris.Where(tip => tip.OzellikTipiId == id).First());

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
