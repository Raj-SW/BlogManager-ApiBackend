﻿using Microsoft.AspNetCore.Http;
using Model.BlogPost;
using Model.Utils;

namespace BusinessLayer.BlogPostService
{
    public interface IBlogPostService
    {
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsAsync();
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags);
        Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByAuthorAsyncFromToken();
        Task<GenericResult<BlogPost>> GetBlogPostByIdAsync(string id);
        Task<Result> CreateBlogPostAsync(BlogPost blogPost, IFormFile formFile);
        Task<Result> UpdateBlogPostAsync(BlogPost updatedPost);
        Task<Result> DeleteBlogPostAsync(string blogPostId);
        Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog);
        Task<GenericResult<IEnumerable<BlogPost>>> SearchBlogAsync(string searchCriteria);
    }
}
