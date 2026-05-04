using Azure.Storage.Blobs;
using BulkyBook.Business.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorageConnection");
            var containerName = configuration["AzureStorage:ContainerName"] ?? "bulky-book-product-image";

            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public Task DeleteImageAsync(string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImageAsync(Stream fileStream, string fileName, string contentType)
        {
            throw new NotImplementedException();
        }
    }
}
