using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class AssignmentlinkOutDTO
    {
        public int Id { get; set; }

        public string? AssignmentLink { get; set; }

        public static AssignmentlinkOutDTO FromAssignmentLink(Assignmentlink assignmentLink)
        {
            return new AssignmentlinkOutDTO
            {
                Id = assignmentLink.Id,
                AssignmentLink = assignmentLink.AssignmentLink
            };
        }
    }
}
