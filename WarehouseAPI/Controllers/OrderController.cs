using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;
using Warehouse.Data.OrderModels;
using Warehouse.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        // GET: api/values
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE, CUSTOMER")]

        public IActionResult PlaceOrder([FromBody] NewOrderRequest request)
        {
   
            
            var order = _orderService.PlaceOrder(request);

            // You can return the created order or an appropriate response based on your API design
            return Ok(order);
        }

        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE, CUSTOMER")]

        public IActionResult GetOrder(int orderId)
        {
            var order = _orderService.GetOrder(orderId);
            if (order == null)
            {
                // Order not found, return an appropriate response
                return NotFound();
            }

            // Return the order details
            return Ok(order);
        }

        [HttpPatch("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER")]

        public IActionResult UpdateOrderStatus(int Id, UpdateOrderStatusRequest request)
        {
            // Assuming you have an UpdateOrderStatusRequest class to receive the updated order status from the client

            // Validate the request data here (e.g., request validation, authorization, etc.)

            _orderService.UpdateOrderStatus(Id, request.NewStatus);

            // Return a success response
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE")]

        public IActionResult CancelOrder(int orderId)
        {
            _orderService.CancelOrder(orderId);

            // Return a success response
            return Ok();
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE")]

        public IActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();

            // Return the orders associated with the customer
            return Ok(orders);
        }

        [HttpGet("customer/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE")]

        public IActionResult GetOrdersByCustomer(int customerId)
        {
            var orders = _orderService.GetOrdersByCustomer(customerId);

            // Return the orders associated with the customer
            return Ok(orders);
        }
    }
}

