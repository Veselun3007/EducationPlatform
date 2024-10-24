﻿using EPChat.Domain.Entities;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Message> MessageRepository { get; }

        IMinRepository<MessageMedia> MessageMediaRepository { get; }

        Task<int> CommitAsync();
    }
}
