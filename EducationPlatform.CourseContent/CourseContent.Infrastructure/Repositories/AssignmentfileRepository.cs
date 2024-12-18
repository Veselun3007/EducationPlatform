﻿using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces.Base;
using System.Linq.Expressions;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentfileRepository(IEntityRepository<Assignmentfile> repository) :
        IEntityRepository<Assignmentfile>
    {
        private readonly IEntityRepository<Assignmentfile> _repository = repository;


        public async Task<Assignmentfile> AddAsync(Assignmentfile entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Assignmentfile?> GetByIdAsync(int id, params Expression<Func<Assignmentfile, object>>[] includes)
        {
            return await _repository.GetByIdAsync(id, includes);
        }
    }
}
