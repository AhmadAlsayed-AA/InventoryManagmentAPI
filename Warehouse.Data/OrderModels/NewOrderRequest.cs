using System;
using Warehouse.Data.CustomerModels;
using Warehouse.Data.ProductModels;

namespace Warehouse.Data.OrderModels
{
    public class NewOrderRequest
    {

        public int CustomerId { get; set; }
        public string PaymentDetails { get; set; }
        public Address ShippingAddress { get; set; }
        public ICollection<OrderProductRequest> Products { get; set; }


        
    }

}

