using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;
namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class PlacedOrderEvent
    {
        public interface IPlacedOrderEvent { }

        public record PlacedOrderSucceededEvent : IPlacedOrderEvent
        {
            public ValidatedOrder Order { get; }
            public Price TotalPrice { get; }

            internal PlacedOrderSucceededEvent(ValidatedOrder order, Price totalPrice)
            {
                Order = order;
                TotalPrice = totalPrice;
            }

        }

        public record PlacedOrderFailedEvent : IPlacedOrderEvent
        {
            public string Reason { get; }

            internal PlacedOrderFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
