using Microsoft.AspNetCore.Identity;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Primitives.Enum;

namespace ShopEasy.Ims.Infrastructure.Seeds
{
    public static class SeedUserRole
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                var roleName = role.ToString()!;
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        CreatedDate = DateTime.Now,
                    });
                }
            }
        }
    }
}
