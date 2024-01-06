using Exemple.Domain.Models;
using Exemple.Domain.Operations;
using Exemple.Domain.Interfaces;
using static Exemple.Domain.Models.PaymentEvent;

namespace Exemple.Domain.WorkFlow
{
    public class PaymentWorkflow
    {
        public IPaymentEvent execute(PaymentCommand command)
        {
            PaymentCommand paymentCommand = command;
            PaymentOperation paymentOperation = new PaymentOperation();
            bool payed = paymentOperation.start(paymentCommand.price);
            if (!payed)
            {
                return new PaymentFaileddEvent("Payment failed");
            }
            return new PaymentSucceedEvent(paymentOperation.paymentMethod!);
        }
    }
}
