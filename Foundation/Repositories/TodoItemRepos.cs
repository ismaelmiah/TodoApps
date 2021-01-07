using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Entities;
using Microsoft.Extensions.Options;

namespace API.Foundation.Repositories
{
    public class TodoItemRepos : Repository<TodoItem, int, TodoContext>, ITodoItemRepos
    {
        public TodoItemRepos(TodoContext context, IOptions<DataAccessLayer.Cassandra> options) : base(context, options)
        {
        }
    }
}
