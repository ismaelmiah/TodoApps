using API.DataAccessLayer;
using API.Foundation.Contexts;
using API.Foundation.Entities;

namespace API.Foundation.Repositories
{
    public interface ITodoItemRepos : IRepository<TodoItem, int, TodoContext>
    {

    }
}