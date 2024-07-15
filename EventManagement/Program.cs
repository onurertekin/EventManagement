using DatabaseModel;
using DomainService.Config;
using DomainService.Operations;
using Host.Helpers.Swagger;
using Host.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace EventManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Config

            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            builder.WebHost.UseConfiguration(configuration);

            builder.Services.Configure<SmtpConfig>(configuration.GetSection("Smtp"));

            #endregion

            builder.Services.AddControllers();

            #region Swagger

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.OperationFilter<TokenParameterFilter>();
                c.CustomSchemaIds((type) => type.FullName.Replace("+", "_"));
                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}{e.HttpMethod}{e.RelativePath}");
            });

            #endregion

            #region EntityFramework

            string connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            builder.Services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(connectionString));

            #endregion

            #region Registirations

            builder.Services.AddTransient<OrganizerOperations>();
            builder.Services.AddTransient<EventOperations>();
            builder.Services.AddTransient<ParticipantOperations>();
            builder.Services.AddTransient<AuthenticationOperations>();
            builder.Services.AddTransient<JobOperations>();
            builder.Services.AddTransient<EMailOperations>();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Middlewares

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<TransactionMiddleware>();

            #endregion

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
