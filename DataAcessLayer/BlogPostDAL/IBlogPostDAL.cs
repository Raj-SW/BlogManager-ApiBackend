using Model.BlogPost;
using Model.Utils;

namespace DataAcessLayer.BlogPostDAL
{
    public interface IBlogPostDAL
    {
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsAsync();
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByAuthorUserNameAsync(string AuthorEmail);
        Task<GenericResult<BlogPost>?> GetBlogPostByIdAsync(int id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost);
        Task DeleteBlogPostAsync(int blogPost);
        Task<GenericResult<List<BlogPost>>> SearchBlogAsync(string searchCriteria);
    }
}
