using BusinessLayer.AuthenthicationService;
using DataAcessLayer.AuthenticationDAL;
using DataAcessLayer.UserDAL;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Model.DTO.Authentication;
using Model.Enum;
using Model.Utils;
using User = Model.User.User;

namespace BusinessLayer.AuthenticationService
{
    public class FirebaseAuthenticationService(IAuthenticationDAL authenticationDAL, IUserDAL userDAL, IConfiguration configuration) : IAuthService
    {
        private readonly IAuthenticationDAL _authenticationDAL = authenticationDAL;
        private readonly IConfiguration _config = configuration;
        private readonly IUserDAL _userDAL = userDAL;

        public async Task<Result> NativeRegisterAsync(NativeSignUpDto nativeSignUpDTO)
        {
            Result result = new();
            User user = new();

            try
            {
                User? isExistingUser = await GetUserByEmailAsync(nativeSignUpDTO.Email);

                if (isExistingUser == null)
                {
                    FirebaseAuthLink createdUserInfo = await _authenticationDAL.NativeRegisterAsync(nativeSignUpDTO);
                    user.UserId = createdUserInfo.User.LocalId;
                    user.Email = createdUserInfo.User.Email;
                    user.FirstName = nativeSignUpDTO.FirstName;
                    user.LastName = nativeSignUpDTO.LastName;
                    user.UserName = nativeSignUpDTO.UserName;
                    user.Role = RoleEnum.LoggedUser.ToString();

                    await _userDAL.CreateUserAsync(user);
                    result.IsSuccess = true;

                    return result;
                }

                if (isExistingUser.Email == nativeSignUpDTO.Email)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("Email already taken");
                }

                if (isExistingUser.UserName == nativeSignUpDTO.UserName)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User name already taken");
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

        public async Task<GenericResult<User>> NativeLoginAsync(LoginDto loginDto)
        {
            GenericResult<User> result = new();

            if (string.IsNullOrWhiteSpace(loginDto.Email))
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Invalid email");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Invalid password");
            }

            try
            {

                FirebaseAuthLink? successfulLoginInfo = await _authenticationDAL.NativeLoginAsync(loginDto);

                User? user = await GetUserByEmailAsync(loginDto.Email);
                var secretKey = _config["Jwt:SecretKey"];
                user.Token = JwtManager.GenerateToken(user.UserId, user.UserName, user.Role, secretKey);

                result.ResultObject = user;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<User?> GetUserByEmailAsync(string email) => await _userDAL.GetUserByEmailAsync(email);
    }
}
