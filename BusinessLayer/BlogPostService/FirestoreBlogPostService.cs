using BusinessLayer.ImageUpload;
using DataAcessLayer.BlogPostDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Model.BlogPost;
using Model.Utils;
using System.Security.Claims;

namespace BusinessLayer.BlogPostService
{
    public class FirestoreBlogPostService : IBlogPostService
    {
        private readonly IBlogPostDAL _blogPostDAL;
        private readonly ILogger<FirestoreBlogPostService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileImageUpload _imageService;

        public FirestoreBlogPostService(
            ILogger<FirestoreBlogPostService> logger,
            IBlogPostDAL blogPostDAL,
            IHttpContextAccessor httpContextAccessor, IFileImageUpload fileImageUpload)
        {
            _blogPostDAL = blogPostDAL;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _imageService = fileImageUpload;
        }

        public async Task<Result> CreateBlogPostAsync(BlogPost blogPost, IFormFile formFile)
        {
            Result result = new();
            try
            {
                ClaimsPrincipal? userClaims = _httpContextAccessor.HttpContext?.User;
                string? userName = userClaims?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User not authenticated");
                    return result;
                }

                blogPost.CreatedBy = userName;
                blogPost.CreatedDate = DateTime.UtcNow;
                blogPost.ThumbNailLink = await UploadBlogThumbnailImage(formFile);
                result = await _blogPostDAL.CreateBlogPostAsync(blogPost);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
            }
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

        public async Task<Result> GetAllBlogPostsByAuthorAsyncFromToken()
        {
            Result result = new();
            try
            {
                ClaimsPrincipal userClaims = _httpContextAccessor.HttpContext?.User;
                string userName = userClaims?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User not authenticated");
                    return result;
                }

                result = await _blogPostDAL.GetAllBlogPostsByAuthorAsync(userName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching blog post.");
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
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

        public Task<Result> GetAllBlogPostsByAuthorNameAsync(string AuthorName)
        {
            throw new NotImplementedException();
        }

        Task<Result> IBlogPostService.UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadBlogThumbnailImage(IFormFile formFile)
        {
            try
            {
                if (formFile != null)
                {
                    return await _imageService.UploadImageAsync(formFile);
                }
                throw new NotImplementedException("Image is not uploaded");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
