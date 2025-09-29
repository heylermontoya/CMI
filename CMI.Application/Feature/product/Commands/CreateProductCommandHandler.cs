using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public class CreateProductCommandHandler(
        ProductService service
    ) : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(
            CreateProductCommand command,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.CreateProductAsync(
                command.Name,
                command.Description,
                command.Price,
                command.Stock
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
