using Firebase.Auth;

namespace DataAcessLayer.AuthenticationDAL
{
    public interface IAuthenticationDAL
    {
        Task<FirebaseAuthLink> NativeRegisterAsync(string email, string password);
        Task<FirebaseAuthLink> NativeLoginAsync(string email, string password);
        Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password);
        Task LogoutAsync();
    }
}
