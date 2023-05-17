using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;
using Warehouse.Data.OrderModels;
using Warehouse.Services;

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
        public IActionResult PlaceOrder([FromBody] NewOrderRequest request)
        {
   
            
            var order = _orderService.PlaceOrder(request);

            // You can return the created order or an appropriate response based on your API design
            return Ok(order);
        }

        [HttpGet("{Id}")]
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

        [HttpPut("{orderId}/status")]
        public IActionResult UpdateOrderStatus(int orderId, UpdateOrderStatusRequest request)
        {
            // Assuming you have an UpdateOrderStatusRequest class to receive the updated order status from the client

            // Validate the request data here (e.g., request validation, authorization, etc.)

            _orderService.UpdateOrderStatus(orderId, request.NewStatus);

            // Return a success response
            return Ok();
        }

        [HttpDelete("{orderId}")]
        public IActionResult CancelOrder(int orderId)
        {
            _orderService.CancelOrder(orderId);

            // Return a success response
            return Ok();
        }
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();

            // Return the orders associated with the customer
            return Ok(orders);
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(int customerId)
        {
            var orders = _orderService.GetOrdersByCustomer(customerId);

            // Return the orders associated with the customer
            return Ok(orders);
        }
    }
}

