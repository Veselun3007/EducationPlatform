using CourseService.Domain.Enums;

namespace CourseService.Application.DTO.Response
{
    public class CourseuserOutDTO
    {
        public int CourseuserId { get; set; }

        public Roles Role { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string? UserImage { get; set; }
    }
}
