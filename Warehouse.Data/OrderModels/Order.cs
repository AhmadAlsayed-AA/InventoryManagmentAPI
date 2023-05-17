using System;
using Warehouse.Data.CustomerModels;
using Warehouse.Data.ProductModels;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

namespace Warehouse.Data.OrderModels
{
	public class Order
	{
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentDetails { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }


        public Order()
		{
		}
	}
}

