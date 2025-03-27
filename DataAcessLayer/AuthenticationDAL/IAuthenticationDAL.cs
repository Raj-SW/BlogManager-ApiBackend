using Model.DTO.Authentication;
using Model.User;

namespace DataAcessLayer.AuthenticationDAL
{
    public interface IAuthenticationDAL
    {
        Task<User> NativeRegisterAsync(NativeSignUpDto nativeSignUpDto);
        Task<User> NativeLoginAsync(LoginDto nativeLoginDto);
        Task<User> LoginByGoogleAsync(LoginDto nativeLoginDto);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByUserNameAsync(string userName);
        Task<bool> InsertUserModelAsync(NativeSignUpDto nativeSignUpDTO);
    }
}
