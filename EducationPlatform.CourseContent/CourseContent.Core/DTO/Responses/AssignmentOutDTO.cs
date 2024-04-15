using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class AssignmentOutDTO
    {
        public required string AssignmentName { get; set; }

        public string? AssignmentDescription { get; set; }

        public DateTime AssignmentDatePublication { get; set; }

        public DateTime AssignmentDeadline { get; set; }

        public int MaxMark { get; set; }

        public int MinMark { get; set; }

        public bool IsRequired { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedTime { get; set; }

        public ICollection<AssignmentfileOutDTO>? Assignmentfiles { get; set; }

        public ICollection<AssignmentlinkOutDTO>? Assignmentlinks { get; set; }

        public static AssignmentOutDTO FromAssignment(Assignment assignment)
        {
            return new AssignmentOutDTO
            {
                AssignmentName = assignment.AssignmentName,
                AssignmentDescription = assignment.AssignmentDescription,
                AssignmentDatePublication = assignment.AssignmentDatePublication,
                AssignmentDeadline = assignment.AssignmentDeadline,
                MaxMark = assignment.MaxMark,
                MinMark = assignment.MinMark,
                IsRequired = assignment.IsRequired,
                EditedTime = assignment.EditedTime,
                Assignmentfiles = assignment
                    .Assignmentfiles.Select(af => AssignmentfileOutDTO
                    .FromAssignmentFile(af)).ToList(),
                Assignmentlinks = assignment
                    .Assignmentlinks.Select(al => AssignmentlinkOutDTO
                    .FromAssignmentLink(al)).ToList()
            };
        }

    }
}
