﻿using EPChat.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EPChat.Core.DTO.Request
{
    public class MessageUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? ReplyToMessageId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; } = DateTime.UtcNow;

        public List<IFormFile>? AttachedFiles { get; set; }

        public static Message FromMessageUpdateDTO(MessageUpdateDTO messageDTO)
        {
            return new Message
            {
                Id = messageDTO.Id,
                CourseId = messageDTO.CourseId,
                ReplyToMessageId = messageDTO.ReplyToMessageId,
                MessageText = messageDTO.MessageText,
                CreatorId = messageDTO.CreatorId,
                CreatedIn = messageDTO.CreatedIn,
            };
        }
    }
}
