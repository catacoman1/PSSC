using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class ShoppingEvent
    {
        public interface IShoppingEvent { }

        public record ShoppingSucceedEvent : IShoppingEvent
        {
            public DateTime date;
            public double totalPrice;

            internal ShoppingSucceedEvent(DateTime date, double totalPrice)
            {
                this.date = date;
                this.totalPrice = totalPrice;
            }
        }

        public record ShoppingFailedEvent : IShoppingEvent
        {
            public string error;

            internal ShoppingFailedEvent(string error)
            {
                this.error = error;
            }
        }
    }
}
