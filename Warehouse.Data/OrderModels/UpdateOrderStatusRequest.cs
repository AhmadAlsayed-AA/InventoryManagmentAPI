using System;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

namespace Warehouse.Data.OrderModels
{
	public class UpdateOrderStatusRequest
	{
        public OrderStatus NewStatus { get; set; }

    }
}

