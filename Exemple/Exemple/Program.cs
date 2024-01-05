using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Models.ProductOrder;
using Exemple.Domain;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Example.Data.Repositories;
using Example.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Exemple.Domain.Commands;
using Exemple.Domain.Repositories;
using Exemple.Domain.WorkFlow;

namespace Exemple
{
    class Program
    {


        static async Task Main(string[] args)
        {

        const string connectionString = "Server=localhost\\SQLEXPRESS;Database=Magazin;Trusted_Connection=True;";

        var dbContextBuilder = new DbContextOptionsBuilder<ProductsContext>()
                                                .UseSqlServer(connectionString);

            ProductsContext productsContext = new(dbContextBuilder.Options);

            //Console.WriteLine("Debug: Successfully connected to the database.");

            OrderHeadersRepository ordersRepository = new(productsContext);
            ProductsRepository productsRepository = new(productsContext);
            var order = ReadOrder();
            PlaceOrderCommand command = new(order);
            PlaceOrderWorkflow workflow = new(ordersRepository, productsRepository);
            var result = await workflow.Execute(command);
            //Console.WriteLine(result);

            result.Match(
                    whenPlacedOrderSucceededEvent: @event =>
                    {
                        Console.WriteLine($"Order succeeded. {@event.TotalPrice}");
                        return @event;
                    },
                    whenPlacedOrderFailedEvent: @event =>
                    {
                        Console.WriteLine($"Order failed : {@event.Reason}");
                        return @event;
                    }
                );
        }


        private static UnvalidatedOrder ReadOrder()
        {

            var adress = ReadValue("Adress: ");

            List<UnvalidatedProduct> listOfProducts = new();
            do
            {
                var productCode = ReadValue("Product name (or enter 'done' to finish): ");
                if (string.IsNullOrEmpty(productCode) || productCode.ToLower() == "done")
                {
                    break;
                }

                var quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

                var price = ReadValue("Price: ");
                if (string.IsNullOrEmpty(price))
                {
                    break;
                }

                listOfProducts.Add(new(productCode, quantity, price));
                //Console.WriteLine(listOfProducts.Count);
            } while (true);
            return new UnvalidatedOrder(adress, listOfProducts);
            
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
