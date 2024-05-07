using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.ErrorModels;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using EPChat.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace EPChat.Web.Hubs
{
    public class ChatHub
        (IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, 
            MessageMediaOutDTO, Error> messageOperation,
        IQuery<MessageOutDTO, CourseUser> messageQuery) : Hub
    {
        private readonly IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, 
            MessageMediaOutDTO, Error> _messageOperation = messageOperation;
        private readonly IQuery<MessageOutDTO, CourseUser> _messageQuery = messageQuery;

        /*public async Task AddUsersToGroup(int courseId)
        {
            var chatMembers = await _messageQuery
                .GetMembersAsync(cm => cm.CourseId == courseId);

            foreach (var chatMember in chatMembers)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatMember.CourseId.ToString());
            }
        }*/



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

        public async Task GetFirstPackMessage(int courseId)
        {
            await Clients.Caller.SendAsync("ReceiveMessages",
                await _messageQuery.GetFirstPackMessageAsync(courseId));
        }

        public async Task GetNextPackMessage(int courseId, int oldestMessageId)
        {
            var messages = await _messageQuery
                .GetNextPackMessageAsync(courseId, oldestMessageId);

            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }

        public async Task DeleteMessage(int courseId, int messageId,
    DeleteOptionsEnum deleteOptions)
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

        /* public async Task DeleteMessage(int courseId, int messageId,
             DeleteOptionsEnum deleteOptions)
         {
             var deletedMessage = await _messageOperation
                 .DeleteAsync(messageId, deleteOptions);

             await Clients.Group(courseId.ToString())
                 .SendAsync("BroadCastDeleteMessage",
                     Context.ConnectionId, deletedMessage);
         }

         public async Task DeleteMessageRange(int courseId,
             List<int> entitiesToDelete,
             DeleteOptionsEnum deleteOptions)
         {
             var deletedMessage = _messageOperation
                 .RemoveRangeAsync(entitiesToDelete, deleteOptions);

             await Clients.Group(courseId.ToString())
                 .SendAsync("BroadCastDeleteMessage",
                     Context.ConnectionId, deletedMessage);
         }*/
    }
}
