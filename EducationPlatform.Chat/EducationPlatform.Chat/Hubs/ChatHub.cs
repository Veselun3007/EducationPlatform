using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace EPChat.Web.Hubs
{
    public class ChatHub(IOperation<Message, MessageMedia> messageOperation, 
        IMessageQuery<Message> messageQuery) : Hub
    {
        private readonly IOperation<Message, MessageMedia> _messageOperation = messageOperation;
        private readonly IMessageQuery<Message> _messageQuery = messageQuery;

        public async Task SendMessage(Message message)
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
