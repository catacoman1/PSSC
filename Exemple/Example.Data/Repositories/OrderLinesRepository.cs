using Example.Data.Models;
using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Repositories
{
    public class OrderLinesRepository
    {
        private readonly CartAppDbContext dbContext;

        public OrderLinesRepository(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GenerateRandomID()
        {
            Guid guid;
            string stringID = "";
            bool exist = true;
            while (exist)
            {
                guid = Guid.NewGuid();
                stringID = guid.ToString();
                var found = await dbContext.OrderLines.FindAsync(stringID);
                if (found == null)
                {
                    exist = false;
                }
            }

            return stringID;
        }

        public async Task AddProductLine(ValidatedProduct product, string orderID)
        {
            OrderLineDto orderLine = new OrderLineDto();
            orderLine.OrderId = orderID;
            orderLine.ProductId = product.productID.Value;
            orderLine.Quantity = product.quantity;
            orderLine.Price = product.price;
            orderLine.OrderLineId = await GenerateRandomID();
            await dbContext.OrderLines.AddAsync(orderLine);
            dbContext.SaveChanges();
        }
    }
}
