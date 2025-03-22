using Firebase.Auth;
using Microsoft.Extensions.Configuration;

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
        public Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<FirebaseAuthLink> NativeLoginAsync(string email, string password)
        {
            FirebaseAuthLink result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
            return result;
        }

        public async Task<FirebaseAuthLink> NativeRegisterAsync(string email, string password)
        {
            FirebaseAuthLink result = await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            return result;
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
