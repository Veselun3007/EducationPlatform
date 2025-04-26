namespace CourseContent.Core.DTO.Requests
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

        public DateTime AssignmentDatePublication { get; set; }

        public DateTime AssignmentDeadline { get; set; }

        public bool IsEdited { get; set; } = true;

        public DateTime EditedTime { get; set; } = DateTime.UtcNow;
    }
}
