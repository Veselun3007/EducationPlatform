using Identity.Core.DTO.Requests;
using Identity.Core.DTO.Responses;
using Identity.Core.Interfaces;
using Identity.Domain.Entities;

namespace Identity.Core.Services
{
    public class UserService
    {
        private readonly IBaseDbOperation<User> _dbOperation;
        private readonly IIdentityService _identityService;
        private readonly IAwsFileService _filesHelper;

        public UserService(IBaseDbOperation<User> dbOperation, IIdentityService identityService, IAwsFileService filesHelper)
        {
            _dbOperation = dbOperation;
            _identityService = identityService;
            _filesHelper = filesHelper;
        }

        public async Task<UserOutDTO?> AddAsync(UserDTO entity, string id)
        {
            try
            {
                User userEntity = await ToUserAsync(entity, id);
                await _dbOperation.AddAsync(userEntity);
                return await FromUser(userEntity);
            }
            catch
            {
                await _identityService.DeleteAsync(id);
                return null;
            }
        }

        public async Task DeleteAsync(string id)
        {
            var dbDeleteTask = _dbOperation.DeleteAsync(id);
            var identityDeleteTask = _identityService.DeleteAsync(id);
            await Task.WhenAll(dbDeleteTask, identityDeleteTask);
        }

        public async Task<UserOutDTO> UpdateAsync(UserUpdateDTO entity, string id)
        {
            var userEntity = await ToUserAsync(entity, id);
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

        private async Task<User> ToUserAsync(UserDTO entity, string id)
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

        private async Task<User> ToUserAsync(UserUpdateDTO entity, string id)
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
