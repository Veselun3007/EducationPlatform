namespace StudentResult.Application.DTO.Response
{
    public class CommentInfoDto
    {
        public int CommentId { get; set; }
        public int CourseUserId { get; set; }
        public DateTime CommentDate { get; set; }
        public string? CommentText { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? ImageLink { get; set; }
    }
}
