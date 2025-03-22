using BusinessLayer.AuthenthicationService;
using DataAcessLayer.AuthenticationDAL;
using DataAcessLayer.UserDAL;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Model.Enum;
using Model.Utils;
using User = Model.User.User;

namespace BusinessLayer.AuthenticationService
{
    public class FirebaseAuthenticationService(IAuthenticationDAL authenticationDAL, IUserDAL userDAL, IConfiguration configuration) : IAuthenticationService
    {
        private readonly IAuthenticationDAL _authenticationDAL = authenticationDAL;
        private readonly IConfiguration _config = configuration;
        private readonly IUserDAL _userDAL = userDAL;
        public async Task<Result> NativeRegisterAsync(User userRegistrationDTO)
        {
            Result result = new();

            try
            {
                User? isExistingUser = await GetUserByEmailAsync(userRegistrationDTO.Email);

                if (isExistingUser == null)
                {
                    FirebaseAuthLink createdUserInfo = await _authenticationDAL.NativeRegisterAsync(userRegistrationDTO);
                    userRegistrationDTO.UserId = createdUserInfo.User.LocalId;
                    userRegistrationDTO.Email = createdUserInfo.User.Email;
                    userRegistrationDTO.Role = RoleEnum.LoggedUser.ToString();

                    await _userDAL.CreateUserAsync(userRegistrationDTO);
                    result.IsSuccess = true;

                    return result;
                }

                if (isExistingUser.Email == userRegistrationDTO.Email)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("Email already taken");
                }

                if (isExistingUser.UserName == userRegistrationDTO.UserName)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User namer already taken");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
            }

            return result;
        }

        public async Task<Result> NativeLoginAsync(string email, string password)
        {
            Result result = new Result();

            if (string.IsNullOrWhiteSpace(email))
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Invalid email");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Invalid password");
            }

            try
            {

                FirebaseAuthLink? successfulLoginInfo = await _authenticationDAL.NativeLoginAsync(email, password);

                User? user = await GetUserByEmailAsync(email);
                var secretKey = _config["Jwt:SecretKey"];
                user.Token = JwtManager.GenerateToken(user.UserId, user.Role, secretKey);

                result.ResultObject = user;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
            }

            return result;
        }

        public Task<Result> LoginByGoogleAsync(string email, string password)
        {
            Result result = new Result();
            _authenticationDAL.LoginByGoogleAsync(email, password);
            return Task.FromResult(result);
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<User?> GetUserByEmailAsync(string email) => await _userDAL.GetUserByEmailAsync(email);
    }
}
