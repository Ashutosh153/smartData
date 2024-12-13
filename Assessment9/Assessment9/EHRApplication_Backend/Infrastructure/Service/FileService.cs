using Core.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService(IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath ?? throw new ArgumentNullException(nameof(environment.WebRootPath));
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            var path = Path.Combine(_webRootPath, "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return Path.Combine("Uploads", fileName); // Return relative path to access file via URL
        }
    }
}
