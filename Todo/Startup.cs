using API.Foundation;
using API.Foundation.Contexts;
using API.Todo.Middleware;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace API.Todo
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public static ILifetimeScope AutofacContainer { get; private set; }
        public (string connectionString, string migrationAssemblyName) ConnectionAndMigration()
        {

            var connectionStringName = "SQLSERVERCONNECTIONS";
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Startup).Assembly.FullName;

            return (connectionString, migrationAssemblyName);
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var (connectionString, migrationAssemblyName) = ConnectionAndMigration();

            builder.RegisterModule(new ApiModule(connectionString, migrationAssemblyName));
            builder.RegisterModule(new FoundationModule(connectionString, migrationAssemblyName));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var (connectionString, migrationAssemblyName) = ConnectionAndMigration();
            
            services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(connectionString, m 
                    => m.MigrationsAssembly(migrationAssemblyName)));

            services.Configure<DataAccessLayer.Cassandra>(Configuration.GetSection("Cassandra"));
            
            services.AddControllers().AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<RequestMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
