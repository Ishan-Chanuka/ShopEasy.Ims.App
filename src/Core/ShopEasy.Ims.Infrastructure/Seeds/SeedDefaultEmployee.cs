using Microsoft.AspNetCore.Identity;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Primitives.Enum;

namespace ShopEasy.Ims.Infrastructure.Seeds
{
    public static class SeedDefaultEmployee
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "Employee",
                Email = "employee@abc.net",
                FirstName = "Employee",
                LastName = "Employee",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "Employee@123");
                await userManager.AddToRoleAsync(defaultUser, UserRole.Employee.ToString());
            }
        }
    }
}
