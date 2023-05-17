using System;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;
using Warehouse.Data.OrderModels;
using Warehouse.Repository;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.ProductModels;

namespace Warehouse.Services
{
    public interface IOrderService
    {
        Order PlaceOrder(NewOrderRequest request);
        Order GetOrder(int orderId);
        void UpdateOrderStatus(int orderId, OrderStatus newStatus);
        void CancelOrder(int orderId);
        List<Order> GetOrdersByCustomer(int customerId);
        List<Order> GetOrders();
        // Additional methods as needed
    }
    public class OrderService : IOrderService

    {
        private readonly WarehouseContext _context;

        public OrderService(WarehouseContext dbContext)
        {
            _context = dbContext;
        }

        public Order PlaceOrder(NewOrderRequest request)
        {
            var customer = _context.Customers.AsNoTracking().Include(u=> u.User).SingleOrDefault(ui => ui.Id == request.CustomerId);
            var products = _context.Products.AsNoTracking().ToList();
            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.Now,
                AddressId = request.ShippingAddress.Id,
                Address = request.ShippingAddress,
                PaymentDetails = request.PaymentDetails,
                OrderStatus = OrderStatus.Pending,
                OrderProducts = new List<OrderProduct>(),
            };
            _context.Orders.Add(order);

            _context.SaveChanges();

            var savedOrder = _context.Orders.Include(o => o.Address).Include(oi => oi.Customer).Include(oit => oit.OrderProducts).SingleOrDefault(o => o.Id == order.Id);
            decimal totalAmount = 0;
            foreach (var productRequest in request.Products)
            {
                var product = products.FirstOrDefault(p => p.Id == productRequest.ProductId);

                if (product != null)
                {
                    var orderItem = new OrderProduct
                    {
                        Order = order,
                        Product = product,
                        Quantity = productRequest.Quantity,
                        UnitPrice = product.Price * productRequest.Quantity
                    };
                    savedOrder.OrderProducts.Add(orderItem);
                    totalAmount += orderItem.UnitPrice;
                }
            }
            savedOrder.TotalAmount = totalAmount;
            _context.SaveChanges();

            return savedOrder;
        }

        public Order GetOrder(int orderId)
        {
            return _context.Orders.AsNoTracking().Include(o => o.Address).Include(oi => oi.OrderProducts).Include(oit => oit.Customer).ThenInclude(u => u.User).FirstOrDefault(o => o.Id == orderId);
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _context.Orders.Include(i => i.OrderProducts).FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                if (order.OrderStatus == OrderStatus.Shipped)
                {
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        var product = _context.Products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                        if (product != null)
                        {
                            product.Quantity -= orderProduct.Quantity;
                        }
                    }

                    
                }
                order.OrderStatus = newStatus;
                _context.SaveChanges();
            }
            
        }

        public void CancelOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.AsNoTracking().Include(o => o.Address).Include(oi => oi.OrderProducts).ThenInclude(op => op.Product).Include(oit => oit.Customer).ThenInclude(u => u.User).ToList();
        }
    }
}

