using MediatR;
using CMI.Application.DTOs;

namespace CMI.Application.Feature.product.Queries
{
    public record GetProductoByIdQuery(
        int Id
    ) : IRequest<ProductoDto>;
}
