namespace ShopEasy.Ims.Domain.Models.RequestModels
{
    public class LoginRequestModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
