﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Models
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string Code { get; set; }
        public int Stoc { get; set; }
        public double PricePerPiece { get; set; }

        public ICollection<OrderLineDto> OrderLines { get; set; }
    }
}
