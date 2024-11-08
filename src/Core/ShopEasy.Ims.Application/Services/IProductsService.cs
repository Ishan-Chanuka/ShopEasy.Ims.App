using ShopEasy.Ims.Domain.Models.RequestModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;

namespace ShopEasy.Ims.Application.Services
{
    public interface IProductsService
    {
        Task<ApiResponse<int>> AddProductAsync(ProductRequestModel request);
        Task<ApiResponse<bool>> DeleteProductAsync(int id, int userId);
        Task<ApiResponse<bool>> UpdateProductAsync(int productId, ProductRequestModel request);
        Task<ApiResponse<IEnumerable<ProductResponseModel>>> GetProductAsync();
        Task<ApiResponse<ProductResponseModel>> GetProductByIdAsync(int id);
    }
}
