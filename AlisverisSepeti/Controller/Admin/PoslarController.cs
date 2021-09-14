using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Poslar", Name = "Poslar")]
    public class PoslarController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Poslar = context.Poslars.ToList();
            }
                 
            return View("~/Views/AdminPanel/Poslar/Index.cshtml");
        }
        #endregion
        #region Detail
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.Poslar = context.Poslars.AsNoTracking().Where(pos => pos.PosId == id).First();
                return View("~/Views/AdminPanel/Poslar/Detail.cshtml");
            }
        }
        #endregion
        #region Add
        [HttpGet("PoslarForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Pos = new Models.Poslar();
            ViewBag.SubmitButtonValue = "Ekle";
            return View("~/Views/AdminPanel/Poslar/PoslarForm.cshtml");
        }

        [HttpPost("PoslarForm/Add")]
        public IActionResult Add(Models.Poslar pos)
        {
            if (pos == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Poslars.AsNoTracking().Any(poslar => poslar.PosBankaAdi == pos.PosBankaAdi))
                    {
                        ViewBag.error = "Aynı banka adında başka bir kayıt var";
                        ViewBag.Pos = pos;
                        ViewBag.SubmitButtonValue = "Ekle";
                        return View("~/Views/AdminPanel/Poslar/PoslarForm.cshtml");
                    }
                    else
                    {
                        context.Poslars.Add(pos);
                        context.SaveChanges();
                        TempData["success"] = "Başarıyla Kaydedildi.";
                        return RedirectToAction("Index");
                    }
                }
            }
        }
        #endregion
        #region Update
        [HttpGet("PoslarForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Pos = context.Poslars.AsNoTracking().Where(poslar => poslar.PosId == id).First();
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View("~/Views/AdminPanel/Poslar/PoslarForm.cshtml");
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost("PoslarForm/Update/{id:int}")]
        public IActionResult Update(int id , Models.Poslar poslar)
        {
            if (poslar == null)
            {
                TempData["error"] = "Eksi veri";
                return RedirectToAction("Index");
            }
            
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {

                    if (context.Poslars.AsNoTracking().Any(pos => pos.PosBankaAdi == poslar.PosBankaAdi && pos.PosId != id))
                    {
                        ViewBag.error = "Aynı banka adina sahip başka bir kayıt var";
                        ViewBag.SubmitButtonValue = "Guncelle";
                        ViewBag.Pos = poslar;
                        return View("~/Views/AdminPanel/Poslar/PoslarForm.cshtml");
                    }
                    else
                    {
                        try
                        {
                            poslar.PosId = id;
                            context.Poslars.Update(poslar);
                            context.SaveChanges();
                            TempData["success"] = "Başarıyla Güncellendi.";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException)
                        {
                            TempData["error"] = "Kayıt Güncellenirken Hata ile karşılaşıldı.";
                            return RedirectToAction("Index");
                        }
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
                    context.Poslars.Remove(context.Poslars.AsNoTracking().Where(poslar => poslar.PosId==id).First());
                    context.SaveChanges();
                    TempData["success"] = "Başarıyla Silindi.";
                    return new EmptyResult();
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayit Silinemedi.";
                    return new EmptyResult();
                }
            }
        }
        #endregion

    }
}
