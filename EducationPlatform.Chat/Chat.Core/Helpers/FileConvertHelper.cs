using Microsoft.AspNetCore.Http;


namespace EPChat.Core.Helpers
{
    public static class FileConvertHelper
    {
        public static IFormFile ConvertBase64ToIFormFile(string base64File, string fileName)
        {
            var fileBytes = Convert.FromBase64String(base64File);
            var memoryStream = new MemoryStream(fileBytes);
            IFormFile file = new FormFile(memoryStream, 0, fileBytes.Length, "atachment", fileName);
            return file;
        }
    }
}
