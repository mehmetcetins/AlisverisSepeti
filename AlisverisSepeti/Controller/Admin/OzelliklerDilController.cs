using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzelliklerDil",Name ="OzelliklerDil")]
    public class OzelliklerDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzelliklerDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/OzelliklerDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzelliklerDil/OzelliklerDilForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
                ViewBag.Ozellikler = context.Ozelliklers.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzelliklerDil = context.OzelliklerDils
                    .AsNoTracking()
                    .Include(dil => dil.Dil)
                    .Include(dil => dil.Ozellik)
                    .Include(dil => dil.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                    .OrderBy(dil => dil.OzellikDilId)
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
                    ViewBag.OzelliklerDil = context.OzelliklerDils
                    .AsNoTracking()
                    .Where(ozellikdil => ozellikdil.OzellikDilId == id)
                    .Include(ozellikdil => ozellikdil.Dil)
                    .Include(ozellikdil => ozellikdil.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                    .Include(ozellikdil => ozellikdil.Ozellik.OzellikGrup.OzellikgrupDils.Where(dil => dil.Dil.Varsayilanmi == true))
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
        [HttpGet("OzelliklerDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            ViewBag.OzelliklerDil = new Models.OzelliklerDil();
            return View(FormCS);
        }
        [HttpPost("OzelliklerDilForm/Add")]
        public IActionResult Add(Models.OzelliklerDil ozelliklerDil)
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
                    ViewBag.OzelliklerDil = ozelliklerDil;
                    extras();
                    try
                    {
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == ozelliklerDil.OzellikId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozelliklerDil.DilId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.OzelliklerDils.AsNoTracking().Any(
                        ozellikdil => 
                        (
                            (
                            ozellikdil.OzellikAdi == ozelliklerDil.OzellikAdi
                            &&
                            ozellikdil.DilId == ozelliklerDil.DilId
                            )
                            ||
                            (
                            ozellikdil.OzellikId == ozelliklerDil.OzellikId
                            &&
                            ozellikdil.DilId == ozelliklerDil.DilId
                            )
                        )
                        ))
                    {
                        ViewBag.error = "Aynı Dilde Bu İsimde Ozellik Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.OzelliklerDils.Add(ozelliklerDil);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
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
        [HttpGet("OzelliklerDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OzelliklerDil = context.OzelliklerDils.AsNoTracking().Where(ozellikdil => ozellikdil.OzellikDilId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            
            return View(FormCS);
        }
        [HttpPost("OzelliklerDilForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.OzelliklerDil ozelliklerDil)
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
                    ViewBag.OzelliklerDil = ozelliklerDil;
                    extras();
                    try
                    {
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == ozelliklerDil.OzellikId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozelliklerDil.DilId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.OzelliklerDils.AsNoTracking().Any(
                        ozellikdil =>
                        (
                            (
                                (
                                ozellikdil.OzellikAdi == ozelliklerDil.OzellikAdi
                                &&
                                ozellikdil.DilId != ozelliklerDil.DilId
                                &&
                                ozellikdil.OzellikId == ozelliklerDil.OzellikId
                                )
                                ||
                                (
                                ozellikdil.OzellikId == ozelliklerDil.OzellikId
                                &&
                                ozellikdil.DilId == ozelliklerDil.DilId
                                )
                            )
                            &&
                            ozellikdil.OzellikDilId != id
                        )
                        
                         
                        ))
                    {
                        ViewBag.error = "Aynı Dilde Bu İsimde Ozellik Adı Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            ozelliklerDil.OzellikDilId = id;
                            context.OzelliklerDils.Update(ozelliklerDil);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
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
                        context.OzelliklerDils.Remove(context.OzelliklerDils.Where(ozellikdil => ozellikdil.OzellikDilId == id).First());

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
