using System;
using System.Collections.Generic;
using API.Foundation.Entities;
using API.Foundation.Services;
using Autofac;
using System.Linq;
using System.Threading.Tasks;

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

        public TodoItemModel Add(TodoItemModel model)
        {
            var data = Services.AddTodo(ModelToEntity(model));
            return EntityToModel(data);
        }

        public async Task Remove(int id)
        {
            await Services.RemoveTodo(id);
        }

        public  IEnumerable<TodoItemModel> GetByDate(DateTime dateTime)
        {
            var data =  Services.GetByDate(dateTime);
            var sendModels = from x in data
                             select new TodoItemModel
                             {
                                 Id = x.Id,
                                 Title = x.Title,
                                 DateTime = x.DateTime
                             };

            return sendModels;
        }
        public async Task<TodoItemModel> Get(int id)
        {
            var data = await Services.GetItem(id);
            return data == null ? null : EntityToModel(data);
        }

        public async Task PatchConfigure(int id, TodoItemModel model)
        {
            await Services.EditTodo(id, ModelToEntity(model), true);
        }
        public async Task<IEnumerable<TodoItemModel>> GetAll()
        {
            var data = await Services.GetAllItems();
            var newList = from x in data select EntityToModel(x);
            return newList;
        }
        public async Task Update(int id, TodoItemModel model)
        {
            await Services.EditTodo(id, ModelToEntity(model), false);
        }
        public TodoItem ModelToEntity(TodoItemModel model)
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
