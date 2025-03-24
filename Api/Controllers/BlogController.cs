using BusinessLayer.BlogPostService;
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

        public BlogController(IBlogPostService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// POST: api/Blog/CreateBlogPostAsync
        /// Creates a new blog post.
        /// </summary>
        [HttpPost("CreateBlogPostAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> CreateBlogPostAsync([FromBody] BlogPost newPost)
        {
            if (newPost == null)
                return BadRequest("Blog post data is required.");

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

            Result createdPost = await _blogService.CreateBlogPostAsync(newPost);

            return Ok(createdPost);
        }

        [HttpGet("GetAllBlogPostsAsync")]
        public async Task<IActionResult> GetAllBlogPostsAsync()
        {
            var posts = await _blogService.GetAllBlogPostsAsync();
            return Ok(posts);
        }

        [HttpGet("GetBlogPostByBlogPostIdAsync")]
        public async Task<IActionResult> GetBlogPostByBlogPostIdAsync(string id)
        {
            var post = await _blogService.GetBlogPostByIdAsync(id);
            if (post == null)
                return NotFound($"Blog post with ID {id} not found.");
            return Ok(post);
        }

        [HttpGet("GetAllBlogPostByUserNameAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> GetAllBlogPostByUserNameAsync()
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

            var result = await _blogService.GetAllBlogPostsByAuthorAsyncFromToken();

            return Ok(result);
        }

        [HttpPost("UpdatePostAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> UpdateBlogPostAsync(string id, [FromBody] BlogPost updatedPost)
        {
            if (updatedPost == null)
                return BadRequest("Blog post data is required.");

            var existingPost = await _blogService.GetBlogPostByIdAsync(id);
            if (existingPost == null)
                return NotFound($"Blog post with ID {id} not found.");

            await _blogService.UpdateBlogPostAsync(id, updatedPost);
            return NoContent();
        }

        [HttpPost("DeletePostAsync")]
        [Authorize(Policy = "LoggedUser")]
        public async Task<IActionResult> DeleteBlogPostAsync(string id)
        {
            var existingPost = await _blogService.GetBlogPostByIdAsync(id);
            if (existingPost == null)
                return NotFound($"Blog post with ID {id} not found.");

            await _blogService.DeleteBlogPostAsync(id);
            return NoContent();
        }
    }
}
