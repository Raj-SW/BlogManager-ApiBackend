using Model.User;

namespace BusinessLayer.UserService
{
    public class UserService : IUserService
    {
        public Task<User> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByDocumentIdAsync(string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(string documentId, User user)
        {
            throw new NotImplementedException();
        }
    }
}
