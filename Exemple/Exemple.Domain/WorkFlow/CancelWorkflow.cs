using Example.Data;
using Exemple.Domain.Models;
using Exemple.Domain.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.WorkFlow
{
    public class CancelWorkflow
    {
        private readonly CancelOperation cancelOperation;
        public CancelWorkflow(CartAppDbContext dbContext)
        {
            this.cancelOperation = new CancelOperation(dbContext);
        }

        public async Task<CancellationEvent> Execute(string orderId)
        {
            return await cancelOperation.ExecuteCancellation(orderId);
        }
    }
}
