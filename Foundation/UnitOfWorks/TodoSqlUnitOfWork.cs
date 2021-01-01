using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public class TodoSqlUnitOfWork : UnitOfWork, ITodoSqlUnitOfWork
    {
        internal TodoContext Context { get; }
        public ITodoItemRepos TodoItemRepos { get; set; }

        public TodoSqlUnitOfWork(TodoContext context,
            ITodoItemRepos todoItemRepos
        )
            : base(context)
        {
            Context = context;
            TodoItemRepos = todoItemRepos;
        }
    }
}
