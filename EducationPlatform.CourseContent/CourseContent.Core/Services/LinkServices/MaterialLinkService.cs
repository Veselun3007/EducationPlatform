using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;

namespace CourseContent.Core.Services.LinkServices
{
    public class MaterialLinkService : ILinkServices<MateriallinkOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MateriallinkOutDTO> AddLinkAsync(string link, int id)
        {
            Materiallink MaterialLink = MappingHelpers.CreateMaterialLink(link, id);
            var addedLink = await _unitOfWork.MateriallinkRepository.AddAsync(MaterialLink);
            await _unitOfWork.CommitAsync();

            return MateriallinkOutDTO.FromMaterialLink(addedLink);
        }

        public async Task DeleteLinkAsync(int linkId)
        {
            await _unitOfWork.MateriallinkRepository.DeleteAsync(linkId);
            await _unitOfWork.CommitAsync();
        }
    }
}
