using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzellikDegerDil",Name ="OzellikdegerleriDil")]
    public class OzellikDegerDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzellikDegerDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/OzellikDegerDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzellikDegerDil/OzellikDegerDilForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
                ViewBag.OzellikDegerleri = context.Ozellikdegerleris
                    .AsNoTracking()
                    .Include(deger => deger.Ozellik.OzelliklerDils.Where(ozellikdil => ozellikdil.Dil.Varsayilanmi == true))
                    .ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikDegerDil = context.OzellikdegerleriDils
                    .AsNoTracking()
                    .Include(dil => dil.Dil)
                    .Include(degerdil =>degerdil.OzellikDegerNavigation.Ozellik)
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
                    ViewBag.OzellikDegerDil = context.OzellikdegerleriDils
                    .AsNoTracking()
                    .Where(degerdil => degerdil.OzellikDegerDilId == id)
                    .Include(degerdil => degerdil.Dil)
                    .Include(degerdil => degerdil.OzellikDegerNavigation.Ozellik.OzellikGrup.OzellikgrupDils.Where(grupdil => grupdil.Dil.Varsayilanmi == true))
                    .Include(degerdil => degerdil.OzellikDegerNavigation.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
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
        #region Add
        [HttpGet("OzellikDegerDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            ViewBag.OzellikDegerDil = new Models.OzellikdegerleriDil();
            return View(FormCS);
        }
        [HttpPost("OzellikDegerDilForm/Add")]
        public IActionResult Add(Models.OzellikdegerleriDil ozellikdegerDil)
        {
            if(ozellikdegerDil== null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.OzellikDegerDil = ozellikdegerDil;
                    extras();
                    try
                    {
                        context.Ozellikdegerleris.AsNoTracking().Where(deger=> deger.OzellikDegerId == ozellikdegerDil.OzellikDegerId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozellikdegerDil.DilId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                   
                    try
                    {
                        context.OzellikdegerleriDils.Add(ozellikdegerDil);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                    
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("OzellikDegerDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OzellikDegerDil = context.OzellikdegerleriDils.AsNoTracking().Where(degerdil => degerdil.OzellikDegerDilId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            
            return View(FormCS);
        }
        [HttpPost("OzellikDegerDilForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.OzellikdegerleriDil ozellikdegerDil)
        {
            if (ozellikdegerDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.OzellikDegerDil = ozellikdegerDil;
                    extras();
                    try
                    {
                        context.OzellikdegerleriDils.AsNoTracking().Where(degerdil => degerdil.OzellikDegerDilId == id).First();
                        context.Ozellikdegerleris.AsNoTracking().Where(deger => deger.OzellikDegerId == ozellikdegerDil.OzellikDegerId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozellikdegerDil.DilId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    
                    try
                    {
                        ozellikdegerDil.OzellikDegerDilId = id;
                        context.OzellikdegerleriDils.Update(ozellikdegerDil);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
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
                        context.Remove(context.OzellikdegerleriDils.Where(degerdil => degerdil.OzellikDegerDilId == id).First());

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
