using Microsoft.AspNetCore.Http;

namespace Model.BlogPost
{
    public class BlogPost
    {
        public int? BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Excerpt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsFeatured { get; set; } = false;

        public int UserId { get; set; }

        public string? ThumbnailLink { get; set; }

        public IFormFile File { get; set; }
    }
}