using login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login.HelperClass.Login;

namespace login.Controllers.UserAuthentication
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var userDb = new UserContext())
                    {
                        var UserExists = userDb.Users.Any(u => u.UserName == user.UserName || u.Email == user.Email);
                        if (!UserExists)
                        {
                            var key = PasswordGenerator.GenerateSalt(10);
                            var encryptedPass = PasswordGenerator.EncriptPassword(user.Password, key);
                            user.PasswordSalt = key;
                            user.Password = encryptedPass;
                            userDb.Users.Add(user);
                            userDb.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "User Allredy Exixts");
                            //ViewBag.ErrorMessage = "User Allredy Exixts!!!!!!!!!!";
                            return View();
                        }


                    }
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = "Some exception occured" + e;
                    return View();
                }
                //else
                //{
                //    ModelState.AddModelError("", "Zle dane");
                //}
            }
            else
                return View();
        }
    }
}