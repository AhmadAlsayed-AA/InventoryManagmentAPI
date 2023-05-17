using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.OrderModels
{
	public class Address
	{
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }
    }
}

