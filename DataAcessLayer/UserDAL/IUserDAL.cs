using Model.User;
using FirebaseUser = Firebase.Auth.User;
namespace DataAcessLayer.UserDAL
{
    public interface IUserDAL
    {
        Task<IEnumerable<FirebaseUser>> GetAllUsersAsync();
        Task<FirebaseUser?> GetUserByDocumentIdAsync(string documentId);
        Task<User?> GetUserByEmailAsync(string documentId);
        Task<FirebaseUser?> GetUserByUserIdAsync(string userId);
        Task<bool> CreateUserAsync(User user);
        Task UpdateUserAsync(string documentId, User user);
        Task DeleteUserAsync(string documentId);
    }
}
