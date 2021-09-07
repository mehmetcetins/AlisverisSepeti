﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti
{
    
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/maymun/{id:int?}")]
        public IActionResult Maymun(int? id)
        {
            ViewBag.sayi = id;
            return View(new {sayi = id});
        }
    }
}
