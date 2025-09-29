using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Queries
{
    public class GetProductByIdQueryHandler(
        ProductService service
    ) : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(
            GetProductByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.GetProductById(
                command.Id
            );

            return MapProductToProductDto(product);
        }

        private static ProductDto MapProductToProductDto(Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}
