using MediatR;
using CMI.Application.DTOs;

namespace CMI.Application.Feature.product.Commands
{
    public record UpdateProductoCommand(
        int Id,
        string Nombre,
        string Descripcion,
        decimal Precio,
        int Stock
    ) : IRequest<ProductoDto>;
}
