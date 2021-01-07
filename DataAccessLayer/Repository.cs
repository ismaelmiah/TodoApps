using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.DataAccessLayer
{
    public abstract class Repository<TEntity, TKey, TContext> : IUnitOfWork, IRepository<TEntity, TKey, TContext>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        protected TContext DbContext;
        protected DbSet<TEntity> DbSet;
        protected Cluster Cluster;
        protected ISession Session;

        private readonly Cassandra _options;

        public Repository(TContext context, IOptions<Cassandra> options)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
            _options = options.Value;
            Cluster = Connect();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            Save();
            CassandraAdded(entity, entity.Id);
            Dispose();
        }

        private void CassandraAdded(TEntity entity, TKey id)
        {
            var (title, _, dateTime) = GenerateCassandraData(entity);

            var keyspaceName = _options.CassandraOptions.KeyspaceName;

            var statement = new SimpleStatement($"INSERT INTO {keyspaceName}.todoitem (id, title, datetime) VALUES (?,?,?)",
                id, title, dateTime);

            try
            {
                Session = Cluster.Connect();
                Session.Execute(statement);
                Session.Dispose();
            }
            catch (InvalidQueryException)
            {

                var _class = _options.CassandraOptions.Replication.Class;
                var reflicationfector = _options.CassandraOptions.Replication.replication_factor;
                
                if(Session==null) Session = Cluster.Connect();
                
                var query = $"CREATE KEYSPACE IF NOT EXISTS {keyspaceName} WITH replication = {{'class': '{_class}', 'replication_factor' : {reflicationfector}}}";
                Session.Execute(query);

                var entty = new Table<TEntity>(Session, new MappingConfiguration(), tableName: "todoitem", keyspaceName);
                entty.CreateIfNotExists();

                Session.Execute(statement);

                Session.Dispose();
            }
        }

        public void Remove(TKey id)
        {
            var entityToDelete = DbSet.Find(id);
            DbSet.Remove(entityToDelete);
            Save();
            RemovedCassandra(entityToDelete.Id);
            Dispose();
        }

        private void RemovedCassandra(TKey id)
        {
            var keyspaceName = _options.CassandraOptions.KeyspaceName;

            var statement = new SimpleStatement($"DELETE FROM {keyspaceName}.todoitem where id = ?", id);

            try
            {
                Session = Cluster.Connect();
                Session.Execute(statement);
                Session.Dispose();
            }
            catch (InvalidQueryException exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Edit(TEntity entityToUpdate)
        {
            CassandraUpdate(entityToUpdate);
            DbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;
            Save();
            Dispose();
        }

        private void CassandraUpdate(TEntity entity)
        {
            var (title, id, dateTime) = GenerateCassandraData(entity);

            var keyspaceName = _options.CassandraOptions.KeyspaceName;

            var statement = new SimpleStatement($"UPDATE {keyspaceName}.todoitem SET title =?, datetime =? WHERE id = ?", title, dateTime, id);

            try
            {
                Session = Cluster.Connect();
                Session.Execute(statement);
                Session.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            var keyspaceName = _options.CassandraOptions.KeyspaceName;

            var date = ToLocalDate(dateTime);
            List<Tuple<int, string, DateTime>> tupleList;
            try
            {
                Session = Cluster.Connect();
                var query = $"SELECT * FROM  {keyspaceName}.todoitem  where datetime = '{date}' allow filtering";
                var results = Session.Execute(query);
                Session.Dispose();
                
                 tupleList = (from result in results
                    let localdate = result
                        .GetValue<LocalDate>("datetime").ToString()
                    select new Tuple<int, string, DateTime>
                    (result.GetValue<int>("id"),
                        result.GetValue<string>("title"),
                        Convert.ToDateTime(localdate))).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return tupleList;
        }

        private (string Title, int id, LocalDate date) GenerateCassandraData(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());
            var title = properties[1].GetValue(entity).ToString();
            var date = properties[2].GetValue(entity).ToString();
            var dateTime = ToLocalDate(Convert.ToDateTime(date));

            return (title, id, dateTime);
        }
        public static LocalDate ToLocalDate(DateTime dateTime) => new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);

        private Cluster Connect() =>  Cluster.Builder().AddContactPoints(_options.CassandraNodes).Build();

        public void Dispose() => DbContext.Dispose();

        public void Save() => DbContext.SaveChanges();
    }
}