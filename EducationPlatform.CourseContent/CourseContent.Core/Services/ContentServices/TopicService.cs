using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Mappings;

namespace CourseContent.Core.Services.ContentServices
{
    public class TopicService : IContentServices<TopicDTO, TopicOutDTO, TopicUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TopicOutDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.TopicRepository.GetByIdAsync(id);
            return NativeMapper.FromTopic(entity);
        }

        public async Task<IEnumerable<TopicOutDTO>?> GetAllByCourseAsync(int courseId)
        {
            var topics = await _unitOfWork.TopicRepository
                .FindAllByAsync(m => m.CourseId == courseId);
            return topics.Select(NativeMapper.FromTopic).ToList();
        }

        public async Task<TopicOutDTO?> CreateAsync(TopicDTO entity)
        {
            var topic = NativeMapper.ToTopic(entity);
            await _unitOfWork.TopicRepository.AddAsync(topic);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromTopic(topic);
        }

        public async Task<TopicOutDTO?> UpdateAsync(TopicUpdateDTO entity, int id)
        {
            var topic = NativeMapper.ToTopic(entity);
            await _unitOfWork.TopicRepository.UpdateAsync(id, topic);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromTopic(topic);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.TopicRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(List<int> entities)
        {
            await _unitOfWork.TopicRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }
    }
}
