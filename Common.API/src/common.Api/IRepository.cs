
using System.Linq.Expressions;

namespace common.Api
{
    public interface IRepository<P> where P : IEntity
    {
        Task CreateAsync(P payment);
        Task<IReadOnlyCollection<P>> GetAllAsync();
        Task<IReadOnlyCollection<P>> GetAllAsync(Expression<Func<P, bool>> filter);
        Task<P> GetAsync(Guid id);
        Task<P> GetAsync(Expression<Func<P, bool>> filter);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(P payment);
    }
}