using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task EditTodo(int id, TodoItem item, bool datetime)
        {
            var exits = await _todoContext.TodoItemRepos.GetById(id);
            if (datetime)
            {
                exits.DateTime = item.DateTime;
            }
            else
            {
                exits.DateTime = item.DateTime;
                exits.Title = item.Title;
            }
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

        public IList<TodoItem> GetByDate(DateTime filterDateTime)
        {
            var cassandraList =  _todoContext.TodoItemRepos.GetAllByDate(filterDateTime);
            var originalData = from x in cassandraList
                select new TodoItem
                {
                    Id = x.Item1,
                    Title = x.Item2,
                    DateTime = x.Item3
                };
            return originalData.ToList();
        }
    }
}