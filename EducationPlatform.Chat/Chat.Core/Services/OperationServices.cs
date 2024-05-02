using CSharpFunctionalExtensions;
using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Helpers;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.ErrorModels;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using EPChat.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EPChat.Core.Services
{
    public class OperationServices(IUnitOfWork unitOfWork, FileHelper fileHelper) :
        IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, Error>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FileHelper _fileHelper = fileHelper;

        public async Task<Result<MessageOutDTO, Error>> AddAsync(MessageDTO messageDto)
        {
            var message = MessageDTO.FromMessageDTO(messageDto);
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.CommitAsync();
            if (messageDto.AttachedFiles is not null)
            {
                await AddFilesAsync(message, messageDto.AttachedFiles);
            }
            return Result.Success<MessageOutDTO, Error>(MessageOutDTO.FromMessage(message));
        }

        private async Task AddFilesAsync(Message message, List<IFormFile> attachedFiles)
        {
            foreach (var file in attachedFiles)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                var media = new MessageMedia
                {
                    MediaLink = fileLink,
                    MessageId = message.Id
                };
                await _unitOfWork.MessageMediaRepository.AddAsync(media);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<Result<MessageOutDTO?, Error>> EditAsync(MessageUpdateDTO entity)
        {
            try
            {
                var message = MessageUpdateDTO.FromMessageUpdateDTO(entity);

                message.IsEdit = true;
                message.EditedIn = DateTime.UtcNow;
                await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                await _unitOfWork.CommitAsync();

                var updated = await _unitOfWork.MessageRepository.GetById(message.Id, a => a.AttachedMedias);
                return Result.Success<MessageOutDTO?, Error>(MessageOutDTO.FromMessage(updated));
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<MessageOutDTO?, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string?, Error>> GetMediaByIdAsync(int id)
        {
            var media = await _unitOfWork.MessageMediaRepository.GetById(id);
            if (media is null || media.MediaLink is null)
            {
                return Result.Failure<string?, Error>(Errors.General.NotFound());
            }
            return Result.Success<string?, Error>(await _fileHelper.GetFileLink(media.MediaLink));
        }

        public async Task<Result<string?, Error>> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var message = await _unitOfWork.MessageRepository.GetById(messageId);

            if (message is null)
            {
                return Result.Failure<string?, Error>(Errors.General.NotFound());
            }

            switch (deleteOptions)
            {
                case DeleteOptionsEnum.DeleteForEveryone:
                    await _unitOfWork.MessageRepository.DeleteAsync(message.Id);
                    break;
                case DeleteOptionsEnum.DeleteForMe:
                    message.IsDeleted = true;
                    await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                    break;
                default:
                    return Result.Failure<string?, Error>(Errors.General.Unpredictable());
            }

            await _unitOfWork.CommitAsync();
            return Result.Success<string?, Error>("Deleted was successful");
        }

        public async Task<Result<string?, Error>> RemoveRangeAsync(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions)
        {
            if (entitiesToDelete is null)
            {
                return Result.Failure<string?, Error>(Errors.General.NotRecords());
            }
            switch (deleteOptions)
            {
                case DeleteOptionsEnum.DeleteForEveryone:
                    await DeleteForEveryoneRange(entitiesToDelete);
                    break;
                case DeleteOptionsEnum.DeleteForMe:
                    await DeleteForMeRange(entitiesToDelete);
                    break;
                default:
                    return Result.Failure<string?, Error>(Errors.General.Unpredictable());
            }

            await _unitOfWork.CommitAsync();
            return Result.Success<string?, Error>("Deleted successfully");
        }

        private async Task DeleteForEveryoneRange(List<int> entitiesToDelete)
        {
            await _unitOfWork.MessageRepository.RemoveRangeAsync(entitiesToDelete);
        }

        private async Task DeleteForMeRange(List<int> entitiesToDelete)
        {
            foreach (var id in entitiesToDelete)
            {
                var message = await _unitOfWork.MessageRepository.GetById(id);

                if (message is not null)
                {
                    message.IsDeleted = true;
                    await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                }
            }
        }

        /*public async Task<bool> RemoveRangeAsync(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions)
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
                await _unitOfWork.CommitAsync();
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
                    var message = await _unitOfWork.MessageRepository.GetById(id);

                    if (message is not null)
                    {
                        message.IsDeleted = true;
                        await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                    }
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                return false;
            }*/

        /*public async Task<Result<string?, Error>> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var message = await _unitOfWork.MessageRepository.GetById(messageId);

            return message switch
            {
                null => Result.Failure<string?, Error>(Errors.General.NotFound()),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForEveryone => await DeleteForEveryone(message),
                _ when deleteOptions is DeleteOptionsEnum.DeleteForMe => await DeleteForMe(message),
                _ => ""
            };
        }

        private async Task DeleteForEveryone(Message message)
        {

                await _unitOfWork.MessageRepository.DeleteAsync(message.Id);
                await _unitOfWork.CommitAsync();
        }

        private async Task<bool> DeleteForMe(Message message)
        {
            try
            {
                message.IsDeleted = true;
                await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }*/
    }
}

