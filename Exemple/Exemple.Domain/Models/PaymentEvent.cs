using CSharp.Choices;
using Exemple.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class PaymentEvent
    {
        public interface IPaymentEvent { }

        public record PaymentSucceedEvent : IPaymentEvent
        {
            public IPaymentMethod paymentMethod;

            public PaymentSucceedEvent(IPaymentMethod paymentMethod)
            {
                this.paymentMethod = paymentMethod;
            }
        }

        public record PaymentFaileddEvent : IPaymentEvent
        {
            public string error;

            public PaymentFaileddEvent(string error)
            {
                this.error = error;
            }
        }
    }
}
