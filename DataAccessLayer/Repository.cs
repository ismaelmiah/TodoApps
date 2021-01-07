using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        protected ISession _session;

        private readonly Cassandra Options;

        public Repository(TContext context, IOptions<Cassandra> options)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
            Options = options.Value;
            Cluster = Connect();
        }
        
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            Save();
            var id = entity.Id;
            CassandraAdded(entity, id);
        }

        private void CassandraAdded(TEntity entity, TKey id)
        {
            var (title, dateTime) = GenerateCassandraData(entity);


            var keyspaceName = Options.CassandraOptions.KeyspaceName;
            var _class = Options.CassandraOptions.Replication.Class;
            var reflicationFector = Options.CassandraOptions.Replication.replication_factor;


            var statement = new SimpleStatement($"INSERT INTO {keyspaceName}.todoitem (id, title, datetime) VALUES (?,?,?)",
                id, title, dateTime);
            
            try
            {
                _session = Cluster.Connect();
                _session.Execute(statement);
                _session.Dispose();
            }
            catch (InvalidQueryException exception)
            {
                
                var query = $"CREATE KEYSPACE IF NOT EXISTS {keyspaceName} WITH replication = {{'class': '{_class}', 'replication_factor' : {reflicationFector}}}";
                _session.Execute(query);
                
                var entty = new Table<TEntity>(_session, new MappingConfiguration(),tableName: "todoitem", keyspaceName);
                entty.CreateIfNotExists();
                
                _session.Dispose();
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
            _session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());
            var title = properties[1].GetValue(entity).ToString();
            var date = properties[2].GetValue(entity).ToString();
            var dateTime = ToDateTimeOffset(date);

            var statement = new SimpleStatement("UPDATE todoitems SET title =?, datetime =? WHERE id = ?", title,dateTime,id);

            try
            {
                _session.Execute(statement);
                _session.Dispose();
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

        public List<Tuple<int, string, DateTimeOffset>> GetAllByDate(string dateTime)
        {
            _session = Cluster.Connect("tododb");
            var date = ToDateTimeOffset(dateTime);
            var statement = new SimpleStatement("SELECT * FROM todoitems where datetime = ? allow filtering", date);
            var results = _session.Execute(statement);

            var tupleList = (from result in results let localdate = result
                    .GetValue<LocalDate>("datetime").ToString()
                             select new Tuple<int, string, DateTimeOffset>
                                 (result.GetValue<int>("id"),
                                 result.GetValue<string>("title"),
                                 Convert.ToDateTime(localdate))).ToList();

            return tupleList;
        }
        public static DateTimeOffset ToDateTimeOffset(string dateTime)
        {
            return DateTimeOffset.Parse(dateTime);
            //return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        private (string Title, DateTimeOffset date) GenerateCassandraData(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var title = properties[1].GetValue(entity).ToString();
            var date = properties[2].GetValue(entity).ToString();
            var dateTime = ToDateTimeOffset(date);


            return (title, dateTime);
        }

        private bool RemovedCassandra(TEntity entity)
        {
            _session = Cluster.Connect("tododb");

            var properties = entity.GetType().GetProperties();
            var id = int.Parse(properties[0].GetValue(entity).ToString());

            var statement = new SimpleStatement("DELETE FROM todoitems where id = ?", id);
            try
            {
                _session.Execute(statement);
                _session.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private Cluster Connect()
        {
            var nodes = Options.CassandraNodes;
            
            var cluster = Cluster.Builder().AddContactPoints(nodes).Build();

            return cluster;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public int Save() => DbContext.SaveChanges();
    }
}