using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._04_MethodInjectionService
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
        public void UpdatesNameGivenNewUniqueName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            var checker = new UniquessCheckerService(_productRepository);
            string newName = Guid.NewGuid().ToString();

            product.UpdateName(newName, checker);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void UpdatesNameGivenCurrentName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            var checker = new UniquessCheckerService(_productRepository);
            string newName = product.Name;

            product.UpdateName(newName, checker);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void InsertsNewProductGivenUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var product = new Product(newName) { Id = 4 };
            var checker = new UniquessCheckerService(_productRepository);

            product.UpdateName(newName, checker);
            _productRepository.Add(product);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameAfterUpdate()
        {
            var product = _productRepository.GetById(TEST_ID2);
            var checker = new UniquessCheckerService(_productRepository);

            var result = Assert.Throws<Exception>(() => product.UpdateName(TEST_NAME, checker));

            Assert.Equal("Duplicate name.", result.Message);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameForNewEntity()
        {
            var newproduct = new Product() { Id = 3 };
            var checker = new UniquessCheckerService(_productRepository);

            var result = Assert.Throws<Exception>(() => newproduct.UpdateName(TEST_NAME, checker));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
