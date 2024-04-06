using MusicStore.Entities;
using System.Linq.Expressions;


namespace MusicStoreRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<ICollection<TEntity>> GetAsync();
        //recibe un delegado
        Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity,bool>> predicate,Expression<Func<TEntity,TKey>>orderBy);
        Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(int id);
        Task<int> AddAsync(TEntity entity);
        Task UpdateAsync();
        Task DeleteAsync(int id);

    }
}
