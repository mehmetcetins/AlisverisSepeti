using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/UrunOzellikler",Name ="Urunozellikleri")]
    public class UrunOzellikleriController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/UrunOzellikler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/UrunOzellikler/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/UrunOzellikler/UrunOzelliklerForm.cshtml";
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Ozellikler = context.Ozelliklers
                    .AsNoTracking()
                    .Include(ozellik => ozellik.OzelliklerDils.Where(ozellikdil => ozellikdil.Dil.Varsayilanmi == true))
                    .ToList();
                ViewBag.Urunler = context.Urunlers
                    .AsNoTracking()
                    .Include(urun => urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
                    .ToList();
            }
        }
        #region Index

        public IActionResult Index()
        {
            using (var context=new Models.AlisverisSepetiContext())
            {
                ViewBag.UrunOzellikler = context.Urunozellikleris
                    .AsNoTracking()
                    .Include(urunoz => urunoz.Urun.UrunlerDils.Where(urundil => urundil.Dil.Varsayilanmi == true))
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
                ViewBag.UrunOzellikler = context.Urunozellikleris
                    .AsNoTracking()
                    .Include(urunoz=>urunoz.Urun.UrunTipiNavigation)
                    .Include(urunoz=>urunoz.Urun.Marka)
                    .Include(urunoz=>urunoz.Urun.StokDurum)
                    .Include(urunoz=>urunoz.Urun.EkleyenNavigation)
                    .Include(urunoz=>urunoz.Urun.GuncelleyenNavigation)
                    .Include(urunoz=>urunoz.Ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                    .Include(urunoz=>urunoz.Ozellik.OzellikGrup.OzellikgrupDils.Where(grupoz=>grupoz.Dil.Varsayilanmi == true))
                    .First();
            }
            return View(DetailCS);
        }
        #endregion
        #region Form
        #region Get.Add
        [HttpGet("UrunOzelliklerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.UrunOzellikler = new Models.Urunozellikleri();
            ViewBag.SubmitButtonValue = "Ekle";

            extras();
            return View(FormCS);
        }
        #endregion
        #region Post.Add
        [HttpPost("UrunOzelliklerForm/Add")]
        public IActionResult Add(Models.Urunozellikleri urunozellikleri)
        {
            if (urunozellikleri == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.UrunOzellikler = urunozellikleri;
                    ViewBag.SubmitButtonValue = "Ekle";
                    extras();
                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunozellikleri.UrunId).First();
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == urunozellikleri.OzellikId).First();
                    }
                    catch(InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    if (context.Urunozellikleris.AsNoTracking().Any(urunoz=> urunoz.OzellikAdi == urunozellikleri.OzellikAdi))
                    {
                        ViewBag.error = "Bu Ozellik Adında Kayıt Bulunuyor.";
                        return View(FormCS);
                    }
                    else
                    {
                        try
                        {
                            context.Urunozellikleris.Add(urunozellikleri);
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
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Get.Update
        [HttpGet("UrunOzelliklerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.UrunOzellikler = context.Urunozellikleris.Where(urunoz => urunoz.UrunOzellikId == id).First();
                }
                catch (InvalidOperationException e) {
                    TempData["error"] = "Kayıt Bulunamadi. "+ e.Message;
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("UrunOzelliklerForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Urunozellikleri urunozellikleri)
        {
            if (urunozellikleri == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.UrunOzellikler = urunozellikleri;
                    ViewBag.SubmitButtonValue = "Güncelle";
                    extras();
                    try
                    {
                        context.Urunozellikleris.AsNoTracking().Where(urunoz => urunoz.UrunOzellikId == id).First();
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == urunozellikleri.UrunId).First();
                        context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == urunozellikleri.OzellikId).First();
                    }
                    catch (InvalidOperationException e)
                    {
                        ViewBag.error = e.Message;
                        return View(FormCS);
                    }
                    
                    try
                    {
                        urunozellikleri.UrunOzellikId = id;
                        context.Urunozellikleris.Update(urunozellikleri);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
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
                        context.Urunozellikleris.Remove(context.Urunozellikleris.Where(urunoz => urunoz.UrunOzellikId == id).First());

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
