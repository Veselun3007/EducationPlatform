using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPChat.Core.Services
{
    public class QueryService(IUnitOfWork unitOfWork) : IQuery<MessageOutDTO, ChatMember>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<MessageOutDTO>> GetFirstPackMessageAsync(int chatId)
        {
            var messages = _unitOfWork.MessageRepository
                .GetQueryable()
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.CreatedIn)
                .Include(m => m.AttachedMedias)
                .Take(100);

            return await messages.Select(m => MessageOutDTO.FromMessage(m)).ToListAsync();
        }

        public async Task<IEnumerable<MessageOutDTO>> GetNextPackMessageAsync(int oldestMessageId, int chatId)
        {
            var messages = _unitOfWork.MessageRepository
                .GetQueryable()
                .Where(m => m.Id < oldestMessageId && m.ChatId == chatId)
                .Include(m => m.AttachedMedias)
                .OrderByDescending(m => m.Id)
                .Take(100);

            return await messages.Select(m => MessageOutDTO.FromMessage(m)).ToListAsync();
        }

        public async Task<IEnumerable<ChatMember>> GetMembersAsync(Expression<Func<ChatMember, bool>> filter)
        {
            return await _unitOfWork.MemberRepository.GetAsync(filter);
        }
    }
}
