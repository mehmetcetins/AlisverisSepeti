using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Ozellikler",Name ="Ozellikler")]
    public class OzelliklerController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/Ozellikler/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/Ozellikler/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/Ozellikler/OzelliklerForm.cshtml";
        [NonAction]
        public void extras()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikTipleri = context.Ozelliktipleris.AsNoTracking().ToList();
                ViewBag.OzellikGrup = context.Ozellikgrups
                    .AsNoTracking()
                    .Include(grup => grup.OzellikgrupDils.Where(dil => dil.Dil.Varsayilanmi == true))
                    .ToList();
            }
        }
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Ozellikler = context.Ozelliklers
                    .AsNoTracking()
                    .Include(ozellik => ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                    .ToList();

            }
            return View(IndexCS);
        }
        #endregion
        #region Detail
        [HttpGet("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Ozellikler = context.Ozelliklers
                        .AsNoTracking()
                        .Where(ozellik => ozellik.OzellikId == id)
                        .Include(ozellik => ozellik.OzellikTipiNavigation.DegiskenTipiNavigation)
                        .Include(ozellik => ozellik.OzellikGrup.OzellikgrupDils.Where(dil => dil.Dil.Varsayilanmi == true))
                        .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            return View(DetailCS);
        }
        #endregion
        #region Add
        [HttpGet("OzelliklerForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Ozellikler = new Models.Ozellikler();
            extras();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("OzelliklerForm/Add")]
        public IActionResult Add(Models.Ozellikler ozellikler)
        {
            if(ozellikler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.Ozellikler = ozellikler;
                    extras();
                    Models.Ozelliktipleri tip;
                    try
                    {
                        tip = context.Ozelliktipleris.AsNoTracking().Where(tip => tip.OzellikTipiId == ozellikler.OzellikTipiId).Include(tip=>tip.DegiskenTipiNavigation).First();
                        context.Ozellikgrups.AsNoTracking().Where(grup => grup.OzellikGrupId == ozellikler.OzellikGrupId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Ozellik Tipinde veya Grubunda Hata";
                        return View(FormCS);
                    }
                    try
                    {
                        ozellikler.OzellikTipi = tip.OzellikTipi;
                        ozellikler.DegiskenTipi = tip.DegiskenTipiNavigation.DegiskenAdi;
                        ozellikler.EklenmeTarihi = DateTime.Now.ToString();
                        context.Ozelliklers.Add(ozellikler);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("OzelliklerForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Ozellikler = context.Ozelliklers.Where(ozellik => ozellik.OzellikId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            extras();
            return View(FormCS);
        }
        [HttpPost("OzelliklerForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Ozellikler ozellikler)
        {
            if (ozellikler == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Ozellikler previousEntry;
                    try
                    {
                        previousEntry = context.Ozelliklers.AsNoTracking().Where(ozellik => ozellik.OzellikId == id).First();
                    }
                    
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    Models.Ozelliktipleri tip;
                    try
                    {
                        tip = context.Ozelliktipleris.AsNoTracking().Where(tip => tip.OzellikTipiId == ozellikler.OzellikTipiId).Include(tip => tip.DegiskenTipiNavigation).First();
                        context.Ozellikgrups.AsNoTracking().Where(grup => grup.OzellikGrupId == ozellikler.OzellikGrupId).First();
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.error = "Ozellik Tipinde veya Grubunda Hata";
                        return View(FormCS);
                    }
                    try
                    {
                        ozellikler.OzellikTipi = tip.OzellikTipi;
                        ozellikler.DegiskenTipi = tip.DegiskenTipiNavigation.DegiskenAdi;
                        ozellikler.EklenmeTarihi = previousEntry.EklenmeTarihi;
                        ozellikler.GuncellenmeTarihi = DateTime.Now.ToString();
                        ozellikler.OzellikId = id;
                        context.Ozelliklers.Update(ozellikler);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        ViewBag.error = e.InnerException.Message;
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Güncellendi.";
            return RedirectToAction("Index");
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
                        context.Ozelliklers.Remove(context.Ozelliklers.Where(ozellik => ozellik.OzellikId == id).First());

                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return new EmptyResult();
                    }
                    context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    TempData["error"] = "Silme Sırasında Bir Hata Oluştu.";
                    return new EmptyResult();
                }
            }
            TempData["success"] = "Başarıyla Silindi.";
            return new EmptyResult();
        }
        #endregion
    }
}
