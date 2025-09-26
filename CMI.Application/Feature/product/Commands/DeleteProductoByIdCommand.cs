using CMI.Application.DTOs;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public record DeleteProductoByIdCommand(
        int Id
    ) : IRequest<Unit>;
}
