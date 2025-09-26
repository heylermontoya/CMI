using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public class DeleteProductoByIdCommandHandler(
        ProductoService service
    ) : IRequestHandler<DeleteProductoByIdCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteProductoByIdCommand command,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteProductoAsync(
                command.Id
            );

            return Unit.Value;
        }
    }
}
