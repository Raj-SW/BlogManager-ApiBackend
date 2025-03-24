using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.BlogPost;
using Model.Utils;

namespace DataAcessLayer.BlogPostDAL
{
    public class FirestoreBlogPostDAL : IBlogPostDAL
    {
        private readonly FirestoreDb _db;
        private readonly ILogger<FirestoreBlogPostDAL> _logger;
        private readonly string? _blogCollectionName;
        private readonly string? _userCollectionName;
        private readonly string? _credentialPath;
        private readonly string? _projectId;

        public FirestoreBlogPostDAL(ILogger<FirestoreBlogPostDAL> logger, IConfiguration config)
        {
            _credentialPath = config["GoogleCloud:CredentialPath"];
            _projectId = config["GoogleCloud:ProjectId"];
            _blogCollectionName = config["GoogleCloud:BlogCollectionName"];
            _userCollectionName = config["GoogleCloud:UserCollectionName"];
            _db = FirestoreDb.Create(_projectId);
            _logger = logger;
        }

        public async Task<Result> GetAllBlogPostsAsync()
        {
            Result result = new();

            try
            {
                QuerySnapshot? snapshot = await _db.Collection(_blogCollectionName).GetSnapshotAsync();
                List<BlogPost> posts = new List<BlogPost>();
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        BlogPost post = doc.ConvertTo<BlogPost>();
                        post.BlogPostDocumentId = doc.Id;
                        posts.Add(post);
                    }
                }
                result.ResultObject = posts;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all blog posts.");
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
            }
        }

        public async Task<Result?> GetBlogPostByIdAsync(string id)
        {
            Result result = new();
            try
            {
                var docRef = _db.Collection(_blogCollectionName).Document(id);
                var snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    var post = snapshot.ConvertTo<BlogPost>();
                    post.BlogPostDocumentId = snapshot.Id;
                    result.ResultObject = post;
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving blog post with ID {id}.");
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
            }
        }

        public async Task<Result> CreateBlogPostAsync(BlogPost blogPost)
        {
            Result result = new();

            try
            {
                blogPost.CreatedDate = DateTime.UtcNow;
                var collectionRef = _db.Collection(_blogCollectionName);
                var docRef = await collectionRef.AddAsync(blogPost);
                blogPost.BlogPostDocumentId = docRef.Id;
                result.ResultObject = blogPost;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new blog post.");
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
            }
        }

        public async Task<Result> UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        {
            Result result = new();

            try
            {
                var docRef = _db.Collection(_blogCollectionName).Document(documentId);
                // Ensure the document ID remains consistent.
                updatedPost.BlogPostDocumentId = documentId;
                await docRef.SetAsync(updatedPost, SetOptions.Overwrite);
                result.ResultObject = updatedPost;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating blog post with ID {documentId}.");
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
                return result;
            }
        }

        public async Task DeleteBlogPostAsync(string id)
        {
            Result result = new();

            try
            {
                var docRef = _db.Collection(_blogCollectionName).Document(id);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting blog post with ID {id}.");
                throw ex;
            }
        }

        public Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAllBlogPostsByTagsAsync(List<string> tags)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> GetAllBlogPostsByAuthorAsync(string userName)
        {
            Result result = new Result();

            try
            {
                var collectionRef = _db.Collection("BlogPosts");

                var query = collectionRef.WhereEqualTo("CreatedBy", userName);

                var snapshot = await query.GetSnapshotAsync();

                List<BlogPost> blogPosts = new List<BlogPost>();
                foreach (var doc in snapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        BlogPost blogPost = doc.ConvertTo<BlogPost>();
                        blogPost.BlogPostDocumentId = doc.Id;
                        blogPosts.Add(blogPost);
                    }
                }

                result.IsSuccess = true;
                result.ResultObject = blogPosts;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add(ex.Message);
            }

            return result;
        }

        public Task<Result> SearchBlogPostAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}
