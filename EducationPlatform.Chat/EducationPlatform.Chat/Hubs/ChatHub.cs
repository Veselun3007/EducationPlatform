using EPChat.Core.Interfaces;
using EPChat.Domain.Entities;
using EPChat.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace EPChat.Web.Hubs
{
    public class ChatHub(IMessageOperation messageOperation, 
        IMessageQuery messageQuery) : Hub
    {

        private readonly IMessageOperation _messageOperation = messageOperation;
        private readonly IMessageQuery _messageQuery = messageQuery;

        public async Task SendMessage(Message message)
        {          
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        /*public async Task GetFirstPackMessage()
        {
            var messages = _messageQuery.GetFirstPackMessage();
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }

        public async Task GetNextPackMessage(int oldestMessageId)
        {
            var messages = _messageQuery.GetNextPackMessage(oldestMessageId);
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }*/

        public async Task DeleteMessage(int messageId, DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = await _messageOperation.DeleteMessage(messageId, deleteOptions);
            await Clients.All.SendAsync("BroadCastDeleteMessage", Context.ConnectionId, deletedMessage);
        }

        public async Task DeleteMessageRange(IEnumerable<Message> entitiesToDelete, DeleteOptionsEnum deleteOptions)
        {
            var deletedMessage = _messageOperation.DeleteRange(entitiesToDelete, deleteOptions);
            await Clients.All.SendAsync("BroadCastDeleteMessage", Context.ConnectionId, deletedMessage);
        }
    }
}
