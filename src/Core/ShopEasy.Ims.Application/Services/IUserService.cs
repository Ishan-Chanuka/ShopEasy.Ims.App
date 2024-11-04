using ShopEasy.Ims.Domain.Primitives.ApiResponse;

namespace ShopEasy.Ims.Application.Services
{
    public interface IUserService
    {
        Task<ApiResponse<bool>> LoginAsync();
        Task<ApiResponse<bool>> RegisterAsync();
    }
}
