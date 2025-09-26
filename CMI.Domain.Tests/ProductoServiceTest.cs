using CMI.Domain.Entities;
using CMI.Domain.Exceptions;
using CMI.Domain.Ports;
using CMI.Domain.Services;
using CMI.Domain.Tests.DataBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace CMI.Domain.Tests
{
    [TestClass]
    public class ProductoServiceTest
    {
        private ProductoService Service { get; set; } = default!;
        private IGenericRepository<Producto> Repository { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<Producto>>();
            Service = new ProductoService(Repository);
        }

        [TestMethod]
        public async Task CreateProductoAsync_Ok()
        {
            //Arrange
            var producto = new ProductoBuilder()
                .WithId(1)
                .WithNombre("Laptop")
                .WithDescripcion("Laptop gamer")
                .WithPrecio(2000)
                .WithStock(10)
                .Build();

            Repository.GetAsync(Arg.Any<Expression<Func<Producto, bool>>>())
                      .Returns([]);
            Repository.AddAsync(Arg.Any<Producto>())
                      .Returns(producto);

            //Act
            var result = await Service.CreateProductoAsync(producto.Nombre, producto.Descripcion, producto.Precio, producto.Stock);

            //Assert
            Assert.AreEqual(producto.Id, result.Id);
            Assert.AreEqual(producto.Nombre, result.Nombre);
            Assert.AreEqual(producto.Descripcion, result.Descripcion);
            Assert.AreEqual(producto.Precio, result.Precio);
            Assert.AreEqual(producto.Stock, result.Stock);

            await Repository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<Producto>());
            await Repository.ReceivedWithAnyArgs(1).GetAsync(Arg.Any<Expression<Func<Producto, bool>>>());
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedNameRequired()
        {
            //Act
            string nombre = "";
            string decripcion = "desc";
            decimal precio = 100;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            //Assert
            Assert.AreEqual(MessagesExceptions.RequiredProductName, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedNameTooLong()
        {
            string nombre = new('a', 21);
            string decripcion = "desc";
            decimal precio = 1;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.MaxLengthProductName, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedDescriptionTooLong()
        {
            string nombre = "Nombre";
            string decripcion = new('d', 41);
            decimal precio = 1;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.MaxLengthProductDescription, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedPriceZeroOrNegative()
        {
            string nombre = "Nombre";
            string decripcion = "desc";
            decimal precio = 0;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.InvalidProductPrice, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedStockZeroOrNegative()
        {
            string nombre = "Nombre";
            string decripcion = "desc";
            decimal precio = 100;
            int stock = 0;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.InvalidProductStock, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedPriceTooHigh()
        {
            string nombre = "Nombre";
            string decripcion = "desc";
            decimal precio = 2000000;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.MaxValueProductPrice, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedStockTooHigh()
        {
            string nombre = "Nombre";
            string decripcion = "desc";
            decimal precio = 100;
            int stock = 20000;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.MaxValueProductStock, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductoAsync_FailedDuplicateName()
        {
            string nombre = "Nombre";
            string decripcion = "desc";
            decimal precio = 1500;
            int stock = 8;

            var producto = new ProductoBuilder()
                .WithId(1)
                .WithNombre("Nombre")
                .Build();

            Repository.GetAsync(Arg.Any<Expression<Func<Producto, bool>>>())
                      .Returns([producto]);

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductoAsync(nombre, decripcion, precio, stock));

            Assert.AreEqual(MessagesExceptions.DuplicatedProductName, ex.Message);
        }

        [TestMethod]
        public async Task UpdateProductoAsync_Ok()
        {
            string nombre = "Laptop Nueva";
            string decripcion = "\"desc\"";
            decimal precio = 1500;
            int stock = 8;

            var producto = new ProductoBuilder()
                .WithId(1)
                .WithNombre("Laptop")
                .WithDescripcion("Laptop gamer")
                .WithPrecio(2000)
                .WithStock(10)
                .Build();

            Repository.GetByIdAsync(producto.Id).Returns(producto);
            Repository.GetAsync(Arg.Any<Expression<Func<Producto, bool>>>())
                      .Returns([]);
            Repository.UpdateAsync(Arg.Any<Producto>())
                      .Returns(producto);

            var result = await Service.UpdateProductoAsync(producto.Id, nombre, decripcion, precio, stock);

            Assert.AreEqual(producto.Id, result.Id);
            Assert.AreEqual(nombre, result.Nombre);
            Assert.AreEqual(precio, result.Precio);
            Assert.AreEqual(stock, result.Stock);

            await Repository.Received(1).UpdateAsync(Arg.Any<Producto>());
            await Repository.Received(1).GetAsync(Arg.Any<Expression<Func<Producto, bool>>>());
            await Repository.Received(1).GetByIdAsync(Arg.Any<int>());
        }

        [TestMethod]
        public async Task GetProductoById_Ok()
        {
            var producto = new ProductoBuilder()
                .WithId(1)
                .WithNombre("Laptop")
                .Build();

            Repository.GetByIdAsync(producto.Id).Returns(producto);

            var result = await Service.GetProductoById(producto.Id);

            Assert.AreEqual(producto.Nombre, result.Nombre);
            await Repository.Received(1).GetByIdAsync(producto.Id);
        }

        [TestMethod]
        public async Task GetProductoById_NotFound()
        {
            Repository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            var ex = await Assert.ThrowsExceptionAsync<AppException>(() =>
                Service.GetProductoById(99));

            Assert.AreEqual(MessagesExceptions.NotExistProduct, ex.Message);
        }
    }
}
