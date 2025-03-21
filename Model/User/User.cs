using Google.Cloud.Firestore;

namespace Model.User
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public required string DocumentId { get; set; }
        [FirestoreProperty]
        public required int UserId { get; set; }
        [FirestoreProperty]
        public required string UserName { get; set; }
        [FirestoreProperty]
        public required string FirstName { get; set; }
        [FirestoreProperty]
        public required string LastName { get; set; }
        [FirestoreProperty]
        public required IList<string> Roles { get; set; }
    }
}
