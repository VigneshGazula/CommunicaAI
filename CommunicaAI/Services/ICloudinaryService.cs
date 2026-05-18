using CloudinaryDotNet.Actions;
using CommunicaAI.Models;
using System.Security.AccessControl;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace CommunicaAI.Services
{
    public interface ICloudinaryService
    {
        Task<MediaUploadResult> UploadAudioAsync(IFormFile file, Guid userId);
        Task<MediaUploadResult> UploadVideoAsync(IFormFile file, Guid userId);
        Task DeleteAsync(string publicId, ResourceType resourceType);
    }
}
