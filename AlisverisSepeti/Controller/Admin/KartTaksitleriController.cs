using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/KartTaksitleri",Name ="Karttaksitleri")]
    public class KartTaksitleriController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/KartTaksitleri/Index.cshtml";
        private string DetailCS = "~/Views/AdminPanel/KartTaksitleri/Detail.cshtml";
        private string FormCS = "~/Views/AdminPanel/KartTaksitleri/KartTaksitForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.KartTaksitleri = context.Karttaksitleris.AsNoTracking().Include(taksit => taksit.Kart).ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Detail
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.KartTaksitleri = context.Karttaksitleris.AsNoTracking().Where(taksit => taksit.TaksitId == id).Include(taksit=> taksit.Kart).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadı.";
                    return RedirectToAction("Index");
                }
                
            }
            return View(DetailCS);
        }
        #endregion
        #region Add
        [HttpGet("KartTaksitForm/Add")]
        public IActionResult Add()
        {
            ViewBag.KartTaksitleri = new Models.Karttaksitleri();
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.KrediKartlari = context.Kredikartlaris.AsNoTracking().ToList();
            }
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("KartTaksitForm/Add")]
        public IActionResult Add(Models.Karttaksitleri karttaksitleri)
        {
            if (karttaksitleri == null)
            {
                TempData["error"] = "Kayıt Sirasında Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.KartTaksitleri = karttaksitleri;
                    ViewBag.KrediKartlari = context.Kredikartlaris.AsNoTracking().ToList();
                    try
                    {
                        karttaksitleri.Kart = context.Kredikartlaris.Where(kart => kart.KartId == karttaksitleri.KartId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Seçilen Kredi Kartı Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        
                        context.Karttaksitleris.Add(karttaksitleri);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        TempData["error"] = "Kayıt Sırasında Bir Hata Oluştu.";
                        return RedirectToAction("Index");
                    }
                    
                }
                TempData["success"] = "Başarıyla Kaydedildi.";
                return RedirectToAction("Index");

            }
        }
        #endregion
        #region Update
        [HttpGet("KartTaksitForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.KartTaksitleri = context.Karttaksitleris.AsNoTracking().Where(taksit=> taksit.TaksitId == id).Include(taksit=> taksit.Kart).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
                ViewBag.KrediKartlari = context.Kredikartlaris.AsNoTracking().ToList();
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        [HttpPost("KartTaksitForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Karttaksitleri karttaksitleri)
        {
            
            if (karttaksitleri == null)
            {
                TempData["error"] = "Kayıt Sirasında Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {

                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.KartTaksitleri = karttaksitleri;
                    ViewBag.KrediKartlari = context.Kredikartlaris.AsNoTracking().ToList();
                    try
                    {
                        karttaksitleri.Kart = context.Kredikartlaris.Where(kart => kart.KartId == karttaksitleri.KartId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Seçilen Kredi Kartı Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        
                        karttaksitleri.TaksitId = id;
                        context.Karttaksitleris.Update(karttaksitleri);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        TempData["error"] = "Güncelleme Sırasında Bir Hata Oluştu.";
                        return RedirectToAction("Index");
                    }

                }
                TempData["success"] = "Başarıyla Güncellendi.";
                return RedirectToAction("Index");

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
                        context.Karttaksitleris.Remove(context.Karttaksitleris.Where(taksit => taksit.TaksitId == id).First());

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
