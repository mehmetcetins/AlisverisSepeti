using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/SiparisKalemler/",Name = "Sipariskalemler")]
    public class SiparisKalemlerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/SiparisKalemler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/SiparisKalemler/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/SiparisKalemler/SiparisKalemlerForm.cshtml";

        [NonAction]
        private void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Urunler = context.Urunlers
                    .AsNoTracking()
                    .Include(urun => urun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
                    .ToList();
                ViewBag.Siparisler = context.Siparislers.AsNoTracking().ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.SiparisKalemler = context.Sipariskalemlers
                    .AsNoTracking()
                    .Include(kalem => kalem.Urun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
                    .Include(kalem => kalem.Purun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
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
                ViewBag.SiparisKalemler = context.Sipariskalemlers
                    .AsNoTracking()
                    .Where(kalem => kalem.SiparisKalemId == id)
                    .Include(kalem => kalem.Urun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
                    .Include(kalem => kalem.Purun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
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
        #region Form
        #region Get.Add
        [HttpGet("SiparisKalemlerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SiparisKalemler = new Models.Sipariskalemler();
            ViewBag.SubmitButtonValue = "Ekle";
            extras();
            return View(FormCS);
        }

        #endregion
        #region Post.Add
        [HttpPost("SiparisKalemlerForm/Add")]
        public IActionResult Add(Models.Sipariskalemler sipariskalemler)
        {
            if(sipariskalemler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SiparisKalemler = sipariskalemler;
                ViewBag.SubmitButtonValue = "Ekle";
                extras();
                using (var context = new Models.AlisverisSepetiContext())
                {


                    try
                    {
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == sipariskalemler.UrunId).First();
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == sipariskalemler.PurunId).First();
                        context.Siparislers.AsNoTracking().Where(siparis => siparis.SiparisId == sipariskalemler.SiparisId).First();
                    }
                    catch(InvalidOperationException)
                    {
                        ViewBag.error = "Urun Bulunamadi.";
                        return View(FormCS);
                    }

                    try
                    {
                        context.Sipariskalemlers.Add(sipariskalemler);
                        context.SaveChanges();
                    }catch(DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                    
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Get.Update
        [HttpGet("SiparisKalemlerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.SiparisKalemler = context.Sipariskalemlers
                        .AsNoTracking()
                        .Where(kalem => kalem.SiparisKalemId == id)
                        .Include(kalem => kalem.Urun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
                        .Include(kalem => kalem.Purun.UrunlerDils.Where(dil => dil.Dil.Varsayilanmi == true))
                        .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt bulunmadi.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            return View(FormCS);
        }
        #endregion
        #region Post.Update
        [HttpPost("SiparisKalemlerForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Sipariskalemler sipariskalemler)
        {
            if (sipariskalemler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SiparisKalemler = sipariskalemler;
                ViewBag.SubmitButtonValue = "Güncelle";
                extras();
                using (var context = new Models.AlisverisSepetiContext())
                {


                    try
                    {
                        try
                        {
                            context.Sipariskalemlers.AsNoTracking().Where(kalem => kalem.SiparisKalemId == id).First();
                        }
                        catch (InvalidOperationException)
                        {
                            TempData["error"] = "Kayıt Bulunamadi";
                            return RedirectToAction("Index");
                        }
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == sipariskalemler.UrunId).First();
                        context.Urunlers.AsNoTracking().Where(urun => urun.UrunId == sipariskalemler.PurunId).First();
                        context.Siparislers.Where(siparis => siparis.SiparisId == sipariskalemler.SiparisId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Urun Bulunamadi.";
                        return View(FormCS);
                    }

                    try
                    {
                        sipariskalemler.SiparisKalemId = id;
                        context.Sipariskalemlers.Update(sipariskalemler);
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
                        context.Sipariskalemlers.Remove(context.Sipariskalemlers.Where(kalem => kalem.SiparisKalemId == id).First());

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
