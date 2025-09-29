using CMI.Domain.Entities;
using CMI.Domain.Exceptions;
using CMI.Domain.Ports;

namespace CMI.Domain.Services
{
    [DomainService]
    public class ProductService(IGenericRepository<Product> productRepository)
    {
        public async Task<Product> CreateProductAsync(
            string name,
            string description,
            decimal price,
            int stock
        )
        {
            #region validations

            await ValidateProductAsync(name, description, price, stock);

            #endregion

            Product product = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock
            };

            product = await productRepository.AddAsync(product);

            return product;
        }

        public async Task<Product> UpdateProductAsync(
            int id,
            string name,
            string description,
            decimal price,
            int stock
        )
        {
            Product product = await (GetProductById(id));

            #region validations

            await ValidateProductAsync(name, description, price, stock, id);

            #endregion

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Stock = stock;

            product = await productRepository.UpdateAsync(product);

            return product;
        }

        public async Task<Product> GetProductById(int id)
        {
            Product? product = await productRepository.GetByIdAsync(id);

            return product ?? throw new AppException(MessagesExceptions.NotExistProduct);
        }

        private async Task ValidateProductAsync(string name, string description, decimal price, int stock, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidatorException(MessagesExceptions.RequiredProductName);
            }

            if (name.Length > 20)
            {
                throw new ValidatorException(MessagesExceptions.MaxLengthProductName);
            }

            if (!string.IsNullOrWhiteSpace(description) && description.Length > 40)
            {
                throw new ValidatorException(MessagesExceptions.MaxLengthProductDescription);
            }

            if (price <= 0)
            {
                throw new ValidatorException(MessagesExceptions.InvalidProductPrice);
            }

            if (stock <= 0)
            {
                throw new ValidatorException(MessagesExceptions.InvalidProductStock);
            }

            if (price > 1000000)
            {
                throw new ValidatorException(MessagesExceptions.MaxValueProductPrice);
            }

            if (stock > 10000)
            {
                throw new ValidatorException(MessagesExceptions.MaxValueProductStock);
            }

            var existing = await productRepository.GetAsync(p => p.Name == name && (id == null || p.Id != id));
            if (existing.Any())
            {
                throw new ValidatorException(MessagesExceptions.DuplicatedProductName);
            }
        }

        public async Task DeleteProductAsync(
            int id
        )
        {
            Product product = await (GetProductById(id));
            await productRepository.DeleteAsync(product);
        }
    }
}
