using System;
using Xunit;

namespace NoDuplicatesDesigns._02_DomainService
{
    public class ProductServiceUpdateNameTests
    {
        private ProductRepository _productRepository = new ProductRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;

        public ProductServiceUpdateNameTests()
        {
            SeedData();
        }

        private void SeedData()
        {
            _productRepository.Add(new Product { Id = TEST_ID1, Name = TEST_NAME });
            _productRepository.Add(new Product { Id = TEST_ID2, Name = Guid.NewGuid().ToString() });
        }

        [Fact]
        public void UpdatesNameGivenNewUniqueName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            string newName = Guid.NewGuid().ToString();
            var productService = new ProductService(_productRepository);

            productService.UpdateName(product, newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void UpdatesNameGivenCurrentName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            string newName = product.Name;
            var productService = new ProductService(_productRepository);

            productService.UpdateName(product, newName);

            // here's why I don't like this approach - the client ends up with 2 ways to work with product: through a service or directly
            product.UpdatePrice(2);
            _productRepository.Update(product);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void InsertsNewProductGivenUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var product = new Product(newName) { Id = 4 };
            var productService = new ProductService(_productRepository);

            productService.Add(product);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            var productService = new ProductService(_productRepository);

            var result = Assert.Throws<Exception>(() => productService.UpdateName(product, TEST_NAME));

            Assert.Equal("Duplicate name.", result.Message);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameOnInsert()
        {
            var productService = new ProductService(_productRepository);
            var newproduct = new Product(TEST_NAME) { Id = 3 };

            var result = Assert.Throws<Exception>(() => productService.Add(newproduct));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
