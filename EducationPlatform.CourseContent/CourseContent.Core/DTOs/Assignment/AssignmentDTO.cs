using CourseContent.Core.DTOs.Validation;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTOs
{
    public class AssignmentDTO
    {
        public int CourseId { get; set; }

        public required string AssignmentName { get; set; }

        public string? AssignmentDescription { get; set; }

        public DateTime AssignmentDatePublication { get; set; } = DateTime.UtcNow;

        public DateTime AssignmentDeadline { get; set; }

        [ValidateFile([".png", ".jpg", ".jpeg", ".doc", ".pdf", ".docx"], ErrorMessage = "Файл має непідтримуване розширення")]
        public List<IFormFile>? AssignmentFiles { get; set; }

        public static Assignment FromAssignmentDto(AssignmentDTO assignmentDto)
        {
            return new Assignment
            {
                CourseId = assignmentDto.CourseId,
                AssignmentName = assignmentDto.AssignmentName,
                AssignmentDescription = assignmentDto.AssignmentDescription,
                AssignmentDatePublication = assignmentDto.AssignmentDatePublication,
                AssignmentDeadline = assignmentDto.AssignmentDeadline
            };
        }
    }
}
