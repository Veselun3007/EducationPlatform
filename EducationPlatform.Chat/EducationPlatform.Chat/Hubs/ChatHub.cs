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
            await Clients.All.SendAsync("ReceiveMessages", await _messageOperation.AddAsync(message));
        }

        public async Task GetFirstPackMessage(int courseId)
        {
            var messages = await _messageQuery.GetFirstPackMessageAsync(courseId);
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }

        public async Task GetNextPackMessage(int courseId, int oldestMessageId)
        {
            var messages = await _messageQuery.GetNextPackMessageAsync(courseId, oldestMessageId);
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }

        public async Task DeleteMessage(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = await _messageOperation.DeleteAsync(messageId, deleteOptions);
            await Clients.Caller.SendAsync("BroadCastDeleteMessage", Context.ConnectionId, deletedMessage);
        }

        public async Task DeleteMessageRange(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = _messageOperation.RemoveRangeAsync(entitiesToDelete, deleteOptions);
            await Clients.Caller.SendAsync("BroadCastDeleteMessage", Context.ConnectionId, deletedMessage);
        }
    }
}
