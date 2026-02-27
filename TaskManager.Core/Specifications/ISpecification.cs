using System.Linq.Expressions;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        Expression<Func<T, bool>>? Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; set; }
        Expression<Func<T, object>>? OrderByDesc { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        bool IsPaginationEnabled { get; set; }
    }
}
