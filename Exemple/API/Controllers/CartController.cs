using Example.Data;
using Exemple.Domain.Models;
using Exemple.Domain.Operations;
using Microsoft.AspNetCore.Mvc;
using static Exemple.Domain.Models.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
 
        [ApiController]
        [Route("[controller]")]
        public class CartController : ControllerBase
    {
            private readonly CartAppDbContext context;
            public static ICart myCart = null;
            public CartController(CartAppDbContext context)
            {
                this.context = context;
            }

            public struct GetCartResponse
            {
                public string stare { get; set; }
                public ICart myCart { get; set; }

                public GetCartResponse(string stare, ICart myCart)
                {
                    this.stare = stare;
                    this.myCart = myCart;
                }
            }

            [HttpGet]
            public async Task<ActionResult> GetCart()
            {
                if (myCart is null)
                {
                    return NotFound();
                }
                string stare = "";
                myCart.Match(
                        whenEmptyCart: @event =>
                        {
                            stare = "EmptyCart";
                            return @event;
                        },
                        whenUnvalidatedCart: @event =>
                        {
                            stare = "UnvalidatedCart";
                            return @event;
                        },
                        whenValidatedCart: @event =>
                        {
                            stare = "ValidatedCart";
                            return @event;
                        },
                        whenCalculatedCart: @event =>
                        {
                            stare = "CalculatedCart";
                            return @event;
                        },
                        whenPaidCart: @event =>
                        {
                            stare = "PaidCart";
                            return @event;
                        }
                    );

                return Ok(new GetCartResponse(stare, myCart));
            }

            [HttpPost]
            public async Task<ActionResult> AddProduct(IReadOnlyCollection<UnvalidatedProduct> products)
            {
                CartOperation cartOperation = new CartOperation(context);
                myCart = CartOperation.addProductToCart((EmptyCart)myCart, products);
                if (myCart is not UnvalidatedCart)
                {
                    return Ok(new string("Empty"));
                }
                myCart = await cartOperation.validateCart((UnvalidatedCart)myCart);
                if (myCart is not ValidatedCart)
                {
                    return Ok(new string("UnvalidatedCart"));
                }
                myCart = CartOperation.calculateCart((ValidatedCart)myCart);
                if (myCart is not CalculatedCart)
                {
                    return Ok(new string("ValidatedCart"));
                }
                return Ok(new string("Calculated"));
            }

            [HttpPost]
            [Route("pay")]
            public async Task<ActionResult> PayCart()
            {
                if (myCart is not CalculatedCart)
                {
                    return BadRequest();
                }
                myCart = CartOperation.payCart((CalculatedCart)myCart);
                if (myCart is PaidCart)
                {
                    CartOperation cartOperation = new CartOperation(context);
                    await cartOperation.sendCart((PaidCart)myCart);
                }
                return Ok();
            }

            [HttpPost]
            [Route("newCart")]
            public async Task<ActionResult> NewCart(Client client)
            {
                myCart = new EmptyCart(client);

                return Ok();
            }
        }
    }

