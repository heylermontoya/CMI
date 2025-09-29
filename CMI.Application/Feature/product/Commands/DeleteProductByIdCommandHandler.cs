using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public class DeleteProductByIdCommandHandler(
        ProductService service
    ) : IRequestHandler<DeleteProductByIdCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteProductByIdCommand command,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteProductAsync(
                command.Id
            );

            return Unit.Value;
        }
    }
}
