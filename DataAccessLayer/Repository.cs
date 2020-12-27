using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public abstract class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey, TContext>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        protected TContext DbContext;
        protected DbSet<TEntity> DbSet;

        protected Repository(TContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(TKey id)
        {
            var entityToDelete = DbSet.Find(id);
            Remove(entityToDelete);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Edit(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }
    }
}