using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class AssignmentfileOutDTO
    {
        public int Id { get; set; }

        public string? AssignmentFile { get; set; }

        public static AssignmentfileOutDTO FromAssignmentFile(Assignmentfile assignment)
        {
            return new AssignmentfileOutDTO
            {
                Id = assignment.Id,
                AssignmentFile = assignment.AssignmentFile
            };
        }
    }
}
