using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;
using CourseContent.Domain.Entities;

namespace CourseContent.Core.Services.LinkServices
{
    public class MaterialLinkService : ILinkService<MateriallinkOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MateriallinkOutDTO> AddLinkAsync(string link, int id)
        {
            Materiallink MaterialLink = NativeMapper.ToMaterialLink(link, id);
            var addedLink = await _unitOfWork.MateriallinkRepository.AddAsync(MaterialLink);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromMaterialLink(addedLink);
        }

        public async Task DeleteLinkAsync(int linkId)
        {
            await _unitOfWork.MateriallinkRepository.DeleteAsync(linkId);
            await _unitOfWork.CommitAsync();
        }
    }
}
