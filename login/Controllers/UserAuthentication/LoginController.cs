using login.HelperClass.Login;
using login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace login.Controllers.UserAuthentication
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {           
            try
            {
                using (var userDb = new UserContext())
                {
                    var getUser = userDb.Users.Where(u => u.UserName == user.UserName || u.Email == user.Email).FirstOrDefault();
                    if (getUser != null)
                    {
                        string userSalt = getUser.PasswordSalt;
                        string encodedPassword = PasswordGenerator.EncriptPassword(user.Password, userSalt);
                        if (encodedPassword == getUser.Password)
                        {
                            FormsAuthentication.SetAuthCookie(user.Email, false);
                            bool isAuth = Request.IsAuthenticated;
                            //RenewCurrentUser();
                            isAuth = Request.IsAuthenticated;
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Bledny login lub haslo");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Podany uzytkownik nie istnieje");
                        return View();
                    }
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Some exception occured" + e;
                return View();
            }
        }

        private void RenewCurrentUser()
        {
            System.Web.HttpCookie authCookie =
  System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && !authTicket.Expired)
                {
                    FormsAuthenticationTicket newAuthTicket = authTicket;

                    if (FormsAuthentication.SlidingExpiration)
                    {
                        newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                    }
                    string userData = newAuthTicket.UserData;
                    string[] roles = userData.Split(',');

                    System.Web.HttpContext.Current.User =
                      new System.Security.Principal.GenericPrincipal(new FormsIdentity(newAuthTicket), roles);
                }
            };
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}