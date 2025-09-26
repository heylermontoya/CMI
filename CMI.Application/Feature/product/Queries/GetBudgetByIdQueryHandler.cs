using CMI.Application.DTOs;
using CMI.Domain.Entities;
using CMI.Domain.Services;
using MediatR;

namespace CMI.Application.Feature.product.Queries
{
    public class GetProductoByIdQueryHandler(
        ProductoService service
    ) : IRequestHandler<GetProductoByIdQuery, ProductoDto>
    {
        public async Task<ProductoDto> Handle(
            GetProductoByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            Producto Producto = await service.GetProductoById(
                command.Id
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
