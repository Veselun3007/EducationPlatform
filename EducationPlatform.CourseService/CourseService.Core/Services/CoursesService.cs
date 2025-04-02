using CourseService.Application.DTO.Request;
using CourseService.Application.DTOs;
using CourseService.Application.Helpers;
using CourseService.Domain.Entities;
using CourseService.Domain.Enums;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Services
{
    public class CoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public CoursesService(IUnitOfWork unitOfWork, FileHelper fileHelper, CourseuserService service)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<List<CourseInfo?>> GetAllCourseAsync(string userId)
        {
            var courses = await _unitOfWork.CourseRepository.FindAllAsync(
                c => c.Courseusers.Any(u => u.UserId == userId),
                c => c.Courseusers
            );

            List<CourseInfo> response = new List<CourseInfo>();

            foreach(var course in courses)
            {
                var courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == userId);
                var admin_courseuser = course.Courseusers.FirstOrDefault(cu => cu.Role == Roles.Admin);
                var admin = await _unitOfWork.UserRepository.GetByIdAsync(admin_courseuser.UserId);
                CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);

                response.Add(courseInfo);
            }
            return response;
        }

        public async Task<CourseInfo?> GetCourseAsync(string userId, int courseId)
        {

            var course = await _unitOfWork.CourseRepository
                .FindAnyAsync(c => c.Id == courseId && c.Courseusers
                .Any(cu => cu.UserId == userId));


            var courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == userId);
            var admin = await _unitOfWork.UserRepository
                .GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.Role == Roles.Admin).UserId);

            CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);

            return courseInfo;
        }

        private async Task<CourseInfo> SetAdminInfo(Course course, Courseuser? courseuser, User? admin)
        {
            CourseInfo courseInfo = new CourseInfo(course, courseuser);

            courseInfo.AdminInfo.AdminName = admin.UserName;
            if(admin.UserImage != null)
            {

                courseInfo.AdminInfo.ImageLink = await _fileHelper.GetFileLink(admin.UserImage);
            }
            else
            {
                courseInfo.AdminInfo.ImageLink = String.Empty;
            }

            return courseInfo;
        }

        public async Task DeleteCourseAsync(string userId, int courseId)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(
                c => c.Id == courseId,
                c => c.Courseusers
            );
            var courseuser = course?.Courseusers.FirstOrDefault(cu => cu.UserId == userId);

            if(courseuser.Role == Roles.Admin)
            {
                await _unitOfWork.CourseRepository.DeleteAsync(course.Id);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<CourseInfo> UpdateCourseAsync(UpdateCourseDTO request)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(
                c => c.Id == request.CourseId,
                c => c.Courseusers
            );
            var admin = await _unitOfWork.UserRepository
                .GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.Role == Roles.Admin).UserId);
            var courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);

            if(admin != null && courseuser != null && admin.Id == request.UserId)
            {
                course.CourseName = request.CourseName;
                course.CourseDescription = request.CourseDescription;
                course = await _unitOfWork.CourseRepository.UpdateAsync(course.Id, course);
                await _unitOfWork.CommitAsync();
            }

            CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);

            return courseInfo;
        }

        public async Task<Course> CreateCourseAsync(CourseDTO request)
        {
            string courseLink = $"{request.CourseName[..30]}" + Guid.NewGuid().ToString();
            var course = new Course(request.CourseName, request.CourseDescription, courseLink);
            course = await _unitOfWork.CourseRepository.AddAsync(course);
            await _unitOfWork.CommitAsync();
            return course;
        }
    }
}
