using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Warehouse.Data.User;
using Warehouse.Repository;
using Warehouse.Services.Helpers.SecurityHelper;
using Warehouse.Services.Helpers.Validation;

namespace Warehouse.Services
{
	 public interface IUserService
    {
        public Task<List<User>> getAll();

        public Task<User> getById(int id);

        public Task<UserResponse> register(RegisterRequest request);

        public Task<UserResponse> update(int id,UpdateRequest request);

        public Task delete(int id);

        public Task<UserResponse> signIn(AuthRequest authRequest);
        public Task changeIsActive(int id, bool isActive);
    }

    public class UserService: IUserService
	{
        private readonly IConfiguration _configuration;
        private readonly WarehouseContext _context;
        private readonly IMapper _mapper;
        private readonly Validate _validate;
        private readonly Security _security;
        private ITokenService _tokenService;
        public UserService(WarehouseContext context, IConfiguration configuration, IMapper mapper, Validate validate, Security security, ITokenService tokenService)
        {
            _validate = validate;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _security = security;
            _tokenService = tokenService;
        }



        public async Task<UserResponse> register(RegisterRequest request)
        {
            try { 
            var validationResponse = _validate.ValidateRegisterRequest(request);
            if (!validationResponse.IsValid)
            {
                throw new ValidationException(validationResponse.Errors);
            }

             // hash password
                _security.createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // map model to new user object
                var user = _mapper.Map<User>(request);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                // save user
                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return _mapper.Map<UserResponse>(user);
            }
            catch (ValidationException)
            {
                // Re-throw the exception since it already contains the correct error information
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException(new List<string> { ex.Message });
            }
        }


        public async Task<List<User>> getAll()
        {
            //.Include(x => x.Orders)
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> getById(int id)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserResponse> signIn( AuthRequest authRequest)
        {
            User user;
            var validationResponse = _validate.ValidateAuthRequest(authRequest);

            if (!validationResponse.IsValid)
            {
                throw new ValidationException(validationResponse.Errors);
            }
            user = await _context.Users.SingleOrDefaultAsync(i => i.Email == authRequest.Email);

            if (user == null)
            {
                validationResponse.Errors.Add("Incorrect Credentials");
            }
            else if (!_security.verifyPasswordHash(authRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                validationResponse.Errors.Add("Incorrect Password");
            }

            if (validationResponse.Errors.Any())
            {
                throw new ValidationException(validationResponse.Errors);
            }

            var response = _mapper.Map<UserResponse>(user);
            response.Token = _tokenService.GenerateToken(user);


            return response;
        }

        public async Task delete(int id)
        {
            var user = await getById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<UserResponse> update(int id, UpdateRequest request)
        {
            try
            {
                var user = _context.Users.Find(id);

                // validate the input
                var validationResponse = _validate.ValidateUpdateRequest(request, user);

                if (!validationResponse.IsValid)
                {
                    throw new ValidationException(validationResponse.Errors);
                }

                _mapper.Map(request, user);


                await _context.SaveChangesAsync();

                return _mapper.Map<UserResponse>(user);
            }
            catch (ValidationException)
            {
                // Re-throw the exception since it already contains the correct error information
                throw;
            }
            catch (Exception ex)
            {
                // Create a new ValidationException with the caught exception message
                throw new ValidationException(new List<string> { "Internal Server Errror" });
            }
        }
        public async Task changeIsActive(int id, bool isActive)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

    }
}

