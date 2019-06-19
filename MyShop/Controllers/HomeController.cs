using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyShop.Models;
using Microsoft.AspNet.Identity;
using MyShop.Models.interfaces.Repositories;

namespace MyShop.Controllers
{
    public enum Order
    {
        
        not_sort ,
        first_cheap,
        first_expensive
    };

    public class HomeController : Controller
    {
        
        private readonly IGenericRepository<Product, int> _product;
        private readonly IGenericRepository<ProductCategory, int> _category;
        private readonly IGenericRepository<ProductGroup, int> _group;
        private readonly IGenericRepository<User, int> _user;
        private int pageSize = 8;
        public HomeController(IGenericRepository<Product, int> productrRepository, 
            IGenericRepository<ProductCategory, int> categoryRepository,
            IGenericRepository<ProductGroup, int> groupRepository,
            IGenericRepository<User, int> userRepository
            )
        {
          
            _product = productrRepository;
            _category = categoryRepository;
            _group = groupRepository;
            _user = userRepository;
        }
            
        public ActionResult Index(int? category, int? group, Order sort= Order.not_sort, int page = 1)
        {
            IEnumerable<Product> products = _product.GetAll().ToList().Where(c => category == null || c.ProductGroup.ProductCategoryid == category).
                Where(g => group == null || g.ProductGroupId == group);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            switch (sort)
            {
                case Order.not_sort:
                    products = products.OrderBy(C => C.Id).Skip((page - 1) * pageSize).Take(pageSize);
                break;
                case Order.first_expensive:
                    products = products.OrderByDescending(C => C.Price).Skip((page - 1) * pageSize).Take(pageSize);
                break;
                case Order.first_cheap:
                    products = products.OrderBy(C => C.Price).Skip((page - 1) * pageSize).Take(pageSize);
                break;
               


            }
           

            IndexProductViewModel ivm = new IndexProductViewModel
            {
                PageInfo = pageInfo,
                Products = products,
                CurrentCategory = _category.GetAll().Where(c => c.Id == category).FirstOrDefault(),
                CurrentGroup = _group.GetAll().Where(g => g.Id == group).FirstOrDefault(),
                CurrentOrder = sort
            };


            return View(ivm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Menu()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string email = User.Identity.GetUserName();
                User u = _user.GetAll().ToList().FirstOrDefault(c => c.UserEmail == email);
                return PartialView(u.Credential);
            }
            return RedirectToAction("Login", "Account");
            
        }
        public PartialViewResult VerticalMenu()
        {
          return PartialView(_category.GetAll().ToList());
        }

    }
}