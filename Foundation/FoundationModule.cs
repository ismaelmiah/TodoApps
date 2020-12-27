using API.Foundation.Contexts;
using API.Foundation.Repositories;
using Autofac;

namespace API.Foundation
{
    public class FoundationModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public FoundationModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TodoContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();


            builder.RegisterType<TodoItemRepos>().As<ITodoItemRepos>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
