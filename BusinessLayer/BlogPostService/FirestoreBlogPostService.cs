using DataAcessLayer.BlogPostDAL;
using Microsoft.Extensions.Logging;
using Model.BlogPost;
using Model.Utils;

namespace BusinessLayer.BlogPostService
{
    public class FirestoreBlogPostService : IBlogPostService
    {
        private readonly IBlogPostDAL _blogPostDAL;
        private readonly ILogger<FirestoreBlogPostService> _logger;

        public FirestoreBlogPostService(ILogger<FirestoreBlogPostService> logger, IBlogPostDAL blogPostDAL)
        {
            _blogPostDAL = blogPostDAL;
            _logger = logger;
        }

        public async Task<Result> GetAllBlogPostsAsync()
        {
            try
            {
                return await _blogPostDAL.GetAllBlogPostsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all blog posts.");
                throw;
            }
        }

        public async Task<Result> GetBlogPostByIdAsync(string id)
        {
            try
            {
                return await _blogPostDAL.GetBlogPostByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving blog post with ID {id}.");
                throw;
            }
        }

        public async Task<Result> CreateBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                return await _blogPostDAL.CreateBlogPostAsync(blogPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new blog post.");
                throw;
            }
        }

        public async Task UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        {
            try
            {
                await _blogPostDAL.UpdateBlogPostAsync(documentId, updatedPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating blog post with ID {documentId}.");
                throw;
            }
        }

        public async Task DeleteBlogPostAsync(string id)
        {
            try
            {
                await _blogPostDAL.DeleteBlogPostAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting blog post with ID {id}.");
                throw;
            }
        }

        public async Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog)
        {
            await _blogPostDAL.SuggestEditBlogPostAsync(suggestEditBlog);
        }

        public Task<Result> SearchBlogPostAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAllBlogPostsByTagsAsync(List<string> tags)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAllBlogPostsByAuthorAsync(string AuthorName)
        {
            throw new NotImplementedException();
        }

        Task<Result> IBlogPostService.UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        {
            throw new NotImplementedException();
        }
    }
}
