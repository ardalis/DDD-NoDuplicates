using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._06_PassFilteredDataToMethod
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
            string newName = Guid.NewGuid().ToString();
            var matchingNames = _productRepository.List(p => p.Name == newName)
                .Select(p => p.Name)
                .ToArray();

            product.UpdateName(newName, matchingNames);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void UpdatesNameGivenCurrentName()
        {
            var product = _productRepository.GetById(TEST_ID2);
            string newName = product.Name;
            var matchingNames = _productRepository.List(p => p.Name == newName)
                .Select(p => p.Name)
                .ToArray();

            product.UpdateName(newName, matchingNames);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void InsertsNewProductGivenUniqueName()
        {
            var product = new Product() { Id = 4 };
            string newName = Guid.NewGuid().ToString();
            var matchingNames = _productRepository.List(p => p.Name == newName)
                .Select(p => p.Name)
                .ToArray();

            product.UpdateName(newName, matchingNames);

            _productRepository.Add(product);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameAfterUpdate()
        {
            var product = _productRepository.GetById(TEST_ID2);
            string newName = TEST_NAME;
            var matchingNames = _productRepository.List(p => p.Name == newName)
                .Select(p => p.Name)
                .ToArray();

            var result = Assert.Throws<Exception>(() => product.UpdateName(newName, matchingNames));

            Assert.Equal("Duplicate name.", result.Message);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameForNewEntity()
        {
            var newproduct = new Product() { Id = 3 };
            string newName = TEST_NAME;
            var matchingNames = _productRepository.List(p => p.Name == newName)
                .Select(p => p.Name)
                .ToArray();

            var result = Assert.Throws<Exception>(() => newproduct.UpdateName(newName, matchingNames));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
