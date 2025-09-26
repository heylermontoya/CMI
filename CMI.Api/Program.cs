using System.Reflection;
using MediatR;
using Serilog;
using Prometheus;
using Microsoft.EntityFrameworkCore;
using CMI.Api.Filters;
using CMI.Infrastructure.Extensions;
using CMI.Infrastructure.Context;

namespace CMI.Api
{
    public partial class Program
    {
        protected Program() { }

        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigurationManager config = builder.Configuration;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers(opts =>
            {
                opts.Filters.Add(typeof(AppExceptionFilterAttribute));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(
                Assembly.Load("CMI.Application"),
                typeof(Program).Assembly
            );

            builder.Services.AddAutoMapper(
                Assembly.Load("CMI.Application")
            );

            string stringConnection = config["StringConnection"]!;

            builder.Services.AddDbContext<PersistenceContext>(opt =>
            {
                opt.UseSqlServer(stringConnection, sqlopts =>
                {
                    sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("SchemaName"));
                });
            });

            builder.Services
                .AddHealthChecks();

            builder.Services
                .AddLogging(loggingBuilder => loggingBuilder.AddConsole()
                .AddSerilog(dispose: true));

            builder.Services.AddHttpContextAccessor();

            builder.Services
                .AddPersistence(stringConnection)
                .AddDomainServices();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "CMI", Version = "version 1.0.0" });
                options.CustomSchemaIds(schema => schema.FullName);
                options.DocumentFilter<BasePathFilter>(config["BasePathFilter"]);
            });

            Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

            WebApplication app = builder.Build();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMI"));

            app.UseRouting();

            app.UseHttpMetrics().UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapHealthChecks("/health");
            });

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
