using CourseService.Domain.Entities;

namespace CourseService.Application.DTO.Request
{
    public class AdminDTO
    {
        public Course Course { get; set; }
        public Courseuser UserInfo { get; set; }
        public string? ImageLink { get; set; }
        public string AdminName { get; set; }
        public string UserId { get; set; }
    }
}
