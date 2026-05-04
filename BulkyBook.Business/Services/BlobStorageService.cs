using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            try
            {
                var blobName = Path.GetFileName(new Uri(imageUrl).LocalPath);
                await _containerClient.GetBlobClient(blobName).DeleteIfExistsAsync();
            }
            catch
            {
                // Silently handle deletion errors
            }

        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string contentType)
        {
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var blobClient = _containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            });
            return blobClient.Uri.ToString();
        }
    }
}
