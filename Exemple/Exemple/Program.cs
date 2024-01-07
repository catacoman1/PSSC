using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using Exemple.Domain;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Example.Data.Repositories;
using Example.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Exemple.Domain.WorkFlow;
using static Exemple.Domain.Models.ShoppingEvent;

namespace Exemple
{
    class Program
    {
        private static string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Magazin1;Trusted_Connection=True;TrustServerCertificate=True;";

        static async Task Main(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<CartAppDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options;



            using (var dbContextName = new CartAppDbContext())
            {
                dbContextName.Database.Migrate();
            }

            var dbContext = new CartAppDbContext();
            StartShoppingCommand shoppingCommand = addShoppingDetails();
            ShoppingWorkflow shoppingWorkflow = new ShoppingWorkflow(dbContext);
            IShoppingEvent res = await shoppingWorkflow.execute(shoppingCommand);
            res.Match(
                    whenShoppingFailedEvent: @event =>
                    {
                        Console.WriteLine($"\nShopping failed: {@event.error}");
                        return @event;
                    },
                    whenShoppingSucceedEvent: @event =>
                    {
                        Console.WriteLine($"\nShopping succeed:\nDate: {@event.date}\nPrice payed: {@event.totalPrice}");
                        return @event;
                    }


                    );

            Console.Write("\nEnter Order ID to cancel or leave empty to skip: ");
            var orderId = Console.ReadLine();
            if (!string.IsNullOrEmpty(orderId))
            {
                CancelWorkflow cancelWorkflow = new CancelWorkflow(dbContext);
                var cancellationResult = await cancelWorkflow.Execute(orderId);
                cancellationResult.Match(
                whenCancellationSucceedEvent: @event =>
                    {
                        Console.WriteLine($"\nOrder cancellation succeeded: Order ID {@event.OrderId} has been cancelled.");
                        return @event;
                    },
                whenCancellationFailedEvent: @event =>
                    {
                        Console.WriteLine($"\nOrder cancellation failed: {@event.Error}");
                        return @event;
                    }
                );
            }
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();




        }
        static StartShoppingCommand addShoppingDetails()
        {
            Console.Write("Nume: ");
            var nume = Console.ReadLine();
            Console.Write("Prenume: ");
            var prenume = Console.ReadLine();
            Console.Write("Telefon: ");
            var telefon = Console.ReadLine();
            Console.Write("Adresa: ");
            var adresa = Console.ReadLine();

            Client client = new Client(nume!, prenume!, Convert.ToInt64(telefon), adresa!);
            List<UnvalidatedProduct> unvalidatedProducts = new List<UnvalidatedProduct>();
            string productId;
            Console.WriteLine("\n To stop the adding of products press 0.");
            do
            {
                Console.Write("Add product with id = ");
                productId = Console.ReadLine()!;
                if (productId != "0")
                {
                    Console.Write("Add product quantity = ");
                    int quantity = Convert.ToInt32(Console.ReadLine())!;
                    unvalidatedProducts.Add(new UnvalidatedProduct(productId, quantity));
                }
            } while (productId != "0");

            return new StartShoppingCommand(client, unvalidatedProducts);
        }
    }
}

