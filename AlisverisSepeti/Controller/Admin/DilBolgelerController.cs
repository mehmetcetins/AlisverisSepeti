using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/DilBolgeler",Name ="Dilbolgeler")]
    public class DilBolgelerController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/DilBolgeler/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/DilBolgeler/DilBolgeForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.DilBolge = context.Dilbolgelers.ToList();
            }
            return View(IndexCS);
        }
        #endregion
        #region Add
        [HttpGet("DilBolgeForm/Add")]
        public IActionResult Add()
        {
            ViewBag.SubmitButtonValue = "Ekle";
            ViewBag.DilBolge = new Models.Dilbolgeler();
            return View(FormCS);
            
        }
        [HttpPost("DilBolgeForm/Add")]
        public IActionResult Add(Models.Dilbolgeler dilbolge)
        {
            if (dilbolge == null)
            {
                TempData["error"] = "Kayıt Yapılamadi.";
                return RedirectToAction("Index");

            }
            using (var context = new Models.AlisverisSepetiContext())
            {
               if (context.Dilbolgelers.AsNoTracking().Any(dil => dil.DilKodu == dilbolge.DilKodu))
               {
                    ViewBag.error = "Aynı DilKodunda Başka Bir Kayıt Var.";
                    ViewBag.DilBolge = dilbolge;
                    ViewBag.SubmitButtonValue = "Ekle";
                    return View(FormCS);
               }
               else
               {
                    try
                    {
                        context.Dilbolgelers.Add(dilbolge);
                        context.SaveChanges();
                        TempData["success"] = "Başarıyla Güncellendi.";
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateException e)
                    {
                        TempData["error"] = "Kayıt Sırasında Bir Hata Oluştu.";
                        return RedirectToAction("Index");
                    }
                    
                   
               }
            }
        }
        #endregion
        #region Update
        [HttpGet("DilBolgeForm/Update/{id:int}")]
        public IActionResult Update(short id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.DilBolge = context.Dilbolgelers.AsNoTracking().Where(dil => dil.BolgeId == id).First();
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View(FormCS);
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost("DilBolgeForm/Update/{id:int}")]
        public IActionResult Update(short id,Models.Dilbolgeler dilbolge)
        {
            if (dilbolge == null)
            {
                TempData["error"] = "Kayıt Yapılamadi.";
                return RedirectToAction("Index");

            }
            using (var context = new Models.AlisverisSepetiContext())
            {
                if (context.Dilbolgelers.AsNoTracking().Any(dil => dil.DilKodu == dilbolge.DilKodu && dil.BolgeId != id))
                {
                    ViewBag.error = "Aynı DilKodunda Başka Bir Kayıt Var.";
                    ViewBag.DilBolge = dilbolge;
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View(FormCS);
                }
                else
                {
                    try
                    {
                        dilbolge.BolgeId = id;
                        context.Dilbolgelers.Update(dilbolge);
                        context.SaveChanges();
                        TempData["success"] = "Basariyla Guncellendi.";
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateException e)
                    {
                        TempData["error"] = "Kayıt Sırasında Bir Hata Oluştu.";
                        return RedirectToAction("Index");
                    }


                }
            }
        }
        #endregion
        #region Delete
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    try
                    {
                        context.Dilbolgelers.Remove(context.Dilbolgelers.AsNoTracking().Where(dil => dil.BolgeId == id).First());
                    }
                    catch(InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadı.";
                        return new EmptyResult();
                    }
                    context.SaveChanges();
                    TempData["success"] = "Başarıyla Silindi.";
                }
                catch(DbUpdateException e)
                {
                    TempData["error"] = "Kayıt Silinirken Bir Hata Oluştu.";
                    return new EmptyResult();
                }
            }
            return new EmptyResult();

        }
        #endregion
    }
}
