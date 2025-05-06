using CourseService.Application.DTO.Request;
using CourseService.Application.DTO.Response;
using CourseService.Application.Interfaces;
using CourseService.Application.Mappings;
using CourseService.Domain.Enums;

namespace CourseService.Application.Services
{
    public class CourseuserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SpecificMapper _mapper;

        public CourseuserService(IUnitOfWork unitOfWork, SpecificMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseUserOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var assignments = await _unitOfWork.CourseuserRepository
                .FindAllAsync(cu => cu.CourseId == courseId);
            return await Task.WhenAll(assignments.Select(cu => _mapper.FromCourseuser(cu)));
        }

        public async Task<CourseInfoOutDTO> CreateAdminAsync(AdminDTO adminDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(adminDTO.UserId);
            var courseuser = NativeMapper.ToCourseuser(Roles.Admin, adminDTO.Course, user);
            
            courseuser = await _unitOfWork.CourseuserRepository.AddAsync(courseuser);
            await _unitOfWork.CommitAsync();

            return await _mapper.From(adminDTO.Course, courseuser, user);
        }

        public async Task CreateStudentAsync(StudentDTO studentDTO)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(c => c.CourseLink == studentDTO.CourseLink);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(studentDTO.UserId);

            if(!await _unitOfWork.CourseuserRepository.AnyAsync(cu => cu.UserId == user.Id && cu.CourseId == course.Id))
            {
                var courseuser = NativeMapper.ToCourseuser(Roles.Student, course, user);
                courseuser = await _unitOfWork.CourseuserRepository.AddAsync(courseuser);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<CourseUserOutDTO> UpdateCourseuserAsync(UpdateCourseuserDTO courseuserDTO)
        {
            var courseuser = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu => cu.Id == courseuserDTO.CourseuserId);
            var admin = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu =>
                (cu.UserId == courseuserDTO.UserId) && 
                (cu.Role == Roles.Admin) &&
                (cu.CourseId == courseuser.CourseId) && 
                (cu.UserId != courseuser.UserId));

            if(admin is null)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this user.");
            }

            courseuser.Role = courseuserDTO.Role;
            courseuser = await _unitOfWork.CourseuserRepository.UpdateAsync(courseuser.Id, courseuser);
            await _unitOfWork.CommitAsync();
            return await _mapper.FromCourseuser(courseuser);
        }

        public async Task DeleteCourseuserAsync(string userId, int id)
        {
            var userToDelete = await _unitOfWork.CourseuserRepository.GetByIdAsync(id);
            if(userToDelete is null)
            {
                return;
            }

            var issuer = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu =>
                cu.CourseId == userToDelete.CourseId && cu.UserId == userId);
            if(issuer is null)
            {
                return;
            }

            bool isSelfRemoval = issuer.Id == userToDelete.Id && userToDelete.Role != Roles.Admin;
            bool isAdminRemovingNonAdmin = issuer.Role == Roles.Admin && userToDelete.Role != Roles.Admin;
            if(isSelfRemoval || isAdminRemovingNonAdmin)
            {
                await _unitOfWork.CourseuserRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
