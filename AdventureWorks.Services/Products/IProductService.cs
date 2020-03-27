using System.Collections.Generic;

namespace AdventureWorks.Services.Products
{
    public interface IProductService
    {
        ICollection<Product> GetProducts();

        Product GetProduct(int productId);

        int CreateProduct(ProductCreateParameters parameters);

        void UpdateProduct(int productId, ProductChangeParameters parameters);

        void DeleteProduct(int productId);
    }
}