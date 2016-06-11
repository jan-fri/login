using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using login.Models;
using Crypto;
using System.Data.Entity.Validation;

namespace login.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            //if (IsValid(user.Email, user.Password))
            //{
            //    FormsAuthentication.SetAuthCookie(user.Email, false);
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Bledne logowanie");
            //}
            return View(user);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.User user)
        {
                if (ModelState.IsValid)
                {
                    using (var db = new login.Models.UserContext())
                    {
                        
                       // var crypto = new SimpleCrypto.PBKDF2();
                      //  var encrypPass = crypto.Compute(user.Password);

                        db.Users.Add(new Models.User
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Password =user.Password,

                            Email = user.Email

                        });

                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Zle dane");
                }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        //private bool IsValid(string email, string password)
        //{
        //    var crypto = new SimpleCrypto.PBKDF2();
        //    bool IsValid = false;

        //    using (var db = new login.Models.UserContext())
        //    {
        //        var user = db.Users.FirstOrDefault(u => u.Email == email);
        //        if (user != null)
        //        {
        //            if (user.Password == crypto.Compute(password, user.PasswordSalt))
        //            {
        //                IsValid = true;
        //            }
        //        }
        //    }
        //    return IsValid;
        //}
    }
}
