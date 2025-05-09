namespace StudentResult.Application.DTO.Request
{
    public class CommentDto
    {
        public string CommentText { get; set; }

        public int CourseUserId { get; set; }

        public int StudentAssignmentId { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
    }
}
