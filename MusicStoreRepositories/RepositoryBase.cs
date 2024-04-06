using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using System.Linq.Expressions;

namespace MusicStoreRepositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        //para que puedan utilizarla las clases de heredan
        protected readonly DbContext context;

        public RepositoryBase(DbContext context)
        {
            this.context = context;
        }
        public virtual async Task<ICollection<TEntity>> GetAsync()
        {
            return await context.Set<TEntity>()
                 .AsNoTracking()
                 .ToListAsync();
        }
#pragma warning disable CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
        public  async Task<TEntity?> GetAsync(int id)
#pragma warning restore CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
        {
            return await context.Set<TEntity>()
                .FindAsync(id);
        }
        public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        {
            return await context.Set<TEntity>()
                 .Where(predicate)
                 .OrderBy(orderBy)
                 .AsNoTracking()
                 .ToListAsync();
        }
        public async Task<int> AddAsync(TEntity entity)
        {
            await context.Set<TEntity>()
                .AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task UpdateAsync()
        {
            await context.SaveChangesAsync();
        }   
        public async Task DeleteAsync(int id)
        {
            var item = await GetAsync(id);
            if (item is not null){
                item.Status = false;
                await UpdateAsync();
            }
        }
    }
}
