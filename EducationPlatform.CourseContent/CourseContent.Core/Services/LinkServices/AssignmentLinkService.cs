using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;

namespace CourseContent.Core.Services.LinkServices
{
    public class AssignmentLinkService : ILinkServices<AssignmentlinkOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignmentLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignmentlinkOutDTO> AddLinkAsync(string link, int id)
        {
            Assignmentlink assignmentLink = NativeMapper.ToAssignmentLink(link, id);
            var addedLink = await _unitOfWork.AssignmentlinkRepository.AddAsync(assignmentLink);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromAssignmentLink(addedLink);
        }

        public async Task DeleteLinkAsync(int linkId)
        {
            await _unitOfWork.AssignmentlinkRepository.DeleteAsync(linkId);
            await _unitOfWork.CommitAsync();
        }
    }
}
