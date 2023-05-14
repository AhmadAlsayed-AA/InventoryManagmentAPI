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

        [EnumDataType(typeof(Roles))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Roles Role { get; set; }

        public bool IsActive { get; set; }

        public RegisterRequest()
        {
            IsActive = true; // Set the default value of isActive to true
        }
    }
}

