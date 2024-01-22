using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Interfaces
{
    public interface IBusinessUserOperation
    {
        Task<UserDTO> AddAsync(UserDTO entity);

        Task<UserDTO> UpdateAsync(UserDTO entity);

        Task<UserDTO> DeleteAsync(UserDTO entity);

        void GenerateTokens();
    }
}
