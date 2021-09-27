using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Admin
{
    [Route("admin/Users",Name = "User")]
    public class UsersController : Controller
    {
        private string IndexCS = "~/Views/AdminPanel/Users/Index.cshtml";
        private string DetailCS = "~/Views/AdminPanel/Users/Detail.cshtml";
        private string FormCS = "~/Views/AdminPanel/Users/UsersForm.cshtml";
        #region Index
        public IActionResult Index()
        {
            using (var context= new Models.AlisverisSepetiContext())
            {
                ViewBag.Users = context.Users.AsNoTracking().ToList();
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
                    ViewBag.Users = context.Users.AsNoTracking().Where(user => user.UserId == id).First();
                    return View(DetailCS);
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
        [HttpGet("UsersForm/Add")]
        public IActionResult Add()
        {
            ViewBag.Users = new Models.User();
            ViewBag.SubmitButtonValue = "Ekle";
            return View(FormCS);
        }
        [HttpPost("UsersForm/Add")]
        public IActionResult Add(Models.User user)
        {
            if (user == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Users = user;
                ViewBag.SubmitButtonValue = "Ekle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Users.AsNoTracking().Any(users => users.KullaniciIsim == user.KullaniciIsim || users.Email == user.Email))
                    {
                        ViewBag.error = "Aynı Kullanıcı Adina veya Emaile Sahip Üye Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        if (Utils.Utils.IsValidEmail(user.Email))
                        {
                            try
                            {
                                context.Users.Add(user);
                                context.SaveChanges();
                            }
                            catch(DbUpdateException)
                            {
                                ViewBag.error = "Kayıt Sırasında Bir Hata Oluştu.";
                                return View(FormCS);
                            }
                        }
                        else
                        {
                            ViewBag.error = "Email Hatali.";
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Kayıt Başarılı.";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet("UsersForm/Update/{id:int}")]
        public IActionResult Update(int id)
        {
            using (var context= new Models.AlisverisSepetiContext())
            {
                try
                {
                    ViewBag.Users = context.Users.AsNoTracking().Where(user => user.UserId == id).First();
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
        [HttpPost("UsersForm/Update/{id:int}")]
        public IActionResult Update(int id , Models.User user)
        {
            if (user == null)
            {
                TempData["error"] = "Bir Hata Oluştu.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Users = user;
                ViewBag.SubmitButtonValue = "Güncelle";
                using (var context = new Models.AlisverisSepetiContext())
                {
                    if (context.Users.AsNoTracking().Any(users => (users.KullaniciIsim == user.KullaniciIsim || users.Email == user.Email) && users.UserId != id ) )
                    {
                        ViewBag.error = "Aynı Kullanıcı Adina veya Emaile Sahip Üye Bulunmakta.";
                        return View(FormCS);
                    }
                    else
                    {
                        if (Utils.Utils.IsValidEmail(user.Email))
                        {
                            try
                            {
                                user.UserId = id;
                                context.Users.Update(user);
                                context.SaveChanges();
                            }
                            catch (DbUpdateException)
                            {
                                ViewBag.error = "Güncelleme Sırasında Bir Hata Oluştu.";
                                return View(FormCS);
                            }
                        }
                        else
                        {
                            ViewBag.error = "Email Hatali.";
                            return View(FormCS);
                        }
                    }
                }
            }
            TempData["success"] = "Güncelleme Başarılı.";
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
                        context.Users.Remove(context.Users.Where(user=> user.UserId== id).First());

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
