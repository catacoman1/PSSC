using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public class PaymentCommand
    {
        public double price { get; }
        public PaymentCommand(double price)
        {
            this.price = price;
        }
    }
}
