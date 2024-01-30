using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using EPChat.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Core.Services
{
    public class MessageOperationServices(IUnitOfWork unitOfWork) : IMessageOperation
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public void Add(Message message)
        {
            _unitOfWork.MessageRepository.Add(message);
            CompleteChange();
        }

        public void Edit(Message message)
        {
            message.IsDeleted = true;
            _unitOfWork.MessageRepository.Update(message);
            CompleteChange();
        }

        public async Task<bool> DeleteMessage(int messageId,
            DeleteOptionsEnum deleteOptions)
        {
            var message = await _unitOfWork.MessageRepository
                .Get(m => m.Id == messageId)
                .FirstOrDefaultAsync();

            return message switch
            {
                null => false,
                _ when deleteOptions is DeleteOptionsEnum.DeleteForEveryone => DeleteForEveryone(messageId),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForMe => DeleteForMe(message),
                _ => false
            };
        }

        public bool DeleteRange(IEnumerable<Message> entitiesToDelete, DeleteOptionsEnum deleteOptions)
        {
            return deleteOptions switch
            {
                _ when deleteOptions is DeleteOptionsEnum.DeleteForEveryone => DeleteForEveryoneRange(entitiesToDelete),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForMe => DeleteForMeRange(entitiesToDelete),
                _ => false
            };
        }

        public async Task<MessageMedia?> GetByIdAsync(int id)
        {
            return await _unitOfWork.MessageMediaRepository.GetById(id);
        }

        #region *** Additional Methods ***
        private bool DeleteForEveryone(int messageId)
        {
            _unitOfWork.MessageRepository.Delete(messageId);
            CompleteChange();
            return true;
        }

        private bool DeleteForMe(Message message)
        {
            message.IsDeleted = true;
            _unitOfWork.MessageRepository.Update(message);
            CompleteChange();
            return true;
        }

        private bool DeleteForEveryoneRange(IEnumerable<Message> entitiesToDelete)
        {
            _unitOfWork.MessageRepository.DeleteRange(entitiesToDelete);
            CompleteChange();
            return true;
        }

        private bool DeleteForMeRange(IEnumerable<Message> entitiesToDelete)
        {
            foreach (var message in entitiesToDelete)
            {
                message.IsDeleted = true;
                _unitOfWork.MessageRepository.Update(message);
            }
            CompleteChange();
            return true;
        }

        private void CompleteChange()
        {
            _unitOfWork.ComplectAsync();
            _unitOfWork.Dispose();
        }
        #endregion
    }
}
