using ImageMagick;
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
    [Route("/admin/Markalar/",Name ="Markalar")]
    public class MarkalarController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.markalar = context.Markalars.ToList();
            }
            return View("~/Views/AdminPanel/Markalar/Index.cshtml");
        }
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Markalar = context.Markalars.AsNoTracking().Where(marka => marka.MarkaId == id).First();
            }
            return View("~/Views/AdminPanel/Markalar/Detail.cshtml");
        }

        [HttpGet("MarkaForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Marka = new Models.Markalar();
            ViewBag.SubmitButtonValue = "Ekle";
            return View("~/Views/AdminPanel/Markalar/MarkaForm.cshtml");
        }
        [HttpPost("MarkaForm/Add")]
        public IActionResult Add(IFormFile MarkaLogo, IFormFile MarkaBanner, Models.Markalar marka)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                if (context.Markalars.Where(markalar => markalar.MarkaAdi == marka.MarkaAdi).Count() > 0)
                {
                    
                    ViewBag.error = "Aynı marka adında başka bir kayıt var";
                    ViewBag.Marka = marka;
                    ViewBag.SubmitButtonValue = "Ekle";
                    return View("~/Views/AdminPanel/Markalar/MarkaForm.cshtml");
                }
                else
                {
                    context.Markalars.Add(marka);
                    int id = context.SaveChanges();
                    if (MarkaLogo == null)
                    {
                        marka.MarkaLogo = null;
                    }
                    else
                    {
                        marka.MarkaLogo = Utils.Utils.ToFileName(MarkaLogo.FileName, id);
                        MagickImage imageMagick = new MagickImage(MarkaLogo.OpenReadStream());
                        imageMagick.Resize(200, 200);
                        imageMagick.Write(new FileStream(Path.Combine("Public/images/Markalar/", marka.MarkaLogo), FileMode.Create));
                    }

                    if (MarkaBanner == null)
                    {
                        marka.MarkaBanner = null;
                    }
                    else
                    {
                        marka.MarkaBanner = Utils.Utils.ToFileName(MarkaBanner.FileName, id);
                        MarkaBanner.CopyTo(new FileStream(Path.Combine("Public/images/Markalar/", marka.MarkaLogo), FileMode.Create));
                    }
                    context.Markalars.Update(marka);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet("MarkaForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Markalar marka =  context.Markalars.Where(marka => marka.MarkaId == id).First();
                if (marka == null)
                {
                    ViewBag.error = "Marka Bulunamadi.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Marka = marka;
                    ViewBag.SubmitButtonValue = "Güncelle";
                    return View("~/Views/AdminPanel/Markalar/MarkaForm.cshtml");
                }
            }
        }
        [HttpPost("MarkaForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Markalar marka, IFormFile MarkaLogo, IFormFile MarkaBanner)
        {
            string markaLogoName;
            string markaBannerName;
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Markalar eskiMarka = context.Markalars.AsNoTracking().Where(markalar => markalar.MarkaId == id).First();
                if (eskiMarka == null)
                {
                    ViewBag.error = "marka bulunamadı.";
                    ViewBag.SubmitButtonValue = "Guncelle";
                    ViewBag.Marka = marka;
                    return RedirectToAction("Index");
                }
                else
                {
                    if (MarkaLogo == null)
                    {
                        markaLogoName = eskiMarka.MarkaLogo;
                    }
                    else
                    {
                        // değiştirmelerde eski resimler silinmeli mi?
                        MagickImage magickImage = new MagickImage(MarkaLogo.OpenReadStream());
                        magickImage.Resize(200,200);
                        markaLogoName = Utils.Utils.ToFileName(MarkaLogo.FileName,eskiMarka.MarkaId);
                        magickImage.Write(new FileStream(Path.Combine("Public/images/Markalar/", markaLogoName), FileMode.Create));
                    }

                    if (MarkaBanner == null)
                    {
                        markaBannerName = eskiMarka.MarkaBanner;
                    }
                    else
                    {
                        markaBannerName = Utils.Utils.ToFileName(MarkaBanner.FileName,eskiMarka.MarkaId);
                        MarkaBanner.CopyTo(new FileStream(Path.Combine("Public/images/Markalar/", markaBannerName), FileMode.Create));
                    }

                    marka.MarkaBanner = markaBannerName;
                    marka.MarkaLogo = markaLogoName;
                    marka.MarkaId = id;
                    context.Markalars.Update(marka);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
        }
        
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                context.Markalars.Remove(context.Markalars.Where(marka => marka.MarkaId == id).First());
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
