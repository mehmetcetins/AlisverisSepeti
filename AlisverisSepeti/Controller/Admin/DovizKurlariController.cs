using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Models
{
    [Route("admin/DovizKurlari/",Name = "Dovizkurlari")]
    public class DovizKurlariController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/DovizKurlari/Index.cshtml";
        private string DetailCS = "~/Views/AdminPanel/DovizKurlari/Detail.cshtml";
        private string FormCS = "~/Views/AdminPanel/DovizKurlari/DovizKurForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new AlisverisSepetiContext())
            {
                ViewBag.Kur = context.Dovizkurlaris.Include(kurs => kurs.Doviz).ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Detail
        [HttpGet("{id:int}")]
        public IActionResult Detail (int id)
        {
            using (var context = new AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Kur = context.Dovizkurlaris.AsNoTracking().Where(kur => kur.DovizKurId == id).Include(kur => kur.Doviz).First();
                    return View(DetailCS);
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadı.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
        #region Add
        [HttpGet("DovizKurForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.Kur = new Models.Dovizkurlari();
            using (var context = new AlisverisSepetiContext())
            {
                ViewBag.Dovizler = context.Dovizlers.ToList();
            }
                
            return View(FormCS);
        }
        [HttpPost("DovizKurForm/Add")]
        public IActionResult Add(Models.Dovizkurlari dovizkurlari)
        {
            if (dovizkurlari == null)
            {
                TempData["error"] = "Eklenemedi.";
                return RedirectToAction("Index");
            }
            else
            {
                if (dovizkurlari.DovizId < 1)
                {
                    ViewBag.error = "Doviz Kodu Seçilmemiş";
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.Kur = dovizkurlari;
                    return View(FormCS);
                }
                else {
                    using (var context = new AlisverisSepetiContext())
                    {
                        
                        Dovizler doviz;
                        try
                        {
                            doviz =  context.Dovizlers.Where(dovizler => dovizler.DovizId == dovizkurlari.DovizId).First();
                        }
                        catch (InvalidOperationException)
                        {
                            ViewBag.error = "Geçersiz Doviz Kodu";
                            ViewBag.Kur = dovizkurlari;
                            ViewBag.Dovizler = context.Dovizlers.ToList();
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }
                        dovizkurlari.Doviz = doviz;
                        dovizkurlari.DovizKodu = doviz.DovizKodu;
                        
                        try
                        {
                            context.Dovizkurlaris.Add(dovizkurlari);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Kaydedildi.";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = "Kayıt Sırasında Hata" +e.Message ;
                            ViewBag.Kur = dovizkurlari;
                            ViewBag.Dovizler = context.Dovizlers.ToList();
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View(FormCS);
                        }

                        
                    }
                }
            }
        }
        #endregion
        #region Update
        [HttpGet("DovizKurForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Kur = context.Dovizkurlaris.AsNoTracking().Where(kur => kur.DovizKurId == id).First();
                    ViewBag.Dovizler = context.Dovizlers.ToList();
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View(FormCS);
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost("DovizKurForm/Update/{id:int}")]
        public IActionResult Update (int id, Models.Dovizkurlari dovizkurlari)
        {
            if (dovizkurlari == null)
            {
                TempData["error"] = "Eklenemedi.";
                return RedirectToAction("Index");
            }
            else
            {
                if (dovizkurlari.DovizId < 1)
                {
                    ViewBag.error = "Doviz Kodu Seçilmemiş";
                    ViewBag.SubmitButtonValue = "Guncelle";
                    ViewBag.Kur = dovizkurlari;
                    return View(FormCS);
                }
                else
                {
                    using (var context = new AlisverisSepetiContext())
                    {

                        Dovizler doviz;
                        try
                        {
                            doviz = context.Dovizlers.Where(dovizler => dovizler.DovizId == dovizkurlari.DovizId).First();
                        }
                        catch (InvalidOperationException)
                        {
                            ViewBag.error = "Geçersiz Doviz Kodu";
                            ViewBag.Kur = dovizkurlari;
                            ViewBag.Dovizler = context.Dovizlers.ToList();
                            ViewBag.SubmitButtonValue = "Guncelle";
                            return View(FormCS);
                        }
                        dovizkurlari.Doviz = doviz;
                        dovizkurlari.DovizKodu = doviz.DovizKodu;

                        try
                        {
                            dovizkurlari.DovizKurId = id;
                            context.Dovizkurlaris.Update(dovizkurlari);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Guncellendi.";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = "Kayıt Sırasında Hata" + e.Message;
                            ViewBag.Kur = dovizkurlari;
                            ViewBag.Dovizler = context.Dovizlers.ToList();
                            ViewBag.SubmitButtonValue = "Guncelle";
                            return View(FormCS);
                        }


                    }
                }
            }
        }
        #endregion
        #region Delete
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete( int id)
        {
            using (var context = new AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Dovizkurlaris.Remove(context.Dovizkurlaris.Where(kur => kur.DovizKurId == id).First());
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return new EmptyResult();
                    }
                    context.SaveChanges();
                    TempData["success"] = "Başarıyla Silindi.";
                    return new EmptyResult();
                }
                catch (DbUpdateException e)
                {
                    TempData["error"] = "Kayıt Silinemedi: " + e.Message;
                    return new EmptyResult();
                }
            }
        }
        #endregion
    }
}
