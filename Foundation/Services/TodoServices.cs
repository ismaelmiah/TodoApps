using API.Foundation.Entities;
using API.Foundation.UnitOfWorks;

namespace API.Foundation.Services
{
    public class TodoServices : ITodoServices
    {
        private readonly ITodoSqlUnitOfWork _todoContext;
        public TodoServices(ITodoSqlUnitOfWork todoContext)
        {
            _todoContext = todoContext;
        }

        public void AddTodo(TodoItem item)
        {
            _todoContext.TodoItemRepos.Add(item);
        }

        public void RemoveTodo(int id)
        {
            _todoContext.TodoItemRepos.Remove(id);
        }

        public void EditTodo(TodoItem item)
        {
            _todoContext.TodoItemRepos.Edit(item);
        }

        public TodoItem GetItem(int id)
        {
            return _todoContext.TodoItemRepos.GetById(id);
        }
    }
}