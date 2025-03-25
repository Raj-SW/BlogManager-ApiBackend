using BusinessLayer.AuthenthicationService;
using BusinessLayer.AuthenticationService;
using BusinessLayer.BlogPostService;
using BusinessLayer.CommentService;
using BusinessLayer.ImageUpload;
using BusinessLayer.UserService;
using DataAcessLayer.AuthenticationDAL;
using DataAcessLayer.BlogPostDAL;
using DataAcessLayer.CommentDAL;
using DataAcessLayer.UserDAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------------
// Read from appsettings.json for Firestore
// ------------------------------------------------------------------------
var credentialPath = builder.Configuration["GoogleCloud:CredentialPath"];
var projectId = builder.Configuration["GoogleCloud:ProjectId"];

if (!string.IsNullOrWhiteSpace(credentialPath))
{
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
}

// ------------------------------------------------------------------------
// Configure Services
// ------------------------------------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------------------------
// Register Business/Data Access Services
// ------------------------------------------------------------------------
builder.Services.AddScoped<IBlogPostService, FirestoreBlogPostService>();
builder.Services.AddScoped<IAuthService, FirebaseAuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IBlogPostDAL, FirestoreBlogPostDAL>();
builder.Services.AddScoped<IAuthenticationDAL, FirebaseAuthenticationDAL>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<ICommentDAL, CommentDAL>();
builder.Services.AddScoped<IFileImageUpload, CloudinaryImageService>();

// ------------------------------------------------------------------------
// Configure CORS
// ------------------------------------------------------------------------
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

// ------------------------------------------------------------------------
// Authentication & Authorization for JwT Token
// ------------------------------------------------------------------------
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = true,
        ValidAudience = jwtAudience,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });

    options.AddPolicy("LoggedUser", policy =>
    {
        policy.RequireRole("LoggedUser", "Editor");
    });
});

//-------------------------------------------------------------------------
// File hosting
//-------------------------------------------------------------------------

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));


// ------------------------------------------------------------------------
// Build the App
// ------------------------------------------------------------------------

var app = builder.Build();

// ------------------------------------------------------------------------
// Middleware Pipeline
// ------------------------------------------------------------------------
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
