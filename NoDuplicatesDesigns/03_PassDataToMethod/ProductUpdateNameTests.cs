using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._03_PassDataToMethod
{
    public class ProductUpdateNameTests
    {
        private ProductRepository _productRepository = new ProductRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;

        public ProductUpdateNameTests()
        {
            SeedData();
        }

        private void SeedData()
        {
            _productRepository.Add(new Product(TEST_NAME) { Id = TEST_ID1 });
            _productRepository.Add(new Product(Guid.NewGuid().ToString()) { Id = TEST_ID2 });
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameAfterUpdate()
        {
            var product = _productRepository.GetById(TEST_ID2);
            var otherProductNames = _productRepository.List(p => p.Id != product.Id)
                .Select(p => p.Name)
                .ToArray();

            var result = Assert.Throws<Exception>(() => product.UpdateName(TEST_NAME, otherProductNames));

            Assert.Equal("Duplicate name.", result.Message);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameForNewEntity()
        {
            var newproduct = new Product() { Id = 3 };
            var otherProductNames = _productRepository.List(p => p.Id != newproduct.Id)
                .Select(p => p.Name)
                .ToArray();

            var result = Assert.Throws<Exception>(() => newproduct.UpdateName(TEST_NAME, otherProductNames));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
