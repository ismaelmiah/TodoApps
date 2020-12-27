using System;
using API.Foundation.Entities;
using API.Foundation.Services;
using Autofac;

namespace API.Todo.Models
{
    public class TodoItemModel
    {
        internal ITodoServices Services { get; }

        public TodoItemModel(ITodoServices services)
        {
            Services = services;
        }

        public TodoItemModel()
        {
            Services = Startup.AutofacContainer.Resolve<ITodoServices>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }

        public void Add(TodoItemModel model)
        {
            Services.AddTodo(ModelToEntity(model));
        }

        public void Remove(int id)
        {
            Services.RemoveTodo(id);
        }
        public TodoItem Get(int id)
        {
            return Services.GetItem(id);
        }

        private static TodoItem ModelToEntity(TodoItemModel model)
        {
            return new TodoItem()
            {
                Id = model.Id,
                Title = model.Title,
                DateTime = model.DateTime
            };
        }
    }
}
