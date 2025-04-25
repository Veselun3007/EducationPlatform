using CourseService.Domain.Enums;

namespace CourseService.Application.DTO.Request
{
    public class CourseuserDTO
    {
        public string CourseLink { get; set; }
        public string UserId { get; set; }
        public Roles Role { get; set; }
        public bool IsAdmin { get; set; }
    }
}
