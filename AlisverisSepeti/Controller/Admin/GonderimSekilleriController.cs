using Microsoft.AspNetCore.Mvc;
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

        }
    }
}
