using MediatR;
using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;

namespace CMI.Application.Feature.product.Commands
{
    public class UpdateProductCommandHandler(
        ProductService service
    ) : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(
            UpdateProductCommand command,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.UpdateProductAsync(
                command.Id,
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
