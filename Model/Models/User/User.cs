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
        public string? UserName { get; set; }

        [FirestoreProperty]
        public string? FirstName { get; set; }

        [FirestoreProperty]
        public string? LastName { get; set; }

        [FirestoreProperty]
        public string? Role { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
