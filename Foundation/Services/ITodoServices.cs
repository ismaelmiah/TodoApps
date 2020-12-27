using System.Collections.Generic;
using API.Foundation.Entities;

namespace API.Foundation.Services
{
    public interface ITodoServices
    {
        void AddTodo(TodoItem item);
        void RemoveTodo(int id);
        void EditTodo(int id, TodoItem item);
        TodoItem GetItem(int id);
        IList<TodoItem> GetAllItems();
    }
}
