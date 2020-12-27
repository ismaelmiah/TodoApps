using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public interface IRepository<TEntity, TKey, TContext>
                    where TEntity : class, IEntity<TKey>
                    where TContext : DbContext
    {
        void Add(TEntity entity);
        void Remove(TKey id);
        void Edit(TEntity entityToUpdate);
        TEntity GetById(TKey id);
    }
}
