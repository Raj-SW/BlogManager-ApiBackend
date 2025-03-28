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
                const string GET_ALL_BLOG_QUERY = @"SELECT Blog.*, UserInfo.UserName
                                                    FROM Blog
                                                    INNER JOIN UserInfo ON UserInfo.UserId = Blog.UserId";
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
                const string GET_ALL_BLOG_QUERY_BY_BLOG_ID = @"SELECT TOP 1 Blog.*, UserInfo.UserName
                                                            FROM Blog
                                                            INNER JOIN UserInfo ON UserInfo.UserId = Blog.UserId
                                                            WHERE BlogId = @BlogId;
                                                            ";

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

        public async Task DeleteBlogPostAsync(int blogId)
        {
            try
            {
                const string DELETE_BLOG_QUERY_BY_ID = @"Delete 
                                                        FROM Blog 
                                                        WHERE BlogId = @BlogId
                                                        ";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@BlogId", blogId));

                await _command.InsertUpdateDataAsync(DELETE_BLOG_QUERY_BY_ID, parameters);
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting blog post.", ex.Message, ex);
                throw;
            }
        }

        public async Task<GenericResult<List<BlogPost>>> SearchBlogAsync(string searchCriteria)
        {
            GenericResult<List<BlogPost>> result = new GenericResult<List<BlogPost>>();
            try
            {
                string query = @"SELECT Blog.*, UserInfo.UserName
                                FROM Blog
                                INNER JOIN UserInfo 
                                    ON UserInfo.UserId = Blog.UserId
                                WHERE Title LIKE '%' + @SearchCriteria + '%'
                                   OR Content LIKE '%' + @SearchCriteria + '%'
                                   OR UserName LIKE '%' + @SearchCriteria + '%';
                                ";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@SearchCriteria",searchCriteria)
                };

                List<BlogPost> blogPosts = await _command.GetDataWithConditionsAsync<BlogPost>(query, parameters);
                result.IsSuccess = true;
                result.ResultObject = blogPosts;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage.Add("Errror fetching from the data base");
                Log.Error("Errror fetching from the data base", ex.Message);
            }
            return result;
        }
    }
}
