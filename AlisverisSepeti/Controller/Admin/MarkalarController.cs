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
    [Route("/admin/Markalar",Name ="Markalar")]
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
                }
                else
                {
                    MagickImage imageMagick = new MagickImage(MarkaLogo.OpenReadStream());
                    imageMagick.Resize(200,200);

                    context.Markalars.Add(marka);
                    int id = context.SaveChanges();

                    marka.MarkaLogo = Utils.Utils.ToFileName(MarkaLogo.FileName,id);
                    marka.MarkaBanner = Utils.Utils.ToFileName(MarkaBanner.FileName,id);

                    MarkaBanner.CopyTo(new FileStream(Path.Combine("Public/images/Markalar/", marka.MarkaLogo), FileMode.Create));
                    imageMagick.Write(new FileStream(Path.Combine("Public/images/Markalar/", marka.MarkaBanner), FileMode.Create));

                    context.Markalars.Update(marka);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
