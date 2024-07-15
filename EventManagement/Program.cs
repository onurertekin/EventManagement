
using Castle.Core.Configuration;
using DatabaseModel;
using DomainService.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseConfiguration(configuration);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            #region EntityFramework

            string connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            builder.Services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(connectionString));

            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            #endregion

            #region Registirations
            builder.Services.AddTransient<OrganizerOperations>();
            builder.Services.AddTransient<EventOperations>();
            builder.Services.AddTransient<ParticipantOperations>();
            #endregion



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

            app.Run();
        }
    }
}
