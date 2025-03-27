using DataAcessLayer.Common;
using Microsoft.Data.SqlClient;
using Model.DTO.Authentication;
using Model.User;
using Serilog;

namespace DataAcessLayer.UserDAL
{
    public class UserDAL : IUserDAL
    {
        private readonly IDBCommand _dBCommand;
        public UserDAL(IDBCommand dBCommand)
        {
            _dBCommand = dBCommand;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                const string GET_All_USERS = @"SELECT TOP 1 * FROM [UserInfo]";
                List<User> userList = await _dBCommand.GetDataAsync<User>(GET_All_USERS);

                return userList;
            }
            catch (Exception ex)
            {
                Log.Error("Error retrieving all users.", ex);
                throw ex;
            }
        }

        public async Task<User?> GetUserByUserIdAsync(int userId)
        {
            try
            {
                const string GET_USER_BY_ID = @"SELECT TOP 1 * FROM [UserInfo] WHERE UserId = @UserId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserId", userId));
                List<User> user = await _dBCommand.GetDataWithConditionsAsync<User>(GET_USER_BY_ID, parameters);

                return user.First();
            }
            catch (Exception ex)
            {
                Log.Error($"Error user with id: {userId}", ex);
                throw ex;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                const string GET_USER_BY_EMAIL = @"SELECT TOP 1 * FROM [UserInfo] WHERE Email = @Email";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Email", email.ToLower()));
                List<User> user = await _dBCommand.GetDataWithConditionsAsync<User>(GET_USER_BY_EMAIL, parameters);

                if (user.Count == 0)
                {
                    return null;
                }
                return user.First();
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching user with email: {email}", ex);
                throw ex;
            }
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            try
            {
                const string GET_USER_BY_USERNAME = @"SELECT TOP 1 * FROM [UserInfo] WHERE UserName = @UserName";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserName", userName.ToLower()));
                List<User> user = await _dBCommand.GetDataWithConditionsAsync<User>(GET_USER_BY_USERNAME, parameters);

                if (user.Count == 0)
                {
                    return null;
                }

                return user.First();
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching user with UserName: {userName}", ex);
                throw ex;
            }
        }

        public async Task InsertUserAsync(NativeSignUpDto user)
        {
            try
            {
                const string INSERT_USER_QUERY = @"INSERT INTO [UserInfo] (UserName, FirstName, LastName, Role, Email, PasswordHash) VALUES (@UserName, @FirstName, @LastName, @Role, @Email, @PasswordHash)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserName", user.UserName));
                parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                parameters.Add(new SqlParameter("@LastName", user.LastName));
                parameters.Add(new SqlParameter("@Email", user.Email));
                parameters.Add(new SqlParameter("@PasswordHash", user.PasswordHash));
                parameters.Add(new SqlParameter("@Role", user.Role));

                await _dBCommand.InsertUpdateDataAsync(INSERT_USER_QUERY, parameters);
            }
            catch (Exception ex)
            {
                Log.Error("Error inserting new user.", ex);
                throw;
            }
        }
    }
}
