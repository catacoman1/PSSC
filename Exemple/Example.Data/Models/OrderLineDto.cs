using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Models
{
    public class OrderLineDto
    {
        public string OrderLineId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public OrderHeaderDto OrderHeader { get; set; }
        public string OrderId { get; set; }

        public ProductDto Product { get; set; }
        public string ProductId { get; set; }
    }
}
