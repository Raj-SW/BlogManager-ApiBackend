using Google.Cloud.Firestore;

namespace Model.User
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public string? DocumentId { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public required string UserName { get; set; }
        [FirestoreProperty]
        public required string FirstName { get; set; }
        [FirestoreProperty]
        public required string LastName { get; set; }
        [FirestoreProperty]
        public string? Role { get; set; }
        [FirestoreProperty]
        public required string Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
