using CMI.Api.Tests.DataBuilder;
using CMI.Infrastructure.Context;

namespace CMI.Api.Tests.ConfigureDataSeed
{
    public static class ProductSeed
    {
        public static void ConfigureDataSeed(PersistenceContext context)
        {
            ProductoBuilder _builderRegisterForDeleted = new();
            context.Add(
                _builderRegisterForDeleted
                    .WithId(1)
                    .Build()
            );
        }
    }
}
