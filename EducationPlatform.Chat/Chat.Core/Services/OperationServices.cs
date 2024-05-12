using CSharpFunctionalExtensions;
using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Helpers;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.ErrorModels;
using EPChat.Core.Models.HelperModel;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using EPChat.Infrastructure.Interfaces;

namespace EPChat.Core.Services
{
    public class OperationServices(IUnitOfWork unitOfWork, FileHelper fileHelper) :
        IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, MediaMessage, Error>
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

        private async Task AddFilesAsync(Message message, List<MediaMessage> attachedFiles)
        {
            foreach (var file in attachedFiles)
            {
                var mediaFile = FileConvertHelper.ConvertBase64ToIFormFile(file.FileBase64!, file.FileName!);
                var fileLink = await _fileHelper.AddFileAsync(mediaFile);
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

                if (updated is null)
                    return Result.Failure<MessageOutDTO?, Error>(Errors.General.NotFound());

                return Result.Success<MessageOutDTO?, Error>(MessageOutDTO.FromMessage(updated));
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<MessageOutDTO?, Error>(Errors.General.NotFound());
            }
        }


        public async Task<Result<string?, Error>> DeleteFileAsync(int id)
        {
            try
            {
                var mediaFile = await _unitOfWork.MessageMediaRepository.GetById(id);
                if (mediaFile is not null && mediaFile.MediaLink is not null)
                {
                    await _fileHelper.DeleteFileAsync(mediaFile.MediaLink);
                }

                await _unitOfWork.MessageMediaRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                return Result.Success<string?, Error>(id.ToString());
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string?, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<MessageMediaOutDTO?, Error>> AddFileAsync(MediaMessage file, int id)
        {
            var media = FileConvertHelper.ConvertBase64ToIFormFile(file.FileBase64!, file.FileName!);
            var fileLink = await _fileHelper.AddFileAsync(media);
            MessageMedia mediaFile = new()
            {
                MessageId = id,
                MediaLink = fileLink
            };
            var addedFile = await _unitOfWork.MessageMediaRepository.AddAsync(mediaFile);
            await _unitOfWork.CommitAsync();

            return Result.Success<MessageMediaOutDTO?, Error>(MessageMediaOutDTO.FromMessageMedia(addedFile));
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
            return Result.Success<string?, Error>(messageId.ToString());
        }
    }
}

