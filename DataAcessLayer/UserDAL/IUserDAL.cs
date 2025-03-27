using Model.DTO.Authentication;
using Model.User;

namespace DataAcessLayer.UserDAL
{
    public interface IUserDAL
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserIdAsync(int userId);
        Task<User?> GetUserByUserName(string userName);
        Task InsertUserAsync(NativeSignUpDto user);
    }
}
