using API.Todo.Models;
using Autofac;

namespace API.Todo
{
    public class ApiModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ApiModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TodoItemModel>().AsSelf().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
