using System;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

namespace Warehouse.Data.User
{
	public class UpdateRequest
	{
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public Roles role { get; set; }

        public bool IsActive { get; set; }

       
    }
}

