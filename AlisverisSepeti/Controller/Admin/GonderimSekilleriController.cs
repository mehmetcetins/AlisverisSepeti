using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/GonderimSekilleri", Name = "GonderimSekilleri")]
    public class GonderimSekilleriController : Controller
    {

        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.GonderimSekilleri = context.Gonderimsekilleris.ToList();
            }
            return View("~/Views/AdminPanel/GonderimSekilleri/Index.cshtml");
        }

        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                // yoksa nasıl hata döndürelecek ?
                ViewBag.GonderimSekilleri = context.Gonderimsekilleris.Where(gonderim => gonderim.GonderimId == id).First();
                return View("~/Views/AdminPanel/GonderimSekilleri/Detail.cshtml");
            }
        }
        [HttpGet("GonderimSekilleriForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.GonderimSekli = new Models.Gonderimsekilleri();
            return View("~/Views/AdminPanel/GonderimSekilleri/GonderimSekilleriForm.cshtml");
        }
        [HttpPost("GonderimSekilleriForm/Add")]
        public IActionResult Add(Models.Gonderimsekilleri gonderim)
        {
            if(gonderim == null)
            {
                return RedirectToAction("Add");
            }
            using (var context = new Models.AlisverisSepetiContext())
            {
                if (context.Gonderimsekilleris.Where(gonderim => gonderim.GonderimSekli == gonderim.GonderimSekli).Count() > 0)
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.error = "Ayni isimde gönderim sekli bulunmakta." ;
                    ViewBag.GonderimSekli = gonderim;
                    return View("~/Views/AdminPanel/GonderimSekilleri/GonderimSekilleriForm.cshtml");
                }
                else
                {
                    context.Gonderimsekilleris.Add(gonderim);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet("GonderimSekilleriForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Gonderimsekilleri gonderim = context.Gonderimsekilleris.AsNoTracking().Where(gonderimSekli => gonderimSekli.GonderimId == id).First();
                if (gonderim == null)
                {
                    ViewBag.error = "GonderimSekli Bulunamadi.";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.GonderimSekli = gonderim;
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View("~/Views/AdminPanel/GonderimSekilleri/GonderimSekilleriForm.cshtml");
                }
            }
        }
    }
}
