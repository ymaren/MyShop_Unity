using Store.Logic.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Store.Logic.ProductStore.Models.ViewModels
{
    //[Bind(Exclude = "Id")]
    public class ProductCategoryViewModel
    {
        public int Id { get;  set; }
        
        public string Name { get;  set; }

        public string Description { get;  set; }

        public ProductCategoryViewModel (int Id, string Name, string Description)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
        }
        public ProductCategoryViewModel(ProductCategory productCategory)
        {
            this.Id = productCategory.Id;
            this.Name = productCategory.Name;
            this.Description = productCategory.Description;
        }

        public ProductCategoryViewModel()
        {

        }

    }
}
