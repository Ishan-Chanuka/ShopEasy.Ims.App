using System.ComponentModel.DataAnnotations;

namespace ShopEasy.Ims.Domain.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Role { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }
}
