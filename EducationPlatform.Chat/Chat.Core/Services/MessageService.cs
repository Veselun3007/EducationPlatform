using Chat.Core.DTO.Request;
using Chat.Core.DTO.Response;
using Chat.Core.Helpers;
using Chat.Core.Interfaces;
using Chat.Core.Interfaces.Infrastructure;
using Chat.Core.Mapping;
using Chat.Domain.Entities;
using Chat.Domain.Enums;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService<MessageDTO, MessageUpdateDTO, MessageOutDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public MessageService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<MessageOutDTO> AddAsync(MessageDTO messageDto)
        {
            var message = NativeMapper.ToMessage(messageDto);
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.CommitAsync();

            if(messageDto.AttachedFiles is not null)
            {
                await AddFilesAsync(message, messageDto.AttachedFiles);
            }
            return NativeMapper.FromMessage(message);
        }

        private async Task AddFilesAsync(Message message, List<MessageMediaDTO> attachedFiles)
        {
            foreach(var file in attachedFiles)
            {
                var mediaFile = FileConvertHelper.ConvertBase64ToIFormFile(file.FileBase64!, file.FileName!);
                var fileLink = await _fileService.AddFileAsync(mediaFile);
                MessageMedia media = NativeMapper.ToMessageMedia(message.Id, fileLink);
                await _unitOfWork.MessageMediaRepository.AddAsync(media);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<MessageOutDTO?> EditAsync(MessageUpdateDTO entity)
        {

            var message = NativeMapper.ToMessage(entity);
            message.IsEdit = true;
            message.EditedIn = DateTime.UtcNow;

            await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
            await _unitOfWork.CommitAsync();
            var updated = await _unitOfWork.MessageRepository.GetByIdAsync(message.Id, a => a.AttachedMedias);

            //if(updated is null)
            //    return Result.Failure<MessageOutDTO?, Error>(Errors.General.NotFound());

            return NativeMapper.FromMessage(updated);

        }
        
        public async Task DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageId);

            if(message is not null)
            {
                switch(deleteOptions)
                {
                    case DeleteOptionsEnum.DeleteForEveryone:
                        await _unitOfWork.MessageRepository.DeleteAsync(message.Id);
                        break;
                    case DeleteOptionsEnum.DeleteForMe:
                        message.IsDeleted = true;
                        await _unitOfWork.MessageRepository.UpdateAsync(message.Id, message);
                        break;
                    default:
                        return;
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<MessageOutDTO>> GetFirstPackMessageAsync(int courseId)
        {
            var messages = await _unitOfWork.MessageRepository.GetEntitiesAsync(
                m => m.CourseId == courseId,
                query => query.OrderByDescending(m => m.Id), 
                100,
                m => m.AttachedMedias);

            return messages.Select(m => NativeMapper.FromMessage(m));
        }

        public async Task<IEnumerable<MessageOutDTO>> GetNextPackMessageAsync(int courseId, int oldestMessageId)
        {
            var messages = await _unitOfWork.MessageRepository.GetEntitiesAsync(
                m => m.Id < oldestMessageId && m.CourseId == courseId,
                query => query.OrderByDescending(m => m.Id),
                100,
                m => m.AttachedMedias);

            return messages.Select(m => NativeMapper.FromMessage(m));
        }
    }
}

