﻿using System;
using Warehouse.Data.OrderModels;

namespace Warehouse.Data.ProductModels
{
	public class Product
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }

    }
}

