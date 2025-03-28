using Microsoft.AspNetCore.Http;
using Model.BlogPost;
using Model.Utils;

namespace BusinessLayer.BlogPostService
{
    public interface IBlogPostService
    {
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsAsync();
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags);
        Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByAuthorUserNameAsyncFromToken();
        Task<GenericResult<BlogPost>>? GetBlogPostByIdAsync(int id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost, IFormFile formFile);
        Task<Result> UpdateBlogPostAsync(BlogPost updatedPost);
        Task<Result> DeleteBlogPostAsync(int blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
        Task<GenericResult<List<BlogPost>>> SearchBlogAsync(string searchCriteria);
    }
}
