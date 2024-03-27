using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Infrastructure.Interfaces;

namespace EPChat.Core.Services
{
    public class MessageQueryService(IUnitOfWork unitOfWork) : IMessageQuery<Message>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Message>> GetFirstPackMessageAsync(int chatId)
        {
            var messages = await _unitOfWork
                .MessageRepository.GetAsync(m => m.ChatId == chatId);
            
            var query = messages
                .OrderByDescending(m => m.CreatedIn)
                .Take(100)
                .ToList();

            return query;
        }

        public async Task<IEnumerable<Message>> GetNextPackMessageAsync(int oldestMessageId, int chatId)
        {
            var message = await _unitOfWork.MessageRepository
                .GetAsync(m => m.Id < oldestMessageId);

            var query = message.Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.Id)
                .Take(100)
                .ToList();

            return query;
        }
    }
}
