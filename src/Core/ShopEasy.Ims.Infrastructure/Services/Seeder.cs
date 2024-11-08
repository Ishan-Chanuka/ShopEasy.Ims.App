using Microsoft.AspNetCore.Identity;
using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Infrastructure.Seeds;

namespace ShopEasy.Ims.Infrastructure.Services
{
    public class Seeder : ISeeder
    {
        #region Private Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        #endregion

        #region Constructor
        public Seeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        public async Task SeedAsync()
        {
            await SeedUserRole.SeedAsync(_roleManager);
            await SeedDefaultAdmin.SeedAsync(_userManager);
            await SeedDefaultEmployee.SeedAsync(_userManager);
        }
    }
}
