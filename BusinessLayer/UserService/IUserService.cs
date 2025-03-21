using Model.User;

namespace BusinessLayer.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByDocumentIdAsync(string documentId);
        Task<User?> GetUserByUserIdAsync(string userId);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(string documentId, User user);
        Task DeleteUserAsync(string documentId);
    }
}
