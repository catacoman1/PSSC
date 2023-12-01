using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record UnvalidatedOrder(string Adress, List<UnvalidatedProduct> Products);
    public record UnvalidatedProduct(string ProductCode, string Quantity, string Price);
}
