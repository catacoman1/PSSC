using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Commands
{
    public record PlaceOrderCommand
    {
        public PlaceOrderCommand(UnvalidatedOrder inputOrder)
        {
            InputOrder = inputOrder;
        }

        public UnvalidatedOrder InputOrder { get; }
    }
}
