using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            _todoContext.TodoItemRepos.Add(item);
            await _todoContext.Save();
            return item;
        }

        public async Task RemoveTodo(int id)
        {
            _todoContext.TodoItemRepos.Remove(id);
            await _todoContext.Save();
        }

        public async Task EditTodo(int id, TodoItem item)
        {
            var exits = await _todoContext.TodoItemRepos.GetById(id);
            exits.Title = item.Title;
            exits.DateTime = item.DateTime;
            _todoContext.TodoItemRepos.Edit(exits);
            await _todoContext.Save();
        }

        public async Task<TodoItem> GetItem(int id)
        {
            return await _todoContext.TodoItemRepos.GetById(id);
        }

        public async Task<IList<TodoItem>> GetAllItems()
        {
            return await _todoContext.TodoItemRepos.GetAll();
        }
    }
}