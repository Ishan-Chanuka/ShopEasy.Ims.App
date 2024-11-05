using ShopEasy.Ims.Domain.Models.RequestModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;

namespace ShopEasy.Ims.Application.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request);
        Task<ApiResponse<int>> RegisterAsync(RegisterRequestModel request);
    }
}
