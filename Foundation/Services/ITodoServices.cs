using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Foundation.Entities;

namespace API.Foundation.Services
{
    public interface ITodoServices
    {
        TodoItem AddTodo(TodoItem item);
        Task RemoveTodo(int id);
        Task EditTodo(int id, TodoItem item, bool datetime);
        Task<TodoItem> GetItem(int id);
        Task<IList<TodoItem>> GetAllItems();
        IList<TodoItem> GetByDate(DateTime filterDateTime);
    }
}
