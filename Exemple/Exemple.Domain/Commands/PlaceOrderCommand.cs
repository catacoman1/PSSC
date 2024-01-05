using Exemple.Domain.Interfaces;
using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Commands
{
    public record UnvalidatedProduct(string productId, int quantity) : IProduct;
}
