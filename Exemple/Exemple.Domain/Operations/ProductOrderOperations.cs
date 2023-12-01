using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.ProductOrder;

namespace Exemple.Domain.Operations
{
    public static class ProductOrderOperations
    {

        public static IProductOrder ValidateOrder(UnvalidatedOrder order, List<ProductCode> productCodes)
        {
            List<ValidatedProduct> validatedProducts = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var unvalidatedProduct in order.Products)
            {
                if (!Quantity.TryParseQuantity(unvalidatedProduct.Quantity, out Quantity quantity))
                {
                    invalidReson = $"Invalid quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Quantity})";
                    isValidList = false;
                    break;
                }
                if (!Price.TryParsePrice(unvalidatedProduct.Price, out Price price))
                {
                    invalidReson = $"Invalid price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Price})";
                    isValidList = false;
                    break;
                }
                if (!ProductCode.TryParse(unvalidatedProduct.ProductCode, productCodes.Select(p => p.Value).ToList(), out ProductCode productCode))
                {
                    invalidReson = $"Invalid product code ({unvalidatedProduct.ProductCode})";
                    isValidList = false;
                    break;
                }
                ValidatedProduct validGrade = new(productCode, quantity, price);
                validatedProducts.Add(validGrade);
            }

            if (isValidList)
            {
                return new ValidatedProductOrder(new ValidatedOrder(order.Adress, validatedProducts));
            }
            else
            {
                return new InvalidatedProductOrder(invalidReson);
            }

        }

        public static Task<IProductOrder> VerifyStock(IProductOrder order, IProductsRepository productRepository) => order.MatchAsync(
                whenInvalidatedProductOrder: async invalidOrder => invalidOrder,
                whenValidatedProductOrder: async validOrder => {
                    List<ValidatedProduct> validatedProducts = new();
                    bool isValidList = true;
                    string invalidReson = string.Empty;
                    foreach (var validatedProduct in validOrder.ValidatedOrder.Products)
                    {
                        var stoc = await productRepository.TryGetQuantityForProduct(validatedProduct.productCode);
                        if (validatedProduct.Quantity.Value > stoc.Value)
                        {
                            invalidReson = $"Not enough stock ({validatedProduct.productCode}, {validatedProduct.Quantity})";
                            isValidList = false;
                            break;
                        }
                        validatedProducts.Add(validatedProduct);
                    }

                    if (isValidList)
                    {
                        return new ValidatedStock(validOrder.ValidatedOrder);
                    }
                    else
                    {
                        return new InvalidatedProductOrder(invalidReson);
                    }

                },
                whenValidatedStock: async validStock => validStock,
                whenCalculatedPrice: async calculatedPrice => calculatedPrice
            );


        public static Task<IProductOrder> CalculateTotalPrice(IProductOrder order, IProductsRepository productsRepository) => order.MatchAsync(
            whenInvalidatedProductOrder: async invalidOrder => invalidOrder,
            whenValidatedProductOrder: async validOrder => validOrder,
            whenValidatedStock: async validStock => {

                decimal totalPrice = 0;

                foreach (var validatedProduct in validStock.ValidatedOrder.Products)
                {
                    var productPrice = await productsRepository.TryGetPrice(validatedProduct.productCode);
                    totalPrice += productPrice.Value * validatedProduct.Quantity.Value;
                }

                return new CalculatedPrice(validStock.ValidatedOrder, new Price(totalPrice));
            },
            whenCalculatedPrice: async calculatedPrice => calculatedPrice
        );



    }
}
