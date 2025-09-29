using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using CMI.Api.Tests.ConfigureDataSeed;
using CMI.Domain.Exceptions;
using CMI.Domain.Ports;
using CMI.Infrastructure.Adapters;
using CMI.Infrastructure.Context;
using System.Diagnostics.CodeAnalysis;

namespace CMI.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestStartup<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        private IServiceScopeFactory? _scopeFactory;

        public TestStartup()
        {
            _scopeFactory = Services.GetService<IServiceScopeFactory>()
                            ?? throw new InvalidOperationException("IServiceScopeFactory is not available.");
        }

        [SetUp]
        public void SetUp()
        {
            _scopeFactory = Services.GetService<IServiceScopeFactory>();
            if (_scopeFactory == null)
            {
                throw new InvalidOperationException("IServiceScopeFactory is not available.");
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            InMemoryDatabaseRoot rootDb = new();

            builder.ConfigureServices(services =>
            {

                ServiceDescriptor descriptor = services.SingleOrDefault(
                    descriptors => descriptors.ServiceType == typeof(DbContextOptions<PersistenceContext>)
                )!;

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<PersistenceContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryReservationSystemContext", rootDb);
                    options.EnableServiceProviderCaching(false);
                });

                ServiceProvider sp = services.BuildServiceProvider();
                using IServiceScope scope = sp.CreateScope();
                using PersistenceContext appContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
                services.AddTransient(typeof(IQueryWrapper), typeof(DapperWrapper));

                try
                {
                    appContext.Database.EnsureCreated();

                    #region Product
                    ProductSeed.ConfigureDataSeed(appContext);
                    appContext.SaveChanges();
                    #endregion                    
                }
                catch (Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            });

            builder.UseEnvironment("Development");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
