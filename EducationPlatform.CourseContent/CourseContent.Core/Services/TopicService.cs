using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Models.ErrorModels;
using CourseContent.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;

namespace CourseContent.Core.Services
{
    public class TopicService(IUnitOfWork unitOfWork) :
        IBaseOperation<TopicOutDTO, Error, TopicDTO, TopicUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<TopicOutDTO, Error>> CreateAsync(TopicDTO entity)
        {
            var topic = TopicDTO.FromTopicDto(entity);

            await _unitOfWork.TopicRepository.AddAsync(topic);
            await _unitOfWork.CompleteAsync();

            return Result.Success<TopicOutDTO, Error>(TopicOutDTO.FromTopic(topic));
        }

        public async Task<Result<string, Error>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.TopicRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<TopicOutDTO, Error>> UpdateAsync(TopicUpdateDTO entity, int id)
        {
            try
            {
                var topic = TopicUpdateDTO.FromTopicUpdateDto(entity);          
                await _unitOfWork.TopicRepository.UpdateAsync(id, topic);
                await _unitOfWork.CompleteAsync();

                return Result.Success<TopicOutDTO, Error>(TopicOutDTO.FromTopic(topic));
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<TopicOutDTO, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<TopicOutDTO, Error>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.TopicRepository.GetByIdAsync(id);
            if (entity is null)
            {
                return Result.Failure<TopicOutDTO, Error>(Errors.General.NotFound());
            }
            return Result.Success<TopicOutDTO, Error>(TopicOutDTO.FromTopic(entity));
        }

        public async Task<IEnumerable<TopicOutDTO>> GetAllByCourseAsync(int courseId)
        {
            var topics = await _unitOfWork.TopicRepository
                .GetAllByCourseAsync(m => m.CourseId == courseId);
            return topics.Select(TopicOutDTO.FromTopic).ToList();
        }
    }
}
