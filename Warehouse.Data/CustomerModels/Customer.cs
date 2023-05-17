using System;
using System.Net;
using Warehouse.Data.OrderModels;
using Warehouse.Data.UserModels;

namespace Warehouse.Data.CustomerModels
{
	public class Customer
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

