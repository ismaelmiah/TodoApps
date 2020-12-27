using API.Foundation.Entities;

namespace API.Foundation.Services
{
    public interface ITodoServices
    {
        void AddTodo(TodoItem item);
        void RemoveTodo(int id);
        void EditTodo(TodoItem item);
        TodoItem GetItem(int id);
    }
}
