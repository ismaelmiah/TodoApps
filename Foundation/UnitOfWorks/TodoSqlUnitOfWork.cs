using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Repositories;

namespace API.Foundation.UnitOfWorks
{
    public class TodoSqlUnitOfWork : UnitOfWork, ITodoSqlUnitOfWork
    {
        private readonly TodoContext _context;
        public ITodoItemRepos TodoItemRepos { get; set; }

        public TodoSqlUnitOfWork(TodoContext context,
            ITodoItemRepos _TodoItemRepos
        )
            : base(context)
        {
            _context = context;
            TodoItemRepos = _TodoItemRepos;
        }
    }
}
