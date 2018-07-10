using Amazon.S3;
using ImageManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Services
{
    public interface IPersistentService
    {
        Task<bool> ImageExistsAsync(string bucketName, string objectKey);
        Task GetImageAsync(string bucketName, string objectKey, string filePath);
        Task<ImageUploadedModel> UploadImageAsync(string bucketName, string objectKey, ImageInfo image);
    }
}
