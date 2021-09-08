using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("/admin/Diller",Name ="Diller")]
    public class DillerController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/AdminPanel/Diller/Index");
        }
    }
}
