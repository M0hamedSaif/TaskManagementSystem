using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Repositories.Contract;
using TaskManager.Core.Specifications;
using TaskManager.Repository.Data.Context;

namespace TaskManager.Repository.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly TaskManagerDbContext _dbContext;

        public GenericRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).AsNoTracking().ToListAsync();

        public async Task<T?> GetWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).CountAsync();

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);

        public async Task AddAsync(T entity) => await _dbContext.AddAsync(entity);
        public void Update(T entity) => _dbContext.Update(entity);
        public void Delete(T entity) => _dbContext.Remove(entity);
    }
}
