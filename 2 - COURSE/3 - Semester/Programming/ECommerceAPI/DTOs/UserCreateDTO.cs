namespace ECommerceAPI.DTOs
{
    public class UserCreateDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
