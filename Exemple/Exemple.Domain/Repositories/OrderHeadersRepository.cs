using Example.Data.Models;
using Exemple.Domain.Models;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Repositories
{
    public class OrderHeadersRepository
    {
        private readonly CartAppDbContext dbContext;

        public OrderHeadersRepository(CartAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GenerateRandomID()
        {
            Guid guid;
            string stringID = "";
            bool exist = true;
            while (exist)
            {
                guid = Guid.NewGuid();
                stringID = guid.ToString();
                var found = await dbContext.OrderHeaders.FindAsync(stringID);
                if (found == null)
                {
                    exist = false;
                }
            }

            return stringID;
        }

        public async Task<string> createNewOrderHeader(Client client, double total, string date)
        {
            OrderHeaderDto orderHeader = new OrderHeaderDto();
            orderHeader.OrderId = await GenerateRandomID();
            orderHeader.ClientId = client.clientId.ToString();
            orderHeader.FirstName = client.prenume;
            orderHeader.LastName = client.nume;
            orderHeader.PhoneNumber = client.numarDeTelefon;
            orderHeader.Date = date;
            orderHeader.Address = client.adresa;
            orderHeader.Total = total;
            await dbContext.OrderHeaders.AddAsync(orderHeader);
            dbContext.SaveChanges();
            return orderHeader.OrderId;
        }
    }
}

