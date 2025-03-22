using Model.BlogPost;
using Model.Utils;

namespace DataAcessLayer.BlogPostDAL
{
    public interface IBlogPostDAL
    {
        Task<Result> SearchBlogPostAsync(string search);
        Task<Result> GetAllBlogPostsAsync();
        Task<Result> GetAllBlogPostsByTagsAsync(List<string> tags);
        Task<Result> GetAllBlogPostsByAuthorAsync(string AuthorName);
        Task<Result> GetBlogPostByIdAsync(string id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost);
        Task<Result> UpdateBlogPostAsync(string documentId, BlogPost updatedPost);
        Task DeleteBlogPostAsync(string blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
    }
}
