using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Services;
using Warehouse.Services.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Warehouse.Data.UserModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private ITokenService _tokenService;
        private IMapper _mapper;

        public UserController(
            IUserService userService, IMapper mapper, ITokenService tokenService)

        {
            _mapper = mapper;

            _userService = userService;
            _tokenService = tokenService;


        }


        [HttpPost("SignIn")]

        public async Task<ActionResult<UserResponse>> signIn([FromBody] AuthRequest request)
        {
            try
            {
                UserResponse response;


                return Ok(await _userService.signIn(request));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
        {
            try
            {
                var response = await _userService.register(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult<User[]>> GetAll()
        {
            var users = await _userService.getAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = " ADMIN")]
        public IActionResult GetById(int id)
        {
            var user = _userService.getById(id);
            if (user is null)
                return NotFound("User Does not Exist");

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = " ADMIN")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UpdateRequest request)
        {
            try
            {
                var response = await _userService.update(id, request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.delete(id);
                return Ok("User Deleted");

            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, MANAGER")]
        public async Task<ActionResult> ChangeIsActive(int id, bool isActive)
        {
            try
            {
                await _userService.changeIsActive(id, isActive);
                return Ok(new { message = "IsActive updated successfully" });

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

