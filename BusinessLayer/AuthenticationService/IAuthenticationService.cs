using Firebase.Auth;
using User = Model.User.User;

namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthenticationService
    {
        Task<FirebaseAuthLink> NativeRegisterAsync(User userRegistrationDTO);
        Task<FirebaseAuthLink> NativeLoginAsync(string email, string password);
        Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password);
        Task LogoutAsync();
    }
}
