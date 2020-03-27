using System;
using System.Collections.Generic;
using System.Web.Http;
using AdventureWorks.Services.Products;
using AdventureWorks.Web.Handlers;

namespace AdventureWorks.Web.Controllers
{
    public class ProductController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            ProductService productService = new ProductService();
            return productService.GetProducts();
        }

        public Product Get(int id)
        {

            ProductService productService = new ProductService();
            return productService.GetProduct(id);
        }

        public void Post([FromBody] ProductCreateParameters parameters)
        {
            ProductService productService = new ProductService();
            productService.CreateProduct(parameters);
        }

        public void Put(int id, [FromBody] ProductChangeParameters parameters)
        {
            ProductService productService = new ProductService();
            productService.UpdateProduct(id, parameters);
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
            ProductService productService = new ProductService();
            productService.DeleteProduct(id);

        }
    }
}
