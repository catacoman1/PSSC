using Example.Data;
using Example.Data.Repositories;
using Exemple.Domain.Models;
using Exemple.Domain.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.Cart;
using static Exemple.Domain.Models.PaymentEvent;

namespace Exemple.Domain.Operations
{
    public class CartOperation
    {
        private readonly CartAppDbContext dbContext;
        ProductsRepository productsRepository;
        OrderHeadersRepository orderHeadersRepository;
        OrderLinesRepository orderLinesRepository;

        public CartOperation(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
            productsRepository = new ProductsRepository(dbContext);
            orderHeadersRepository = new OrderHeadersRepository(dbContext);
            orderLinesRepository = new OrderLinesRepository(dbContext);
        }
        public static ICart addProductToCart(EmptyCart emptyCart, IReadOnlyCollection<UnvalidatedProduct> unvalidatedProducts)
        {
            return unvalidatedProducts.Count > 0 ? new UnvalidatedCart(emptyCart.client, unvalidatedProducts) : emptyCart;
        }

        public async Task<ICart> validateCart(UnvalidatedCart unvalidatedCart)
        {
            bool isValid = true;
            List<ValidatedProduct> cartProducts = new List<ValidatedProduct>();
            foreach (var product in unvalidatedCart.products)
            {
                ValidatedProduct validProduct = await productsRepository.TryGetProduct(product.productId, product.quantity);
                if (validProduct != null)
                {
                    cartProducts.Add(validProduct);
                }
                else
                {
                    isValid = false;
                    break;
                }
            }

            return isValid ? new ValidatedCart(unvalidatedCart.client, cartProducts) : unvalidatedCart;
        }

        public static CalculatedCart calculateCart(ValidatedCart validatedCart)
        {
            double total = validatedCart.products.Sum(product => product.price);

            return new CalculatedCart(validatedCart.client, validatedCart.products, total);
        }

        public static ICart payCart(CalculatedCart calculatedCart)
        {
            PaymentWorkflow paymentWorkflow = new PaymentWorkflow();
            PaymentCommand paymentCommand = new PaymentCommand(calculatedCart.price);
            IPaymentEvent res = paymentWorkflow.execute(paymentCommand);
            bool payed = true;
            res.Match(
                whenPaymentSucceedEvent: @event =>
                {
                    Console.WriteLine($"The cart was payed with {@event.paymentMethod.name}");
                    payed = true;
                    return @event;
                },
               whenPaymentFaileddEvent: @event =>
               {
                   Console.WriteLine($"{@event.error}");
                   payed = false;
                   return @event;
               });


            return payed ? new PaidCart(calculatedCart.client, calculatedCart.products, calculatedCart.price, DateTime.Now) : calculatedCart;
        }

        public async Task sendCart(PaidCart cart)
        {
            string orderId = await orderHeadersRepository.createNewOrderHeader(cart.client, cart.finalPrice, cart.data.ToString());
            foreach (var product in cart.products)
            {
                await productsRepository.TryRemoveStoc(product.productID.Value, product.quantity);
                await orderLinesRepository.AddProductLine(product, orderId);
            }
        }
    }
}
