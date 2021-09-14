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
                if (!context.Gonderimsekilleris.Any(gonderim => gonderim.GonderimSekli == gonderim.GonderimSekli))
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
        [HttpPost("GonderimSekilleriForm/Update/{id:int}")]
        public IActionResult Update(int id,Models.Gonderimsekilleri gonderim)
        {
            if (gonderim == null)
            {
                ViewBag.error = "GonderimSekli boş";
                return RedirectToAction("Index");
            }
            using (var context=new Models.AlisverisSepetiContext())
            {
                if (context.Gonderimsekilleris.Where(gonderimSekli => gonderimSekli.GonderimSekli == gonderim.GonderimSekli).Count() > 1)
                {
                    ViewBag.GonderimSekli = gonderim;
                    ViewBag.error = "Aynı GonderimSeklinde başka bir kayıt var";
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View("~/Views/AdminPanel/GonderimSekilleri/GonderimSekilleriForm.cshtml");
                }
                else
                {
                    ViewBag.success = "Basariyla Kaydedildi.";
                    gonderim.GonderimId = id;
                    context.Gonderimsekilleris.Update(gonderim);
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
                if (context.Gonderimsekilleris.Any(gonderim => gonderim.GonderimId == id))
                {
                    context.Gonderimsekilleris.Remove(context.Gonderimsekilleris.AsNoTracking().Where(gonderim => gonderim.GonderimId == id).First());
                    context.SaveChanges();
                }
                else
                {
                    ViewBag.error = "GonderimSekli bulunamadi.";
                    
                }
            }
            return new EmptyResult();
        }
    }
}
