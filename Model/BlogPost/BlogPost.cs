using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Model.BlogPost
{
    [FirestoreData]
    public class BlogPost
    {
        [FirestoreDocumentId]
        public string? BlogPostDocumentId { get; set; }
        [FirestoreProperty]
        [Required]
        public required string Title { get; set; }
        [FirestoreProperty]
        [Required]
        public required string Content { get; set; }
        [FirestoreProperty]
        public DateTime? CreatedDate { get; set; }
        [FirestoreProperty]
        public int Likes { get; set; } = 0;
        [FirestoreProperty]
        public bool IsFeatured { get; set; } = false;
        [FirestoreProperty]
        public IEnumerable<string> Tags { get; set; } = [];
        [FirestoreProperty]
        public required string CreatedBy { get; set; }
    }
}
