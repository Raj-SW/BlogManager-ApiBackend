using Google.Cloud.Firestore;

namespace Model.Models.Comment
{
    [FirestoreData]
    public class Comment
    {
        [FirestoreDocumentId]
        public required string DocumentId { get; set; }
        [FirestoreProperty]
        public required int CommentId { get; set; }
        [FirestoreProperty]
        public required string Content { get; set; }
        [FirestoreProperty]
        public required int Likes { get; set; }
        [FirestoreProperty]
        public required string PostedByUser { get; set; }
        [FirestoreProperty]
        public IEnumerable<Comment>? ReplyList { get; set; }
        [FirestoreProperty]
        public required DateTime CreatedAt { get; set; }
    }
}