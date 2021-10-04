using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunKategoriler",Name ="Urunkategoriler")]
    public class UrunKategorilerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunKategoriler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunKategoriler/Detail.cshtml";
        private readonly string FormCS= "~/Views/AdminPanel/UrunKategoriler/UrunKategorilerForm.cshtml";
        private readonly string ImageFolder = "UrunKategoriler";
        [NonAction]
        private void extras(int? id = null)
        {
            using (var context= new Models.AlisverisSepetiContext())
            {
                if(id != null)
                {
                    ViewBag.Kategoriler = context.Urunkategorilers
                        .AsNoTracking()
                        .Where(kategori => kategori.KategoriId != id)
                        .ToList();
                }
                else
                {
                    ViewBag.Kategoriler = context.Urunkategorilers.AsNoTracking().ToList();

                }
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunKategoriler = context.Urunkategorilers
                    .AsNoTracking()
                    .Include(kategoriler => kategoriler.EkleyenNavigation)
                    .Include(kategoriler => kategoriler.Pkategori)
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
                    ViewBag.UrunKategoriler = context.Urunkategorilers
                        .AsNoTracking()
                        .Where(kategori => kategori.KategoriId == id)
                        .Include(kategori=> kategori.EkleyenNavigation)
                        .Include(kategori=> kategori.GuncelleyenNavigation)
                        .Include(kategori=> kategori.Pkategori)
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
        #region Form
        #region Get.Add
        [HttpGet("UrunKategorilerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunKategoriler = new Models.Urunkategoriler();
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            return View(FormCS);  
        }
        #endregion
        #region Post.Add
        [HttpPost("UrunKategorilerForm/Add")]
        public IActionResult Add(IFormFile KategoriLogo, Models.Urunkategoriler urunkategoriler)
        {
            if (urunkategoriler == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunKategoriler = urunkategoriler;
                    extras();
                    if (context.Urunkategorilers.AsNoTracking().Any(kategori => kategori.KategoriAdi == urunkategoriler.KategoriAdi))
                    {
                        ViewBag.error = "Aynı İsimde Kategori Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        var firstUser = context.Users.AsNoTracking().First();
                        urunkategoriler.Ekleyen = firstUser.KullaniciIsim;
                        urunkategoriler.EkleyenId = firstUser.UserId;
                        urunkategoriler.EklenmeTarihi = DateTime.Now.ToString();
                        if (urunkategoriler.PkategoriId != 0)
                        {
                            try
                            {
                                Models.Urunkategoriler pkategori = context.Urunkategorilers.Where(kategori => kategori.KategoriId == urunkategoriler.PkategoriId).First();
                                urunkategoriler.PkategoriId = pkategori.KategoriId;
                                urunkategoriler.Depth = pkategori.Depth + 1;
                                urunkategoriler.PkategoriAdi = pkategori.KategoriAdi;
                            }
                            catch(InvalidOperationException)
                            {
                                ViewBag.error = "Parent Kategori Bulunamadi.";
                                return View(FormCS);
                            }
                            
                            urunkategoriler.Lineage = 0;
                        }
                        else
                        {
                            urunkategoriler.PkategoriId = null;
                        }
                        try
                        {
                            context.Urunkategorilers.Add(urunkategoriler);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = e.InnerException.Message;
                            return View(FormCS);
                        }
                        if (KategoriLogo != null)
                        {

                            urunkategoriler.KategoriLogo = Utils.Utils.ToFileName(KategoriLogo.FileName,urunkategoriler.KategoriId);
                            Utils.ImageUtils.ResizeAndSave(
                                KategoriLogo.OpenReadStream(),
                                200,
                                200,
                                ImageFolder,
                                urunkategoriler.KategoriLogo
                                );
                            try
                            {
                                context.Urunkategorilers.Update(urunkategoriler);
                                context.SaveChanges();
                            }
                            catch(DbUpdateException e)
                            {
                                ViewBag.error = e.InnerException.Message;
                                return View(FormCS);
                            }
                        }
                        
                    }
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Get.Update
        [HttpGet("UrunKategorilerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunKategoriler = context.Urunkategorilers
                       .AsNoTracking()
                       .Where(kategori => kategori.KategoriId == id)
                       .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
               
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            extras(id);
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("UrunKategorilerForm/Update/{id:int}")]
        public IActionResult Update(int id,IFormFile KategoriLogo,Models.Urunkategoriler urunkategoriler)
        {
            if (urunkategoriler == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urunkategoriler previousKategori;
                    try
                    {
                        previousKategori = context.Urunkategorilers.AsNoTracking().Where(kategori => kategori.KategoriId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunKategoriler = urunkategoriler;
                    extras(id);
                    if (context.Urunkategorilers.AsNoTracking().Any(kategori => kategori.KategoriAdi == urunkategoriler.KategoriAdi && kategori.KategoriId != id))
                    {
                        ViewBag.error = "Aynı İsimde Kategori Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        urunkategoriler.Ekleyen = previousKategori.Ekleyen;
                        urunkategoriler.EkleyenId = previousKategori.EkleyenId;
                        urunkategoriler.EklenmeTarihi = previousKategori.EklenmeTarihi;
                        urunkategoriler.Guncelleyen = previousKategori.Ekleyen;
                        urunkategoriler.GuncelleyenId = previousKategori.EkleyenId;
                        urunkategoriler.GuncellenmeTarihi = DateTime.Now.ToString();
                        if (urunkategoriler.PkategoriId != 0)
                        {
                            try
                            {
                                Models.Urunkategoriler pkategori = context.Urunkategorilers.Where(kategori => kategori.KategoriId == urunkategoriler.PkategoriId).First();
                                urunkategoriler.PkategoriId = pkategori.KategoriId;
                                urunkategoriler.Depth = pkategori.Depth + 1;
                                urunkategoriler.PkategoriAdi = pkategori.KategoriAdi;
                            }
                            catch (InvalidOperationException)
                            {
                                ViewBag.error = "Parent Kategori Bulunamadi.";
                                return View(FormCS);
                            }

                            urunkategoriler.Lineage = 0;
                        }
                        else
                        {
                            urunkategoriler.PkategoriId = null;
                        }
                        
                        if (KategoriLogo != null)
                        {

                            urunkategoriler.KategoriLogo = Utils.Utils.ToFileName(KategoriLogo.FileName, urunkategoriler.KategoriId);
                            Utils.ImageUtils.ResizeAndSave(
                                KategoriLogo.OpenReadStream(),
                                200,
                                200,
                                ImageFolder,
                                urunkategoriler.KategoriLogo
                                );
                           
                        }
                        else
                        {
                            urunkategoriler.KategoriLogo = previousKategori.KategoriLogo;
                        }
                        try
                        {
                            urunkategoriler.KategoriId = id;
                            context.Urunkategorilers.Update(urunkategoriler);
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
        #endregion
        #region Delete
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete (int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Urunkategoriler urunkategori;
                try
                {
                    urunkategori = context.Urunkategorilers.Where(kategori => kategori.KategoriId == id).First();
                    urunkategori.Silindimi = true;
                    
                }
                catch(InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return new EmptyResult();
                }
                try
                {
                    context.Urunkategorilers.Update(urunkategori);
                    context.SaveChanges();
                }catch(DbUpdateException e)
                {
                    TempData["error"] = e.InnerException.Message;
                    return new EmptyResult();
                }
            }
            TempData["success"] = "Başarıyla Silindi.";
            return new EmptyResult();
        }
        #endregion
    }
}
