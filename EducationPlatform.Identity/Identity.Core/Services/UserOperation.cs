using Amazon.CognitoIdentityProvider.Model;
using CSharpFunctionalExtensions;
using Identity.Core.DTO.User;
using Identity.Core.Helpers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Identity.Core.Services
{
    public class UserOperation(
        IBaseDbOperation<User> dbOperation, ILogger<UserOperation> logger,
        IdentityOperation identityOperation, FileHelper filesHelper)
    {
        private readonly IBaseDbOperation<User> _dbOperation = dbOperation;
        private readonly ILogger<UserOperation> _logger = logger;
        private readonly IdentityOperation _identityOperation = identityOperation;
        private readonly FileHelper _filesHelper = filesHelper;

        public async Task<Result<User>> AddAsync(UserDTO entity, string id)
        {
            User userEntity = await FromUserDtoToUserAsync(entity, id);
            try
            {
                await _dbOperation.AddAsync(userEntity);
                return Result.Success(userEntity);
            }
            catch (Exception ex)
            {
                await _identityOperation.DeleteAsync(id);
                _logger.LogInformation("An error occurred during the Add: {ErrorMessage}", ex.Message);
                return Result.Failure<User>($"Oops, something went wrong");
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
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Delete: {ErrorMessage}", ex.Message);
                return Result.Failure($"Oops, something went wrong");
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
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred during the Update: {ErrorMessage}", ex.Message);
                return Result.Failure<User>($"Oops, something went wrong");
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
