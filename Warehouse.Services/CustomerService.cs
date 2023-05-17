using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.CustomerModels;
using Warehouse.Data.UserModels;
using Warehouse.Repository;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

namespace Warehouse.Services
{
    public interface ICustomerService
    {
        public List<Customer> getAll();
        public Customer getById(int id);
        public Task<Customer> create(RegisterRequest request);
        public void delete(int id);
        public Customer update(int id, UpdateRequest request);

    }
    public class CustomerService : ICustomerService
    {
        private IUserService _userService;
        private readonly WarehouseContext _context;
        private readonly IMapper _mapper;
        public CustomerService(IUserService userService, WarehouseContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Customer> create(RegisterRequest request)
        {
            // Call the user service to register the new user
            request.Role = Roles.CUSTOMER;
            var registeredUser = await _userService.register(request);

            // Check if the user is already tracked by the context
            User user = await _context.Users.FindAsync(registeredUser.Id);

            if (user == null)
            {
                // If the user is not tracked, create a new one
                user = _mapper.Map<User>(registeredUser);
            }
            else
            {
                // If the user is already tracked, update its properties
                _mapper.Map(registeredUser, user);
            }

            // Create the new customer
            var newCustomer = new Customer { User = user };

            // Add the new customer to the context and save changes
            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            return newCustomer;
        }
        public Customer update(int id, UpdateRequest request)
        {
            var customer = getById(id);
            _userService.update(customer.User.Id, request);
            customer = _mapper.Map(request, customer);

            _context.SaveChanges();

            return customer;
        }

        public void delete(int id)
        {
            var customer = getById(id);


            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public List<Customer> getAll()
        {
            return _context.Customers.AsNoTracking().Include(i => i.Orders).Include(x => x.User).ToList();
        }

        public Customer getById(int id)
        {
            return _context.Customers.AsNoTracking().Include(i => i.Orders).Include(i => i.User).SingleOrDefault(u => u.Id == id);
        }

        
    }
}

