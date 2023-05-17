using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Warehouse.Data.UserModels
{
	public class User
	{
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public User()
        {
            IsActive = true; // Set the default value of isActive to true
        }
    }
}

