using MediatR;
using CMI.Application.DTOs;

namespace CMI.Application.Feature.product.Queries
{
    public record GetProductByIdQuery(
        int Id
    ) : IRequest<ProductDto>;
}
