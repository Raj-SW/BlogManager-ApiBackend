using Model.BlogPost;

namespace BusinessLayer.BlogPostService
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        Task<BlogPost?> GetBlogPostByIdAsync(string id);
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
        Task UpdateBlogPostAsync(string documentId, BlogPost updatedPost);
        Task DeleteBlogPostAsync(string blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
    }
}
