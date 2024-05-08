using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPChat.Core.Helpers
{
    public class FileConvertHelper
    {
        public IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName)
        {
            var memoryStream = new MemoryStream(fileBytes);
            return new FormFile(memoryStream, 0, memoryStream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
        }
    }
}
