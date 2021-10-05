using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunKategorilerDil",Name ="UrunkategorilerDil")]
    public class UrunKategorilerDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunKategorilerDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunKategorilerDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunKategorilerDil/UrunKategorilerDilForm.cshtml";
        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
                ViewBag.Kategoriler = context.Urunkategorilers.AsNoTracking().ToList();
            }
        }
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunKategorilerDil = context.UrunkategorilerDils
                    .AsNoTracking()
                    .Include(kategoridil => kategoridil.Kategori)
                    .Include(kategoridil => kategoridil.Dil)
                    .ToList();
            }
            return View(IndexCS);
        }
        [HttpGet("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunKategorilerDil = context.UrunkategorilerDils
                        .AsNoTracking()
                        .Where(kategoridil => kategoridil.KategoriDilId == id)
                        .Include(kategoridil => kategoridil.Dil)
                        .Include(kategoridil => kategoridil.Kategori.EkleyenNavigation)
                        .Include(kategoridil => kategoridil.Kategori.GuncelleyenNavigation)
                        .Include(kategoridil => kategoridil.Kategori.Pkategori)
                        .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi";
                    return RedirectToAction("Index");
                }
                
            }
            return View(DetailCS);
        }
        [HttpGet("UrunKategorilerDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunKategorilerDil = new Models.UrunkategorilerDil();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("UrunKategorilerDilForm/Add")]
        public IActionResult Add(Models.UrunkategorilerDil urunkategorilerDil)
        {
            if (urunkategorilerDil == null)
            {
                
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunKategorilerDil = urunkategorilerDil;
                    extras();
                    try
                    {
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == urunkategorilerDil.DilId).First();
                        context.Urunkategorilers.AsNoTracking().Where(kategori => kategori.KategoriId == urunkategorilerDil.KategoriId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Diller veya Kategori Bulunamadi.";
                        return View(FormCS);
                    }
                    if (context.UrunkategorilerDils.AsNoTracking().Any(kategoridil => 
                            (
                                (
                                    kategoridil.KategoriAdi == urunkategorilerDil.KategoriAdi
                                    &&
                                    kategoridil.KategoriId == urunkategorilerDil.KategoriId
                                    &&
                                    kategoridil.DilId == urunkategorilerDil.DilId
                                )
                                ||
                                (
                                    kategoridil.KategoriId == urunkategorilerDil.KategoriId
                                    &&
                                    kategoridil.DilId == urunkategorilerDil.DilId
                                )
                            )
                        )    
                    )
                    {
                        ViewBag.error = "Bu Aynı Dilde Bu İsimde Kategori Adı Mevcut";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.UrunkategorilerDils.Add(urunkategorilerDil);
                            context.SaveChanges();
                        }catch(DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        [HttpGet("UrunKategorilerDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunKategorilerDil = context.UrunkategorilerDils.AsNoTracking().Where(kategoridil => kategoridil.KategoriDilId == id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            extras();
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        [HttpPost("UrunKategorilerDilForm/Update/{id:int}")]
        public IActionResult Update (int id ,Models.UrunkategorilerDil urunkategorilerDil)
        {
            if (urunkategorilerDil == null)
            {

            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunKategorilerDil = urunkategorilerDil;
                    extras();
                    try
                    {
                        context.UrunkategorilerDils.AsNoTracking().Where(kategoridil=> kategoridil.KategoriDilId == id).First();
                        context.Dillers.AsNoTracking().Where(dil => dil.DilId == urunkategorilerDil.DilId).First();
                        context.Urunkategorilers.AsNoTracking().Where(kategori => kategori.KategoriId == urunkategorilerDil.KategoriId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Diller veya Kategori Bulunamadi.";
                        return View(FormCS);
                    }
                    if (context.UrunkategorilerDils.AsNoTracking().Any(kategoridil =>
                            (
                                (
                                    (
                                        kategoridil.KategoriAdi == urunkategorilerDil.KategoriAdi
                                        &&
                                        kategoridil.KategoriId == urunkategorilerDil.KategoriId
                                        &&
                                        kategoridil.DilId == urunkategorilerDil.DilId
                                    )
                                    ||
                                    (
                                        kategoridil.KategoriId == urunkategorilerDil.KategoriId
                                        &&
                                        kategoridil.DilId == urunkategorilerDil.DilId
                                    )
                                )
                                &&
                                    kategoridil.KategoriDilId != id
                            )
                        )
                    )
                    {
                        ViewBag.error = "Bu Aynı Dilde Bu İsimde Kategori Adı Mevcut";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            urunkategorilerDil.KategoriDilId = id;
                            context.UrunkategorilerDils.Update(urunkategorilerDil);
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

        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.UrunkategorilerDils.Remove(context.UrunkategorilerDils.Where(kategoridil => kategoridil.KategoriDilId == id).First());

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
    }
}
