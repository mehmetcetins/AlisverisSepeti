using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Urunler",Name = "Urunler")]
    public class UrunlerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/Urunler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/Urunler/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/Urunler/UrunlerForm.cshtml";

        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunTipleri = context.Uruntipleris.AsNoTracking().ToList();
                ViewBag.Markalar = context.Markalars.AsNoTracking().ToList();
                ViewBag.StokDurum = context.Stokdurums.AsNoTracking().ToList();
            }
        }

        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers.AsNoTracking()
                    .Include(urun => urun.EkleyenNavigation)
                    .Include(urun => urun.Marka)
                    .Include(urun => urun.StokDurum)
                    .OrderByDescending(urun => urun.UrunId)
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
                    ViewBag.Urunler = context.Urunlers
                        .AsNoTracking()
                        .Where(urunler => urunler.UrunId == id)
                        .Include(urun => urun.EkleyenNavigation)
                        .Include(urun => urun.GuncelleyenNavigation)
                        .Include(urun => urun.Marka)
                        .Include(urun => urun.StokDurum)
                        .Include(urun => urun.UrunTipiNavigation)
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
        [HttpGet("UrunlerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.Urunler = new Models.Urunler();
            extras();
            return View(FormCS);
        }
        [HttpPost("UrunlerForm/Add")]
        public IActionResult Add(Models.Urunler urunler)
        {
            if(urunler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Urunler = urunler;
                ViewBag.SubmitButtonValue = "Ekle";
                extras();
                using (var context = new Models.AlisverisSepetiContext())
                {
                    
                    var firstUser =  context.Users.AsNoTracking().First();
                    urunler.EkleyenId = firstUser.UserId;
                    urunler.Ekleyen = firstUser.KullaniciIsim;
                    urunler.EklenmeTarihi = DateTime.Now.ToString();
                    try
                    {
                        context.Urunlers.Add(urunler);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Hatali Alanlar Var.";
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("UrunlerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            ViewBag.SubmitButtonValue = "Güncelle";
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Urunler = context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == id).First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                }
            }
            extras();
            return View(FormCS);
        }
        [HttpPost("UrunlerForm/Update/{id:int}")]
        public IActionResult Update (int id,Models.Urunler urunler)
        {
            if (urunler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Urunler = urunler;
                ViewBag.SubmitButtonValue = "Güncelle";
                extras();
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Urunler oldUrun;
                    try
                    {
                         oldUrun = context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    
                    urunler.EkleyenId = oldUrun.EkleyenId;
                    urunler.Ekleyen = oldUrun.Ekleyen;
                    urunler.EklenmeTarihi = oldUrun.EklenmeTarihi;
                    urunler.GuncelleyenId = oldUrun.EkleyenId;
                    urunler.Guncelleyen = oldUrun.Ekleyen;
                    urunler.GuncellenmeTarihi = DateTime.Now.ToString();
                    try
                    {
                        urunler.UrunId = id;
                        context.Urunlers.Update(urunler);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Hatali Alanlar Var.";
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
                        context.Urunlers.Remove(context.Urunlers.Where(urunler => urunler.UrunId== id).First());

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
        #endregion
    }
}
