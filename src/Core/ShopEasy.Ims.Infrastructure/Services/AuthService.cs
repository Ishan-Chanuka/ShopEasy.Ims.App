using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Models.RequestModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;
using ShopEasy.Ims.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ShopEasy.Ims.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        #region Private Fields
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string? _secretKey;
        #endregion

        #region Constructor
        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _secretKey = configuration["ApiSettings:SecretKey"];
        }
        #endregion

        public async Task<ApiResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new ApiResponse<LoginResponseModel>((int)HttpStatusCode.BadRequest, "Invalid email or password", false);
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new ApiResponse<LoginResponseModel>((int)HttpStatusCode.BadRequest, "Invalid email or password", false);
            }

            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                    ]),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var response = new LoginResponseModel
                {
                    Token = tokenHandler.WriteToken(token)
                };

                return new ApiResponse<LoginResponseModel>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Login successful",
                    IsSuccess = true,
                    Data = response
                };
            }
            catch (Exception)
            {
                return new ApiResponse<LoginResponseModel>((int)HttpStatusCode.InternalServerError, "An error occurred while logging in", false);
            }
        }

        public async Task<ApiResponse<int>> RegisterAsync(RegisterRequestModel request)
        {
            if (request == null)
            {
                return new ApiResponse<int>((int)HttpStatusCode.BadRequest, "Request cannot be null", false);
            }

            var isExsistingUser = await _userManager.FindByEmailAsync(request.Email);

            if (isExsistingUser != null)
            {
                return new ApiResponse<int>((int)HttpStatusCode.BadRequest, "User already exists", false);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    Email = request.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = request.Email.ToUpper()
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new ApiResponse<int>((int)HttpStatusCode.BadRequest, "An error occurred while creating user", false);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    return new ApiResponse<int>((int)HttpStatusCode.BadRequest, "An error occurred while assigning role", false);
                }

                await transaction.CommitAsync();

                return new ApiResponse<int>()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "User created successfully",
                    IsSuccess = true,
                    Data = user.Id
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<int>((int)HttpStatusCode.InternalServerError, "An error occurred while adding product", false);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
