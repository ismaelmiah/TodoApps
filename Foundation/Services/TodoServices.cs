using System.Collections.Generic;
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

        public void EditTodo(int id, TodoItem item)
        {
            var exits = _todoContext.TodoItemRepos.GetById(id);
            exits.Title = item.Title;
            exits.DateTime = item.DateTime;
            _todoContext.TodoItemRepos.Edit(exits);
        }

        public TodoItem GetItem(int id)
        {
            return _todoContext.TodoItemRepos.GetById(id);
        }

        public IList<TodoItem> GetAllItems()
        {
            return _todoContext.TodoItemRepos.GetAll();
        }
    }
}