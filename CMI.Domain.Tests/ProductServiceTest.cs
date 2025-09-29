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
    public class ProductServiceTest
    {
        private ProductService Service { get; set; } = default!;
        private IGenericRepository<Product> Repository { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            Repository = Substitute.For<IGenericRepository<Product>>();
            Service = new ProductService(Repository);
        }

        [TestMethod]
        public async Task CreateProductAsync_Ok()
        {
            //Arrange
            var product = new ProductBuilder()
                .WithId(1)
                .WithName("Laptop")
                .WithDescription("Laptop gamer")
                .WithPrice(2000)
                .WithStock(10)
                .Build();

            Repository.GetAsync(Arg.Any<Expression<Func<Product, bool>>>())
                      .Returns([]);
            Repository.AddAsync(Arg.Any<Product>())
                      .Returns(product);

            //Act
            var result = await Service.CreateProductAsync(product.Name, product.Description, product.Price, product.Stock);

            //Assert
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.Description, result.Description);
            Assert.AreEqual(product.Price, result.Price);
            Assert.AreEqual(product.Stock, result.Stock);

            await Repository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<Product>());
            await Repository.ReceivedWithAnyArgs(1).GetAsync(Arg.Any<Expression<Func<Product, bool>>>());
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedNameRequired()
        {
            //Act
            string name = "";
            string decription = "desc";
            decimal price = 100;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            //Assert
            Assert.AreEqual(MessagesExceptions.RequiredProductName, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedNameTooLong()
        {
            string name = new('a', 21);
            string decription = "desc";
            decimal price = 1;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.MaxLengthProductName, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedDescriptionTooLong()
        {
            string name = "Name";
            string decription = new('d', 41);
            decimal price = 1;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.MaxLengthProductDescription, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedPriceZeroOrNegative()
        {
            string name = "Name";
            string decription = "desc";
            decimal price = 0;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.InvalidProductPrice, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedStockZeroOrNegative()
        {
            string name = "Name";
            string decription = "desc";
            decimal price = 100;
            int stock = 0;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.InvalidProductStock, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedPriceTooHigh()
        {
            string name = "Name";
            string decription = "desc";
            decimal price = 2000000;
            int stock = 5;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.MaxValueProductPrice, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedStockTooHigh()
        {
            string name = "Name";
            string decription = "desc";
            decimal price = 100;
            int stock = 20000;

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.MaxValueProductStock, ex.Message);
        }

        [TestMethod]
        public async Task CreateProductAsync_FailedDuplicateName()
        {
            string name = "Name";
            string decription = "desc";
            decimal price = 1500;
            int stock = 8;

            var product = new ProductBuilder()
                .WithId(1)
                .WithName("Name")
                .Build();

            Repository.GetAsync(Arg.Any<Expression<Func<Product, bool>>>())
                      .Returns([product]);

            var ex = await Assert.ThrowsExceptionAsync<ValidatorException>(() =>
                Service.CreateProductAsync(name, decription, price, stock));

            Assert.AreEqual(MessagesExceptions.DuplicatedProductName, ex.Message);
        }

        [TestMethod]
        public async Task UpdateProductAsync_Ok()
        {
            string name = "Laptop new";
            string decription = "\"desc\"";
            decimal price = 1500;
            int stock = 8;

            var product = new ProductBuilder()
                .WithId(1)
                .WithName("Laptop")
                .WithDescription("Laptop gamer")
                .WithPrice(2000)
                .WithStock(10)
                .Build();

            Repository.GetByIdAsync(product.Id).Returns(product);
            Repository.GetAsync(Arg.Any<Expression<Func<Product, bool>>>())
                      .Returns([]);
            Repository.UpdateAsync(Arg.Any<Product>())
                      .Returns(product);

            var result = await Service.UpdateProductAsync(product.Id, name, decription, price, stock);

            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(price, result.Price);
            Assert.AreEqual(stock, result.Stock);

            await Repository.Received(1).UpdateAsync(Arg.Any<Product>());
            await Repository.Received(1).GetAsync(Arg.Any<Expression<Func<Product, bool>>>());
            await Repository.Received(1).GetByIdAsync(Arg.Any<int>());
        }

        [TestMethod]
        public async Task GetProductById_Ok()
        {
            var product = new ProductBuilder()
                .WithId(1)
                .WithName("Laptop")
                .Build();

            Repository.GetByIdAsync(product.Id).Returns(product);

            var result = await Service.GetProductById(product.Id);

            Assert.AreEqual(product.Name, result.Name);
            await Repository.Received(1).GetByIdAsync(product.Id);
        }

        [TestMethod]
        public async Task GetProductById_NotFound()
        {
            Repository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            var ex = await Assert.ThrowsExceptionAsync<AppException>(() =>
                Service.GetProductById(99));

            Assert.AreEqual(MessagesExceptions.NotExistProduct, ex.Message);
        }
    }
}
