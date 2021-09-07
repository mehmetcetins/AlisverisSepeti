using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti
{
    [Route("/admin/")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Tables = context.Model.GetEntityTypes().Select(table => table.Name.Split(".")[2]).Distinct().ToList();
            }
            return View("~/Views/AdminPanel/Admin/Index.cshtml");
        }
       
    }
}
