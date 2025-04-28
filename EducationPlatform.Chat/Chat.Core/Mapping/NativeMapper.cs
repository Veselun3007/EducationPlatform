using Chat.Core.DTO.Request;
using Chat.Core.DTO.Response;
using Chat.Domain.Entities;

namespace Chat.Core.Mapping
{
    internal static class NativeMapper
    {

        public static MessageOutDTO FromMessage(Message message)
        {
            return new MessageOutDTO
            {
                Id = message.Id,
                CourseId = message.CourseId,
                MessageText = message.MessageText,
                CreatorId = message.CreatorId,
                CreatedIn = message.CreatedIn,
                IsEdit = message.IsEdit,
                EditedIn = message.EditedIn,
                AttachedFiles = message.AttachedMedias.Select(m => FromMessageMedia(m)).ToList(),
            };
        }

        public static Message ToMessage(MessageDTO messageDTO)
        {
            return new Message
            {
                CourseId = messageDTO.CourseId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
                CreatedIn = messageDTO.CreatedIn,
            };
        }

        public static Message ToMessage(MessageUpdateDTO messageDTO)
        {
            return new Message
            {
                Id = messageDTO.Id,
                CourseId = messageDTO.CourseId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
                CreatedIn = messageDTO.CreatedIn
            };
        }

        public static MessageMediaOutDTO FromMessageMedia(MessageMedia media)
        {
            return new MessageMediaOutDTO
            {
                Id = media.Id,
                MessageId = media.MessageId,
                MediaLink = media.MediaLink
            };
        }

        public static MessageMedia ToMessageMedia(int id, string fileLink)
        {
            return new MessageMedia
            {
                MediaLink = fileLink,
                MessageId = id
            };
        }
    }
}