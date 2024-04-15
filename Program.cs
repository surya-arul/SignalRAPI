using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignalRAPI.DbContexts;
using SignalRAPI.DTOs.Config;
using SignalRAPI.Hubs;
using SignalRAPI.MiddlewareExtensions;
using SignalRAPI.Repositories;
using SignalRAPI.SubscribeTableDependencies;

namespace SignalRAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Binding config settings

            builder.Services.AddOptions<ConnectionStrings>()
                .Bind(builder.Configuration.GetSection(nameof(ConnectionStrings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            #endregion

            builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStrings>>().CurrentValue;
                options.UseSqlServer(dbSettings.DbConnection);
            }, ServiceLifetime.Singleton);

            builder.Services.AddSingleton<EmployeeHub>();
            builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddSingleton<SubscribeEmployeeTableDependency>();

            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<EmployeeHub>("/employee-hub");

            app.UseSqlTableDependency<SubscribeEmployeeTableDependency>();

            app.Run();
        }
    }
}
