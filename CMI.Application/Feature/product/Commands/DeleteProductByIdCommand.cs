using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public record DeleteProductByIdCommand(
        int Id
    ) : IRequest<Unit>;
}
