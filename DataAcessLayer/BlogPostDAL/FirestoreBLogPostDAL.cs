using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.BlogPost;

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

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            try
            {
                var snapshot = await _db.Collection(_blogCollectionName).GetSnapshotAsync();
                var posts = new List<BlogPost>();
                foreach (var doc in snapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        var post = doc.ConvertTo<BlogPost>();
                        post.BlogPostDocumentId = doc.Id;
                        posts.Add(post);
                    }
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all blog posts.");
                throw;
            }
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(string id)
        {
            try
            {
                var docRef = _db.Collection(_blogCollectionName).Document(id);
                var snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    var post = snapshot.ConvertTo<BlogPost>();
                    post.BlogPostDocumentId = snapshot.Id;
                    return post;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving blog post with ID {id}.");
                throw;
            }
        }

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                blogPost.CreatedDate = DateTime.UtcNow;
                var collectionRef = _db.Collection(_blogCollectionName);
                var docRef = await collectionRef.AddAsync(blogPost);
                blogPost.BlogPostDocumentId = docRef.Id;
                return blogPost;
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
                var docRef = _db.Collection(_blogCollectionName).Document(documentId);
                // Ensure the document ID remains consistent.
                updatedPost.BlogPostDocumentId = documentId;
                await docRef.SetAsync(updatedPost, SetOptions.Overwrite);
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
                var docRef = _db.Collection(_blogCollectionName).Document(id);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting blog post with ID {id}.");
                throw;
            }
        }
        public async Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog)
        {
            throw new NotImplementedException();
        }
    }
}
