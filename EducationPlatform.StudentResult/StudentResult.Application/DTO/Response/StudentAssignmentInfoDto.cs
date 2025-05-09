namespace StudentResult.Application.DTO.Response
{
    public class StudentAssignmentInfoDto
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int? CurrentMark { get; set; }
        public bool? IsDone { get; set; }

        public List<CommentInfoDto> Comments { get; set; } = new();
        public List<FileInfoDto> Files { get; set; } = new();
    }
}
