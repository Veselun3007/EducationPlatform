using Chat.Core.DTO.Request;
using Chat.Core.DTO.Response;
using Chat.Core.Interfaces;
using Chat.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Chat.Web.Hubs
{
    [SignalRHub]
    public class ChatHub : Hub
    {
        private readonly IMessageService<MessageDTO, MessageUpdateDTO, MessageOutDTO> _messageService;
        private readonly IMediaSevice<MessageMediaOutDTO, MessageMediaDTO> _mediaService;

        public ChatHub(
            IMessageService<MessageDTO, MessageUpdateDTO, MessageOutDTO> messageService,
            IMediaSevice<MessageMediaOutDTO, MessageMediaDTO> mediaService)
        {
            _messageService = messageService;
            _mediaService = mediaService;
        }

        public async Task JoinRoom(int courseId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, courseId.ToString());
        }

        public async Task SendMessage(MessageDTO message)
        {
            var result = await _messageService.AddAsync(message);
            await Clients.Group(message.CourseId.ToString()).SendAsync("ReceiveMessage", result);
        }

        public async Task EditMessage(int courseId, MessageUpdateDTO message)
        {
            var result = await _messageService.EditAsync(message);
            await Clients.Group(courseId.ToString()).SendAsync("EditMedia", result);
        }

        public async Task DeleteMessage(int courseId, int messageId, DeleteOptionsEnum deleteOptions)
        {
            await _messageService.DeleteAsync(messageId, deleteOptions);
            await Clients.Group(courseId.ToString()).SendAsync("BroadCastDeleteMessage");
        }

        public async Task DeleteMessageMedia(int courseId, int messageMediaId)
        {
            await _mediaService.DeleteFileAsync(messageMediaId);
            await Clients.Group(courseId.ToString()).SendAsync("DeleteMedia");
        }

        public async Task AddMessageMedia(int courseId, MessageMediaDTO file, int messageId)
        {
            var result = await _mediaService.AddFileAsync(file, messageId);
            await Clients.Group(courseId.ToString()).SendAsync("AddMedia", result);
        }

        public async Task GetFileById(int messageMediaId)
        {
            var result = await _mediaService.GetMediaByIdAsync(messageMediaId);
            await Clients.Caller.SendAsync("GetFile", result);
        }

        public async Task GetFirstPackMessage(int courseId)
        {
            await Clients.Caller.SendAsync("ReceiveMessages",
                await _messageService.GetFirstPackMessageAsync(courseId));
        }

        public async Task GetNextPackMessage(int courseId, int oldestMessageId)
        {
            await Clients.Caller.SendAsync("ReceiveMessages",
                await _messageService.GetNextPackMessageAsync(courseId, oldestMessageId));
        }
    }
}
