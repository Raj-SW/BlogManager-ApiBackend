using BusinessLayer.BlogPostService;
using BusinessLayer.ImageUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.BlogPost;
using Model.Utils;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogPostService _blogService;
        private readonly IFileImageUpload _imageService;

        public BlogController(IBlogPostService blogService, IFileImageUpload fileImageUpload)
        {
            _blogService = blogService;
            _imageService = fileImageUpload;
        }

        [HttpPost("CreateBlogPostAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> CreateBlogPostAsync([FromForm] BlogPost blogPost)
        {


            if (blogPost == null)
                return BadRequest("Blog post data is required.");

            if (blogPost.File == null)
                return BadRequest("Blog post thumbail is required.");

            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("No valid token provided.");

            string token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return Unauthorized("No valid token provided.");


            Result createdPost = await _blogService.CreateBlogPostAsync(blogPost, blogPost.File);
            return Ok(createdPost);
        }

        [HttpGet("GetAllBlogPostsAsync")]
        public async Task<IActionResult> GetAllBlogPostsAsync()
        {
            var posts = await _blogService.GetAllBlogPostsAsync();
            return Ok(posts);
        }

        [HttpGet("GetBlogPostByBlogPostIdAsync/{id}")]
        public async Task<IActionResult> GetBlogPostByBlogPostIdAsync(int id)
        {
            var post = await _blogService.GetBlogPostByIdAsync(id);
            if (post == null)
                return NotFound($"Blog post with ID {id} not found.");
            return Ok(post);
        }

        [HttpGet("GetSelfBlogsAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> GetSelfBlogsAsync()
        {

            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("No valid token provided.");
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No valid token provided.");
            }

            var result = await _blogService.GetAllBlogPostsByAuthorUserNameAsyncFromToken();

            return Ok(result);
        }

        [HttpPost("UpdatePostAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> UpdateBlogPostAsync([FromBody] BlogPost updatedPost)
        {

            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("No valid token provided.");

            string token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return Unauthorized("No valid token provided.");

            if (updatedPost == null)
                return BadRequest("Blog post data is required.");

            Result result = await _blogService.UpdateBlogPostAsync(updatedPost);
            return Ok(result);
        }

        [HttpPost("DeletePostAsync/{id}")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> DeleteBlogPostAsync(string id)
        {
            Result result = await _blogService.DeleteBlogPostAsync(id);
            return Ok(result);
        }

        [HttpGet("SearchBlogAsync")]
        public async Task<IActionResult> SearchBlogAsync(string searchCriteria)
        {
            GenericResult<IEnumerable<BlogPost>> result = await _blogService.SearchBlogAsync(searchCriteria);
            return Ok(result);
        }
    }
}
