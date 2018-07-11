using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using ImageManager.Model;
using ImageManager.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager
{
    public class ImageManager
    {
        private readonly IPersistentService _persistenService;
        private readonly IFileSystem _filesystem;
        public ImageManager(IPersistentService persistentService, IFileSystem fileSystem)
        {
            _persistenService = persistentService;
            _filesystem = fileSystem;
        }

        public async Task<ConcurrentBag<ImageUploadedModel>> ProcessUploadAsync(List<string> files)
        {
            if (files == null)
                throw new Exception("files must be provided.");

            var models = new ConcurrentBag<ImageUploadedModel>();

            var tasks = files.Select(async file =>
                {
                    if (file != null && _filesystem.File.Exists(file))
                    {
                        try
                        {
                            using (Stream stream = _filesystem.File.OpenRead(file))
                            {
                                var objectKey = KeyGenerator.GenerateObjectKey(file);
                                var model = await _persistenService.UploadImageAsync(Properties.Settings.Default.OriginalBucketName, objectKey, file);
                                models.Add(model);
                            }
                        }
                        catch (Exception e)
                        {
                            models.Add(new ImageUploadedModel
                            {
                                ObjectKey = file,
                                Exception = e
                            });
                        }
                    }
                });
            await Task.WhenAll(tasks);
            return models;
        }
    }
}
