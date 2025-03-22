using BusinessLayer.AuthenthicationService;
using DataAcessLayer.AuthenticationDAL;
using Firebase.Auth;

namespace BusinessLayer.AuthenticationService
{
    public class FirebaseAuthenticationService(IAuthenticationDAL authenticationDAL) : IAuthenticationService
    {
        private readonly IAuthenticationDAL _authenticationDAL = authenticationDAL;

        public Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password)
        {
            return _authenticationDAL.LoginByGoogleAsync(email, password);
        }

        public async Task<FirebaseAuthLink> NativeLoginAsync(string email, string password)
        {
            return await _authenticationDAL.NativeLoginAsync(email, password);
        }

        public async Task<FirebaseAuthLink> NativeRegisterAsync(string email, string password)
        {
            return await _authenticationDAL.NativeRegisterAsync(email, password);
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
