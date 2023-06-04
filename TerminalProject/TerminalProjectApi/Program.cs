using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using TerminalProjectApi.Database;
using TerminalProjectApi.Hubs;
using TerminalProjectApi.Interfaces;
using TerminalProjectApi.Models;

namespace TerminalProjectApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddAuthorization();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddDbContext<DataDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")),ServiceLifetime.Singleton);
                builder.Services.AddSingleton<IControlTower, TestControlTower>();
                //builder.Services.AddSingleton<DataDbContext>();
                builder.Services.AddSwaggerGen();
                builder.Services.AddControllers();
                builder.Services.AddSignalR();
                builder.Services.AddTransient<TerminalHub>();
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                            .WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });



                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseCors("CorsPolicy");

                app.UseAuthorization();



                app.MapControllers().WithOpenApi();

                app.MapHub<TerminalHub>("/terminalhub");

                app.Run();
            }
            catch (Exception e)
            {
                // NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}