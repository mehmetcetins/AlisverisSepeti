using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("/admin/Dovizler/",Name ="Dovizler")]
    public class DovizlerController : Controller
    {
        public IActionResult Index()
        {
            using ( var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Dovizler = context.Dovizlers.ToList();
            }
            return View("~/Views/AdminPanel/Dovizler/Index.cshtml");
        }
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Doviz = context.Dovizlers.Where(doviz => doviz.DovizId == id).First();
            }
            return View("~/Views/AdminPanel/Dovizler/Detail.cshtml");
        }
        [HttpGet("DovizForm/Add/")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.Doviz = new Models.Dovizler ();
            return View("~/Views/AdminPanel/Dovizler/DovizForm.cshtml");
        }
        [HttpPost("DovizForm/Add/")]

        public IActionResult Add(Models.Dovizler doviz)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                if (context.Dovizlers.Where(dovizler => dovizler.Sembol == doviz.Sembol).Count() > 0)
                {
                    ViewBag.error = "Ayni sembolde kayıt bulunuyor.";
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.Doviz = doviz;
                    return View("~/Views/AdminPanel/Dovizler/DovizForm.cshtml");
                }
                else
                {
                    context.Dovizlers.Add(doviz);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            //return RedirectToAction("Index");
        }
        [HttpGet("DovizForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.SubmitButtonValue = "Güncelle";
                ViewBag.Doviz = context.Dovizlers.Where(doviz => doviz.DovizId == id).First() ;
                return View("~/Views/AdminPanel/Dovizler/DovizForm.cshtml");
            }
        }
        [HttpPost("DovizForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Dovizler doviz)
        {
            doviz.DovizId = id;
            using (var context = new Models.AlisverisSepetiContext())
            {
                context.Dovizlers.Update(doviz);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                context.Dovizlers.Remove(context.Dovizlers.Where(doviz => doviz.DovizId == id).First());
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
