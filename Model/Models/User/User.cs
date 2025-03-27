namespace Model.User
{
    public class User
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public string? Token { get; set; }
    }
}
