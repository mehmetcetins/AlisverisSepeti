using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/OzellikGrup",Name ="Ozellikgrup")]
    public class OzellikGrupController : Controller
    {
        private readonly string IndexCS = "~/Views/AdminPanel/OzellikGrup/Index.cshtml";
        private readonly string DetailCS = "~/Views/AdminPanel/OzellikGrup/Detail.cshtml";
        private readonly string FormCS = "~/Views/AdminPanel/OzellikGrup/OzellikGrupForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.OzellikGrup = context.Ozellikgrups.AsNoTracking().Include(grup => grup.EkleyenNavigation).ToList();
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
                    ViewBag.OzellikGrup = context.Ozellikgrups
                        .AsNoTracking()
                        .Where(grup => grup.OzellikGrupId == id)
                        .Include(grup => grup.EkleyenNavigation)
                        .Include(grup => grup.GuncelleyenNavigation)
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
        [HttpGet("OzellikGrupForm/Add")]
        public IActionResult Add()
        {
            ViewBag.OzellikGrup = new Models.Ozellikgrup();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("OzellikGrupForm/Add")]
        public IActionResult Add(Models.Ozellikgrup ozellikgrup)
        {
            if(ozellikgrup == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    try
                    {
                        var firstUser = context.Users.First();
                        ozellikgrup.EkleyenId = firstUser.UserId;
                        ozellikgrup.Ekleyen = firstUser.KullaniciIsim;
                        ozellikgrup.EklenmeTarihi = DateTime.Now.ToString();
                        context.Ozellikgrups.Add(ozellikgrup);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "DizilisSira Hatali";
                        return View(FormCS);
                    }
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("OzellikGrupForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.OzellikGrup = context.Ozellikgrups.Where(grup => grup.OzellikGrupId == id).First();

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.SubmitButtonValue = "Güncelle";
            return View(FormCS);
        }
        [HttpPost("OzellikGrupForm/Update/{id:int}")]
        public IActionResult Update(int id, Models.Ozellikgrup ozellikgrup)
        {
            if (ozellikgrup == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    Models.Ozellikgrup previousEntry;
                    try
                    {
                        previousEntry = context.Ozellikgrups.AsNoTracking().Where(grup => grup.OzellikGrupId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                    try
                    {
                        
                        ozellikgrup.EkleyenId = previousEntry.EkleyenId;
                        ozellikgrup.Ekleyen = previousEntry.Ekleyen;
                        ozellikgrup.EklenmeTarihi = previousEntry.EklenmeTarihi;
                        ozellikgrup.GuncelleyenId = previousEntry.EkleyenId;
                        ozellikgrup.Guncelleyen = previousEntry.Ekleyen;
                        ozellikgrup.GuncellenmeTarihi = DateTime.Now.ToString();
                        ozellikgrup.OzellikGrupId = id;
                        context.Ozellikgrups.Update(ozellikgrup);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "DizilisSira Hatali";
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
                        context.Ozellikgrups.Remove(context.Ozellikgrups.Where(grup => grup.OzellikGrupId == id).First());

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
