using MyShop.Models;
using MyShop.Models.interfaces.Repositories;
using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace MyShop.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly IGenericRepository<User, int> _user;
        public AccountController (IGenericRepository<User, int> userRepository)
        {
             
            _user = userRepository;
            
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                string result = "Вы не авторизованы";
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                       if (HttpContext.User.IsInRole("Admin"))
                        return RedirectToAction("Menu", "Home");
                       else
                       return RedirectToAction("Index", "Cart");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                 newUser.UserRoleId = 3;
                User foundUser = _user.GetAll().FirstOrDefault(c => c.Id == newUser.Id);
                if (foundUser != null)
                {
                    _user.Update(foundUser);
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(newUser.UserEmail, true);
                    _user.Save();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    _user.Add(newUser);
                    _user.Save();
                    return RedirectToAction("Login");
                }
                
            }
            return View(newUser);
        }
        public ActionResult Register(string returnUrl)
        {
            
            ViewBag.LastURL = returnUrl;
            if (HttpContext.User.Identity.IsAuthenticated)
                return View(_user.GetAll().FirstOrDefault(c => c.UserEmail == HttpContext.User.Identity.Name));


            return View(new User());
        }


        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;
                      
                try
                {
                    User user = _user.GetAll().FirstOrDefault(c=>c.UserEmail==login && c.UserPassword==password); 

                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            
            return isValid;
        }
    }
}