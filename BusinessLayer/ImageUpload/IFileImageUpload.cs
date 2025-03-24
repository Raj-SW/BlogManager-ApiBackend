using Microsoft.AspNetCore.Http;

namespace BusinessLayer.ImageUpload
{
    public interface IFileImageUpload
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
