using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/StokDurum",Name ="StokDurum")]
    public class StokDurumController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/StokDurum/Index.cshtml";
        private string FormCS = "~/Views/AdminPanel/StokDurum/StokDurumForm.cshtml";
        private string DetailCS = "~/Views/AdminPanel/StokDurum/Detail.cshtml";
        private string ImageFolder = "StokDurum";
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.StokDurum = context.Stokdurums
                    .AsNoTracking()
                    .Include(stok=> stok.EkleyenNavigation)
                    .Include(stok => stok.StokdurumDils.Where(stokdil => stokdil.Dil.Varsayilanmi == true))
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
                    ViewBag.StokDurum = context.Stokdurums
                        .AsNoTracking()
                        .Where(stok => stok.StokDurumId == id)
                        .Include(stok=> stok.EkleyenNavigation)
                        .Include(stok=> stok.GuncelleyenNavigation)
                        .Include(stok => stok.StokdurumDils.Where(stokdil => stokdil.Dil.Varsayilanmi == true))
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
        [HttpGet("StokDurumForm/Add")]
        public IActionResult Add()
        {
            ViewBag.StokDurum = new Models.Stokdurum();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("StokDurumForm/Add")]
        public IActionResult Add(IFormFile StokDurumResim,Models.Stokdurum stokdurum)
        {
            if (stokdurum == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.StokDurum = stokdurum;
                    try
                    {
                        var firstUser = context.Users.First();
                        stokdurum.EkleyenId = firstUser.UserId;
                        stokdurum.Ekleyen = firstUser.KullaniciIsim;
                        
                        stokdurum.EklenmeTarihi = DateTime.Now.ToString();
                        context.Stokdurums.Add(stokdurum);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                        return View(FormCS);
                    }
                    if (StokDurumResim != null)
                    {
                        stokdurum.StokDurumResim = Utils.Utils.ToFileName(StokDurumResim.FileName, stokdurum.StokDurumId);
                        Utils.ImageUtils.SaveImage
                        (
                            StokDurumResim.OpenReadStream(),
                            ImageFolder,
                            stokdurum.StokDurumResim
                        );
                        try
                        {
                            context.Stokdurums.Update(stokdurum);
                            context.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                            return View(FormCS);
                        }
                    }
                    
                    
                }
            }
            TempData["success"] = "Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("StokDurumForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.StokDurum = context.Stokdurums
                        .AsNoTracking()
                        .Where(stok => stok.StokDurumId == id)
                        .First();
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
        [HttpPost("StokDurumForm/Update/{id:int}")]
        public IActionResult Update(int id,IFormFile StokDurumResim, Models.Stokdurum stokdurum)
        {
            if (stokdurum == null)
            {
                TempData["error"] = "Beklenmedik Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    ViewBag.SubmitButtonValue = "Ekle";
                    ViewBag.StokDurum = stokdurum;
                    Models.Stokdurum oldStok;
                    try
                    {
                        oldStok = context.Stokdurums.AsNoTracking().Where(stok => stok.StokDurumId == id).First();
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return RedirectToAction("Index");
                    }
                   
                    if (StokDurumResim != null)
                    {
                        stokdurum.StokDurumResim = Utils.Utils.ToFileName(StokDurumResim.FileName, id);
                        Utils.ImageUtils.SaveImage
                            (
                            StokDurumResim.OpenReadStream(),
                            ImageFolder,
                            stokdurum.StokDurumResim
                            );
                    }
                    else
                    {
                        stokdurum.StokDurumResim = oldStok.StokDurumResim;
                    }
                    
                    try
                    {
                        stokdurum.EkleyenId = oldStok.EkleyenId;
                        stokdurum.Ekleyen = oldStok.Ekleyen;
                        stokdurum.EklenmeTarihi = oldStok.EklenmeTarihi;
                        stokdurum.GuncelleyenId = oldStok.EkleyenId;
                        stokdurum.Guncelleyen = oldStok.Ekleyen;
                        stokdurum.GuncellenmeTarihi = DateTime.Now.ToString();
                        stokdurum.StokDurumId = id;
                        context.Stokdurums.Update(stokdurum);
                        context.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
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
                        context.Stokdurums.Remove(context.Stokdurums.AsNoTracking().Where(stok=> stok.StokDurumId == id).First());

                    }
                    catch (InvalidOperationException)
                    {
                        TempData["error"] = "Kayıt Bulunamadi.";
                        return new EmptyResult();
                    }
                    context.SaveChanges();
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
