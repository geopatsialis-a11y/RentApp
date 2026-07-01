using System;
using CloudinaryDotNet.Actions;

namespace API.Interfaces;



public record PhotoUploadResult(string Url, string PublicId);

public interface IPhotoService
{
    // Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task DeletePhotoAsync(string publicId);
}

