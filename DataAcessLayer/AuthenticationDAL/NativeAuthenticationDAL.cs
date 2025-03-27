using DataAcessLayer.Common;
using Microsoft.Data.SqlClient;
using Model.DTO.Authentication;
using Model.User;

namespace DataAcessLayer.AuthenticationDAL
{
    public class NativeAuthenticationDAL : IAuthenticationDAL
    {
        private readonly IDBCommand _dBCommand;

        public NativeAuthenticationDAL(IDBCommand dBCommand)
        {
            _dBCommand = dBCommand;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            const string RETRIEVE_USER_CREDENTIALS_BY_EMAIL_QUERY = @"SELECT TOP 1 * FROM Users WHERE Email = @Email";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", email.ToLower()));
            List<User> userList = await _dBCommand.GetDataWithConditionsAsync<User>(RETRIEVE_USER_CREDENTIALS_BY_EMAIL_QUERY, parameters);
            return userList.First<User>();
        }

        public Task<User> FindUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertUserModelAsync(NativeSignUpDto nativeSignUpDTO)
        {
            throw new NotImplementedException();
        }

        public Task<User> LoginByGoogleAsync(LoginDto nativeLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<User> NativeLoginAsync(LoginDto nativeLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<User> NativeRegisterAsync(NativeSignUpDto nativeSignUpDto)
        {
            throw new NotImplementedException();
        }
    }
}
