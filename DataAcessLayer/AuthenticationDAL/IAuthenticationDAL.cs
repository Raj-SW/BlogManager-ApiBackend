using Firebase.Auth;
using Model.DTO.Authentication;

namespace DataAcessLayer.AuthenticationDAL
{
    public interface IAuthenticationDAL
    {
        Task<FirebaseAuthLink> NativeRegisterAsync(NativeSignUpDto nativeSignUpDto);
        Task<FirebaseAuthLink> NativeLoginAsync(LoginDto nativeLoginDto);
        Task<FirebaseAuthLink> LoginByGoogleAsync(LoginDto nativeLoginDto);
        Task<Firebase.Auth.User> FindUserByEmail(string email);
    }
}
