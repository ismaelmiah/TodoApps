using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public abstract class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey, TContext>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        public void Add(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(TKey id)
        {
            throw new System.NotImplementedException();
        }

        public void Edit(TEntity entityToUpdate)
        {
            throw new System.NotImplementedException();
        }

        public TEntity GetById(TKey id)
        {
            throw new System.NotImplementedException();
        }
    }
}