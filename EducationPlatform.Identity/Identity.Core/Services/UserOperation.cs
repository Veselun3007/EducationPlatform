using CSharpFunctionalExtensions;
using Identity.Core.DTO.User;
using Identity.Core.Helpers;
using Identity.Core.Models;
using Identity.Domain.Entities;
using Identity.Infrastructure.Interfaces;

namespace Identity.Core.Services
{
    public class UserOperation(IBaseDbOperation<User> dbOperation, 
        IdentityOperation identityOperation, FileHelper filesHelper)
    {
        private readonly IBaseDbOperation<User> _dbOperation = dbOperation;
        private readonly IdentityOperation _identityOperation = identityOperation;
        private readonly FileHelper _filesHelper = filesHelper;

        public async Task<Result<User, Error>> AddAsync(UserDTO entity, string id)
        {
            try
            {
                User userEntity = await FromUserDtoToUserAsync(entity, id);

                await _dbOperation.AddAsync(userEntity);
                return Result.Success<User, Error>(userEntity);
            }
            finally
            {
                await _identityOperation.DeleteAsync(id);
            }
        }

        public async Task<Result<string, Error>> DeleteAsync(string id)
        {
            try
            {
                var dbDeleteTask = _dbOperation.DeleteAsync(id);
                var identityDeleteTask = _identityOperation.DeleteAsync(id);

                await Task.WhenAll(dbDeleteTask, identityDeleteTask);
                return Result.Success<string, Error>("Delete successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<User, Error>> UpdateAsync(UserDTO entity, string id)
        {
            var userEntity = await FromUserDtoToUserAsync(entity, id);
            try
            {
                await _dbOperation.UpdateAsync(userEntity, id);
                return Result.Success<User, Error>(userEntity);
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<User, Error>(Errors.General.NotFound());
            }
        }

        private async Task<User> FromUserDtoToUserAsync(UserDTO entity, string id)
        {
            return new User
            {
                Id = id,
                UserName = entity.UserName,
                Email = entity.Email,
                UserImage = entity.UserImage is not null ? await _filesHelper.AddFileAsync(entity.UserImage) : null
            };
        }
    }
}
