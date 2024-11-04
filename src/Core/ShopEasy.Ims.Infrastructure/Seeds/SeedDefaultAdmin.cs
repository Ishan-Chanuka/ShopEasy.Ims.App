using Microsoft.AspNetCore.Identity;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Primitives.Enum;

namespace ShopEasy.Ims.Infrastructure.Seeds
{
    public static class SeedDefaultAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@abc.net",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, UserRole.Admin.ToString());
            }
        }
    }
}
