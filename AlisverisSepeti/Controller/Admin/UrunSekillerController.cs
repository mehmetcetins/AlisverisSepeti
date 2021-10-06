using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunSekilleri",Name = "Urunsekilleri")]
    public class UrunSekillerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunSekilleri/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunSekilleri/Detail.cshtml";
        private readonly string FormCS= "~/Views/AdminPanel/UrunSekilleri/UrunSekillerForm.cshtml";
        
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunSekiller = context.Urunsekilleris
                    .AsNoTracking()
                    .Include(sekil => sekil.EkleyenNavigation)
                    .Include(sekil => sekil.GuncelleyenNavigation)
                    .Include(sekil => sekil.SilenNavigation)
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
                    ViewBag.UrunSekiller = context.Urunsekilleris
                        .AsNoTracking()
                        .Where(sekil => sekil.UrunSekilId == id)
                        .Include(sekil=> sekil.EkleyenNavigation)
                        .Include(sekil => sekil.GuncelleyenNavigation)
                        .Include(sekil => sekil.SilenNavigation)
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
        [HttpGet("UrunSekillerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunSekiller = new Models.Urunsekilleri();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);  
        }
        #endregion
        #region Post.Add
        [HttpPost("UrunSekillerForm/Add")]
        public IActionResult Add(IFormFile KategoriLogo, Models.Urunsekilleri urunsekiller)
        {
            if (urunsekiller == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunSekiller = urunsekiller;
                    if (context.Urunsekilleris.AsNoTracking().Any(sekil => sekil.UrunSekilAdi == urunsekiller.UrunSekilAdi))
                    {
                        ViewBag.error = "Aynı İsimde Kategori Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        var firstUser = context.Users.AsNoTracking().First();
                        urunsekiller.Ekleyen = firstUser.KullaniciIsim;
                        urunsekiller.EkleyenId = firstUser.UserId;
                        urunsekiller.EklenmeTarihi = DateTime.Now.ToString();
                        
                        try
                        {
                            context.Urunsekilleris.Add(urunsekiller);
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
        #region Get.Update
        [HttpGet("UrunSekillerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunSekiller = context.Urunsekilleris
                       .AsNoTracking()
                       .Where(sekil => sekil.UrunSekilId == id)
                       .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
               
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("UrunSekillerForm/Update/{id:int}")]
        public IActionResult Update(int id,IFormFile KategoriLogo,Models.Urunsekilleri urunsekiller)
        {
            if (urunsekiller == null)
            {
                TempData["error"] = "Beklenmedik Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urunsekilleri previousSekil;
                    try
                    {
                        previousSekil = context.Urunsekilleris.AsNoTracking().Where(sekil => sekil.UrunSekilId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunSekiller = urunsekiller;
                    if (context.Urunsekilleris.AsNoTracking().Any(sekil => sekil.UrunSekilAdi == urunsekiller.UrunSekilAdi && sekil.UrunSekilId != id))
                    {
                        ViewBag.error = "Aynı İsimde Kategori Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        urunsekiller.Ekleyen = previousSekil.Ekleyen;
                        urunsekiller.EkleyenId = previousSekil.EkleyenId;
                        urunsekiller.EklenmeTarihi = previousSekil.EklenmeTarihi;
                        if (urunsekiller.Silindimi == false)
                        {
                            urunsekiller.Silen = null;
                            urunsekiller.SilenId = null;
                        }
                        else
                        {
                            urunsekiller.Silen = previousSekil.Silen;
                            urunsekiller.SilenId = previousSekil.SilenId;
                        }

                        
                        urunsekiller.Guncelleyen = previousSekil.Ekleyen;
                        urunsekiller.GuncelleyenId = previousSekil.EkleyenId;
                        urunsekiller.GuncellenmeTarihi = DateTime.Now.ToString();
                        
                        try
                        {
                            urunsekiller.UrunSekilId = id;
                            context.Urunsekilleris.Update(urunsekiller);
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
                Models.Urunsekilleri urunsekil;
                try
                {
                    var firstUser = context.Users.AsNoTracking().First();
                    urunsekil = context.Urunsekilleris.Where(sekil => sekil.UrunSekilId == id).First();
                    urunsekil.Silindimi = true;

                    urunsekil.Silen = firstUser.KullaniciIsim;
                    urunsekil.SilenId = firstUser.UserId;
                    
                }
                catch(InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return new EmptyResult();
                }
                try
                {
                    context.Urunsekilleris.Update(urunsekil);
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
