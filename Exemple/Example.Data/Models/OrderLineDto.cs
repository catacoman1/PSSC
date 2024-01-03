using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Models
{
    public class OrderLineDto
    {
        public int OrderLineId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
