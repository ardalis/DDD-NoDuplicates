using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._11_DomainEventsMediatR
{
    public class AddProductTests
    {
        private ProductRepository _productRepository = new ProductRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;
        private const int TEST_CATALOG_ID = 123;

        public AddProductTests()
        {
            SeedData();
        }

        private void SeedData()
        {
            _productRepository.Add(new Product(TEST_NAME) { Id = TEST_ID1 });
            _productRepository.Add(new Product(Guid.NewGuid().ToString()) { Id = TEST_ID2 });
        }

        [Fact]
        public void InsertsNewProductGivenUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var product = new Product(newName) { Id = 4 };

            _productRepository.Add(product);

            Assert.Contains(newName, _productRepository.List(p => true).Select(p => p.Name));
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameOnInsert()
        {
            var newproduct = new Product(TEST_NAME) { Id = 3 };

            var result = Assert.Throws<Exception>(() =>
            {
                // we don't want to put business logic in the repo so there's no good place to put this except around the call
                if (_productRepository.List(p => p.Name == newproduct.Name).Any()) throw new Exception("Duplicate name.");
                _productRepository.Add(newproduct);
            });

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
