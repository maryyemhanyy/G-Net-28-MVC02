using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.AttachmentServices
{
    public interface IAttachmentService
    {
        Task<string?> UploadAttachmentAsync(Stream fileStream , string fileName , string folderName , CancellationToken ct = default);

        bool DeleteAttachment(string fileName , string folderName);

        (Stream stream , string contentType)? GetFile(string fileName, string folderName);
    }
}
