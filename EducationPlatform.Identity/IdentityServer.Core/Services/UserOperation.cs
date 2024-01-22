using IdentityServer.Core.Interfaces;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Services
{
    internal class UserOperation(IUnitOfWork unitOfWork) : IBusinessUserOperation
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task<UserDTO> AddAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> DeleteAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }

        public void GenerateTokens()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> UpdateAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
