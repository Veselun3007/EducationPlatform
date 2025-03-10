using Identity.Core.DTO.Requests;
using Identity.Core.DTO.Responses;
using Identity.Core.Helpers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Interfaces;

namespace Identity.Core.Services
{
    public class UserService(IBaseDbOperation<User> dbOperation, IdentityService identityOperation, FileHelper filesHelper)
    {
        private readonly IBaseDbOperation<User> _dbOperation = dbOperation;
        private readonly IdentityService _identityOperation = identityOperation;
        private readonly FileHelper _filesHelper = filesHelper;


        public async Task<UserOutDTO?> AddAsync(UserDTO entity, string id)
        {
            try
            {
                User userEntity = await FromUserDtoToUserAsync(entity, id);
                await _dbOperation.AddAsync(userEntity);
                return await FromUser(userEntity);
            }
            catch
            {
                await _identityOperation.DeleteAsync(id);
                return null;
            }
        }

        public async Task DeleteAsync(string id)
        {
            var dbDeleteTask = _dbOperation.DeleteAsync(id);
            var identityDeleteTask = _identityOperation.DeleteAsync(id);
            await Task.WhenAll(dbDeleteTask, identityDeleteTask);
        }

        public async Task<UserOutDTO> UpdateAsync(UserUpdateDTO entity, string id)
        {
            var userEntity = await FromUserUpdateDtoToUserAsync(entity, id);
            await _dbOperation.UpdateAsync(userEntity, id);
            return await FromUser(userEntity);
        }

        public async Task<UserOutDTO?> GetByIdAsync(string id)
        {
            var entity = await _dbOperation.GetByIdAsync(id);
            return await FromUser(entity);
        }

        private async Task<UserOutDTO> FromUser(User entity)
        {
            return new UserOutDTO
            {
                UserName = entity.UserName,
                Email = entity.Email,
                UserImage = entity.UserImage is not null ? await _filesHelper
                                                .GetFileLink(entity.UserImage) : null
            };
        }

        private async Task<User> FromUserDtoToUserAsync(UserDTO entity, string id)
        {
            return new User
            {
                Id = id,
                UserName = entity.UserName,
                Email = entity.Email,
                UserImage = entity.UserImage is not null ? await _filesHelper
                                                .AddFileAsync(entity.UserImage) : null
            };
        }

        private async Task<User> FromUserUpdateDtoToUserAsync(UserUpdateDTO entity, string id)
        {
            return new User
            {
                Id = id,
                UserName = entity.UserName,
                Email = entity.Email,
                UserImage = entity.UserImage is not null ? await _filesHelper
                                                .AddFileAsync(entity.UserImage) : null
            };
        }
    }
}
