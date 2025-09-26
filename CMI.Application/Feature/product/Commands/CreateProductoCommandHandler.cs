using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Commands
{
    public class CreateProductoCommandHandler(
        ProductoService service
    ) : IRequestHandler<CreateProductoCommand, ProductoDto>
    {
        public async Task<ProductoDto> Handle(
            CreateProductoCommand command,
            CancellationToken cancellationToken
        )
        {
            Producto Producto = await service.CreateProductoAsync(
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
