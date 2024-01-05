using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { Client client { get; } }
        public record EmptyCart(Client client) : ICart;
        public record UnvalidatedCart(Client client, IReadOnlyCollection<UnvalidatedProduct> products) : ICart;
        public record ValidatedCart(Client client, IReadOnlyCollection<ValidatedProduct> products) : ICart;
        public record CalculatedCart(Client client, IReadOnlyCollection<ValidatedProduct> products, double price) : ICart;
        public record PaidCart(Client client, IReadOnlyCollection<ValidatedProduct> products, double finalPrice, DateTime data) : ICart;
    }
}
