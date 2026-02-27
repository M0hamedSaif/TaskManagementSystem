using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Repositories.Contract;
using TaskManager.Repository.Data.Context;
using TaskManager.Repository.Generic_Repository;

namespace TaskManager.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagerDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(_dbContext);
                _repositories.Add(type, repository);
            }

            return (IGenericRepository<T>)_repositories[type]!;
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
