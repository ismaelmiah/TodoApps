using System;
using System.Collections.Generic;
using API.Foundation.Entities;
using API.Foundation.Services;
using Autofac;
using System.Linq;

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
        public TodoItemModel Get(int id)
        {
            var data = Services.GetItem(id);
            return EntityToModel(data);
        }

        public IEnumerable<TodoItemModel> GetAll()
        {
            var data = Services.GetAllItems();
            var newList = from x in data select new TodoItemModel();
            return newList;
        }
        public void Update(int id, TodoItemModel model)
        {
            Services.EditTodo(id, ModelToEntity(model));
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

        private static TodoItemModel EntityToModel(TodoItem model)
        {
            return new TodoItemModel()
            {
                Id = model.Id,
                Title = model.Title,
                DateTime = model.DateTime
            };
        }

        public bool Exits(int id)
        {
            return true;
        }
    }
}
