using CourseService.Domain.Enums;

namespace CourseService.Application.DTO.Request
{
    public class UpdateCourseuserDTO
    {
        public string UserId { get; set; }
        public int CourseuserId { get; set; }
        public Roles Role { get; set; }
    }
}
