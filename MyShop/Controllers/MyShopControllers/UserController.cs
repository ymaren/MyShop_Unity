
using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyShop.Models;
using MyShop.Models.interfaces.Repositories;

namespace StoreWeb.Controllers
{
    //[Authorize(Roles = "dicUser")]
    public class UserController : Controller
    {

        private readonly IGenericRepository<User, int> _user;
        private readonly IGenericRepository<UserRole, int> _userRole;
        private readonly IGenericRepository<Credential, int> _credential;

        public UserController(IGenericRepository<User, int> userRepository, IGenericRepository<UserRole, int> userRoleRepository,
            IGenericRepository<Credential, int> credentialRepository)
        {
            _user=userRepository;
            _userRole= userRoleRepository;
            _credential= credentialRepository;
        }

        // GET: User
        public ActionResult Index()
        {
            ViewBag.Roles = new SelectList(_userRole.GetAll(), "Id", "UserRoleName");
            List<User> users = _user.GetAll().ToList();
            return View(users);
        }
      
        public ActionResult IndexSearch(int? UserRoleId)
        {
            ViewBag.Roles = new SelectList(_userRole.GetAll(), "UserRoleId", "UserRoleName");
            var users = _user.GetAll().Where(u => UserRoleId == null||  u.UserRoleId== UserRoleId);
            return PartialView(users.ToList());
        }

        public ActionResult RoleTemplateIndexSearch(int? UserRoleId)
        {
            ViewBag.Roles = new SelectList(_userRole.GetAll(), "UserRoleId", "UserRoleName");
            var user = _user.GetAll().FirstOrDefault();
            if (UserRoleId != null)
            {
                var role = _userRole.GetAll().First(r => UserRoleId == null || r.Id == UserRoleId);
                user.Credential.Clear();
                user.Credential = role.Credential;
            }
            return PartialView(new UserViewModel(user, _credential.GetAll().ToList()));
        }

        public ActionResult CreateChange(int? Id)
        {          
            
            User user = _user.GetAll().FirstOrDefault(u => u.Id == Id);

            SelectList roles = new SelectList(_userRole.GetAll(), "Id", "UserRoleName",user!=null?user.UserRoleId:0);
            ViewBag.Roles = roles;
            return View( new UserViewModel (user?? new User(), _credential.GetAll().ToList()));
        }

        [HttpPost]
        public ActionResult CreateChange(UserViewModel userViewModel)
        {
            if (userViewModel.SelectedCredential != null)
            {
                foreach (int item in userViewModel.SelectedCredential)
                {
                    userViewModel.user.Credential.Add(_credential.GetAll().FirstOrDefault(c => c.Id == item));
                }
            }

            
            if (ModelState.IsValid)
            {
                User foundUser = _user.GetAll().FirstOrDefault(c => c.Id == userViewModel.user.Id);
                if (foundUser!=null)
                {
                    foundUser.Credential = userViewModel.user.Credential;
                    foundUser.UserName = userViewModel.user.UserName;
                    foundUser.UserPassword = userViewModel.user.UserPassword;
                    foundUser.UserRoleId = userViewModel.user.UserRoleId;
                    TempData["message"] = string.Format("User \"{0}\" uploaded", userViewModel.user.UserEmail);
                    _user.Update(foundUser);
                    _user.Save();
                }
                else
                {
                    _user.Add(userViewModel.user);
                    TempData["message"] = string.Format("User\"{0}\" uploaded ", userViewModel.user.UserEmail);
                   _user.Save();
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                SelectList roles = new SelectList(_userRole.GetAll(), "Id", "UserRoleName", userViewModel.user != null ? userViewModel.user.UserRoleId : 0);
                ViewBag.Roles = roles;
                // Что-то не так со значениями данных
                return View(userViewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int UserId)
        {
            User foundUser = _user.GetAll().FirstOrDefault(c => c.Id == UserId);

            if (foundUser != null)
            {

                TempData["message"] = string.Format("User  was deleted");
                _user.Delete(foundUser.Id);
                _user.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("User was not found");
            }

            return RedirectToAction("Index");
        }


}
}