using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public class StartShoppingCommand
    {
        public Client client { get; }
        public List<UnvalidatedProduct> unvalidatedProducts { get; }
        public StartShoppingCommand(Client client, List<UnvalidatedProduct> unvalidatedProducts)
        {
            this.client = client;
            this.unvalidatedProducts = unvalidatedProducts;
        }
    }
}
