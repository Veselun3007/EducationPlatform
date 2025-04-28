using Chat.Core.DTO.Request;
using Chat.Core.DTO.Response;
using Chat.Core.Helpers;
using Chat.Core.Interfaces;
using Chat.Core.Interfaces.Infrastructure;
using Chat.Core.Mapping;
using Chat.Domain.Entities;

namespace Chat.Core.Services
{
    internal class MediaService : IMediaSevice<MessageMediaOutDTO, MessageMediaDTO>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsFileService _fileService;

        public MediaService(IUnitOfWork unitOfWork, IAwsFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task DeleteFileAsync(int id)
        {

            var mediaFile = await _unitOfWork.MessageMediaRepository.GetByIdAsync(id);
            if(mediaFile is not null && mediaFile.MediaLink is not null)
            {
                await _fileService.DeleteFileAsync(mediaFile.MediaLink);
            }

            await _unitOfWork.MessageMediaRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<MessageMediaOutDTO?> AddFileAsync(MessageMediaDTO file, int id)
        {
            var media = FileConvertHelper.ConvertBase64ToIFormFile(file.FileBase64!, file.FileName!);
            var fileLink = await _fileService.AddFileAsync(media);
            MessageMedia mediaFile = NativeMapper.ToMessageMedia(id, fileLink);
            var addedFile = await _unitOfWork.MessageMediaRepository.AddAsync(mediaFile);
            await _unitOfWork.CommitAsync();

            return NativeMapper.FromMessageMedia(addedFile);
        }

        public async Task<string?> GetMediaByIdAsync(int id)
        {
            var media = await _unitOfWork.MessageMediaRepository.GetByIdAsync(id);
            return await _fileService.GetFileLink(media.MediaLink);
        }
    }
}
