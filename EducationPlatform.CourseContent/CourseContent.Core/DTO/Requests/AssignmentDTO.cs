using CourseContent.Core.DTO.CommonValidation;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTO.Requests
{
    public class AssignmentDTO
    {
        public int CourseId { get; set; }

        public int? TopicId { get; set; }

        public required string AssignmentName { get; set; }

        public string? AssignmentDescription { get; set; }

        public int MaxMark { get; set; }

        public int MinMark { get; set; }

        public bool IsRequired { get; set; }

        public DateTime AssignmentDatePublication { get; set; } = DateTime.UtcNow;

        public DateTime AssignmentDeadline { get; set; }

        [ValidateFile([".png", ".jpg", ".jpeg", ".doc",
            ".pdf", ".docx", ".pptx", ".ppt", ".xls", ".xlsx"],
            ErrorMessage = "Файл має непідтримуване розширення")]
        public List<IFormFile>? AssignmentFiles { get; set; }

        public List<string>? AssignmentLinks { get; set; }
    }
}
