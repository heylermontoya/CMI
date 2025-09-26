using CMI.Domain.Entities;
using CMI.Domain.Exceptions;
using CMI.Domain.Ports;

namespace CMI.Domain.Services
{
    [DomainService]
    public class ProductoService(IGenericRepository<Producto> productoRepository)
    {
        public async Task<Producto> CreateProductoAsync(
            string nombre,
            string descripcion,
            decimal precio,
            int stock
        )
        {
            #region validations

            await ValidateProductoAsync(nombre, descripcion, precio, stock);

            #endregion

            Producto producto = new()
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = precio,
                Stock = stock
            };

            producto = await productoRepository.AddAsync(producto);

            return producto;
        }

        public async Task<Producto> UpdateProductoAsync(
            int id,
            string nombre,
            string descripcion,
            decimal precio,
            int stock
        )
        {
            Producto producto = await (GetProductoById(id));

            #region validations

            await ValidateProductoAsync(nombre, descripcion, precio, stock, id);

            #endregion

            producto.Nombre = nombre;
            producto.Descripcion = descripcion;
            producto.Precio = precio;
            producto.Stock = stock;

            producto = await productoRepository.UpdateAsync(producto);

            return producto;
        }

        public async Task<Producto> GetProductoById(int id)
        {
            Producto? producto = await productoRepository.GetByIdAsync(id);

            return producto ?? throw new AppException(MessagesExceptions.NotExistProduct);
        }

        private async Task ValidateProductoAsync(string nombre, string descripcion, decimal precio, int stock, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ValidatorException(MessagesExceptions.RequiredProductName);
            }

            if (nombre.Length > 20)
            {
                throw new ValidatorException(MessagesExceptions.MaxLengthProductName);
            }

            if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Length > 40)
            {
                throw new ValidatorException(MessagesExceptions.MaxLengthProductDescription);
            }

            if (precio <= 0)
            {
                throw new ValidatorException(MessagesExceptions.InvalidProductPrice);
            }

            if (stock <= 0)
            {
                throw new ValidatorException(MessagesExceptions.InvalidProductStock);
            }

            if (precio > 1000000)
            {
                throw new ValidatorException(MessagesExceptions.MaxValueProductPrice);
            }

            if (stock > 10000)
            {
                throw new ValidatorException(MessagesExceptions.MaxValueProductStock);
            }

            var existente = await productoRepository.GetAsync(p => p.Nombre == nombre && (id == null || p.Id != id));
            if (existente.Any())
            {
                throw new ValidatorException(MessagesExceptions.DuplicatedProductName);
            }
        }
    }
}
