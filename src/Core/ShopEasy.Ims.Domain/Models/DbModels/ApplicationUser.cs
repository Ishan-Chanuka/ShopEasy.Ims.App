using Microsoft.AspNetCore.Identity;

namespace ShopEasy.Ims.Domain.Models.DbModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
