using Exemple.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Repositories
{
    public class ProductsRepository
    {
        private readonly CartAppDbContext dbContext;

        public ProductsRepository(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ValidatedProduct> TryGetProduct(string searchedProduct, int quantity)
        {
            var product = await dbContext.Products
                                         .Where(p => p.ProductId == searchedProduct && p.Stoc >= quantity)
                                         .FirstOrDefaultAsync();

            ValidatedProduct validatedProduct = null;
            if (product != null)
            {
                ProductID productID = new ProductID(searchedProduct);
                validatedProduct = new ValidatedProduct(productID, product.Code, quantity, product.PricePerPiece * quantity);
            }

            return validatedProduct;
        }

        public async Task TryRemoveStoc(string productId, int quantity)
        {
            var product = await dbContext.Products
                                         .Where(p => p.ProductId == productId)
                                         .FirstOrDefaultAsync();

            if (product != null)
            {
                product.Stoc -= quantity;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
