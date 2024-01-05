using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public class CashPayementMethod : IPaymentMethod
    {
        public CashPayementMethod()
        {
            this.name = "Cash";
        }

        public string name { get; }
    }
}
