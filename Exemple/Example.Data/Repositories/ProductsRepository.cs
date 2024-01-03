using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsContext productsContext;

        public ProductsRepository(ProductsContext dBContext)
        {
            this.productsContext = dBContext;
        }

        public async Task<List<ProductCode>> TryGetExistingProductCodes()
        {
            var products = await productsContext.Products.AsNoTracking().ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                           .ToList();
        }
        public async Task<Quantity> TryGetQuantityForProduct(ProductCode code)
        {
            var product = await productsContext.Products.FirstAsync(product => product.Code.Equals(code.Value));

            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            else
            {
                return new Quantity((int)product.Stoc);
            }
        }
        public async Task<Price> TryGetPrice(ProductCode code)
        {
            var price = from p in productsContext.Products
                        join o in productsContext.Orders on p.ProductId equals o.ProductId
                        where p.Code == code.Value
                        select o.Price; 
            var priceValue = await price.FirstOrDefaultAsync();

            if (priceValue == null)
            {
                throw new Exception("Product not found in order header!");
            }
            else
            {
                return new Price((decimal)priceValue);
            }
        }
    }
}
