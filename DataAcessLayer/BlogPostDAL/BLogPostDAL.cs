using DataAcessLayer.Common;
using Microsoft.Data.SqlClient;
using Model.BlogPost;
using Model.Utils;
using Serilog;

namespace DataAcessLayer.BlogPostDAL
{
    public class BLogPostDAL : IBlogPostDAL
    {
        private readonly IDBCommand _command;

        public BLogPostDAL(IDBCommand command)
        {
            _command = command;
        }

        public async Task<GenericResult<List<BlogPost>>> GetAllBlogPostsAsync()
        {
            GenericResult<List<BlogPost>> result = new();

            try
            {
                const string GET_ALL_BLOG_QUERY = "SELECT * FROM Blog";
                result.ResultObject = await _command.GetDataAsync<BlogPost>(GET_ALL_BLOG_QUERY);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving all blog posts.", ex.Message, ex);
                throw;
            }
        }

        public async Task<GenericResult<List<BlogPost>>> GetAllBlogPostsByAuthorUserNameAsync(string AuthorUserName)
        {
            GenericResult<List<BlogPost>> result = new();

            try
            {
                const string GET_ALL_BLOG_QUERY_BY_AUTHOR_USERNAME = @"SELECT TOP 10 [Blog].* 
                                                            FROM [Blog] INNER JOIN [UserInfo] 
                                                            ON Blog.UserId = UserInfo.UserId
                                                            WHERE UserInfo.UserName = @UserName";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserName", AuthorUserName));

                result.ResultObject = await _command.GetDataWithConditionsAsync<BlogPost>(GET_ALL_BLOG_QUERY_BY_AUTHOR_USERNAME, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching blogs post by email {AuthorUserName}.", ex.Message, ex);
                throw;
            }
        }

        public async Task<GenericResult<BlogPost>?> GetBlogPostByIdAsync(int id)
        {
            GenericResult<BlogPost> result = new();
            try
            {
                const string GET_ALL_BLOG_QUERY_BY_BLOG_ID = @"SELECT TOP 1 *
                                                            FROM Blog
                                                            WHERE BlogId = @BlogId";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@BlogId", id));

                var queryResult = await _command.GetDataWithConditionsAsync<BlogPost>(GET_ALL_BLOG_QUERY_BY_BLOG_ID, parameters);
                result.ResultObject = queryResult.First();

                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving blog post with ID {id}.", ex.Message, ex);
                throw;
            }
        }

        public async Task<Result> CreateBlogPostAsync(BlogPost blogPost)
        {
            Result result = new();

            try
            {
                const string CREATE_BLOG_QUERY = @"
                        INSERT INTO Blog (UserId, Title, Excerpt, Content, CreatedDate, IsFeatured, ThumbnailLink)
                        VALUES (@UserId, @Title, @Excerpt, @Content, @CreatedDate, @IsFeatured, @ThumbnailLink);
                    ";
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@UserId", blogPost.UserId));
                parameters.Add(new SqlParameter("@Title", blogPost.Title));
                parameters.Add(new SqlParameter("@Excerpt", blogPost.Excerpt));
                parameters.Add(new SqlParameter("@Content", blogPost.Content));
                parameters.Add(new SqlParameter("@CreatedDate", blogPost.CreatedDate));
                parameters.Add(new SqlParameter("@IsFeatured", blogPost.IsFeatured));
                parameters.Add(new SqlParameter("@ThumbnailLink", blogPost.ThumbnailLink));

                await _command.InsertUpdateDataAsync(CREATE_BLOG_QUERY, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating blog post.", ex.Message, ex);
                throw;
            }
        }

        //public async Task<Result> UpdateBlogPostAsync(string documentId, BlogPost updatedPost)
        //{
        //    Result result = new();

        //    try
        //    {
        //        var docRef = _db.Collection(_blogCollectionName).Document(documentId);
        //        await docRef.SetAsync(updatedPost, SetOptions.Overwrite);
        //        result.IsSuccess = true;

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Error updating blog post with ID {documentId}.", ex.Message, ex);
        //        throw;
        //    }
        //}

        //public async Task DeleteBlogPostByIdAsync(string id)
        //{
        //    try
        //    {
        //        var docRef = _db.Collection(_blogCollectionName).Document(id);
        //        await docRef.DeleteAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Error deleting blog post with ID {id}.", ex.Message, ex);
        //        throw;
        //    }
        //}


        //public async Task<GenericResult<IEnumerable<BlogPost>>> SearchBlogAsync(string searchCriteria)
        //{
        //    GenericResult<IEnumerable<BlogPost>> blogList = new();
        //    try
        //    {
        //        var collectionRef = _db.Collection(_blogCollectionName);
        //        var snapshot = await collectionRef.GetSnapshotAsync();

        //        var allPosts = snapshot.Documents
        //            .Select(doc => doc.ConvertTo<BlogPost>())
        //            .ToList();

        //        if (string.IsNullOrWhiteSpace(searchCriteria))
        //        {
        //            blogList.ResultObject = allPosts;
        //            blogList.IsSuccess = true;

        //            return blogList;
        //        }

        //        string lowerSearch = searchCriteria.ToLower();

        //        var filtered = allPosts.Where(post =>
        //            (post.Title ?? "").ToLower().Contains(lowerSearch) ||
        //            (post.Content ?? "").ToLower().Contains(lowerSearch) ||
        //            (post.CreatedBy ?? "").ToLower().Contains(lowerSearch)
        //        ).ToList();

        //        blogList.ResultObject = filtered;
        //        blogList.IsSuccess = true;

        //        return blogList;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Error searching blog post by criteria {searchCriteria}.", ex.Message, ex);
        //        throw;
        //    }
        //}

        //public Task SuggestEditBlogPostAsync(BlogPost suggestEditBlog)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<GenericResult<IEnumerable<BlogPost>>> GetAllBlogPostsByTagsAsync(List<string> tags)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
