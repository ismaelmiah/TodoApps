using API.DataAccessLayer;
using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public interface ITodoSqlUnitOfWork : IUnitOfWork
    {
        ITodoItemRepos TodoItemRepos { get; set; }
    }
}