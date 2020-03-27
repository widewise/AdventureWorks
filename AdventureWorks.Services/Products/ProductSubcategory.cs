namespace AdventureWorks.Services.Products
{
    public class ProductSubcategory
    {
        public int Id { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
    }
}