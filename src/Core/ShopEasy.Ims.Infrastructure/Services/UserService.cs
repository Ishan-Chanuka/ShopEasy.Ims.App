using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;
using ShopEasy.Ims.Infrastructure.Context;

namespace ShopEasy.Ims.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        #region Private Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public Task<ApiResponse<bool>> LoginAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> RegisterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
