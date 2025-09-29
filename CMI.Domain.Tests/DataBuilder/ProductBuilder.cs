using CMI.Domain.Entities;

namespace CMI.Domain.Tests.DataBuilder
{
    public class ProductBuilder
    {
        private int _id;
        private string _name;
        private string _description;
        private decimal _price;
        private int _stock;

        public ProductBuilder()
        {
            _id = 1;
            _name = "Test Product";
            _description = "Test Description";
            _price = 100.00m;
            _stock = 10;
        }

        public Product Build()
        {
            return new Product
            {
                Id = _id,
                Name = _name,
                Description = _description,
                Price = _price,
                Stock = _stock
            };
        }

        public ProductBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ProductBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductBuilder WithStock(int stock)
        {
            _stock = stock;
            return this;
        }
    }
}
