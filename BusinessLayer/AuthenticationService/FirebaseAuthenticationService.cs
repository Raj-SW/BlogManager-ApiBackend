using BusinessLayer.AuthenthicationService;
using DataAcessLayer.AuthenticationDAL;
using Firebase.Auth;
using User = Model.User.User;

namespace BusinessLayer.AuthenticationService
{
    public class FirebaseAuthenticationService(IAuthenticationDAL authenticationDAL) : IAuthenticationService
    {
        private readonly IAuthenticationDAL _authenticationDAL = authenticationDAL;

        public async Task<FirebaseAuthLink> NativeRegisterAsync(User userRegistrationDTO)
        {
            //TODO
            //Check if existing Email
            //Check if existing UserName
            //Else Create User
            //Create Doc For User Info in Firestore
            //Assign its Roles
            //return Sucess and Navigate to login 
            return await _authenticationDAL.NativeRegisterAsync(userRegistrationDTO);
        }

        public async Task<FirebaseAuthLink> NativeLoginAsync(string email, string password)
        {
            //Verify EMail and Password
            //Login User
            //Get Firebase Token
            //Get User Roles
            //Generate Jwt Token for User
            //Navigate to HomePage
            return await _authenticationDAL.NativeLoginAsync(email, password);
        }

        public Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password)
        {
            return _authenticationDAL.LoginByGoogleAsync(email, password);
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
