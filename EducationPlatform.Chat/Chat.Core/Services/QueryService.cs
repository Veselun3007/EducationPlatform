using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Core.Services
{
    public class QueryService(IUnitOfWork unitOfWork) : IQuery<MessageOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<MessageOutDTO>> GetFirstPackMessageAsync(int chatId)
        {
            return await _unitOfWork.MessageRepository
                .GetQueryable()
                .Where(m => m.CourseId == chatId)
                .OrderByDescending(m => m.CreatedIn)
                .Include(m => m.AttachedMedias)
                .Take(100)
                .Select(m => MessageOutDTO.FromMessage(m)).ToListAsync();
        }

        public async Task<IEnumerable<MessageOutDTO>> GetNextPackMessageAsync(int chatId, int oldestMessageId)
        {
            return await _unitOfWork.MessageRepository
                .GetQueryable()
                .Where(m => m.Id < oldestMessageId && m.CourseId == chatId)
                .Include(m => m.AttachedMedias)
                .OrderByDescending(m => m.Id)
                .Take(100)
                .Select(m => MessageOutDTO.FromMessage(m)).ToListAsync();
        }
    }
}
