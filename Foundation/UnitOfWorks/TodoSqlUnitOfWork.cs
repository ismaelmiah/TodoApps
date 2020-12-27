using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public class TodoSqlUnitOfWork : ITodoSqlUnitOfWork
    {
        public ITodoItemRepos TodoItemRepos { get; set; }
    }
}
