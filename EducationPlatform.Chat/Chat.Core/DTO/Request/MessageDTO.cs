﻿using EPChat.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EPChat.Core.DTO.Request
{
    public class MessageDTO
    {
        public int ChatId { get; set; }

        public int? ReplyToMessageId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.UtcNow;

        public List<IFormFile>? AttachedFiles { get; set; }

        public static Message FromMessageDTO(MessageDTO messageDTO)
        {
            return new Message
            {
                ChatId = messageDTO.ChatId,
                ReplyToMessageId = messageDTO.ReplyToMessageId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
                CreatedIn = messageDTO.CreatedIn,
            };
        }
    }
}
