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
            Logger.Log.Info("Getting products ...");

            ProductService productService = new ProductService();
            var products = productService.GetProducts();

            Logger.Log.Info($"Products count: {products.Count}.");

            return products;
        }

        public Product Get(int id)
        {
            Logger.Log.Info($"Getting product with id '{id}' ...");

            ProductService productService = new ProductService();
            var product = productService.GetProduct(id);

            Logger.Log.Info($"Product with id {id} is recieved.");

            return product;
        }

        public void Post([FromBody] ProductCreateParameters parameters)
        {
            Logger.Log.Info($"Creating product ...");

            ProductService productService = new ProductService();
            var id = productService.CreateProduct(parameters);

            Logger.Log.Info($"Created product with id {id}.");
        }

        public void Put(int id, [FromBody] ProductChangeParameters parameters)
        {
            Logger.Log.Info($"Changing product with id '{id}' ...");

            ProductService productService = new ProductService();
            productService.UpdateProduct(id, parameters);

            Logger.Log.Info($"Product with id {id} is changed.");
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
            Logger.Log.Info($"Removing product with id '{id}' ...");

            ProductService productService = new ProductService();
            productService.DeleteProduct(id);

            Logger.Log.Info($"Product with id {id} is removed.");
        }
    }
}