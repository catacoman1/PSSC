using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain.Interfaces;

namespace Exemple.Domain.Models
{
    public record ValidatedProduct(ProductID productID, string code, int quantity, double price) : IProduct;
}
