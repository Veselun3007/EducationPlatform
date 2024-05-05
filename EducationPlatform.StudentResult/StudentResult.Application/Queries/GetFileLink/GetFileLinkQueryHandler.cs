using StudentResult.Application.Abstractions;
using StudentResult.Domain.Entities;
using StudentResult.Application.Abstractions.Errors;
using StudentResult.Infrastructure.Interfaces;
using StudentResult.Infrastructure.AWS;

namespace StudentResult.Application.Queries.GetFileLink {
    public class GetFileLinkQueryHandler : IQueryHandler<GetFileLinkQuery, string> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        public GetFileLinkQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        public async Task<Result<string>> Handle(GetFileLinkQuery request, CancellationToken cancellationToken) {
            AttachedFile file = await _unitOfWork.GetRepository<AttachedFile>().FirstOrDefaultAsync(af => af.AttachedFileId == request.FileId);          
            if (file == null) return Errors.Global.Error404();
            StudentAssignment studentAssignment = await _unitOfWork.GetRepository<StudentAssignment>().FirstOrDefaultAsync(sa => sa.StudentassignmentId == file.StudentassignmentId);
            if (studentAssignment == null) return Errors.Global.Error404();
            CourseUser courseUser = await _unitOfWork.GetRepository<CourseUser>().FirstOrDefaultAsync(cu => cu.UserId == request.UserId && cu.CourseId == studentAssignment.Assignment.CourseId);
            if (courseUser == null) return Errors.Global.Error403();

            string FileLink;
            if (file.AttachedFileName != null) {
                try {
                    FileLink = await _s3.GetObjectTemporaryUrlAsync(file.AttachedFileName);
                }
                catch {
                    return Errors.Global.Error404();
                }
            }
            else {
                return Errors.Global.Error404();
            }

            return FileLink;
        }
    }
}
