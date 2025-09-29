using CMI.Application.DTOs;
using CMI.Application.Feature.product.Commands;
using CMI.Domain.QueryFilters;
using Newtonsoft.Json;
using System.Net;

namespace CMI.Api.Tests
{
    [TestFixture]
    public class ProductControllerTest
    {
        private const string PARAMETER_PATH = "api/product";

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
        public async Task ObtainListProductsAsync()
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
            List<ProductDto>? listProductDto = JsonConvert.DeserializeObject<List<ProductDto>>(data);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(listProductDto, Is.Not.Null);
                Assert.That(listProductDto, Is.Not.Empty);
            });
        }

        [Test]
        public async Task ObtainProductAsync_Ok()
        {
            //Arrange
            int productId = 1;

            HttpRequestMessage requestMessage = new(
                HttpMethod.Get,
                new Uri($"{PARAMETER_PATH}/productById/{productId}", UriKind.Relative)
            );

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();

            string data = await result.Content.ReadAsStringAsync();
            ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(productDto, Is.Not.Null);
                Assert.That(productDto!.Id, Is.EqualTo(productId));
            });
        }

        [Test]
        public async Task CreateProductAsync_Ok()
        {
            //Arrange
            CreateProductCommand createProductCommand = new(
                "nameProduct",
                 "descriptionProduct",
                 1500,
                 10
            );

            HttpRequestMessage requestMessage = new(
                HttpMethod.Post,
                new Uri($"{PARAMETER_PATH}", UriKind.Relative)
            )
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(createProductCommand),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();
            string data = await result.Content.ReadAsStringAsync();
            ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(productDto, Is.Not.Null);
                Assert.That(productDto!.Name, Is.EqualTo(createProductCommand.Name));
                Assert.That(productDto!.Description, Is.EqualTo(createProductCommand.Description));
                Assert.That(productDto!.Price, Is.EqualTo(createProductCommand.Price));
                Assert.That(productDto!.Stock, Is.EqualTo(createProductCommand.Stock));
            });
        }

        [Test]
        public async Task UpdateProductAsync_Ok()
        {
            //Arrange
            UpdateProductCommand updateProductCommand = new(
                1,
                "nameProductUpdate",
                "descriptionProductUpdate",
                15,
                1
            );

            HttpRequestMessage requestMessage = new(
                HttpMethod.Put,
                new Uri($"{PARAMETER_PATH}", UriKind.Relative)
            )
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(updateProductCommand),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            //Act
            HttpResponseMessage result = await httpClient!.SendAsync(requestMessage);

            result.EnsureSuccessStatusCode();
            string data = await result.Content.ReadAsStringAsync();
            ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(data);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(productDto, Is.Not.Null);
                Assert.That(productDto!.Id, Is.EqualTo(updateProductCommand.Id));
                Assert.That(productDto!.Name, Is.EqualTo(updateProductCommand.Name));
                Assert.That(productDto!.Description, Is.EqualTo(updateProductCommand.Description));
                Assert.That(productDto!.Price, Is.EqualTo(updateProductCommand.Price));
                Assert.That(productDto!.Stock, Is.EqualTo(updateProductCommand.Stock));
            });
        }
    }
}
