using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

namespace main_service.Storage
{
    public class StorageManager
    {
        private const string AccessKeyId = "AKIAIAXDKRKUO5QSQF2Q";
        private const string SecretAccessKey = "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";
        private const string BucketName = "maintenance-system-storage";
        
        static readonly AmazonS3Client Client = new AmazonS3Client(AccessKeyId, SecretAccessKey, RegionEndpoint.APSoutheast1);

        public async Task<string> UploadToAwsS3(IFormFile file)
        {
            await using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);
            
            var guid = Guid.NewGuid().ToString();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = guid,
                BucketName = "maintenance-system-storage",
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            return $"http://{BucketName}.s3.{RegionEndpoint.APSoutheast1.SystemName}.amazonaws.com/{guid}";
        }

        public async Task RemoveFile(string key)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = key
                };

                Console.WriteLine("Deleting an object");
                await Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }
    }
}