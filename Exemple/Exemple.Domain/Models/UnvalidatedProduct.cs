using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain.Interfaces;

namespace Exemple.Domain.Models
{
    public record UnvalidatedProduct(string productId, int quantity) : IProduct;
}
