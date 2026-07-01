using System;
using API.Helper;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var account= new Account (
            config.Value.CloudName ?? throw new InvalidOperationException("Cloudinary:CloudName not configured"),
            config.Value.ApiKey ?? throw new InvalidOperationException("Cloudinary:ApiKey not configured"),
            config.Value.ApiSecret ?? throw new InvalidOperationException("Cloudinary:ApiSecret not configured")
        );
        _cloudinary= new Cloudinary(account);
    }


    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(800).Width(800).Crop("limit"),
            Folder = "rentapp/assets"
        };
        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error is not null)
            throw new Exception($"Cloudinary upload failed: {result.Error.Message}");
        return new PhotoUploadResult(result.SecureUrl.ToString(), result.PublicId);
    }

    public async Task DeletePhotoAsync(string publicId)
    {
        var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        if (result.Error is not null)
            throw new Exception($"Cloudinary delete failed: {result.Error.Message}");
    }

}
