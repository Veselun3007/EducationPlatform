using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Infrastructure.Interfaces;

namespace EPChat.Core.Services
{
    public class MessageQueryService(IUnitOfWork unitOfWork) : IMessageQuery
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IEnumerable<Message> GetFirstPackMessage(int chatId)
        {
            return _unitOfWork.MessageRepository
                .Get().Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.Id)
                .Take(100);
        }

        public IEnumerable<Message> GetNextPackMessage(int oldestMessageId, int chatId)
        {
            return _unitOfWork.MessageRepository
                .Get(m => m.Id < oldestMessageId)
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.Id)
                .Take(100);
        }
    }
}
