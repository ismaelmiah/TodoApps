using API.Todo.Models;
using Autofac;

namespace API.Todo
{
    public class ApiModule : Module
    {
        internal string ConnectionString { get; }
        internal string MigrationAssemblyName { get; }

        public ApiModule(string connectionString, string migrationAssemblyName)
        {
            ConnectionString = connectionString;
            MigrationAssemblyName = migrationAssemblyName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TodoItemModel>().AsSelf().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
