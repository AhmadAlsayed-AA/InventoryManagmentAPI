using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

namespace Warehouse.Data.User
{
	public class RegisterRequest
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Roles UserType { get; set; }

    }
}

