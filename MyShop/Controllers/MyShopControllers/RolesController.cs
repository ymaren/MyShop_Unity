
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using MyShop.Models;
using MyShop.Models.interfaces.Repositories;
using MyShop.Models.MyShopModels;

namespace StoreWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly IGenericRepository<Credential, int> _credential;
        private readonly IGenericRepository<UserRole, int> _userRole;

        public RolesController(IGenericRepository<Credential, int> credentialReposirory, IGenericRepository<UserRole, int> userRoleRepository)
        {
            _userRole = userRoleRepository;
            _credential = credentialReposirory;
        }

        //list
        public ViewResult Index()
        {
            return View(_userRole.GetAll().ToList());
        }


        [HttpGet]
        public ViewResult Edit(int? Id)
        {
            UserRole role = _userRole.GetAll().FirstOrDefault(c => c.Id == Id);
            return View(new RoleViewModel(role?? new UserRole(), _credential.GetAll().ToList()));
        }

        [HttpPost]
        public ActionResult Edit(RoleViewModel role)
        {

            if (role.SelectedCredential != null)
            {
                List<Credential> list = _credential.GetAll().ToList();
                foreach (int item in role.SelectedCredential)
                {                    
                    role.userRole.Credential.Add(list.FirstOrDefault(c => c.Id == item));
                }
            }
            
            if (ModelState.IsValid)
            {
                UserRole foundRole = _userRole.GetAll().FirstOrDefault(c => c.Id == role.userRole.Id);
                if (foundRole!=null)
                {
                    foundRole.UserRoleName = role.userRole.UserRoleName;
                    var difCredAdd = role.userRole.Credential.Where(c => !foundRole.Credential.Any(cred => cred.Id == c.Id));
                    var difCredDeleted = foundRole.Credential.Where(c => !role.userRole.Credential.Any(cred => cred.Id == c.Id));
                    difCredDeleted.ToList().ForEach(c => foundRole.Credential.Remove(c));
                    foundRole.Credential.AddRange(difCredAdd);                 
                    _userRole.Update(foundRole);
                    TempData["message"] = string.Format("Role \"{0}\"uploaded", role.userRole.UserRoleName);
                    
                }
                else
                {
                    _userRole.Add(role.userRole);
                    TempData["message"] = string.Format("Role\"{0}\"added", role.userRole.UserRoleName);
                    _userRole.Save();
                }

                return RedirectToAction("Index");
            }
            else
            {

                return View(role);
            }

        }

        [HttpPost]
        public ViewResult Create()
        {
            return View("Edit", new UserRole());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            UserRole foundRole = _userRole.GetAll().FirstOrDefault(c => c.Id == Id);

            if (foundRole != null)
            {

                TempData["message"] = string.Format("Role  was deleted");
                _userRole.Delete(foundRole);
                _userRole.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("Role was not found");
            }

            return RedirectToAction("Index");
        }
    }

}


