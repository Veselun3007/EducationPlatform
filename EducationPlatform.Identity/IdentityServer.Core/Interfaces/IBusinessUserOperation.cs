using IdentityServer.Core.DTOs.User;
using IdentityServer.Domain.Entities;
using IdentityServer.Web.DTOs.User;

namespace IdentityServer.Core.Interfaces
{
    public interface IBusinessUserOperation
    {
        Task<User> AddAsync(UserDTO entity);

        Task<User> UpdateAsync(UserDTO entity, int id);

        Task DeleteAsync(int id);

        Task<UserOutDTO?> GetUserAsync(int id);
    }
}