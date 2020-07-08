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
