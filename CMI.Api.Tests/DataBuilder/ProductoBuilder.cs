using CMI.Domain.Entities;

namespace CMI.Api.Tests.DataBuilder
{
    public class ProductoBuilder
    {
        private int _id;
        private string _nombre;
        private string _descripcion;
        private decimal _precio;
        private int _stock;

        public ProductoBuilder()
        {
            _id = 1;
            _nombre = "Producto de prueba";
            _descripcion = "Descripción de prueba";
            _precio = 100.00m;
            _stock = 10;
        }

        public Producto Build()
        {
            return new Producto
            {
                Id = _id,
                Nombre = _nombre,
                Descripcion = _descripcion,
                Precio = _precio,
                Stock = _stock
            };
        }

        public ProductoBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ProductoBuilder WithNombre(string nombre)
        {
            _nombre = nombre;
            return this;
        }

        public ProductoBuilder WithDescripcion(string descripcion)
        {
            _descripcion = descripcion;
            return this;
        }

        public ProductoBuilder WithPrecio(decimal precio)
        {
            _precio = precio;
            return this;
        }

        public ProductoBuilder WithStock(int stock)
        {
            _stock = stock;
            return this;
        }
    }
}
