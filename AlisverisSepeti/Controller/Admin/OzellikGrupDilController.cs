using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzellikGrupDil",Name ="OzellikgrupDil")]
    public class OzellikGrupDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzellikGrupDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/OzellikGrupDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzellikGrupDil/OzellikGrupDilForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
                ViewBag.OzellikGrup = context.Ozellikgrups.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikGrupDil = context.OzellikgrupDils
                    .AsNoTracking()
                    .Include(dil => dil.Dil)
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
                    ViewBag.OzellikGrupDil = context.OzellikgrupDils
                    .AsNoTracking()
                    .Where(grupdil => grupdil.OzellikGrupDilId == id)
                    .Include(grupdil => grupdil.Dil)
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
        [HttpGet("OzellikGrupDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            ViewBag.OzellikGrupDil = new Models.OzellikgrupDil();
            return View(FormCS);
        }
        [HttpPost("OzellikGrupDilForm/Add")]
        public IActionResult Add(Models.OzellikgrupDil ozellikgrupDil)
        {
            if(ozellikgrupDil== null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.OzellikGrupDil = ozellikgrupDil;
                    extras();
                    try
                    {
                        context.Ozellikgrups.AsNoTracking().Where(grup => grup.OzellikGrupId == ozellikgrupDil.OzellikGrupId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozellikgrupDil.DilId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.OzellikgrupDils.AsNoTracking().Any(grupdil => grupdil.OzellikGrupAdi == ozellikgrupDil.OzellikGrupAdi && grupdil.DilId == ozellikgrupDil.DilId))
                    {
                        ViewBag.error = "Aynı Dilde Bu İsimde Grup Adı Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.OzellikgrupDils.Add(ozellikgrupDil);
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
        [HttpGet("OzellikGrupDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OzellikGrupDil = context.OzellikgrupDils.AsNoTracking().Where(grupdil => grupdil.OzellikGrupDilId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            
            return View(FormCS);
        }
        [HttpPost("OzellikGrupDilForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.OzellikgrupDil ozellikgrupDil)
        {
            if (ozellikgrupDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.OzellikGrupDil = ozellikgrupDil;
                    extras();
                    try
                    {
                        context.Ozellikgrups.AsNoTracking().Where(grup => grup.OzellikGrupId == ozellikgrupDil.OzellikGrupId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == ozellikgrupDil.DilId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.OzellikgrupDils.AsNoTracking().Any(
                        grupdil =>
                        (
                        grupdil.OzellikGrupAdi == ozellikgrupDil.OzellikGrupAdi
                        &&
                        grupdil.DilId == ozellikgrupDil.DilId
                        &&
                        grupdil.OzellikGrupDilId != id
                        )
                        ))
                    {
                        ViewBag.error = "Aynı Dilde Bu İsimde Grup Adı Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            ozellikgrupDil.OzellikGrupDilId = id;
                            context.OzellikgrupDils.Update(ozellikgrupDil);
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
                        context.OzellikgrupDils.Remove(context.OzellikgrupDils.Where(grupdil => grupdil.OzellikGrupDilId == id).First());

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
