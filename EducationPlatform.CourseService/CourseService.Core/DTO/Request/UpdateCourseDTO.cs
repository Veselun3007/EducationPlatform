namespace CourseService.Application.DTO.Request
{
    public class UpdateCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public string UserId { get; set; }
    }
}
