using StudentResult.Application.DTO.Request;
using StudentResult.Application.DTO.Response;
using StudentResult.Application.Interfaces;
using StudentResult.Application.Mappings;
using StudentResult.Domain.Entities;
using StudentResult.Domain.Enums;

namespace StudentResult.Application.Services
{
    public class StudentAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _awsFileService;

        public StudentAssignmentService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _awsFileService = fileService;
        }

        public async Task<IEnumerable<CommentInfoDto>> CreateCommentAsync(string userId, CommentDto commentDto)
        {
            var studentAssignment = await _unitOfWork.StudentAssignmentRepository.GetByIdAsync(commentDto.StudentAssignmentId);
            var student = await _unitOfWork.CourseuserRepository.GetByIdAsync(studentAssignment.StudentId);

            var sender = await _unitOfWork.CourseuserRepository.FindAnyAsync(cu => cu.UserId == userId && 
                cu.CourseId == student.CourseId && (cu.UserId == student.UserId || cu.Role != Roles.Student))
                ?? throw new UnauthorizedAccessException("You do not have permission to comment on this assignment.");

            var newComment = NativeMapper.ToComment(commentDto);
            newComment.CourseUserId = sender.Id;

            await _unitOfWork.CommentRepository.AddAsync(newComment);
            await _unitOfWork.CommitAsync();

            // Оновлюємо список коментарів після створення
            var updatedStudentAssignment = await _unitOfWork.StudentAssignmentRepository.FindAnyAsync(
                sa => sa.Id == studentAssignment.Id,
                sa => sa.Comments);

            var commentSenderIds = updatedStudentAssignment.Comments.Select(c => c.CourseUserId).Distinct().ToList();
            var userDict = await GetCourseUserDictAsync(commentSenderIds);
            var imageLinkDict = await GetImageLinkDictAsync(userDict.Values);

            return GetAllComments(updatedStudentAssignment, userDict, imageLinkDict);
        }

        public async Task<StudentAssignmentInfoDto> UpdateMarkAsync(string userId, UpdateMarkDto updateMarkDto)
        {
            var studentAssignment = await _unitOfWork.StudentAssignmentRepository.FindAnyAsync(
                sa => sa.Id == updateMarkDto.StudentAssignmentId,
                sa => sa.Comments,
                sa => sa.AttachedFiles,
                sa => sa.Assignment);

            var student = await _unitOfWork.CourseuserRepository.FindAnyAsync(
                cu => cu.Id == studentAssignment.StudentId,
                cu => cu.User);

            var teacher = await _unitOfWork.CourseuserRepository.FindAnyAsync(
                cu => cu.UserId == userId && cu.CourseId == student.CourseId && cu.Role != Roles.Student,
                cu => cu.User) ?? throw new UnauthorizedAccessException("You do not have permission to get this.");

            var assignment = studentAssignment.Assignment;

            SetNewMark(updateMarkDto, studentAssignment);
            await _unitOfWork.StudentAssignmentRepository.UpdateAsync(studentAssignment.Id, studentAssignment);
            await _unitOfWork.CommitAsync();

            var commentSenderIds = studentAssignment.Comments.Select(c => c.CourseUserId).Distinct().ToList();
            var userDict = await GetCourseUserDictAsync(commentSenderIds.Append(studentAssignment.StudentId).Distinct());
            var imageLinkDict = await GetImageLinkDictAsync(userDict.Values);

            var commentDtos = GetAllComments(studentAssignment, userDict, imageLinkDict);
            var fileDtos = NativeMapper.FromAttachedFile(studentAssignment.AttachedFiles);

            var studentDto = userDict.GetValueOrDefault(studentAssignment.StudentId);
            return NativeMapper.ToStudentAssignmentInfo(studentAssignment, commentDtos, fileDtos);
        }

        private static void SetNewMark(UpdateMarkDto updateMarkDto, StudentAssignment studentAssignment)
        {
            studentAssignment.CurrentMark = updateMarkDto.NewMark;
            studentAssignment.IsDone = true;
        }

        public async Task<StudentAssignmentInfoDto> UpdateStudentAssignmentAsync(string userId, UpdateStudentAssignmentDto studentAssignmentDto)
        {
            var studentAssignment = await _unitOfWork.StudentAssignmentRepository.FindAnyAsync(
                sa => sa.Id == studentAssignmentDto.StudentAssignmentId,
                sa => sa.Comments,
                sa => sa.AttachedFiles,
                sa => sa.Assignment);

            if(studentAssignment?.CurrentMark is not null)
            {
                throw new InvalidOperationException("Assignment already graded");
            }

            await DeleteExistingFiles(studentAssignment);
            await AddNewFiles(studentAssignmentDto, studentAssignment);

            await _unitOfWork.StudentAssignmentRepository.UpdateAsync(studentAssignment.Id, NativeMapper.ToStudentAssignment(studentAssignmentDto));
            await _unitOfWork.CommitAsync();

            var commentSenderIds = studentAssignment.Comments.Select(c => c.CourseUserId).Distinct();
            var userDict = await GetCourseUserDictAsync(commentSenderIds.Append(studentAssignment.StudentId).Distinct());
            var imageLinkDict = await GetImageLinkDictAsync(userDict.Values);

            var commentDtos = GetAllComments(studentAssignment, userDict, imageLinkDict);
            var fileDtos = NativeMapper.FromAttachedFile(studentAssignment.AttachedFiles);

            return NativeMapper.ToStudentAssignmentInfo(studentAssignment, commentDtos, fileDtos);
        }

        private async Task AddNewFiles(UpdateStudentAssignmentDto studentAssignmentDto, StudentAssignment? studentAssignment)
        {
            if(studentAssignmentDto.AssignmentFiles is not null)
            {
                foreach(var file in studentAssignmentDto.AssignmentFiles)
                {
                    string? fileLink = await _awsFileService.AddFileAsync(file);
                    if(fileLink is not null && studentAssignment is not null)
                    {
                        await _unitOfWork.AttachedFileRepository.AddAsync(NativeMapper.ToAttachedFile(fileLink, studentAssignment.Id));
                    }
                }
            }
        }

        private async Task DeleteExistingFiles(StudentAssignment? studentAssignment)
        {
            if(studentAssignment?.AttachedFiles?.Any() == true)
            {
                foreach(var file in studentAssignment.AttachedFiles)
                {
                    if(await _awsFileService.DeleteFileAsync(file.AttachedFileName))
                    {
                        await _unitOfWork.AttachedFileRepository.DeleteAsync(file.Id);
                    }
                }
            }
        }

        // передавати назву з клієнта, щоб не звертатися зайвий раз до БД
        public async Task<string?> GetFileLinkAsync(int fileId)
        {
            var file = await _unitOfWork.AttachedFileRepository.GetByIdAsync(fileId);
            if(file?.AttachedFileName is null)
            {
                return null;
            }
            return await _awsFileService.GetFileLink(file.AttachedFileName);
        }

        public async Task<StudentAssignmentInfoDto?> GetStudentAssignmentAsync(string userId, int assignmentId)
        {
            var assignment = await _unitOfWork.AssignmentRepository.GetByIdAsync(assignmentId);

            var courseUser = await _unitOfWork.CourseuserRepository.FindAnyAsync(
                cu => cu.UserId == userId && cu.CourseId == assignment.CourseId)
                ?? throw new UnauthorizedAccessException("You do not have permission to get this info");

            var studentAssignment = await _unitOfWork.StudentAssignmentRepository.FindAnyAsync(
                sa => sa.AssignmentId == assignmentId && sa.StudentId == courseUser.Id,
                sa => sa.Comments,
                sa => sa.AttachedFiles);

            var userIds = studentAssignment.Comments.Select(c => c.CourseUserId).Append(courseUser.Id).Distinct().ToList();

            var userDict = await GetCourseUserDictAsync(userIds);
            var imageLinkDict = await GetImageLinkDictAsync(userDict.Values);

            List<CommentInfoDto> commentDtos = GetAllComments(studentAssignment, userDict, imageLinkDict);
            var fileDtos = NativeMapper.FromAttachedFile(studentAssignment.AttachedFiles);
            return NativeMapper.ToStudentAssignmentInfo(studentAssignment, commentDtos, fileDtos);
        }
        
        public async Task<IEnumerable<StudentAssignmentInfoDto>?> GetAllStudentAssignmentAsync(string userId, int assignmentId)
        {
            var assignment = await _unitOfWork.AssignmentRepository.GetByIdAsync(assignmentId);

            var teacher = await _unitOfWork.CourseuserRepository.FindAnyAsync(
                cu => cu.UserId == userId && cu.CourseId == assignment.CourseId && cu.Role != Roles.Student)
                ?? throw new UnauthorizedAccessException("You do not have permission to view this assignment");

            var studentAssignments = await _unitOfWork.StudentAssignmentRepository.FindAllAsync(
                sa => sa.AssignmentId == assignmentId,
                sa => sa.Comments,
                sa => sa.AttachedFiles);

            var studentIds = studentAssignments.Select(sa => sa.StudentId);
            var commentSenderIds = studentAssignments.SelectMany(sa => sa.Comments.Select(c => c.CourseUserId));
            var allUserIds = studentIds.Concat(commentSenderIds).Distinct().ToList();

            var userDict = await GetCourseUserDictAsync(allUserIds);
            var imageLinkDict = await GetImageLinkDictAsync(userDict.Values);

            var result = new List<StudentAssignmentInfoDto>();
            foreach(var studentAssignment in studentAssignments)
            {
                if(!userDict.TryGetValue(studentAssignment.StudentId, out var student))
                {
                    continue;
                }

                List<CommentInfoDto> commentDtos = GetAllComments(studentAssignment, userDict, imageLinkDict);
                var fileDtos = NativeMapper.FromAttachedFile(studentAssignment.AttachedFiles);
                result.Add(NativeMapper.ToStudentAssignmentInfo(studentAssignment, commentDtos, fileDtos));
            }
            return result;
        }

        private List<CommentInfoDto> GetAllComments(StudentAssignment studentAssignment, Dictionary<int, CourseUser> userDict,  Dictionary<string, string?> imageLinkDict)
        {
            return studentAssignment.Comments
                .Where(c => userDict.ContainsKey(c.CourseUserId))
                .Select(c =>
                {
                    var sender = userDict[c.CourseUserId];
                    var img = GetImageLinkOrDefault(sender.User.UserImage, imageLinkDict);
                    return NativeMapper.ToCommentInfo(c, sender.User, img);
                }).ToList();
        }

        private async Task<Dictionary<int, CourseUser>> GetCourseUserDictAsync(IEnumerable<int> courseUserIds)
        {
            var courseUsers = await _unitOfWork.CourseuserRepository.FindAllAsync(
                cu => courseUserIds.Contains(cu.Id),
                cu => cu.User);
            return courseUsers.Where(cu => cu.User is not null).ToDictionary(cu => cu.Id);
        }

        private async Task<Dictionary<string, string?>> GetImageLinkDictAsync(IEnumerable<CourseUser> courseUsers)
        {
            var uniqueImages = courseUsers.Select(cu => cu.User.UserImage)
                .Where(img => !string.IsNullOrEmpty(img)).Distinct().ToList();

            var imageLinkDict = new Dictionary<string, string?>();
            foreach(var image in uniqueImages)
            {
                imageLinkDict[image!] = await _awsFileService.GetFileLink(image!);
            }
            return imageLinkDict;
        }

        private string? GetImageLinkOrDefault(string? imageName, Dictionary<string, string?> imageLinkDict)
        {
            if(string.IsNullOrEmpty(imageName))
            {
                return null;
            }
            return imageLinkDict.TryGetValue(imageName, out var link) ? link : null;
        }
    }
}
