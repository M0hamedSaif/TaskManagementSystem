using TaskManager.Core.Entities;
using TaskManager.Core.Repositories.Contract;

namespace TaskManager.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}
