using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._05_MethodInjectionFunction
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

        private void ValidateNameIsUnique(string name, Product productBeingUpdated)
        {
            if (_productRepository
                .List(p => p.Name == name && p.Id != productBeingUpdated.Id)
                .Any()) throw new Exception("Duplicate name.");
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameAfterUpdate()
        {
            var product = _productRepository.GetById(TEST_ID2);

            var result = Assert.Throws<Exception>(() => product.UpdateName(TEST_NAME, ValidateNameIsUnique));

            Assert.Equal("Duplicate name.", result.Message);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameForNewEntity()
        {
            var newproduct = new Product() { Id = 3 };

            var result = Assert.Throws<Exception>(() => newproduct.UpdateName(TEST_NAME, ValidateNameIsUnique));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
