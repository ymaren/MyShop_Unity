
using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.Entity;
using MyShop.Models.interfaces.Repositories;

namespace StoreWeb.Controllers
{
    [Authorize(Roles = "dicProduct")]
    public class ProductController : Controller
    {

        private readonly IGenericRepository<Product, int> _product;
        private readonly IGenericRepository<ProductGroup, int> _group;
        public ProductController(IGenericRepository<Product, int> productRepository, IGenericRepository<ProductGroup, int> _groupRepository)
        {            
            _product = productRepository;
            _group = _groupRepository;          
        }

        public ViewResult Index()
        {
           return View(_product.GetAll().ToList());
        }
        [HttpGet]
        public ViewResult Edit(int? Id)
        {
            SelectList groups = new SelectList(_group.GetAll(), "Id", "GroupName");
            ViewBag.Groups = groups;
            Product product = _product.GetAll().FirstOrDefault(p => p.Id == Id);
            return View(product);
        }
        [HttpGet]
        public string Load(int? Id)
        {
            SelectList groups = new SelectList(_group.GetAll(), "Id", "GroupName");
            ViewBag.Groups = groups;
            Product product = _product.GetAll().FirstOrDefault(p => p.Id == Id);
            return product.Name;
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase upload)
        {
            
            if (ModelState.IsValid)
            {
                
                if (product.Id>0)
                {
                    
                    TempData["message"] = string.Format("Product \"{0}\" saved", product.Name);
                    _product.Update(product);
                    _product.Save();
                }
                else
                {
                    int  prod =_product.Add(product).Id;
                    _product.Save();
                    TempData["message"] = string.Format("Product\"{0}\"saved", product.Name);
                }
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Pictures в проекте
                    upload.SaveAs(Server.MapPath("~/Pictures/"+product.Id+".jpg"));
                    TempData["message"] = string.Format("Foto add\"{0}\"", product.Name);
                    SelectList groups = new SelectList(_group.GetAll(), "Id", "GroupName");
                    ViewBag.Groups = groups;
                    return RedirectToAction("Edit",new {@id= product.Id }); 
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                SelectList groups = new SelectList(_group.GetAll(), "Id", "GroupName");
                ViewBag.Groups = groups;
                return View(product);
            }
          
        }

        
        public ViewResult Create()
        {
            SelectList groups = new SelectList(_group.GetAll(), "Id", "GroupName");
            ViewBag.Groups = groups;
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Product foundProduct = _product.GetAll().FirstOrDefault(c => c.Id == Id);

            if (foundProduct != null)
            {

                TempData["message"] = string.Format("Product   was deleted");
                _product.Delete(foundProduct);
                _product.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("Product was not found");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase upload, int? Id)
        {
            if (upload != null)
            {
                
                string fileName = System.IO.Path.GetFileName(upload.FileName);
               
                upload.SaveAs(Server.MapPath("~/Pictures/" + fileName));
            }
            return string.Empty;
        }
    }

}


   