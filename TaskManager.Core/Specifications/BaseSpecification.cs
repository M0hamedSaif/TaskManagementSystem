using System.Linq.Expressions;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>>? OrderBy { get; set; } = null;
        public Expression<Func<T, object>>? OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> expression)
        {
            Criteria = expression;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
            => OrderBy = orderByExpression;

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
            => OrderByDesc = orderByDescExpression;

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
