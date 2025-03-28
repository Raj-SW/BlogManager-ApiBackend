﻿using BusinessLayer.ImageUpload;
using DataAcessLayer.BlogPostDAL;
using Microsoft.AspNetCore.Http;
using Model.BlogPost;
using Model.Utils;
using Serilog;
using System.Security.Claims;

namespace BusinessLayer.BlogPostService
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostDAL _blogPostDAL;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileImageUpload _imageService;

        public BlogPostService(
            IBlogPostDAL blogPostDAL,
            IHttpContextAccessor httpContextAccessor, IFileImageUpload fileImageUpload)
        {
            _blogPostDAL = blogPostDAL;
            _httpContextAccessor = httpContextAccessor;
            _imageService = fileImageUpload;
        }

        public async Task<Result> CreateBlogPostAsync(BlogPost blogPost, IFormFile formFile)
        {
            Result result = new();
            try
            {
                ClaimsPrincipal? userClaims = _httpContextAccessor.HttpContext?.User;
                string? userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User not authenticated");
                    Log.Error("Attempt to create blog with no userName in token");
                    return result;
                }

                blogPost.UserId = int.Parse(userIdClaim);
                blogPost.CreatedDate = DateTime.UtcNow;
                blogPost.ThumbnailLink = await UploadBlogThumbnailImage(formFile);
                await _blogPostDAL.CreateBlogPostAsync(blogPost);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to create a blog", ex.Message, ex);
                result.IsSuccess = false;
                result.ErrorMessage.Add("Server error! Failed to create a blog");
                return result;
            }
        }

        public async Task<GenericResult<List<BlogPost>>> GetAllBlogPostsAsync()
        {
            try
            {
                return await _blogPostDAL.GetAllBlogPostsAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error retrieving all blog posts.", ex.Message, ex);
                throw;
            }
        }

        public async Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByAuthorUserNameAsyncFromToken()
        {
            GenericResult<List<BlogPost>> result = new();
            try
            {
                ClaimsPrincipal userClaims = _httpContextAccessor.HttpContext?.User;
                string userName = userClaims?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    result.IsSuccess = false;
                    result.ErrorMessage.Add("User not authenticated");
                    Log.Error("Attempt to get self with no userName in token");
                    return result;
                }

                result = await _blogPostDAL.GetAllBlogPostsByAuthorUserNameAsync(userName);
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                Log.Error("Error fetching all self blogs.", ex.Message, ex);
                return result;
            }
        }

        //public async Task UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        //{
        //    GenericResult<BlogPost>? result = new();

        //    try
        //    {
        //        await _blogPostDAL.UpdateBlogPostAsync(documentId, updatedPost);
        //    }
        //    catch (Exception ex)
        //    {
        //        result!.IsSuccess = false;
        //        result.ErrorMessage.Add($"Error updating blog post with Id {documentId}.");
        //        Log.Error($"Error updating blog post with Id {documentId}.", ex.Message, ex);
        //        throw;
        //    }
        //}

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
                throw new NotImplementedException("Error in uploading image");
            }
        }

        public Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog)
        {
            throw new NotImplementedException();
        }
        public Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByAuthorNameAsync(string AuthorName)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResult<BlogPost>> GetBlogPostByIdAsync(int id)
        {
            GenericResult<BlogPost>? result = new();

            try
            {
                result = await _blogPostDAL.GetBlogPostByIdAsync(id);
                return result!;
            }
            catch (Exception ex)
            {
                result!.IsSuccess = false;
                result.ErrorMessage.Add($"Error retrieving blog post with ID {id}.");
                Log.Error($"Error retrieving blog post with Id {id}.", ex.Message, ex);
                throw;
            }
        }

        public Task<Result> UpdateBlogPostAsync(BlogPost updatedPost)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteBlogPostAsync(int blogPostId)
        {
            Result result = new();

            try
            {
                await _blogPostDAL.DeleteBlogPostAsync(blogPostId);
                result!.IsSuccess = true;

                return result!;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Unexpected server error occured when deleting post");
                Log.Error("Unexpected server error occured when deleting post", ex.Message, ex);

                return result;
            }
        }

        public async Task<GenericResult<List<BlogPost>>> SearchBlogAsync(string searchCriteria)
        {

            return await _blogPostDAL.SearchBlogAsync(searchCriteria);
        }
    }
}
