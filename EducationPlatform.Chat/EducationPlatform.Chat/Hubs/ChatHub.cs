using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.ErrorModels;
using EPChat.Core.Models.HelperModel;
using EPChat.Domain.Enums;
using EPChat.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace EPChat.Web.Hubs
{
    public class ChatHub
        (IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, MediaMessage, Error> messageOperation,
        IQuery<MessageOutDTO> messageQuery) : Hub
    {
        private readonly IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, MediaMessage, Error> _messageOperation = messageOperation;
        private readonly IQuery<MessageOutDTO> _messageQuery = messageQuery;

        public async Task JoinRoom(int courseId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, courseId.ToString());
        }

        public async Task SendMessage(MessageDTO message)
        {
            var result = await _messageOperation.AddAsync(message);

            if (result.IsSuccess)
            {
                await Clients.Group(message.CourseId.ToString()).SendAsync("ReceiveMessage", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", MessageWrapper.Error(result.Error));
            }
        }

        public async Task DeleteMessage(int courseId, int messageId, DeleteOptionsEnum deleteOptions)
        {
            var result = await _messageOperation.DeleteAsync(messageId, deleteOptions);
            if (result.IsSuccess)
            {
                await Clients.Group(courseId.ToString()).SendAsync("BroadCastDeleteMessage", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("BroadCastDeleteMessage", MessageWrapper.Error(result.Error));
            }
        }

        public async Task EditMessage(int courseId, MessageUpdateDTO message)
        {
            var result = await _messageOperation.EditAsync(message);
            if (result.IsSuccess)
            {
                await Clients.Group(courseId.ToString()).SendAsync("EditMessage", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("EditMessage", MessageWrapper.Error(result.Error));
            }
        }

        public async Task DeleteMessageMedia(int courseId, int messageMediaId)
        {
            var result = await _messageOperation.DeleteFileAsync(messageMediaId);
            if (result.IsSuccess)
            {
                await Clients.Group(courseId.ToString()).SendAsync("DeleteMedia", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("DeleteMedia", MessageWrapper.Error(result.Error));
            }
        }

        public async Task AddMessageMedia(int courseId, MediaMessage file, int messageId)
        {
            var result = await _messageOperation.AddFileAsync(file, messageId);
            if (result.IsSuccess)
            {
                await Clients.Group(courseId.ToString()).SendAsync("AddMedia", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("AddMedia", MessageWrapper.Error(result.Error));
            }
        }

        public async Task GetFileById(int messageMediaId)
        {
            var result = await _messageOperation.GetMediaByIdAsync(messageMediaId);
            if (result.IsSuccess)
            {
                await Clients.Caller.SendAsync("GetFile", result.Value);
            }
            else
            {
                await Clients.Caller.SendAsync("GetFile", MessageWrapper.Error(result.Error));
            }
        }

        public async Task GetFirstPackMessage(int courseId)
        {
            await Clients.Caller.SendAsync("ReceiveMessages",
                await _messageQuery.GetFirstPackMessageAsync(courseId));
        }

        public async Task GetNextPackMessage(int courseId, int oldestMessageId)
        {
            await Clients.Caller.SendAsync("ReceiveMessages",
                await _messageQuery.GetNextPackMessageAsync(courseId, oldestMessageId));
        }
    }
}
