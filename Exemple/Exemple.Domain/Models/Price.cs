using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record Price
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidPriceException($"{value} is an invalid price value.");
            }
        }


        public static bool TryParsePrice(string priceString, out Price price)
        {
            bool isValid = false;
            price = null;
            if (decimal.TryParse(priceString, out decimal numericPrice))
            {
                if (IsValid(numericPrice))
                {
                    isValid = true;
                    price = new(numericPrice);
                }
            }

            return isValid;
        }

        private static bool IsValid(decimal numericPrice) => numericPrice > 0;
    }

}
