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
    [Route("/admin/Diller/",Name ="Diller")]
    public class DillerController : Controller
    {
        public IActionResult Index()
        {
            using(var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.ToList();
            }
            return View("~/Views/AdminPanel/Diller/Index.cshtml");
        }

        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Diller = context.Dillers.Where(dil => dil.DilId == id).First();
            }
            return View("~/Views/AdminPanel/Diller/Detail.cshtml");
        }
        [HttpGet("DilForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.Dil = new Models.Diller();
            return View("~/Views/AdminPanel/Diller/DilForm.cshtml");
        }
        [HttpPost("DilForm/Add")]
        public IActionResult Add(Models.Diller dil, IFormFile DilLogo)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                if(context.Dillers.Where(diller => diller.BolgeDilAdi == dil.BolgeDilAdi).Count() > 0)
                {
                    ViewBag.error = "Ayni sembolde kayıt bulunuyor.";
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.Dil = dil;
                    return View("~/Views/AdminPanel/Diller/DilForm.cshtml");
                    
                }
                else
                {
                    DilLogo.CopyTo(new FileStream(Path.Combine("Public/images/Diller/" + DilLogo.FileName), FileMode.Create));
                    dil.DilLogo = DilLogo.FileName;
                    context.Dillers.Add(dil);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet("DilForm/Update/{id:int}")]
        public IActionResult Update (int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Diller dil = context.Dillers.Where(dil => dil.DilId == id).First();
                ViewBag.Dil = dil;
                
                ViewBag.SubmitButtonValue = "Güncelle";
                return View("~/Views/AdminPanel/Diller/DilForm.cshtml");
            }
        }


        [HttpPost("DilForm/Update/{id:int}")]
        public IActionResult Update(int id, IFormFile DilLogo, Models.Diller dil)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                
                if(context.Dillers.Where(diller => diller.BolgeDilAdi == dil.BolgeDilAdi).Where(diller => diller.DilId != id).Count() > 0)
                {
                    ViewBag.Dil = dil;
                    ViewBag.error = "Aynı bolge adi na sahip başka bir kayıt var";
                    ViewBag.SubmitButtonValue = "Güncelle";
                    return View("~/Views/AdminPanel/Diller/DilForm.cshtml");
                }
                else
                {
                   
                    if (DilLogo != null)
                    {
                        dil.DilLogo = DilLogo.FileName;
                        DilLogo.CopyTo(new FileStream(Path.Combine("Public/images/Diller/" + DilLogo.FileName), FileMode.Create));
                    }
                    else
                    {
                        dil.DilLogo = context.Dillers.AsNoTracking().Where(diller => diller.DilId == id).First().DilLogo;
                    }
                    dil.DilId = id;
                    context.Dillers.Update(dil);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
        }

        [HttpDelete("Delete/{id:int}")]
        public EmptyResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext()) { 
                context.Dillers.Remove(context.Dillers.Where(dil => dil.DilId == id).First());
                context.SaveChanges();
                return new EmptyResult();
            }
        }


    }
}
