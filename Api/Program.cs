using BusinessLayer.AuthenthicationService;
using BusinessLayer.AuthenticationService;
using BusinessLayer.BlogPostService;
using BusinessLayer.CommentService;
using BusinessLayer.UserService;
using DataAcessLayer.AuthenticationDAL;
using DataAcessLayer.BlogPostDAL;
using DataAcessLayer.CommentDAL;
using DataAcessLayer.UserDAL;

var builder = WebApplication.CreateBuilder(args);

// Read from appsettings.json for Firestore
var credentialPath = builder.Configuration["GoogleCloud:CredentialPath"];
var projectId = builder.Configuration["GoogleCloud:ProjectId"];

if (!string.IsNullOrWhiteSpace(credentialPath))
{
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
}

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBlogPostService, FirestoreBlogPostService>();
builder.Services.AddScoped<IAuthenticationService, FirebaseAuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IBlogPostDAL, FirestoreBlogPostDAL>();
builder.Services.AddScoped<IAuthenticationDAL, FirebaseAuthenticationDAL>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<ICommentDAL, CommentDAL>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();
