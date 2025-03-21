using Google.Cloud.Firestore;

namespace Model.BlogPost
{
    [FirestoreData]
    public class BlogPost
    {
        [FirestoreDocumentId]
        public string? BlogPostDocumentId { get; set; }
        [FirestoreProperty]
        public required string Title { get; set; }
        [FirestoreProperty]
        public required string Content { get; set; }
        [FirestoreProperty]
        public DateTime? CreatedDate { get; set; }
        [FirestoreProperty]
        public required int Likes { get; set; }
        [FirestoreProperty]
        public required bool IsFeatured { get; set; }
        [FirestoreProperty]
        public required IEnumerable<string> Tags { get; set; }
        [FirestoreProperty]
        public required string CreatedBy { get; set; }
    }
}
