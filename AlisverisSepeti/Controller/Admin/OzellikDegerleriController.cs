using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzellikDegerleri",Name ="Ozellikdegerleri")]
    public class OzellikdegerleriController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzellikDegerleri/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/OzellikDegerleri/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzellikDegerleri/OzellikDegerleriForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Ozellikler = context.Ozelliklers
                    .AsNoTracking()
                    .Include(ozellik => ozellik.OzelliklerDils.Where(ozellikdil => ozellikdil.Dil.Varsayilanmi == true))
                    .ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikDegerleri = context.Ozellikdegerleris
                    .AsNoTracking()
                    .Include(deger => deger.Ozellik.OzelliklerDils.Where(ozellikdil => ozellikdil.Dil.Varsayilanmi == true))
                    .Include(deger => deger.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
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
                    ViewBag.OzellikDegerleri = context.Ozellikdegerleris
                    .AsNoTracking()
                    .Where(deger => deger.OzellikDegerId == id)
                    .Include(deger => deger.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                    .Include(deger => deger.Ozellik.OzelliklerDils.Where(ozellikdil => ozellikdil.Dil.Varsayilanmi == true))
                    .Include(deger => deger.Ozellik.OzellikGrup.OzellikgrupDils.Where(grupdil => grupdil.Dil.Varsayilanmi == true))
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
        [HttpGet("OzellikDegerleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            ViewBag.OzellikDegerleri = new Models.Ozellikdegerleri();
            return View(FormCS);
        }
        [HttpPost("OzellikDegerleriForm/Add")]
        public IActionResult Add(Models.Ozellikdegerleri ozelliklerDil)
        {
            if(ozelliklerDil== null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.OzellikDegerleri = ozelliklerDil;
                    extras();
                    try
                    {
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == ozelliklerDil.OzellikId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    
                    try
                    {
                        ozelliklerDil.EklenmeTarihi = DateTime.Now.ToString();
                        context.Ozellikdegerleris.Add(ozelliklerDil);
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
        [HttpGet("OzellikDegerleriForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OzellikDegerleri = context.Ozellikdegerleris.AsNoTracking().Where(deger => deger.OzellikDegerId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            
            return View(FormCS);
        }
        [HttpPost("OzellikDegerleriForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Ozellikdegerleri ozelliklerDil)
        {
            if (ozelliklerDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.Ozellikdegerleri = ozelliklerDil;
                    extras();
                    Models.Ozellikdegerleri previousEntry;
                    try
                    {
                        previousEntry = context.Ozellikdegerleris.AsNoTracking().Where(deger => deger.OzellikDegerId == id).First();
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == ozelliklerDil.OzellikId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    
                    try
                    {

                        ozelliklerDil.EklenmeTarihi = previousEntry.EklenmeTarihi;
                        ozelliklerDil.GuncellenmeTarihi = DateTime.Now.ToString();
                        ozelliklerDil.OzellikDegerId = id;
                        context.Ozellikdegerleris.Update(ozelliklerDil);
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
                        context.Ozellikdegerleris.Remove(context.Ozellikdegerleris.Where(deger => deger.OzellikDegerId == id).First());

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
