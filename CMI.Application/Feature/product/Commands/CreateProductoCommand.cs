using CMI.Application.DTOs;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public record CreateProductoCommand(
        string Nombre,
        string Descripcion,
        decimal Precio,
        int Stock
    ) : IRequest<ProductoDto>;
}
