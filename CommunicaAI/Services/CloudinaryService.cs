using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommunicaAI.Models;
using CommunicaAI.Models.Configurations;
using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CommunicaAI.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;

        var account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<MediaUploadResult> UploadAudioAsync(IFormFile file, Guid userId)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = $"communica-ai/users/{userId}/audio",
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return new MediaUploadResult
        {
            Url = result.SecureUrl?.ToString() ?? string.Empty,
            PublicId = result.PublicId ?? string.Empty
        };
    }

    public async Task<MediaUploadResult> UploadVideoAsync(IFormFile file, Guid userId)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = $"communica-ai/users/{userId}/video",
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return new MediaUploadResult
        {
            Url = result.SecureUrl?.ToString() ?? string.Empty,
            PublicId = result.PublicId ?? string.Empty
        };
    }

    public async Task DeleteAsync(string publicId, ResourceType resourceType)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return;

        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = resourceType
        };

        await _cloudinary.DestroyAsync(deleteParams);
    }
}