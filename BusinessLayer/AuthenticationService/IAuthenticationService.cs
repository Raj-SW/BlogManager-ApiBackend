using Model.Utils;
using User = Model.User.User;

namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthenticationService
    {
        Task<Result> NativeRegisterAsync(User userRegistrationDTO);
        Task<Result> NativeLoginAsync(string email, string password);
        Task<Result> LoginByGoogleAsync(string email, string password);
        Task LogoutAsync();
    }
}
