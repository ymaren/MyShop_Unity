using Store.Logic.Entity;

namespace Store.Logic.ProductStore.Models.ModifyModels
{
    public class ProductModifyModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int ProductGroupId { get; set; }
        public  ProductGroup ProductGroup { get; set; }
    }
}