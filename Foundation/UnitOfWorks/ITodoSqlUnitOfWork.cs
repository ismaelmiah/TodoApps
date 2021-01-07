using API.DataAccessLayer;
using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public interface ITodoSqlUnitOfWork
    {
        ITodoItemRepos TodoItemRepos { get; set; }
    }
}