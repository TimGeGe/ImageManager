using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using ImageManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager
{
    public class ImageManager
    {
        private readonly IPersistentService _persistenService;
        public ImageManager(IPersistentService persistentService)
        {
            _persistenService = persistentService;
        }

        public async Task ProcessUploadAsync(List<string> files)
        {
            if (files == null)
                throw new Exception("files must be provided.");

            await Task.Run(() =>
                Parallel.ForEach(files, file =>
                    {
                        using (Stream stream = File.OpenRead(file))
                        {
                            var imageInfo = ImageProcessor.GetImageInfo(stream);
                            var objectKey = KeyGenerator.GenerateObjectKey(file);

                            _persistenService.UploadImageAsync(Properties.Settings.Default.OriginalBucketName, objectKey, imageInfo);
                        }
                    }
                ));
        }
    }
}
