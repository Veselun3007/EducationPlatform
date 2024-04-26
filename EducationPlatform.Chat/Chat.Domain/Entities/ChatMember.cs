﻿using EPChat.Domain.Interfaces;

namespace EPChat.Domain.Entities
{
    public class ChatMember : IEntity
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int UserId { get; set; }

        public virtual Chat? Chat { get; set; }

        public virtual ChatsUser? ChatsUser { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = [];

    }
}
