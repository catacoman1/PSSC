using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
	public record ValidatedProduct(ProductCode productCode, Quantity Quantity, Price price);

	public record ValidatedOrder(string Adress, List<ValidatedProduct> Products);
}
