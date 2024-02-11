﻿using IdentityServer.Core.Interfaces;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Services
{
    public class UserOperation(IUnitOfWork unitOfWork,
        UserService userSevice) : IBusinessUserOperation
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserService _userSevice = userSevice;

        public async Task<User> AddAsync(UserDTO entity)
        {
            var userEntity = await _userSevice.ToUserEntity(entity);

            await _unitOfWork.UserRepository.AddAsync(userEntity);
            await _unitOfWork.CompleteAsync();
            return userEntity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<User> UpdateAsync(UserDTO entity, int id)
        {
            var userEntity = await _userSevice.ToUserEntity(entity);

            await _unitOfWork.UserRepository.UpdateAsync(userEntity, id);
            await _unitOfWork.CompleteAsync();
            return userEntity;
        }
    }
}