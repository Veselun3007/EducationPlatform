using CourseContent.Core.DTO.CommonValidation;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTO.Requests
{
    public class MaterialDTO
    {
        public int CourseId { get; set; }

        public int? TopicId { get; set; }

        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; } = DateTime.UtcNow;

        [ValidateFile([".png", ".jpg", ".jpeg", ".doc",
            ".pdf", ".docx", ".pptx", ".ppt", ".xls", ".xlsx"],
            ErrorMessage = "Файл має непідтримуване розширення")]
        public List<IFormFile>? MaterialFiles { get; set; }

        public List<string>? MaterialLinks { get; set; }
    }
}
