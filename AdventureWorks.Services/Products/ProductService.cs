using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorks.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DbModel.Entities _entities = new DbModel.Entities();

        private IQueryable<Product> GetQuery()
        {
            return from p in _entities.Products
                join unitMeasure1 in _entities.UnitMeasures on p.SizeUnitMeasureCode equals unitMeasure1.UnitMeasureCode
                    into sizeUnitMeasures
                from sizeUnitMeasure in sizeUnitMeasures.DefaultIfEmpty()
                join unitMeasure2 in _entities.UnitMeasures on p.WeightUnitMeasureCode equals unitMeasure2
                    .UnitMeasureCode into wieghtUnitMeasures
                from weightUnitMeasure in wieghtUnitMeasures.DefaultIfEmpty()
                join pm in _entities.ProductModels on p.ProductModelID equals pm.ProductModelID into productModels
                from productModel in productModels.DefaultIfEmpty()
                join psc in _entities.ProductSubcategories on p.ProductSubcategoryID equals psc.ProductSubcategoryID
                    into productSubCategories
                from productSubCategory in productSubCategories.DefaultIfEmpty()
                select new Product
                {
                    Id = p.ProductID,
                    Name = p.Name,
                    ProductNumber = p.ProductNumber,
                    ProductModel = productModel != null
                        ? new ProductModel
                        {
                            Id = productModel.ProductModelID,
                            Name = productModel.Name,
                            Instructions = productModel.Instructions,
                            CatalogDescription = productModel.CatalogDescription
                        }
                        : null,
                    ProductSubcategory = productSubCategory != null
                        ? new ProductSubcategory
                        {
                            Id = productSubCategory.ProductSubcategoryID,
                            Name = productSubCategory.Name,
                            ProductCategory = new ProductCategory()
                            {
                                Id = productSubCategory.ProductCategory.ProductCategoryID,
                                Name = productSubCategory.ProductCategory.Name
                            }
                        }
                        : null,
                    Color = p.Color,
                    Class = p.Class,
                    Style = p.Style,
                    Size = p.Size,
                    MakeFlag = p.MakeFlag,
                    FinishedGoodsFlag = p.FinishedGoodsFlag,
                    ReorderPoint = p.ReorderPoint,
                    StandardCost = p.StandardCost,
                    ListPrice = p.ListPrice,
                    DaysToManufacture = p.DaysToManufacture,
                    SellStartDate = p.SellStartDate,
                    SellEndDate = p.SellEndDate,
                    DiscontinuedDate = p.DiscontinuedDate,
                    SafetyStockLevel = p.SafetyStockLevel,
                    ProductLine = p.ProductLine,
                    SizeUnitMeasure = sizeUnitMeasure != null
                        ? new UnitMeasure
                        {
                            UnitMeasureCode = sizeUnitMeasure.UnitMeasureCode,
                            Name = sizeUnitMeasure.Name
                        }
                        : null,
                    Weight = p.Weight,
                    WeightUnitMeasure = weightUnitMeasure != null
                        ? new UnitMeasure
                        {
                            UnitMeasureCode = weightUnitMeasure.UnitMeasureCode,
                            Name = weightUnitMeasure.Name
                        }
                        : null
                };
        }

        public ICollection<Product> GetProducts()
        {
            var query = GetQuery();

            return query.ToArray();
        }

        public Product GetProduct(int productId)
        {
            var query = GetQuery();

            return query.FirstOrDefault(product => product.Id == productId);
        }

        public int CreateProduct(ProductCreateParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentException("Parameters is required", nameof(parameters));
            }

            using (var transaction = _entities.Database.BeginTransaction())
            {
                try
                {
                    var result = CreateProductInternal(parameters);

                    transaction.Commit();

                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private int CreateProductModel(ProductModelCreateParameters parameters)
        {
            var productModel = _entities.ProductModels.Create<DbModel.ProductModel>();
            productModel.Name = parameters.Name;
            productModel.Instructions = parameters.Instructions;
            productModel.CatalogDescription = parameters.CatalogDescription;
            productModel.rowguid = Guid.NewGuid();
            productModel.ModifiedDate = DateTime.Now;

            return productModel.ProductModelID;
        }

        private int CreateProductInternal(ProductCreateParameters parameters)
        {
            var product = _entities.Products.Create<DbModel.Product>();

            if (parameters.ProductModel != null)
            {
                product.ProductModelID = CreateProductModel(parameters.ProductModel);
            }

            if (parameters.ProductSubcategoryId.HasValue)
            {
                var subcategory = _entities.ProductSubcategories.FirstOrDefault(x =>
                    x.ProductSubcategoryID == parameters.ProductSubcategoryId.Value);

                if (subcategory == null)
                {
                    throw new ArgumentException($"Subcategory with id '{parameters.ProductSubcategoryId}' is not found.", nameof(parameters.ProductSubcategoryId));
                }

                product.ProductSubcategoryID = parameters.ProductSubcategoryId.Value;
            }

            product.Name = parameters.Name;
            product.ProductNumber = parameters.ProductNumber;
            product.Class = parameters.Class;
            product.Color = parameters.Color;
            product.DaysToManufacture = parameters.DaysToManufacture;
            product.ReorderPoint = parameters.ReorderPoint;
            product.MakeFlag = parameters.MakeFlag;
            product.FinishedGoodsFlag = parameters.FinishedGoodsFlag;
            product.SafetyStockLevel = parameters.SafetyStockLevel;
            product.StandardCost = parameters.StandardCost;
            product.ListPrice = parameters.ListPrice;
            product.ProductLine = parameters.ProductLine;
            product.Style = parameters.Style;
            product.SellStartDate = parameters.SellStartDate;
            product.SellEndDate = parameters.SellEndDate;
            product.DiscontinuedDate = parameters.DiscontinuedDate;
            product.Size = parameters.Size;
            product.SizeUnitMeasureCode = parameters.SizeUnitMeasureCode;
            product.Weight = parameters.Weight;
            product.WeightUnitMeasureCode = parameters.WeightUnitMeasureCode;

            return product.ProductID;
        }

        private DbModel.Product GetProductEnsure(int productId)
        {
            var product = _entities.Products.FirstOrDefault(p => p.ProductID == productId); 

            if (product == null)
            {
                throw new ArgumentException($"Product with Id '{productId}' is not found.", nameof(productId));
            }

            return product;
        }
        
        public void UpdateProduct(int productId, ProductChangeParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentException("Parameters is required", nameof(parameters));
            }

            var product = GetProductEnsure(productId);

            product.Name = parameters.Name;

            _entities.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = GetProductEnsure(productId);

            _entities.Products.Remove(product);

            _entities.SaveChanges();
        }
    }
}
