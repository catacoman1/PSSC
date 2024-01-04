using Example.Data.Models;
using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ProductsContext productsContext;

        public OrdersRepository(ProductsContext dBContext)
        {
            this.productsContext = dBContext;
        }

        public TryAsync<List<ValidatedProduct>> TryGetExistingOrders()
        {
            throw new NotImplementedException();
        }



        public TryAsync<Unit> TrySaveOrders(ProductOrder.ValidatedProductOrder validatedOrder)
        {
            return async () =>
            {
                foreach (var validatedProduct in validatedOrder.ValidatedOrder.Products)
                {
                    var productCode = validatedProduct.productCode.Value;
                    var quantity = validatedProduct.Quantity.Value;
                    var price = validatedProduct.price.Value;

                    var product = await productsContext.Products
                        .FirstOrDefaultAsync(p => p.Code == productCode);

                    if (product == null)
                    {
                        throw new ArgumentException("Product not found!");
                    }

                    var orderLineDto = new OrderLineDto
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        Price = price
                    };

                    productsContext.Orders.Add(orderLineDto);
                }

                await productsContext.SaveChangesAsync();
                return Unit.Default;
            };
        }
    }
    }

