using Example.Data.Repositories;
using Example.Data;
using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Operations
{
    public class CancelOperation
    {
        private readonly CartAppDbContext dbContext;
        private readonly ProductsRepository productsRepository;
        private readonly OrderHeadersRepository orderHeadersRepository;
        private readonly OrderLinesRepository orderLinesRepository;

        public CancelOperation(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.productsRepository = new ProductsRepository(dbContext);
            this.orderHeadersRepository = new OrderHeadersRepository(dbContext);
            this.orderLinesRepository = new OrderLinesRepository(dbContext);
        }

        public async Task<CancellationEvent> ExecuteCancellation(string orderId)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    
                    var orderHeader = await orderHeadersRepository.GetOrderHeaderById(orderId);
                    if (orderHeader == null)
                    {
                        return new CancellationEvent.CancellationFailedEvent("Order not found.");
                    }

                   
                    var orderLines = await orderLinesRepository.GetOrderLinesByOrderId(orderId);

                    //restock
                    foreach (var line in orderLines)
                    {
                       
                        await productsRepository.RestockProduct(line.ProductId, line.Quantity);
                        dbContext.OrderLines.Remove(line); 
                    }

                    await dbContext.SaveChangesAsync();
                    await orderHeadersRepository.DeleteOrderHeader(orderId);
                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new CancellationEvent.CancellationSucceedEvent(orderId);
                }
                catch (Exception ex)
                {
                    // loguri pentru a prinde InnerException.Message (nu merge daca GetOrderLinesByOrderId din orderlines repo este asincrona)
                    await transaction.RollbackAsync();

                    Console.WriteLine($"Exception: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }

                    return new CancellationEvent.CancellationFailedEvent("An error occurred during cancellation.");
                }
            }
        }
    }
}
