namespace CourseContent.Core.DTO.Responses
{
    public class AssignmentOutDTO
    {
        public int Id { get; set; }

        public int? TopicId { get; set; }

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
    }
}
