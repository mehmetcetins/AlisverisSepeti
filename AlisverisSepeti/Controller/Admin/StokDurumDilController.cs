using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/StokDurumDil",Name ="StokdurumDil")]
    public class StokDurumDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/StokDurumDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/StokDurumDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/StokDurumDil/StokDurumDilForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
                ViewBag.StokDurum = context.Stokdurums.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.StokDurumDil = context.StokdurumDils
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
                    ViewBag.StokDurumDil = context.StokdurumDils
                    .AsNoTracking()
                    .Where(stokdil => stokdil.StokDurumDilId == id)
                    .Include(stokdil => stokdil.Dil)
                    .Include(stokdil => stokdil.StokDurumNavigation)
                    .Include(stokdil => stokdil.StokDurumNavigation.EkleyenNavigation)
                    .Include(stokdil => stokdil.StokDurumNavigation.GuncelleyenNavigation)
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
        [HttpGet("StokDurumDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            ViewBag.StokDurumDil = new Models.StokdurumDil();
            return View(FormCS);
        }
        [HttpPost("StokDurumDilForm/Add")]
        public IActionResult Add(Models.StokdurumDil stokdurumDil)
        {
            if(stokdurumDil== null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.StokDurumDil = stokdurumDil;
                    extras();
                    try
                    {
                        context.Stokdurums.AsNoTracking().Where(stok => stok.StokDurumId == stokdurumDil.StokDurumId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == stokdurumDil.DilId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.StokdurumDils.AsNoTracking().Any(
                        stokdil => 
                        (
                            (
                            stokdil.StokDurum == stokdurumDil.StokDurum
                            &&
                            stokdil.DilId == stokdurumDil.DilId
                            )
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
                            context.StokdurumDils.Add(stokdurumDil);
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
        [HttpGet("StokDurumDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.StokDurumDil = context.StokdurumDils.AsNoTracking().Where(stokdil => stokdil.StokDurumDilId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            
            return View(FormCS);
        }
        [HttpPost("StokDurumDilForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.StokdurumDil stokdurumDil)
        {
            if (stokdurumDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.StokDurumDil = stokdurumDil;
                    extras();
                    try
                    {
                        context.Stokdurums.AsNoTracking().Where(stok => stok.StokDurumId == stokdurumDil.StokDurumId).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == stokdurumDil.DilId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.StokdurumDils.AsNoTracking().Any(
                        stokdil =>
                        (
                            (
                                (
                                    stokdil.StokDurum == stokdurumDil.StokDurum
                                    &&
                                    stokdil.DilId == stokdurumDil.DilId
                                )
                                
                            )
                            &&
                                stokdil.StokDurumDilId != id
                        )
                        ))
                    {
                        ViewBag.error = "Aynı Dilde Bu İsimde Stok Adı Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            stokdurumDil.StokDurumDilId = id;
                            context.StokdurumDils.Update(stokdurumDil);
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
                        context.StokdurumDils.Remove(context.StokdurumDils.Where(stokdil => stokdil.StokDurumDilId == id).First());

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
