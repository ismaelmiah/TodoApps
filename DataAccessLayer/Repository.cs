using System;
using System.Collections.Generic;
using System.Linq;
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
        protected ISession Session;
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
            if (CassandraAdded(entity))
            {
                DbSet.Add(entity);
            }
        }

        private bool CassandraAdded(TEntity entity)
        {
            var (title, id, dateTime) = GenerateCassandraData(entity);

            var statement = new SimpleStatement("INSERT INTO todoitems (id, title, datetime) VALUES (?,?,?)",
                id, title, dateTime);

            try
            {
                Session = Cluster.Connect("tododb");
                Session.Execute(statement);
                Session.Dispose();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }

        public void Remove(TKey id)
        {
            var entityToDelete = DbSet.Find(id);
            if (RemovedCassandra(entityToDelete))
            {
                DbSet.Remove(entityToDelete);
            }
        }
        
        public void Edit(TEntity entityToUpdate)
        {
            if (CassandraUpdate(entityToUpdate))
            {
                DbSet.Attach(entityToUpdate);
                DbContext.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }

        private bool CassandraUpdate(TEntity entity)
        {
            Session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());
            var title = properties[1].GetValue(entity).ToString();
            var date = Convert.ToDateTime(properties[2].GetValue(entity).ToString());
            var dateTime = ToLocalDate(date);

            var statement = new SimpleStatement("UPDATE todoitems SET title =?, datetime =? WHERE id = ?", title,dateTime,id);

            try
            {
                Session.Execute(statement);
                Session.Dispose();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
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

        public List<Tuple<int, string, DateTime>> GetAllByDate(DateTime dateTime)
        {
            Session = Cluster.Connect("tododb");
            var date = ToLocalDate(dateTime);
            var statement = new SimpleStatement("SELECT * FROM todoitems where datetime = ? allow filtering", date);
            var results = Session.Execute(statement);

            var tupleList = (from result in results let localdate = result
                    .GetValue<LocalDate>("datetime").ToString()
                             select new Tuple<int, string, DateTime>
                                 (result.GetValue<int>("id"),
                                 result.GetValue<string>("title"),
                                 Convert.ToDateTime(localdate))).ToList();

            return tupleList;
        }
        public static LocalDate ToLocalDate(DateTime dateTime) => new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);

        private (string Title, int id, LocalDate date) GenerateCassandraData(TEntity entity)
        {
            Session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var title = properties[1].GetValue(entity).ToString();
            var date = Convert.ToDateTime(properties[2].GetValue(entity).ToString());
            var dateTime = ToLocalDate(date);

            var statement = new SimpleStatement("SELECT id FROM todoitems");

            var results = Session.Execute(statement);
            var id = results.Select(result => result.GetValue<int>("id")).Prepend(0).Max();
            Session.Dispose();
            id++;
            return (title, id, dateTime);
        }

        private bool RemovedCassandra(TEntity entity)
        {
            Session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());

            var statement = new SimpleStatement("DELETE FROM todoitems where id = ?", id);
            try
            {
                Session.Execute(statement);
                Session.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}