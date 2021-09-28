using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunDosyalar",Name ="Urundosyalar")]
    public class UrunDosyalarController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunDosyalar/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunDosyalar/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunDosyalar/UrunDosyalarForm.cshtml";
        private readonly string folder = "UrunDosya";

        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunDosyalar = context.Urundosyalars.AsNoTracking().ToList();
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
                    ViewBag.UrunDosyalar = context.Urundosyalars
                        .AsNoTracking()
                        .Where(urundosya => urundosya.UrunDosyaId == id)
                        .Include(dosya => dosya.Urun)
                        .Include(dosya => dosya.Urun.StokDurum)
                        .Include(dosya => dosya.Urun.Marka)
                        .Include(dosya => dosya.Urun.GuncelleyenNavigation)
                        .Include(dosya => dosya.Urun.EkleyenNavigation)
                        .Include(dosya => dosya.Urun.UrunTipiNavigation)
                        .First();
                }
                catch(InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }

            }
            return View(DetailCS);
        }
        #endregion
        #region Add
        [HttpGet("UrunDosyalarForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunDosyalar = new Models.Urundosyalar();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("UrunDosyalarForm/Add")]
        public IActionResult Add(IFormFile FileName,Models.Urundosyalar urundosyalar)
        {
            if (urundosyalar == null)
            {
                TempData["error"] = "Bir Hata İle Karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                   
                    ViewBag.UrunDosyalar = urundosyalar;
                    ViewBag.SubmitButtonValue = "Ekle";
                    extras();
                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urundosyalar.UrunId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Seçilen Ürün Bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        urundosyalar.FileName = FileName.FileName;
                        context.Urundosyalars.Add(urundosyalar);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                        return View(FormCS);
                    }
                    urundosyalar.FileName = Utils.Utils.ToFileName(FileName.FileName, urundosyalar.UrunDosyaId);
                    Utils.ImageUtils.SaveImage(
                        FileName.OpenReadStream(),
                        folder,
                        urundosyalar.FileName
                        );
                    try
                    {
                        context.Urundosyalars.Update(urundosyalar);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
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
        [HttpGet("UrunDosyalarForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunDosyalar = context.Urundosyalars.AsNoTracking().Where(urundosya => urundosya.UrunDosyaId == id).First();
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
        [HttpPost("UrunDosyalarForm/Update/{id:int}")]
        public IActionResult Update(int id, IFormFile FileName, Models.Urundosyalar urundosyalar)
        {
            if (urundosyalar == null)
            {
                TempData["error"] = "Bir Hata İle Karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urundosyalar oldUrunDosya;
                    try
                    {
                        oldUrunDosya = context.Urundosyalars.AsNoTracking().Where(dosya => dosya.UrunDosyaId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    ViewBag.UrunDosyalar = urundosyalar;
                    ViewBag.SubmitButtonValue = "Güncelle";
                    extras();
                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urundosyalar.UrunId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Seçilen Ürün Bulunamadi.";
                        return View(FormCS);
                    }
                    if (FileName != null)
                    {
                        urundosyalar.FileName = Utils.Utils.ToFileName(FileName.FileName,id);
                        Utils.ImageUtils.SaveImage(
                            FileName.OpenReadStream(),
                            folder,
                            urundosyalar.FileName
                            );
                    }
                    else
                    {
                        urundosyalar.FileName = oldUrunDosya.FileName;
                    }
                    try
                    {
                        urundosyalar.UrunDosyaId = id;
                        context.Urundosyalars.Update(urundosyalar);
                        context.SaveChanges();
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
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Urundosyalars.Remove(context.Urundosyalars.Where(dosya => dosya.UrunDosyaId == id).First());

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
