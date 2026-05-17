using CommunicaAI.Models;

namespace CommunicaAI.Services
{
    public interface ICloudinaryService
    {
        Task<MediaUploadResult> UploadAudioAsync(IFormFile file, Guid userId);
        Task<MediaUploadResult> UploadVideoAsync(IFormFile file, Guid userId);
        Task DeleteAsync(string publicId, string resourceType);
    }
}
