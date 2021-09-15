using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/HavaleBankalari",Name ="Havalebankalari")]
    public class HavaleBankalariController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                ViewBag.HavaleBankalari = context.Havalebankalaris.ToList();
            }
            return View("~/Views/AdminPanel/HavaleBankalari/Index.cshtml");
        }
        #endregion
        #region Detail
        [HttpGet("{id:int}")]
        public IActionResult Detail(int id)
        {
            using (var context= new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.HavaleBankalari = context.Havalebankalaris.AsNoTracking().Where(havale => havale.HavaleBankaId == id).First();
                    return View("~/Views/AdminPanel/HavaleBankalari/Detail.cshtml");
                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
        #region Add
        [HttpGet("HavaleBankalariForm/Add")]
        public IActionResult Add()
        {
            ViewBag.HavaleBanka = new Models.Havalebankalari();
            ViewBag.SubmitButtonValue = "Ekle";
            return View("~/Views/AdminPanel/HavaleBankalari/HavaleBankalariForm.cshtml");
        }  
        [HttpPost("HavaleBankalariForm/Add")]
        public IActionResult Add(IFormFile BankaLogo,Models.Havalebankalari havalebankalari)
        {
            if(havalebankalari == null)
            {
                TempData["error"] = "Bir Sorun Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {

                    if (context.Havalebankalaris.AsNoTracking().Any(havaleBanka => havaleBanka.SubeKodu == havalebankalari.SubeKodu || havaleBanka.HesapNo == havalebankalari.HesapNo || havaleBanka.Iban == havalebankalari.Iban))
                    {
                        ViewBag.error = "SubeKodu HesapNo veya Iban önceki kayıtlarla çakışıyor";
                        ViewBag.HavaleBanka = havalebankalari;
                        ViewBag.SubmitButtonValue = "Ekle";
                        return View("~/Views/AdminPanel/HavaleBankalari/HavaleBankalarıForm.cshtml");
                    }
                    else
                    {
                        try
                        {
                            context.Havalebankalaris.Add(havalebankalari);
                            context.SaveChanges();
                            if (BankaLogo != null)
                            {
                                havalebankalari.BankaLogo = Utils.Utils.ToFileName(BankaLogo.FileName,havalebankalari.HavaleBankaId);
                                ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage(BankaLogo.OpenReadStream());
                                magickImage.Resize(200,200);
                                magickImage.Write(new FileStream(Path.Combine("Public/images/HavaleBankalari",havalebankalari.BankaLogo),FileMode.Create));
                                context.Havalebankalaris.Update(havalebankalari);
                                context.SaveChanges();
                            }
                            TempData["success"] = "Başarıyla Kaydedildi.";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException e)
                        {
                            ViewBag.error = e.Message;
                            ViewBag.HavaleBanka = havalebankalari;
                            ViewBag.SubmitButtonValue = "Ekle";
                            return View("~/Views/AdminPanel/HavaleBankalari/HavaleBankalariForm.cshtml");
                        }
                        

                    }

                    
                }
                
            }
        }
        #endregion
        #region Update
        [HttpGet("HavaleBankalariForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context = new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.HavaleBanka = context.Havalebankalaris.AsNoTracking().Where(havale => havale.HavaleBankaId == id).First();
                    ViewBag.SubmitButtonValue = "Guncelle";
                    return View("~/Views/AdminPanel/HavaleBankalari/HavaleBankalariForm.cshtml");

                }
                catch (InvalidOperationException)
                {
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost("HavaleBankalariForm/Update/{id:int}")]
        public IActionResult Update(int id,IFormFile BankaLogo,Models.Havalebankalari havalebankalari)
        {
            if (havalebankalari == null)
            {
                TempData["error"]="Guncellenirken bir hata oluştu";
                return RedirectToAction("Index");
            }
            else
            {
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Havalebankalaris.AsNoTracking().Any(havale => 
                    (
                        havale.SubeKodu == havalebankalari.SubeKodu 
                        || havale.HesapNo == havalebankalari.HesapNo 
                        || havale.Iban == havalebankalari.Iban
                    )
                        &&
                        havale.HavaleBankaId != id
                    ))
                    {
                        ViewBag.error = "SubeKodu, HesapNo veya IBAN başka bir kayıtla çakışıyor.";
                        ViewBag.HavaleBanka = havalebankalari;
                        ViewBag.SubmitButtonValue = "Guncelle";
                        return View("~/Views/AdminPanel/HavaleBankalari/HavaleBankalariForm.cshtml");

                    }
                    else
                    {
                        try
                        {
                            if (BankaLogo == null)
                            {
                                try
                                {
                                    havalebankalari.BankaLogo = context.Havalebankalaris.AsNoTracking().Where(havale => havale.HavaleBankaId == id).First().BankaLogo;
                                }
                                catch (InvalidOperationException)
                                {
                                    TempData["error"] = "Kayıt Bulunamadı.";
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                            {
                                havalebankalari.BankaLogo = Utils.Utils.ToFileName(BankaLogo.FileName,id);
                                ImageMagick.MagickImage magickImage = new ImageMagick.MagickImage(BankaLogo.OpenReadStream());
                                magickImage.Resize(200,200);
                                magickImage.Write(new FileStream(Path.Combine("Public/images/HavaleBankalari", havalebankalari.BankaLogo), FileMode.Create ));
                            }
                            havalebankalari.HavaleBankaId = id;
                            
                            context.Havalebankalaris.Update(havalebankalari);
                            context.SaveChanges();

                            TempData["success"] = "Güncelleme Başarılı";
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException e)
                        {
                            TempData["error"] = "Güncelleme Sırasinda Bir Hata Olustu: "+e.Message;
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
                    context.Remove(context.Havalebankalaris.AsNoTracking().Where(havale=> havale.HavaleBankaId == id).First());
                    context.SaveChanges();
                    TempData["success"] = "Başarıyla Silindi.";
                    return new EmptyResult();
                }
                catch (InvalidOperationException){
                    TempData["error"] = "Kayıt Bulunamadi.";
                    return new EmptyResult();
                }
            }
        }
        #endregion
    }
}
