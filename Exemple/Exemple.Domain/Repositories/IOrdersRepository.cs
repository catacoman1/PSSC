using Exemple.Domain.Models;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.ProductOrder;

namespace Exemple.Domain.Repositories
{
    public interface IOrdersRepository
    {
        TryAsync<List<ValidatedProduct>> TryGetExistingOrders();
        TryAsync<Unit> TrySaveOrders(ValidatedProductOrder order);
    }
}
