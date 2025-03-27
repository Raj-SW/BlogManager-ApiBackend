using Model.BlogPost;
using Model.Utils;

namespace DataAcessLayer.BlogPostDAL
{
    public interface IBlogPostDAL
    {
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsAsync();
        //Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags);
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByAuthorUserNameAsync(string AuthorEmail);
        Task<GenericResult<BlogPost>?> GetBlogPostByIdAsync(int id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost);
        //Task<Result> UpdateBlogPostAsync(string documentId, BlogPost updatedPost);
        //Task DeleteBlogPostByIdAsync(string blogPostId);
        //Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
        //Task<GenericResult<List<BlogPost>>> SearchBlogAsync(string searchCriteria);
    }
}
