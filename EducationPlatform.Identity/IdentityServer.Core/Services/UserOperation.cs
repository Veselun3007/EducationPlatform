using IdentityServer.Core.Interfaces;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Services
{
    public class UserOperation(IUnitOfWork unitOfWork) : IBusinessUserOperation
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<User> AddAsync(UserDTO entity)
        {
            var userEntity = new User
            {
                UserName = entity.UserName,
                UserEmail = entity.UserEmail,
                UserImage = entity.UserImage is not null ? await _unitOfWork.UserRepository.GetName(entity.UserImage) : null
            };

            await _unitOfWork.UserRepository.AddAsync(userEntity);
            await _unitOfWork.CompleteAsync();
            return userEntity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<User> UpdateAsync(UserDTO entity, int id)
        {
            var userEntity = new User
            {
                UserName = entity.UserName,
                UserEmail = entity.UserEmail,
                UserImage = entity.UserImage is not null ? await _unitOfWork.UserRepository.GetName(entity.UserImage) : null
            };

            await _unitOfWork.UserRepository.UpdateAsync(userEntity, id);
            await _unitOfWork.CompleteAsync();
            return userEntity;
        }
    }
}