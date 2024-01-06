using Example.Data;
using Exemple.Domain.Models;
using Exemple.Domain.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.Cart;
using static Exemple.Domain.Models.ShoppingEvent;

namespace Exemple.Domain.WorkFlow
{
    public class ShoppingWorkflow
    {
        private readonly CartAppDbContext dbContext;
        CartOperation cartOperation;
        public ShoppingWorkflow(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
            cartOperation = new CartOperation(dbContext);
        }

        public async Task<IShoppingEvent> execute(StartShoppingCommand command)
        {
            StartShoppingCommand shoppingCommand = command;

            var res = await workflowProcess(shoppingCommand, cartOperation.validateCart);
            IShoppingEvent shoppingEvent = null!;
            res.Match(
                    whenEmptyCart: @event =>
                    {
                        shoppingEvent = new ShoppingFailedEvent("Cart is empty");
                        return @event;
                    },
                    whenUnvalidatedCart: @event =>
                    {
                        shoppingEvent = new ShoppingFailedEvent("Cart is unvalidate");
                        return @event;
                    },
                    whenValidatedCart: @event =>
                    {
                        shoppingEvent = new ShoppingFailedEvent("Cart is validate but we have internal server problems");
                        return @event;
                    },
                    whenCalculatedCart: @event =>
                    {
                        shoppingEvent = new ShoppingFailedEvent("Cart is caluclate but is not payed");
                        return @event;
                    },
                    whenPaidCart: @event =>
                    {
                        shoppingEvent = new ShoppingSucceedEvent(@event.data, @event.finalPrice);
                        return @event;
                    }
                );
            return shoppingEvent!;
        }

        public async Task<ICart> workflowProcess(StartShoppingCommand shoppingCommand, Func<UnvalidatedCart, Task<ICart>> validationFunction)
        {

            ICart cart = new EmptyCart(shoppingCommand.client);
            cart = CartOperation.addProductToCart((EmptyCart)cart, shoppingCommand.unvalidatedProducts);
            if (cart is not UnvalidatedCart)
            {
                return cart;
            }
            cart = await validationFunction((UnvalidatedCart)cart);
            if (cart is not ValidatedCart)
            {
                return cart;
            }
            cart = CartOperation.calculateCart((ValidatedCart)cart);
            if (cart is not CalculatedCart)
            {
                return cart;
            }
            cart = CartOperation.payCart((CalculatedCart)cart);
            if (cart is PaidCart)
            {
                await cartOperation.sendCart((PaidCart)cart);
            }

            return cart;
        }
    }
}
