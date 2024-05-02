using CourseContent.Core.DTO.CommonValidation;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTO.Requests.UpdateDTO
{
    public class AssignmentUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? TopicId { get; set; }

        public required string AssignmentName { get; set; }

        public string? AssignmentDescription { get; set; }

        public int MaxMark { get; set; }

        public int MinMark { get; set; }

        public bool IsRequired { get; set; }

        public DateTime AssignmentDatePublication { get; set; } = DateTime.UtcNow;

        public DateTime AssignmentDeadline { get; set; }

        public static Assignment FromAssignmentUpdateDto(AssignmentUpdateDTO assignmentDto)
        {
            return new Assignment
            {
                Id = assignmentDto.Id,
                CourseId = assignmentDto.CourseId,
                TopicId = assignmentDto.TopicId,
                AssignmentName = assignmentDto.AssignmentName,
                AssignmentDescription = assignmentDto.AssignmentDescription,
                MaxMark = assignmentDto.MaxMark,
                MinMark = assignmentDto.MinMark,
                IsRequired = assignmentDto.IsRequired,
                AssignmentDatePublication = assignmentDto.AssignmentDatePublication,
                AssignmentDeadline = assignmentDto.AssignmentDeadline
            };
        }
    }
}
