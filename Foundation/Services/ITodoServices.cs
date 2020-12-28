using System.Collections.Generic;
using System.Threading.Tasks;
using API.Foundation.Entities;

namespace API.Foundation.Services
{
    public interface ITodoServices
    {
        Task<TodoItem> AddTodo(TodoItem item);
        Task RemoveTodo(int id);
        Task EditTodo(int id, TodoItem item);
        Task<TodoItem> GetItem(int id);
        Task<IList<TodoItem>> GetAllItems();
    }
}
