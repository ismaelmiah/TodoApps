using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public abstract class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey, TContext>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        protected TContext DbContext;
        protected DbSet<TEntity> DbSet;
        protected Cluster Cluster;
        protected ISession session;
        public Repository(TContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
            Cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
        }

        //Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
        //ISession session = cluster.Connect("demo");
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);

            CassandraAdded(entity);
        }

        private void CassandraAdded(TEntity entity)
        {
            var (title, id, dateTime) = GenerateCassandraData(entity);

            var statement = new SimpleStatement("INSERT INTO todoitems (id, title, datetime) VALUES (?,?,?)",
                id, title, dateTime);

            try
            {
                session = Cluster.Connect("tododb");
                session.Execute(statement);
                session.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw new InvalidDataContractException();
            }
        }

        public void Remove(TKey id)
        {
            var entityToDelete = DbSet.Find(id);
            Remove(entityToDelete);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);

            RemovedCassandra(entity);
        }
        
        public void Edit(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;

            CassandraUpdate(entityToUpdate);
        }

        private void CassandraUpdate(TEntity entity)
        {
            session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());
            var title = properties[1].GetValue(entity).ToString();
            var date = Convert.ToDateTime(properties[2].GetValue(entity).ToString());
            var dateTime = ToLocalDate(date);

            var statement = new SimpleStatement("UPDATE todoitems SET title =?, datetime =? WHERE id = ?", title,dateTime,id);

            try
            {
                session.Execute(statement);
                session.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual async Task<IList<TEntity>> GetAll()
        {
            IQueryable<TEntity> query = DbSet;
            return await query.ToListAsync();
        }

        public static LocalDate ToLocalDate(DateTime dateTime) => new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);

        private (string Title, int id, LocalDate date) GenerateCassandraData(TEntity entity)
        {
            session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var title = properties[1].GetValue(entity).ToString();
            var date = Convert.ToDateTime(properties[2].GetValue(entity).ToString());
            var dateTime = ToLocalDate(date);

            var statement = new SimpleStatement("SELECT id FROM todoitems");

            var results = session.Execute(statement);
            var id = results.Select(result => result.GetValue<int>("id")).Prepend(0).Max();
            session.Dispose();
            id++;
            return (title, id, dateTime);
        }

        private void RemovedCassandra(TEntity entity)
        {
            session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());

            var statement = new SimpleStatement("DELETE FROM todoitems where id = ?", id);
            try
            {
                session.Execute(statement);
                session.Dispose();
            }
            catch
            {
                throw new DBConcurrencyException();
            }
        }
    }
}