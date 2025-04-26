using CourseService.Application.DTO.Request;
using CourseService.Application.DTO.Response;
using CourseService.Domain.Entities;
using CourseService.Domain.Enums;

namespace CourseService.Application.Mappings
{
    public class NativeMapper
    {
        public static CourseUserOutDTO FromCourseuser(Courseuser courseUser)
        {
            return new CourseUserOutDTO
            {
                CourseUserId = courseUser.Id,
                Role = courseUser.Role,
                UserId = courseUser.UserId,
                UserName = courseUser.User?.UserName,
                UserEmail = courseUser.User?.UserEmail,
                UserImage = courseUser.User?.UserImage,
            };
        }

        public static Course ToCourse(CourseDTO courseDto, string link)
        {
            return new Course
            {
                CourseName = courseDto.CourseName,
                CourseDescription = courseDto.CourseDescription,
                CourseLink = link
            };
        }

        public static Courseuser ToCourseuser(Roles role, Course? course, User? user)
        {
            return new Courseuser
            {
                Role = role,
                Course = course,
                User = user
            };
        }
    }
}
