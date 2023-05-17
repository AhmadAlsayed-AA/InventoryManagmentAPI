using System;
namespace Warehouse.Data.HelperModels.LocalEnums
{
	public class Enums
	{
        public enum Roles
        {
            ADMIN,
            MANAGER,
            EMPLOYEE,
            CUSTOMER,
        }
        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled
        }
    }
}

