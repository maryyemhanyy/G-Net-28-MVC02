using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.AttachmentServices
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long maxFileSize = 5 * 1024 * 1024;

        private readonly string[] allowedExtensions = {".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AttachmentService> _logger;

        public AttachmentService(IWebHostEnvironment env , ILogger<AttachmentService> logger) {

             _env = env;
            _logger = logger;
        }

        public async Task<string?> UploadAttachmentAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if(fileStream.Length == 0 ) return null;
            if(fileStream.Length > maxFileSize)
            {
                _logger.LogWarning("File size exceeds the maximum limit of {MaxFileSize} bytes.", maxFileSize);
                return null;
            }

            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning("File extension {FileExtension} is not allowed.", fileExtension);
                return null;
            }

            var uploadsFolderPath = Path.Combine(_env.WebRootPath, folderName);

            Directory.CreateDirectory(uploadsFolderPath);

            var storedFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(uploadsFolderPath, storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write , FileShare.None);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the file.");
                return null;
            }
        }
        public bool DeleteAttachment(string fileName, string folderName)
        {
            if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

            try
            {
                var fullPath = Path.Combine(_env.WebRootPath, folderName, fileName);

                if (!File.Exists(fullPath)) return false;

                File.Delete(fullPath);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the file.");
                return false;
            }

        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return null;

            var fullPath = Path.Combine(_env.WebRootPath, folderName, fileName);

            if (!File.Exists(fullPath)) return null;

            var contentType = Path.GetExtension(fullPath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            var stram = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return (stram, contentType);

        }


    }
}
