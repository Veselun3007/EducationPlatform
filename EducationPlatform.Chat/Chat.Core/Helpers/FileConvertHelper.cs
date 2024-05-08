using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace EPChat.Core.Helpers
{
    public static class FileConvertHelper
    {
        public static IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName)
        {
            var memoryStream = new MemoryStream(fileBytes);
            IFormFile file = new FormFile(memoryStream, 0, fileBytes.Length, "atachment", fileName);
            return file;
        }
    }
}
