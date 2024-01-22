using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Repositories
{
    internal class Repository<T>(EducationPlatformContext context, 
        FileHelper filesHelper) : IRepository<T> where T : class
    {
        private readonly EducationPlatformContext _context = context;
        private readonly FileHelper _filesHelper = filesHelper;

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<string> GetName(IFormFile file)
        {
            return await _filesHelper.AddFileAsync(file);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<T?> UpdateAsync(T entity, int id)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }
    }
}
