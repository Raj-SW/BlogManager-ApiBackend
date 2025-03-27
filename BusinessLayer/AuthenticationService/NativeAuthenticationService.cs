using BusinessLayer.AuthenthicationService;
using BusinessLayer.Utils;
using DataAcessLayer.AuthenticationDAL;
using DataAcessLayer.UserDAL;
using Microsoft.Extensions.Configuration;
using Model.DTO.Authentication;
using Model.User;
using Model.Utils;
using Serilog;

namespace BusinessLayer.AuthenticationService
{
    public class NativeAuthenticationService : IAuthService
    {
        private readonly IAuthenticationDAL _nativeAuthDAL;
        private readonly IUserDAL _userDAL;
        private readonly IConfiguration _config;

        public NativeAuthenticationService(IAuthenticationDAL nativeAuthDAL, IUserDAL userDAL, IConfiguration configuration)
        {
            _nativeAuthDAL = nativeAuthDAL;
            _userDAL = userDAL;
            _config = configuration;
        }

        public async Task<GenericResult<User>> NativeLoginAsync(LoginDto loginDto)
        {
            GenericResult<User> result = new();

            try
            {
                User? user = await FindUserByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User not found");
                    return result;
                }

                var hash = PasswordHashing.HashPassword(loginDto.Password);

                if (!hash.SequenceEqual(user.PasswordHash!))
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("Incorrect password");
                    return result;
                }

                string? secretKey = _config["Jwt:SecretKey"];
                string? authToken = JwtManager.GenerateToken(user.UserId.ToString()!, user.UserName!, user.Role!, secretKey!);

                user.Token = authToken;
                user.PasswordHash = null;
                result.ResultObject = user;

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Server error! Failed to authenticate..");
                Log.Error("Error authentication User", ex);
                return result;
            }
        }

        public async Task<Result> NativeRegisterAsync(NativeSignUpDto signUpDTO)
        {
            Result result = new();
            try
            {

                User? user = await FindUserByEmailAsync(signUpDTO.Email);

                if (user != null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("Email already taken");
                    return result;
                }

                user = await FindUserByUserNameAsync(signUpDTO.UserName);

                if (user != null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User name already taken");
                    return result;
                }

                signUpDTO.PasswordHash = PasswordHashing.HashPassword(signUpDTO.Password);
                signUpDTO.Role = "LoggedUser";

                await _userDAL.InsertUserAsync(signUpDTO);

                return result;
            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Server Error! Could not Register User");
                Log.Error("Server Error! Could not Register User", ex.Message, ex);
                return result;
            }
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _userDAL.GetUserByEmailAsync(email);
        }

        public async Task<User?> FindUserByUserNameAsync(string userName)
        {
            return await _userDAL.GetUserByUserName(userName);
        }
    }
}
