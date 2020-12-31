using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public interface IRepository<TEntity, TKey, TContext>
                    where TEntity : class, IEntity<TKey>
                    where TContext : DbContext
    {
        void Add(TEntity entity);
        void Remove(TKey id);
        void Remove(TEntity entity);
        void Edit(TEntity entityToUpdate);
        Task<TEntity> GetById(TKey id);
        Task<IList<TEntity>> GetAll();
        List<Tuple<int, string, DateTime>> GetAllByDate(DateTime filterDateTime);
    }
}
