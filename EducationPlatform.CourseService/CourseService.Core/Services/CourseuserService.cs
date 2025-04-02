using CourseService.Application.DTO.Request;
using CourseService.Application.DTO.Response;
using CourseService.Application.Helpers;
using CourseService.Domain.Entities;
using CourseService.Domain.Enums;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Services
{
    public class CourseuserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public CourseuserService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<IEnumerable<CourseuserOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var assignments = await _unitOfWork.CourseuserRepository.FindAllAsync(cu => cu.CourseId == courseId);
            return await Task.WhenAll(assignments.Select(cu => FromCourseuser(cu)));
        }

        public async Task<AdminOutDTO> CreateAdminAsync(AdminDTO request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            var courseuser = FromCourseuserDTOToAdd(Roles.Admin, request.Course, user);
            courseuser = await _unitOfWork.CourseuserRepository.AddAsync(courseuser);
            await _unitOfWork.CommitAsync();

            request.UserInfo = courseuser;
            request.AdminName = user.UserName;

            return FromAdminToOutDTO(request);
        }

        public async Task CreateStudentAsync(StudentDTO student)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(c => c.CourseLink == student.CourseLink);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(student.UserId);

            if(!await _unitOfWork.CourseuserRepository.AnyAsync(cu => cu.UserId == user.Id && cu.CourseId == course.Id))
            {
                var courseuser = FromCourseuserDTOToAdd(Roles.Student, course, user);
                courseuser = await _unitOfWork.CourseuserRepository.AddAsync(courseuser);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<CourseuserOutDTO> UpdateCourseuserAsync(UpdateCourseuserDTO request)
        {
            var courseuser = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu => cu.Id == request.CourseuserId);
            var admin = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu =>
                (cu.UserId == request.UserId) &&
                (cu.Role == Roles.Admin) &&
                (cu.CourseId == courseuser.CourseId) &&
                (cu.UserId != courseuser.UserId)) ?? throw new UnauthorizedAccessException("You do not have permission to update this user.");

            courseuser.Role = request.Role;
            courseuser = await _unitOfWork.CourseuserRepository.UpdateAsync(courseuser.Id, courseuser);
            await _unitOfWork.CommitAsync();
            return await FromCourseuser(courseuser);
        }

        public async Task DeleteCourseuserAsync(string userId, int id)
        {
            var userToDelete = await _unitOfWork.CourseuserRepository.GetByIdAsync(id);
            var issuer = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu =>
                (cu.CourseId == userToDelete.CourseId) && (cu.UserId == userId));

            if((issuer.Id == userToDelete.Id && userToDelete.Role != Roles.Admin) || (issuer.Role == Roles.Admin && userToDelete.Role != Roles.Admin))
            {
                await _unitOfWork.CourseuserRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
        }

        private static AdminOutDTO FromAdminToOutDTO(AdminDTO admin)
        {
            return new AdminOutDTO
            {
                Course = admin.Course,
                UserInfo = admin.UserInfo,
                ImageLink = admin.ImageLink,
                AdminName = admin.AdminName,
            };
        }

        private static Courseuser FromCourseuserDTOToAdd(Roles role, Course? course, User? user)
        {
            return new Courseuser
            {
                Role = role,
                Course = course,
                User = user
            };
        }

        internal async Task<CourseuserOutDTO> FromCourseuser(Courseuser courseuser)
        {
            return new CourseuserOutDTO
            {
                CourseuserId = courseuser.Id,
                Role = courseuser.Role,
                UserId = courseuser.UserId,
                UserName = courseuser.User.UserName,
                UserEmail = courseuser.User.UserEmail,
                UserImage = await _fileHelper.GetFileLink(courseuser.User.UserImage)
            };
        }


    }
}
