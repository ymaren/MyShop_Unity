

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.Entity;
using MyShop.Models.MyShopModels;
using MyShop.Models.interfaces.Repositories;

namespace StoreWeb.Controllers
{
    [Authorize(Roles = "dicProductGroup")]
    public class GroupController : Controller
    {
        private readonly IGenericRepository<ProductGroup, int> _group;
        private readonly IGenericRepository<ProductCategory, int> _category;
        public GroupController(IGenericRepository<ProductGroup, int> groupRepository, IGenericRepository<ProductCategory, int> categoryRepository)
        {
            _group = groupRepository;
            _category = categoryRepository;

        }

        [HttpGet]
        public ViewResult Index()
        {
           return View(_group.GetAll().ToList());
        }

        [HttpGet]
        public ViewResult Edit(int? Id)
        {
            SelectList categorylist = new SelectList(_category.GetAll(), "Id", "CategoryName");
            ViewBag.Categories = categorylist;
            ProductGroup group = Id != null ? _group.GetSingle(Id ?? 0) : new ProductGroup();
            
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(ProductGroup group)
        {
            
            if (ModelState.IsValid)
            {
              
                if (group.Id>0)
                {
                    
                    TempData["message"] = string.Format("Group \"{0}\"uploaded", group.GroupName);
                    _group.Update(group);
                }
                else
                {
                    _group.Add(group);
                    TempData["message"] = string.Format("Group\"{0}\"added", group.GroupName);
                }
                _group.Save();
               return RedirectToAction("Index");
            }
            else
            {
                
                return View(group);
            }
          
        }

        [HttpPost]
        public ViewResult Create()
        {
            return View("Edit", new ProductGroup());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {          

            ProductGroup foundGroup = _group.GetSingle(Id);

            if (foundGroup!=null)
            {
                TempData["message"] = string.Format("Product group  was deleted");
                _group.Delete(foundGroup);
                _group.Save();
                return RedirectToAction("Index");
            }
            else
            { 
                TempData["message"] = string.Format("Product group was not found");
            }
            return RedirectToAction("Index");
        }
    }

}


   