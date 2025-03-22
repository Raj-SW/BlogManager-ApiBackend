using Firebase.Auth;
using User = Model.User.User;

namespace DataAcessLayer.AuthenticationDAL
{
    public interface IAuthenticationDAL
    {
        Task<FirebaseAuthLink> NativeRegisterAsync(User userRegistrationDTO);
        Task<FirebaseAuthLink> NativeLoginAsync(string email, string password);
        Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password);
        Task LogoutAsync();
        Task<Firebase.Auth.User> FindUserByEmail(string email);
    }
}
