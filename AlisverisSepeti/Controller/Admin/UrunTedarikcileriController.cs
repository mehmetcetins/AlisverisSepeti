using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunTedarikciler", Name ="Uruntedarikcileri")]
    public class UrunTedarikcileriController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunTedarikcileri/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunTedarikcileri/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunTedarikcileri/UrunTedarikcilerForm.cshtml";
        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers
                    .AsNoTracking()
                    .Include(urun => urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
                    .ToList();
                ViewBag.StokDurum = context.Stokdurums
                    .AsNoTracking()
                    .Include(durum => durum.StokdurumDils.Where(stokdil => stokdil.Dil.Varsayilanmi == true))
                    .ToList();
            }
            
        }
        #region Index

        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunTedarikciler = context.Uruntedarikcileris
                    .AsNoTracking()
                    .Include(tedarik => tedarik.User)
                    .Include(tedarik => tedarik.StokDurum.StokdurumDils.Where(stokdurum => stokdurum.Dil.Varsayilanmi == true))
                    .Include(tedarik => tedarik.Urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
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
                    ViewBag.UrunTedarikciler = context.Uruntedarikcileris
                        .AsNoTracking()
                        .Include(tedarik => tedarik.User)
                        .Include(tedarik => tedarik.Urun.UrunTipiNavigation)
                        .Include(tedarik => tedarik.Urun.StokDurum)
                        .Include(tedarik => tedarik.Urun.Marka)
                        .Include(tedarik => tedarik.Urun.EkleyenNavigation)
                        .Include(tedarik => tedarik.Urun.GuncelleyenNavigation)
                        .Include(tedarik => tedarik.StokDurum.StokdurumDils.Where(durumdil => durumdil.Dil.Varsayilanmi == true))
                        .Include(tedarik => tedarik.StokDurum.EkleyenNavigation)
                        .Include(tedarik => tedarik.StokDurum.GuncelleyenNavigation)
                        .First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                }
            }
            return View(DetailCS);
        }
        #endregion
        #region Form
        #region Get.Add
        [HttpGet("UrunTedarikcilerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunTedarikciler = new Models.Uruntedarikcileri();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        #endregion
        #region Post.Add
        [HttpPost("UrunTedarikcilerForm/Add")]
        public IActionResult Add(Models.Uruntedarikcileri uruntedarikcileri)
        {
            if(uruntedarikcileri == null)
            {
                TempData["error"] = "Beklenmeyen bir hata ile karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    extras();
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.UrunTedarikciler = uruntedarikcileri;
                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == uruntedarikcileri.UrunId).First();
                        context.Stokdurums.AsNoTracking().Where(durum => durum.StokDurumId == uruntedarikcileri.StokDurumId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Source + " bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        var firstUser = context.Users.AsNoTracking().First();
                        uruntedarikcileri.UserId = firstUser.UserId;
                        context.Uruntedarikcileris.Add(uruntedarikcileri);
                        context.SaveChanges();
                    }
                    catch(DbUpdateException e)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu: " + e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Kaydedildi.";
            return RedirectToAction("Index");
        }

        #endregion
        #region Get.Update
        [HttpGet("UrunTedarikcilerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunTedarikciler = context.Uruntedarikcileris.AsNoTracking().Where(tedarik => tedarik.UrunTedarikciId == id).First();

            }
            extras();
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("UrunTedarikcilerForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Uruntedarikcileri uruntedarikcileri)
        {
            if (uruntedarikcileri == null)
            {
                TempData["error"] = "Beklenmeyen bir hata ile karşılaşıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    extras();
                    ViewBag.SubmitButtonValue = "Güncelle";
                    ViewBag.UrunTedarikciler = uruntedarikcileri;
                    Models.Uruntedarikcileri previousTedarikci;
                    try
                    {
                        previousTedarikci = context.Uruntedarikcileris.AsNoTracking().Where(tedarik => tedarik.UrunTedarikciId == id).First();
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == uruntedarikcileri.UrunId).First();
                        context.Stokdurums.AsNoTracking().Where(durum => durum.StokDurumId == uruntedarikcileri.StokDurumId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error =  e.Source +" bulunamadi.";
                        return View(FormCS);
                    }
                    try
                    {
                        uruntedarikcileri.UserId = previousTedarikci.UserId;
                        uruntedarikcileri.UrunTedarikciId = id;
                        context.Uruntedarikcileris.Update(uruntedarikcileri);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu: " + e.InnerException.Message;
                        return View(FormCS);
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
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Uruntedarikcileris.Remove(context.Uruntedarikcileris.Where(tedarik => tedarik.UrunTedarikciId == id).First());

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
