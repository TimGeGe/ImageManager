using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ImageManager.Model;
using ImageManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Services
{
    public class PersistentService : IPersistentService
    {
        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        private readonly Settings _settings;
        private readonly IAmazonS3 _s3Client;
        private readonly TransferUtilityConfig _config;

        public PersistentService()
        {
            _settings = Settings.Default;
            var credentials = new BasicAWSCredentials(_settings.AWSAccessKey, _settings.AWSSecretKey);

            var region = RegionEndpoint.GetBySystemName(_settings.AWSRegion);

            _s3Client = new AmazonS3Client(credentials, region);
            _config = new TransferUtilityConfig
            {
                // Use 5 concurrent requests.
                ConcurrentServiceRequests = 5,

                // Use multipart upload for file size greater 20 MB.
                MinSizeBeforePartUpload = 20 * MB_SIZE,
            };
        }

        public async Task<bool> ImageExistsAsync(string bucketName, string objectKey)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                await _s3Client.GetObjectMetadataAsync(request);

                return true;
            }
            catch (AmazonS3Exception)
            {
                return false;
            }
        }

        public async Task GetImageAsync(string bucketName, string objectKey, string filePath)
        {
            using (var transferUtility = new TransferUtility(_s3Client, _config))
            {
                var request = new TransferUtilityDownloadRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    FilePath = filePath
                };

                await transferUtility.DownloadAsync(request);
            }
        }


        public async Task<ImageUploadedModel> UploadImageAsync(string bucketName, string objectKey, string filePath)
        {
            using (var transferUtility = new TransferUtility(_s3Client, _config))
            {
                var model = new ImageUploadedModel();

                var request = new TransferUtilityUploadRequest
                {
                    Key = objectKey,
                    BucketName = _settings.OriginalBucketName,
                    StorageClass = S3StorageClass.Standard,
                    CannedACL = S3CannedACL.Private,
                    FilePath = filePath,
                };
                await transferUtility.UploadAsync(request);

                return model;
            }
        }
    }
}
