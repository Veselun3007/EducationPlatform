using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPChat.Core.Services
{
    public class QueryService(IUnitOfWork unitOfWork) : IQuery<MessageOutDTO, CourseUser>
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

        public async Task<IEnumerable<MessageOutDTO>> GetNextPackMessageAsync(int oldestMessageId, int chatId)
        {
            return await _unitOfWork.MessageRepository
                .GetQueryable()
                .Where(m => m.Id < oldestMessageId && m.CourseId == chatId)
                .Include(m => m.AttachedMedias)
                .OrderByDescending(m => m.Id)
                .Take(100)
                .Select(m => MessageOutDTO.FromMessage(m)).ToListAsync();
        }

        public async Task<IEnumerable<CourseUser>> GetMembersAsync(Expression<Func<CourseUser, bool>> filter)
        {
            return await _unitOfWork.MemberRepository.GetAsync(filter);
        }
    }
}
