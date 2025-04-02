using CourseService.Domain.Entities;
using CourseService.Domain.Enums;

namespace CourseService.Application.DTOs
{
    public class CourseInfo
    {
        public CourseInfo(Course course, Courseuser courseuser)
        {
            Course = course;
            UserInfo = new UserInfo(courseuser);
            AdminInfo = new AdminInfo();
        }

        public Course Course { get; set; }
        public UserInfo UserInfo { get; set; }
        public AdminInfo AdminInfo { get; set; }
    }
    public class UserInfo
    {
        public UserInfo(Courseuser courseuser)
        {
            Role = courseuser.Role;
            CourseuserId = courseuser.Id;
        }

        public Roles Role { get; set; }
        public int CourseuserId { get; set; }
    }
}
