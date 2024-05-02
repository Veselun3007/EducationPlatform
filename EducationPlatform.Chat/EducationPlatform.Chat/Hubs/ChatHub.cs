using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.ErrorModels;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace EPChat.Web.Hubs
{
    public class ChatHub(IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, Error> messageOperation,
        IQuery<Message, ChatMember> messageQuery) : Hub
    {
        private readonly IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, Error> _messageOperation = messageOperation;
        private readonly IQuery<Message, ChatMember> _messageQuery = messageQuery;

        public async Task AddUsersToGroup(int chatId)
        {
            var chatMembers = await _messageQuery
                .GetMembersAsync(cm => cm.ChatId == chatId);

            foreach (var chatMember in chatMembers)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatMember.ChatId.ToString());
            }
        }

        public async Task JoinRoom(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }


        public async Task SendMessage(MessageDTO message)
        {
            await Clients.Group(message.ChatId.ToString())
                .SendAsync("ReceiveMessage",
                    await _messageOperation.AddAsync(message));
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

        public async Task DeleteMessage(int chatId, int messageId,
            DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = await _messageOperation
                .DeleteAsync(messageId, deleteOptions);

            await Clients.Group(chatId.ToString())
                .SendAsync("BroadCastDeleteMessage",
                    Context.ConnectionId, deletedMessage);
        }

        public async Task DeleteMessageRange(int chatId,
            List<int> entitiesToDelete,
            DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = _messageOperation
                .RemoveRangeAsync(entitiesToDelete, deleteOptions);

            await Clients.Group(chatId.ToString())
                .SendAsync("BroadCastDeleteMessage",
                    Context.ConnectionId, deletedMessage);
        }
    }
}
