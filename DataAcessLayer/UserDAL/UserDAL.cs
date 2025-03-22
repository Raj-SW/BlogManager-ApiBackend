using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.User;
using FirebaseUser = Firebase.Auth.User;

namespace DataAcessLayer.UserDAL
{
    public class UserDAL : IUserDAL
    {
        private readonly FirestoreDb _db;
        private readonly ILogger<UserDAL> _logger;
        private readonly string _userCollectionName;
        private readonly string _credentialPath;
        private readonly string _projectId;

        public UserDAL(ILogger<UserDAL> logger, IConfiguration config)
        {
            _logger = logger;

            _credentialPath = config["GoogleCloud:CredentialPath"]
                              ?? throw new ArgumentNullException("GoogleCloud:CredentialPath not found in configuration.");

            _projectId = config["GoogleCloud:ProjectId"]
                         ?? throw new ArgumentNullException("GoogleCloud:ProjectId not found in configuration.");

            _userCollectionName = config["GoogleCloud:UserCollectionName"] ?? "Users";
            _db = FirestoreDb.Create(_projectId);
        }

        public async Task<IEnumerable<FirebaseUser>> GetAllUsersAsync()
        {
            try
            {
                var snapshot = await _db.Collection(_userCollectionName).GetSnapshotAsync();
                var users = new List<FirebaseUser>();

                foreach (var doc in snapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        var user = doc.ConvertTo<FirebaseUser>();
                        user.LocalId = doc.Id;
                        users.Add(user);
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                throw;
            }
        }

        public async Task<FirebaseUser?> GetUserByDocumentIdAsync(string documentId)
        {
            try
            {
                var docRef = _db.Collection(_userCollectionName).Document(documentId);
                var snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var user = snapshot.ConvertTo<FirebaseUser>();
                    user.LocalId = docRef.Id;
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with Document ID {documentId}.");
                throw;
            }
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                Query query = _db.Collection(_userCollectionName).WhereEqualTo("Email", email);
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                if (snapshot.Documents.Count > 0)
                {
                    DocumentSnapshot doc = snapshot.Documents[0];
                    User user = doc.ConvertTo<User>();

                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with email {email}.");
                throw;
            }
        }

        public async Task<FirebaseUser?> GetUserByUserIdAsync(string userId)
        {
            try
            {
                Query query = _db.Collection(_userCollectionName).WhereEqualTo("UserId", userId);
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Documents.Count > 0)
                {
                    var doc = snapshot.Documents[0];
                    var user = doc.ConvertTo<FirebaseUser>();
                    user.LocalId = doc.Id;
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with UserId {userId}.");
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                var collectionRef = _db.Collection(_userCollectionName);
                var docRef = await collectionRef.AddAsync(user);
                user.UserId = docRef.Id;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new user.");
                throw;
            }
        }

        public async Task UpdateUserAsync(string documentId, User user)
        {
            try
            {
                var docRef = _db.Collection(_userCollectionName).Document(documentId);
                user.UserId = docRef.Id;
                await docRef.SetAsync(user, SetOptions.Overwrite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with Document ID {documentId}.");
                throw;
            }
        }

        public async Task DeleteUserAsync(string documentId)
        {
            try
            {
                var docRef = _db.Collection(_userCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with Document ID {documentId}.");
                throw;
            }
        }
    }
}
