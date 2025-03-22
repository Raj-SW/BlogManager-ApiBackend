using Google.Cloud.Firestore;

namespace Model.User
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public string? DocumentId { get; set; }
        [FirestoreProperty]
        public int? UserId { get; set; }
        [FirestoreProperty]
        public required string UserName { get; set; }
        [FirestoreProperty]
        public required string FirstName { get; set; }
        [FirestoreProperty]
        public required string LastName { get; set; }
        [FirestoreProperty]
        public IList<string>? Roles { get; set; }
        [FirestoreProperty]
        public required string Email { get; set; }
        public string? Password { get; set; }
    }
}
