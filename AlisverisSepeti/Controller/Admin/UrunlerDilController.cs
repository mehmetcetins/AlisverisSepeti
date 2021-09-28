using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunlerDil",Name ="UrunlerDil")]
    public class UrunlerDilController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunlerDil/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunlerDil/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunlerDil/UrunlerDilForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers.AsNoTracking().ToList();
                ViewBag.Diller = context.Dillers.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunlerDil = context.UrunlerDils
                    .AsNoTracking()
                    .OrderByDescending(urundil=> urundil.UrunDilId)
                    .Include(urundil => urundil.Dil)
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
                    
                    ViewBag.UrunlerDil = context.UrunlerDils
                        .AsNoTracking()
                        .Where(urundil => urundil.UrunDilId == id)
                        .Include(urundil => urundil.Urun)
                        .Include(urundil=> urundil.Urun.UrunTipiNavigation)
                        .Include(urundil=> urundil.Urun.Marka)
                        .Include(urundil=> urundil.Urun.EkleyenNavigation)
                        .Include(urundil=> urundil.Urun.GuncelleyenNavigation)
                        .Include(urundil=> urundil.Urun.StokDurum)
                        .Include(urundil => urundil.Dil)
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
        [HttpGet("UrunlerDilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunlerDil = new Models.UrunlerDil();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("UrunlerDilForm/Add")]
        public IActionResult Add(Models.UrunlerDil urunlerDil)
        {
            if(urunlerDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UrunlerDil = urunlerDil;
                extras();
                ViewBag.SubmitButtonValue = "Ekle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urunler urun;
                    Models.Diller dil;
                    try
                    {
                        dil = context.Dillers.AsNoTracking().Where(dil => dil.DilId == urunlerDil.DilId).First();
                        urun = context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunlerDil.UrunId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Diller veya Urunler Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {

                        if (context.UrunlerDils.AsNoTracking().Any(

                            urundil =>
                            (
                            urundil.Dil.DilKodu == dil.DilKodu
                            &&
                            urundil.UrunAdi == urunlerDil.UrunAdi
                            )
                            ||
                            (
                            urundil.Dil.DilKodu == dil.DilKodu
                            &&
                            urundil.Urun.UrunKodu == urun.UrunKodu 
                            &&
                            urundil.UrunAdi == urunlerDil.UrunAdi
                            )
                            
                            
                            ))
                        {
                            ViewBag.error = "Aynı Urun Koduna veya Aynı Dil Koduna Sahip Kayıt Bulunuyor";
                            return View(FormCS);
                        }
                        else
                        {
                            context.UrunlerDils.Add(urunlerDil);
                            context.SaveChanges();
                        }
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("UrunlerDilForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunlerDil = context.UrunlerDils.AsNoTracking().Where(urundil => urundil.UrunDilId == id).First();
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
        [HttpPost("UrunlerDilForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.UrunlerDil urunlerDil)
        {
            if (urunlerDil == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UrunlerDil = urunlerDil;
                extras();
                ViewBag.SubmitButtonValue = "Güncelle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urunler urun;
                    Models.Diller dil;
                    try
                    {
                        dil = context.Dillers.AsNoTracking().Where(dil => dil.DilId == urunlerDil.DilId).First();
                        urun = context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunlerDil.UrunId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Diller veya Urunler Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {

                        if (context.UrunlerDils.AsNoTracking().Any(

                            urundil =>
                            (
                            urundil.UrunDilId != id
                            )
                            &&
                            (
                                (
                                urundil.Dil.DilKodu == dil.DilKodu
                                &&
                                urundil.UrunAdi == urunlerDil.UrunAdi
                                )
                                ||
                                (
                                urundil.Dil.DilKodu == dil.DilKodu
                                &&
                                urundil.Urun.UrunKodu == urun.UrunKodu
                                &&
                                urundil.UrunAdi == urunlerDil.UrunAdi
                                )
                            )

                            ))
                        {
                            ViewBag.error = "Aynı Urun Koduna veya Aynı Dil Koduna Sahip Kayıt Bulunuyor";
                            return View(FormCS);
                        }
                        else
                        {
                            urunlerDil.UrunDilId = id;
                            context.UrunlerDils.Update(urunlerDil);
                            context.SaveChanges();
                        }
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
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
        public  IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Remove(context.UrunlerDils.AsNoTracking().Where(urundil => urundil.UrunDilId == id).First()).State = EntityState.Deleted;

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
