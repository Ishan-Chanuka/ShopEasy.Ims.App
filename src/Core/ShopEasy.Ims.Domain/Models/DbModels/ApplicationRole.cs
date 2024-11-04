using Microsoft.AspNetCore.Identity;

namespace ShopEasy.Ims.Domain.Models.DbModels
{
    public class ApplicationRole : IdentityRole<int>
    {
        public DateTime? CreatedDate { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
