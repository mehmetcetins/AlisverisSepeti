using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Kargolar",Name ="Kargolar")]
    public class KargolarController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Kargolar = context.Kargolars.AsNoTracking().ToList();
            }
            return View("~/Views/AdminPanel/Kargolar/Index.cshtml");
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
                    ViewBag.Kargolar = context.Kargolars.AsNoTracking().Where(kargo => kargo.KargoId == id).First();
                    return View("~/Views/AdminPanel/Kargolar/Detail.cshtml");
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
        #region Add
        [HttpGet("KargolarForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Kargo = new Models.Kargolar();
            ViewBag.SubmitButtonValue = "Ekle";
            return View("~/Views/AdminPanel/Kargolar/KargolarForm.cshtml");

        }
        [HttpPost("KargolarForm/Add")]
        public IActionResult Add(Models.Kargolar kargolar, IFormFile KargoLogo)
        {
            if (kargolar == null)
            {
                TempData["error"] = "Kayıt Sırasında bir hata oluştu.";
                return RedirectToAction("Index");

            }
            else
            {
                using (var context= new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        if (context.Kargolars.AsNoTracking().Any(kargo => kargo.KargoAdi == kargolar.KargoAdi))
                        {
                            ViewBag.error = "Aynı Kargo Adinda Başka Bir Kayıt Bulunmakta.";
                            ViewBag.Kargo = kargolar;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View("~/Views/AdminPanel/Kargolar/KargolarForm.cshtml");
                        }
                        else
                        {
                            context.Kargolars.Add(kargolar);
                            context.SaveChanges();
                            if (KargoLogo != null)
                            {
                                kargolar.KargoLogo = Utils.Utils.ToFileName(KargoLogo.FileName, kargolar.KargoId);
                                ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage(KargoLogo.OpenReadStream());
                                magickImage.Resize(200,200);
                                magickImage.Write(new FileStream(Path.Combine("Public/images/Kargolar", kargolar.KargoLogo), FileMode.Create));
                                context.Kargolars.Update(kargolar);
                                context.SaveChanges();
                                
                            }
                            TempData["success"] = "Başarıyla Kaydedildi.";
                            return RedirectToAction("Index");
                        }
                       
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Bir Hata Oluştu: " + e.Message;
                        ViewBag.SubmitButtonValue = "Ekle";
                        ViewBag.Kargo = kargolar;
                        return RedirectToAction("Index");
                    }
                }
            }
        }
        #endregion
        #region Update
        [HttpGet("KargolarForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Kargo = context.Kargolars.AsNoTracking().Where(kargo => kargo.KargoId == id).First();
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View("~/Views/AdminPanel/Kargolar/KargolarForm.cshtml");

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");

                }
            }
        }
        [HttpPost("KargolarForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Kargolar kargolar,IFormFile KargoLogo)
        {
            if (kargolar == null)
            {
                TempData["error"] = "Beklenmekdik Bir Hata Oluştu";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        Models.Kargolar tempKargo;
                        try
                        {
                            tempKargo = context.Kargolars.AsNoTracking().Where(kargo => kargo.KargoId == id).First();
                        }
                        catch (InvalidOperationException)
                        {
                            TempData["error"] = "Kayıt Bulunamadi.";
                            return RedirectToAction("Index");
                        }
                        if (KargoLogo == null)
                        {

                            kargolar.KargoLogo = tempKargo.KargoLogo;
                        }
                        else
                        {
                            kargolar.KargoLogo = Utils.Utils.ToFileName(KargoLogo.FileName, kargolar.KargoId);
                            ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage(KargoLogo.OpenReadStream());
                            magickImage.Resize(200,200);
                            magickImage.Write(new FileStream(Path.Combine("Public/images/Kargolar", kargolar.KargoLogo), FileMode.Create));
                        }
                        if (context.Kargolars.AsNoTracking().Any(kargo => kargo.KargoAdi == kargolar.KargoAdi && kargo.KargoId != id))
                        {
                            ViewBag.error = "Aynı Kargo Adında Başka Bir Kayıt Var";
                            ViewBag.Kargo = kargolar;
                            ViewBag.SubmitButtonValue = "Guncelle";
                            return View("~/Views/AdminPanel/Kargolar/KargolarForm.cshtml");
                        }
                        else
                        {
                            kargolar.KargoId = id;
                            context.Update(kargolar);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Güncellendi.";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = "Bir Hata Oluştu: " + e.Message;
                        ViewBag.SubmitButtonValue = "Guncelle";
                        ViewBag.Kargo = kargolar;
                        
                        return View("~/Views/AdminPanel/Kargolar/KargolarForm.cshtml");
                    }
                    
                }
            }
            
            
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
                    context.Kargolars.Remove(context.Kargolars.AsNoTracking().Where(kargo => kargo.KargoId == id).First());
                    context.SaveChanges();
                    TempData["success"] = "Başarıyla Silindi.";
                    return new EmptyResult();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return new EmptyResult();
                }
            }
        }
        #endregion
    }
}
