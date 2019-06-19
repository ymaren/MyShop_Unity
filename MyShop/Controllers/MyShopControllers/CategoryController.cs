using MyShop.Models.interfaces.Repositories;
using MyShop.Models.MyShopModels;
using System.Linq;
using System.Web.Mvc;

namespace StoreWeb.Controllers
{
    [Authorize(Roles = "dicCategories")]
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<ProductCategory, int> _category;
        public CategoryController(IGenericRepository<ProductCategory, int> categoryRepository)
        {
            _category = categoryRepository;
        }
        [HttpGet]
        public ViewResult Index()
        {
            return View(_category.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int? Id)
        {
            ProductCategory prodcategory = _category.GetAll().FirstOrDefault(c => c.Id == Id);
            return View(prodcategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory category)
        {

            if (ModelState.IsValid)
            {

                if (category.Id > 0)
                {

                    TempData["message"] = string.Format("Category \"{0}\"uploaded", category.CategoryName);
                    _category.Update(category);
                }
                else
                {
                    _category.Add(category);
                    TempData["message"] = string.Format("Category\"{0}\"added", category.CategoryName);
                }
                
                return RedirectToAction("Index");
            }
            else
            {

                return View(category);
            }

        }

        [HttpPost]
        public ViewResult Create()
        {
            return View("Edit", new ProductCategory());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {

            ProductCategory foundCategory = _category.GetAll().FirstOrDefault(c => c.Id == Id);

            if (foundCategory != null)
            {

                TempData["message"] = string.Format("Product category  was deleted");
                _category.Delete(foundCategory);
                _category.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("Product category was not found");
            }

            return RedirectToAction("Index");
        }
    }

}


