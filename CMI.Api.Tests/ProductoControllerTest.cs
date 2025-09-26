using CMI.Application.DTOs;
using CMI.Application.Feature.product.Commands;
using CMI.Domain.Entities;
using CMI.Domain.QueryFilters;
using Newtonsoft.Json;
using System.Net;

namespace CMI.Api.Tests
{
    [TestFixture]
    public class ProductoControllerTest
    {
        private const string PARAMETER_PATH = "api/Producto";

        private TestStartup<Program>? factory;
        private HttpClient? httpClient;

        [SetUp]
        public void SetUp()
        {
            factory = new TestStartup<Program>();
            httpClient = factory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        [TearDown]
        public void TearDown()
        {
            if (factory is IDisposable disposableFactory)
            {
                disposableFactory.Dispose();
            }
            if (httpClient is IDisposable disposableHttpClient)
            {
                disposableHttpClient.Dispose();
            }
        }

        [Test]
        public async Task ObtainListproductossAsync()
        {
            // Arrange
            List<FieldFilter> fieldFilters = [];

            HttpRequestMessage requestMessage = new(
                HttpMethod.Post,
                new Uri($"{PARAMETER_PATH}/list", UriKind.Relative)
            )
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(fieldFilters),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            // Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);
            result.EnsureSuccessStatusCode();

            string data = await result.Content.ReadAsStringAsync();
            List<ProductoDto>? listproductoDto = JsonConvert.DeserializeObject<List<ProductoDto>>(data);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(listproductoDto, Is.Not.Null);
                Assert.That(listproductoDto, Is.Not.Empty);
            });
        }

        [Test]
        public async Task ObtainproductoAsync_Ok()
        {
            //Arrange
            int productoId = 1;

            HttpRequestMessage requestMessage = new(
                HttpMethod.Get,
                new Uri($"{PARAMETER_PATH}/ProductoById/{productoId}", UriKind.Relative)
            );

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();

            string data = await result.Content.ReadAsStringAsync();
            ProductoDto? productoDto = JsonConvert.DeserializeObject<ProductoDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(productoDto, Is.Not.Null);
                Assert.That(productoDto!.Id, Is.EqualTo(productoId));
            });
        }

        [Test]
        public async Task CreateproductoAsync_Ok()
        {
            //Arrange
            CreateProductoCommand createproductoCommand = new(
                "nombreProducto",
                 "descripcionProducto",
                 1500,
                 10
            );

            HttpRequestMessage requestMessage = new(
                HttpMethod.Post,
                new Uri($"{PARAMETER_PATH}", UriKind.Relative)
            )
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(createproductoCommand),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();
            string data = await result.Content.ReadAsStringAsync();
            ProductoDto? productoDto = JsonConvert.DeserializeObject<ProductoDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(productoDto, Is.Not.Null);
                Assert.That(productoDto!.Nombre, Is.EqualTo(createproductoCommand.Nombre));
                Assert.That(productoDto!.Descripcion, Is.EqualTo(createproductoCommand.Descripcion));
                Assert.That(productoDto!.Precio, Is.EqualTo(createproductoCommand.Precio));
                Assert.That(productoDto!.Stock, Is.EqualTo(createproductoCommand.Stock));
            });
        }

        [Test]
        public async Task UpdateCustomerAsync_Ok()
        {
            //Arrange
            UpdateProductoCommand updateproductoCommand = new(
                1,
                "John Orjuela",
                 "123 Main St, New York",
                 15,
                 1
            );

            HttpRequestMessage requestMessage = new(
                HttpMethod.Put,
                new Uri($"{PARAMETER_PATH}", UriKind.Relative)
            )
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(updateproductoCommand),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();
            string data = await result.Content.ReadAsStringAsync();
            ProductoDto? productoDto = JsonConvert.DeserializeObject<ProductoDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(productoDto, Is.Not.Null);
                Assert.That(productoDto!.Id, Is.EqualTo(updateproductoCommand.Id));
                Assert.That(productoDto!.Nombre, Is.EqualTo(updateproductoCommand.Nombre));
                Assert.That(productoDto!.Descripcion, Is.EqualTo(updateproductoCommand.Descripcion));
                Assert.That(productoDto!.Precio, Is.EqualTo(updateproductoCommand.Precio));
                Assert.That(productoDto!.Stock, Is.EqualTo(updateproductoCommand.Stock));
            });
        }
    }
}
