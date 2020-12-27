using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Entities;

namespace API.Foundation.Repositories
{
    public class TodoItemRepos : Repository<TodoItem, int, TodoContext>, ITodoItemRepos
    {
        public TodoItemRepos(TodoContext context) : base(context)
        {
        }
    }
}
