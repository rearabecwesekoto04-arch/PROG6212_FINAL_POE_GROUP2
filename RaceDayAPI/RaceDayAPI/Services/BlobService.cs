using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace RaceDayAPI.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _container;

        public BlobService(IConfiguration cfg)
        {
            var connectionString = cfg["AzureBlobStorage:ConnectionString"];
            var containerName = cfg["AzureBlobStorage:ContainerName"];

            var serviceClient = new BlobServiceClient(connectionString);
            _container = serviceClient.GetBlobContainerClient(containerName);

            _container.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadAsync(IFormFile file, string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });
            return blob.Uri.ToString();
        }

        public async Task DeleteAsync(string fileName)
            => await _container.GetBlobClient(fileName).DeleteIfExistsAsync();
    }
}