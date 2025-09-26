using MediatR;
using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;

namespace CMI.Application.Feature.product.Commands
{
    public class UpdateProductoCommandHandler(
        ProductoService service
    ) : IRequestHandler<UpdateProductoCommand, ProductoDto>
    {
        public async Task<ProductoDto> Handle(
            UpdateProductoCommand command,
            CancellationToken cancellationToken
        )
        {
            Producto Producto = await service.UpdateProductoAsync(
                command.Id,
                command.Nombre,
                command.Descripcion,
                command.Precio,
                command.Stock
            );

            return MapProductoToProductoDto(Producto);
        }

        private static ProductoDto MapProductoToProductoDto(Producto producto)
        {
            return new ProductoDto()
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock
            };
        }
    }
}
