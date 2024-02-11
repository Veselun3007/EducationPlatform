namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IRepository<T> : IBaseRepository<T> where T : class
    {
        Task<T?> UpdateAsync(T entity, int id);
    }
}
