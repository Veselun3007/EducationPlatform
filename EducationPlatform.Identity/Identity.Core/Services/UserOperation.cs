using Amazon.CognitoIdentityProvider.Model;
using CSharpFunctionalExtensions;
using Identity.Core.DTO.User;
using Identity.Core.Helpers;
using Identity.Core.Models;
using Identity.Domain.Entities;
using Identity.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Identity.Core.Services
{
    public class UserOperation(
        IBaseDbOperation<User> dbOperation, IdentityOperation identityOperation, FileHelper filesHelper)
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

        public async Task<Result> DeleteAsync(string id)
        {
            try
            {
                var dbDeleteTask = _dbOperation.DeleteAsync(id);
                var identityDeleteTask = _identityOperation.DeleteAsync(id);

                await Task.WhenAll(dbDeleteTask, identityDeleteTask);
                return Result.Success();
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure($"User wasn`t authorize");
            }
            catch (UserNotFoundException)
            {
                return Result.Failure($"User with id {id} not found");
            }
        }

        public async Task<Result<User>> UpdateAsync(UserDTO entity, string id)
        {
            var userEntity = await FromUserDtoToUserAsync(entity, id);
            try
            {
                await _dbOperation.UpdateAsync(userEntity, id);
                return Result.Success(userEntity);
            }
            catch (NotAuthorizedException)
            {
                return Result.Failure<User>($"User wasn`t authorize");
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<User>($"User with id {id} not found");
            }
        }

        private async Task<string> GetNameAsync(IFormFile file)
        {
            return await _filesHelper.AddFileAsync(file);
        }

        private async Task<User> FromUserDtoToUserAsync(UserDTO entity, string id)
        {
            return new User
            {
                Id = id,
                UserName = entity.UserName,
                Email = entity.Email,
                UserImage = entity.UserImage is not null ? await GetNameAsync(entity.UserImage) : null
            };
        }
    }
}
