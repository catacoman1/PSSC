using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task<List<ProductCode>> TryGetExistingProductCodes();

        Task<Quantity> TryGetQuantityForProduct(ProductCode code);

        Task<Price> TryGetPrice(ProductCode code);
    }
}
