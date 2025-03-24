using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Model.BlogPost
{
    [FirestoreData]
    public class BlogPost
    {
        [FirestoreDocumentId]
        public string? BlogPostDocumentId { get; set; }

        [FirestoreProperty, Required]
        public string Title { get; set; }

        [FirestoreProperty, Required]
        public string Content { get; set; }

        [FirestoreProperty, Required]
        public string Excerpt { get; set; }

        [FirestoreProperty]
        public DateTime? CreatedDate { get; set; }

        [FirestoreProperty]
        public int Likes { get; set; } = 0;

        [FirestoreProperty]
        public bool IsFeatured { get; set; } = false;

        [FirestoreProperty]
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();

        [FirestoreProperty]
        public string CreatedBy { get; set; } = "";

        [FirestoreProperty]
        public string? ThumbNailLink { get; set; }

        public IFormFile File { get; set; }
    }
}