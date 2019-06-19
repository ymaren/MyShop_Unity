namespace Store.Logic.ProductStore.Models.ModifyModels
{
    public class ProductGroupModifyModel
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int ProductCategoryid { get; set; }
        public ProductGroupModifyModel ProductCategory { get; set; }
    }
}