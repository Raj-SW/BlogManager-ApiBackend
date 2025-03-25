using Model.BlogPost;
using Model.Utils;

namespace DataAcessLayer.BlogPostDAL
{
    public interface IBlogPostDAL
    {
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsAsync();
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags);
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByAuthorAsync(string AuthorName);
        Task<GenericResult<BlogPost>> GetBlogPostByIdAsync(string id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost);
        Task<Result> UpdateBlogPostAsync(string documentId, BlogPost updatedPost);
        Task DeleteBlogPostAsync(string blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
        Task<GenericResult<IEnumerable<BlogPost>>> SearchBlogAsync(string searchCriteria);
    }
}
