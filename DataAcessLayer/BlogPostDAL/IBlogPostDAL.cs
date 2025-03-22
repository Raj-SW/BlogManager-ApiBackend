using Model.BlogPost;

namespace DataAcessLayer.BlogPostDAL
{
    public interface IBlogPostDAL
    {
        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        Task<BlogPost?> GetBlogPostByIdAsync(string id);
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
        Task UpdateBlogPostAsync(string documentId, BlogPost updatedPost);
        Task DeleteBlogPostAsync(string blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
    }
}
