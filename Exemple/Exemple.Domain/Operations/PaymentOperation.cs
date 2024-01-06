using Exemple.Domain.Interfaces;
using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exemple.Domain.Operations
{
    public class PaymentOperation
    {
        public IPaymentMethod? paymentMethod { get; private set; }

        public bool start(double price)
        {
            string paymentMethodString = selectPaymentMethod();
            getPaymentMethod(paymentMethodString);
            if (paymentMethod is CardPaymentMethod)
            {
                return true;
            }
            return pay(price);
        }

        private string selectPaymentMethod()
        {
            string paymentMethodString;
            do
            {
                Console.Write("\nAdd the payment method that you want:");
                paymentMethodString = Console.ReadLine()!;

            } while (paymentMethodString.ToLower() != "card" && paymentMethodString.ToLower() != "cash");

            return paymentMethodString;
        }

        private void getPaymentMethod(string paymentMethodString)
        {
            string pattern = @"^\d{2}/\d{2}/\d{4}$";
            if (paymentMethodString == "card")
            {
                Console.Write("Add card number: ");
                long cardNumber = Convert.ToInt64(Console.ReadLine()!);
                Console.Write("Add card cvv: ");
                int cvv = Convert.ToInt32(Console.ReadLine()!);
                string expirationDate;
                do
                {
                    Console.Write("Add card expiration date: ");
                    expirationDate = Console.ReadLine()!;

                } while (!Regex.IsMatch(expirationDate, pattern));
                paymentMethod = new CardPaymentMethod(cardNumber, cvv, expirationDate);
                return;
            }
            paymentMethod = new CashPayementMethod();
        }

        private bool pay(double price)
        {
            Console.WriteLine($"\nYou have to pay {price}$");
            Console.Write("Put the money here: ");
            double payed = Convert.ToDouble(Console.ReadLine());
            if (payed > price)
            {
                Console.WriteLine($"Here is the exchange {payed - price}");
                return true;
            }
            return payed == price ? true : false;
        }

    }
}
