using MediatR;
using CMI.Application.DTOs;

namespace CMI.Application.Feature.product.Commands
{
    public record UpdateProductCommand(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int Stock
    ) : IRequest<ProductDto>;
}
