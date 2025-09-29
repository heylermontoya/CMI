using CMI.Application.DTOs;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Stock
    ) : IRequest<ProductDto>;
}
