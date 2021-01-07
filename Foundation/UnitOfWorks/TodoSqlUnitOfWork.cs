using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public class TodoSqlUnitOfWork : ITodoSqlUnitOfWork
    {
        internal TodoContext Context { get; }
        public ITodoItemRepos TodoItemRepos { get; set; }

        public TodoSqlUnitOfWork(TodoContext context, ITodoItemRepos todoItemRepos)
        {
            Context = context;
            TodoItemRepos = todoItemRepos;
        }
    }
}
