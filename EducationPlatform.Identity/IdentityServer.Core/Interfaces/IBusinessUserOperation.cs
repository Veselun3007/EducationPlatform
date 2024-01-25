﻿using IdentityServer.Domain.Entities;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Core.Interfaces
{
    public interface IBusinessUserOperation
    {
        Task<User> AddAsync(UserDTO entity);

        Task<User> UpdateAsync(UserDTO entity, int id);

        Task<bool> DeleteAsync(int id);
    }
}