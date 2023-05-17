using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Data.CustomerModels;
using Warehouse.Data.UserModels;
using Warehouse.Services;
using static Warehouse.Data.HelperModels.LocalEnums.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        // GET: api/values
        private ICustomerService _customerService;

        public CustomerController(IUserService userService, ICustomerService customerService)
        {

            _customerService = customerService; 
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(RegisterRequest request)
        {
            var customer = await _customerService.create(request);
            return Ok(customer);

        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER")]

        public IActionResult GetAll()
        {
            var customers = _customerService.getAll();
            return Ok(customers);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE")]

        public IActionResult GetById(int id)
        {
            var customer = _customerService.getById(id);
            if (customer is null)
                return NotFound("Customer Does not Exist");
            return Ok(customer);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER, EMPLOYEE")]

        public IActionResult Update(int id, UpdateRequest model)
        {
            try
            {


                return Ok(_customerService.update(id, model));

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Customer Does not Exist");
            }
        }



        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER")]

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public IActionResult Delete(int id)
        {
            try
            {
                _customerService.delete(id);
                return Ok(new { message = "Customer deleted successfully" });

            }
            catch (Exception e)
            {
                return NotFound("Customer does not Exist");
            }
        }
    }
}

