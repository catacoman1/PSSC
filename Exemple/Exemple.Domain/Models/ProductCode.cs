using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record ProductCode
    {
        public string Value { get; }

        private ProductCode(string value, List<string> availableProductsList)
        {
            if (IsValid(value, availableProductsList))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("There is no such product in our database.");
            }
        }
        public ProductCode(string code)
        {
            this.Value = code;
        }

        private static bool IsValid(string stringValue, List<string> availableProductsList) => availableProductsList.Contains(stringValue.ToLower());

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, List<string> availableProductsList, out ProductCode productCode)
        {
            bool isValid = false;
            productCode = null;

            if (IsValid(stringValue, availableProductsList))
            {
                isValid = true;
                productCode = new(stringValue);
            }

            return isValid;
        }
    }
