using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using EPChat.Infrastructure.Interfaces;

namespace EPChat.Core.Services
{
    public class OperationServices(IUnitOfWork unitOfWork) : IOperation<Message, MessageMedia>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Message> AddAsync(Message message)
        {
            var entity = await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.ComplectAsync();
            return entity;
        }

        public async Task<Message?> EditAsync(int id, Message message)
        {
            message.IsEdit = true;
            var entity = await _unitOfWork.MessageRepository.UpdateAsync(id, message);
            await _unitOfWork.ComplectAsync();
            return entity;
        }

        public async Task<MessageMedia?> GetMediaByIdAsync(int id)
        {
            return await _unitOfWork.MessageMediaRepository.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageId);

            return message switch
            {
                null => false,
                _ when deleteOptions is DeleteOptionsEnum.DeleteForEveryone => await DeleteForEveryone(message),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForMe => await DeleteForMe(message),
                _ => false
            };
        }

        private async Task<bool> DeleteForEveryone(Message message)
        {
            try
            {
                await _unitOfWork.MessageRepository.DeleteAsync(message.Id);
                await _unitOfWork.ComplectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> DeleteForMe(Message message)
        {
            try
            {
                message.IsDeleted = true;
                await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                await _unitOfWork.ComplectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveRangeAsync(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions)
        {
            return deleteOptions switch
            {
                _ when deleteOptions is DeleteOptionsEnum.DeleteForEveryone => await DeleteForEveryoneRange(entitiesToDelete),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForMe => await DeleteForMeRange(entitiesToDelete),
                _ => false
            };
        }

        private async Task<bool> DeleteForEveryoneRange(List<int> entitiesToDelete)
        {
            try
            {
                await _unitOfWork.MessageRepository.RemoveRangeAsync(entitiesToDelete);
                await _unitOfWork.ComplectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> DeleteForMeRange(List<int> entitiesToDelete)
        {
            try
            {
                foreach (var id in entitiesToDelete)
                {
                    var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);

                    if (message is not null)
                    {
                        message.IsDeleted = true;
                        await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                    }
                }
                await _unitOfWork.ComplectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
