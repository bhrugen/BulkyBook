using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services.IServices
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteImageAsync(string imageUrl);
    }
}
