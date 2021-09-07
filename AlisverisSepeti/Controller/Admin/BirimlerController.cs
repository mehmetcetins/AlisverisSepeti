using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("/admin/Birimler/",Name = "Birimler")]
    public class BirimlerController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.birimler = context.Birimlers.ToList();
            }
            return View("~/Views/AdminPanel/Birimler/Index.cshtml");
        }
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext()) {
                ViewBag.birim = context.Birimlers.Where(birim => birim.BirimId == id).First();
            }
            return View("~/Views/AdminPanel/Birimler/Detail.cshtml");
        }
        
        [HttpGet("BirimForm/Add")]
        public IActionResult Add()
        {
            
            ViewBag.SubmitButtonValue = "Ekle";
            return View("~/Views/AdminPanel/Birimler/BirimForm.cshtml");
        }
        [HttpPost("BirimForm/Add")]
        public IActionResult Add(string birim)
        {
            Models.Birimler yeniBirim = new Models.Birimler { Birim=birim,EklenmeTarihi=DateTime.Now.ToString() };
            using (var context = new Models.AlisverisSepetiContext())
            {
                context.Birimlers.Add(yeniBirim);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet("BirimForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
          using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.birim = context.Birimlers.Where(birim => birim.BirimId == id).First();
            }
            return View("~/Views/AdminPanel/Birimler/BirimForm.cshtml");
        }
        [HttpPost("BirimForm/Update/{id:int}")]
        public IActionResult Update(int id,string birim)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Birimler updatedBirim = context.Birimlers.Where(birim => birim.BirimId == id).First();
                updatedBirim.Birim = birim;
                updatedBirim.GuncellenmeTarihi = DateTime.Now.ToString();
                context.Birimlers.Update(updatedBirim);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                Models.Birimler willBeDeleted = context.Birimlers.Where(birim => birim.BirimId == id).First();
                context.Birimlers.Remove(willBeDeleted);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
