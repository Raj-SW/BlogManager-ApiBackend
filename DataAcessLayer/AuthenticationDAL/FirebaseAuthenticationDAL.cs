using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Model.DTO.Authentication;

namespace DataAcessLayer.AuthenticationDAL
{
    public class FirebaseAuthenticationDAL : IAuthenticationDAL
    {
        private readonly IFirebaseAuthProvider _firebaseAuthProvider;
        private readonly string? _firebaseAuthKey;

        public FirebaseAuthenticationDAL(IConfiguration config)
        {
            _firebaseAuthKey = config["Firebase:ApiKey"];

            if (string.IsNullOrWhiteSpace(_firebaseAuthKey))
            {
                throw new ArgumentNullException(nameof(_firebaseAuthKey), "Firebase API key is missing in configuration.");
            }

            _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(_firebaseAuthKey));
        }
        public async Task<FirebaseAuthLink> NativeRegisterAsync(NativeSignUpDto nativeSignUpDto)
        {
            FirebaseAuthLink result = await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(nativeSignUpDto.Email, nativeSignUpDto.Password, nativeSignUpDto.UserName);
            return result;
        }

        public async Task<FirebaseAuthLink> NativeLoginAsync(LoginDto loginDto)
        {
            FirebaseAuthLink result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(loginDto.Email, loginDto.Password);
            return result;
        }

        public Task<FirebaseAuthLink> LoginByGoogleAsync(LoginDto nativeLoginDto)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await _firebaseAuthProvider.GetUserAsync(email);
        }
    }
}
