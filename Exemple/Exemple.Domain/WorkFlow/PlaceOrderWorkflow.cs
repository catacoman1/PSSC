using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using System.Threading.Tasks;
using static Exemple.Domain.Models.PlacedOrderEvent;
using static Exemple.Domain.Operations.ProductOrderOperations;
using static Exemple.Domain.Models.ProductOrder;
using Exemple.Domain.Commands;

namespace Exemple.Domain.WorkFlow
{
    public class PlaceOrderWorkflow
    {
        private IOrdersRepository ordersRepository;
        private IProductsRepository productsRepository;

        public PlaceOrderWorkflow(IOrdersRepository ordersRepository, IProductsRepository productsRepository)
        {
            this.ordersRepository = ordersRepository;
            this.productsRepository = productsRepository;
        }

        public async Task<IPlacedOrderEvent> Execute(PlaceOrderCommand command)
        {
            UnvalidatedOrder unvalidatedOrder = command.InputOrder;

            var productCodes = await productsRepository.TryGetExistingProductCodes();

            IProductOrder order = ValidateOrder(unvalidatedOrder, productCodes);
            order = await VerifyStock(order, productsRepository);
            order = await CalculateTotalPrice(order, productsRepository);

            IPlacedOrderEvent result = null;

            order.Match(
                whenInvalidatedProductOrder: invalidOrder =>
                {
                    result = new PlacedOrderFailedEvent(invalidOrder.Reason);
                    return invalidOrder;
                },
                whenValidatedProductOrder: validOrder => {
                    result = new PlacedOrderFailedEvent("Something went wrong.");
                    return validOrder;
                },
                whenValidatedStock: validStock => {
                    result = new PlacedOrderFailedEvent("Something went wrong.");
                    return validStock;
                },
                whenCalculatedPrice: calculatedPrice => {
                    result = new PlacedOrderSucceededEvent(calculatedPrice.ValidatedOrder, calculatedPrice.TotalPrice);
                    return calculatedPrice;
                });

            return result;
        }
    }
}
