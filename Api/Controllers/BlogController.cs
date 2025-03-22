using BusinessLayer.BlogPostService;
using Microsoft.AspNetCore.Mvc;
using Model.BlogPost;

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
        /// POST: api/Blog
        /// Creates a new blog post.
        /// </summary>
        [HttpPost("CreateBlogPostAsync")]
        public async Task<IActionResult> CreateBlogPostAsync(BlogPost newPost)
        {
            if (newPost == null)
                return BadRequest("Blog post data is required.");

            var createdPost = await _blogService.CreateBlogPostAsync(newPost);
            return Ok(createdPost);
        }

        /// <summary>
        /// GET: api/Blog
        /// Retrieves all blog posts.
        /// </summary>
        [HttpGet("GetAllBlogPostsAsync")]
        public async Task<IActionResult> GetAllBlogPostsAsync()
        {
            var posts = await _blogService.GetAllBlogPostsAsync();
            return Ok(posts);
        }

        /// <summary>
        /// GET: api/Blog/{id}
        /// Retrieves a blog post by its ID.
        /// </summary>
        [HttpGet("GetBlogPostByIdAsync")]
        public async Task<IActionResult> GetBlogPostByIdAsync(string id)
        {
            var post = await _blogService.GetBlogPostByIdAsync(id);
            if (post == null)
                return NotFound($"Blog post with ID {id} not found.");
            return Ok(post);
        }

        /// <summary>
        /// PUT: api/Blog/{id}
        /// Updates an existing blog post.
        /// </summary>
        [HttpPost("UpdatePostAsync")]
        public async Task<IActionResult> UpdatePostAsync(string id, [FromBody] BlogPost updatedPost)
        {
            if (updatedPost == null)
                return BadRequest("Blog post data is required.");

            var existingPost = await _blogService.GetBlogPostByIdAsync(id);
            if (existingPost == null)
                return NotFound($"Blog post with ID {id} not found.");

            await _blogService.UpdateBlogPostAsync(id, updatedPost);
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/Blog/{id}
        /// Deletes a blog post.
        /// </summary>
        [HttpPost("DeletePostAsync")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            var existingPost = await _blogService.GetBlogPostByIdAsync(id);
            if (existingPost == null)
                return NotFound($"Blog post with ID {id} not found.");

            await _blogService.DeleteBlogPostAsync(id);
            return NoContent();
        }
    }
}
