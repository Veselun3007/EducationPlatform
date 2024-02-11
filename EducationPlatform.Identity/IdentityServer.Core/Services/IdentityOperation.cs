using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Services
{
    public class IdentityOperation(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            return await _unitOfWork.IdentityRepository.FindByParamAsync(a => a.Email == email);
        }

        public async Task<AppUser?> FindByIdAsync(int userId)
        {
            return await _unitOfWork.IdentityRepository.GetByIdAsync(userId);
        }

        public async Task<AppUser> CreateAsync(UserDTO userDTO)
        {
            var user = UserService.FromUserDtoToAppUser(userDTO);
            var addedUser = await _unitOfWork.IdentityRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return addedUser;
        }

        public async Task<bool> DeleteAsync(UserDTO userDTO)
        {
            var userForDelete = await FindByEmailAsync(userDTO.Email);
            if (userForDelete is not null)
            {
                await _unitOfWork.IdentityRepository.DeleteAsync(userForDelete.Id);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(UserDTO userDTO)
        {
            var userForUpdate = await FindByEmailAsync(userDTO.Email);
            if (userForUpdate is not null)
            {
                var user = UserService.FromUserDtoToAppUser(userDTO);
                await _unitOfWork.IdentityRepository.UpdateAsync(user, userForUpdate.Id);
                await _unitOfWork.CompleteAsync();
            }
            return false;
        }

        public async Task<Token> AddTokenAsync(int userId)
        {
            var token = UserService.ToToken(userId);
            var addedToken = await _unitOfWork.TokenRepository.AddAsync(token);
            await _unitOfWork.CompleteAsync();
            return addedToken;
        }

        public async Task<bool> AddTokenAsync(Token token)
        {
            var addedToken = await _unitOfWork.TokenRepository.AddAsync(token);
            await _unitOfWork.CompleteAsync();
            return addedToken != null;
        }

        public async Task<bool> DeleteTokenAsync(int tokenId)
        {
            if (tokenId is 0)
            {
                return false;
            }
            await _unitOfWork.UserRepository.DeleteAsync(tokenId);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Token?> FindTokenByParamAsync(string refreshToken)
        {
            return await _unitOfWork.TokenRepository.FindByParamAsync(t => t.RefreshToken == refreshToken);
        }
    }
}
