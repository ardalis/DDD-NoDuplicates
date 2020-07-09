using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._08_AggregateWithDoubleDispatch
{
    public class CatalogAddProductTests
    {
        private CatalogRepository _catalogRepository = new CatalogRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;
        private const int TEST_CATALOG_ID = 123;

        public CatalogAddProductTests()
        {
            SeedData();
        }

        private void SeedData()
        {
            var catalog = new Catalog() { Id = TEST_CATALOG_ID };
            catalog.Products.Add(new Product(TEST_NAME) { Id = TEST_ID1 });
            catalog.Products.Add(new Product(Guid.NewGuid().ToString()) { Id = TEST_ID2 });
            _catalogRepository.Add(catalog);
        }

        [Fact]
        public void InsertsNewProductGivenUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var product = new Product(newName) { Id = 4 };
            var catalog = _catalogRepository.GetById(TEST_CATALOG_ID);

            catalog.AddProduct(product);

            _catalogRepository.Update(catalog);

            Assert.Contains(newName, catalog.Products.Select(p => p.Name));
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateNameOnInsert()
        {
            var newproduct = new Product(TEST_NAME) { Id = 3 };
            var catalog = _catalogRepository.GetById(TEST_CATALOG_ID);

            var result = Assert.Throws<Exception>(() => catalog.AddProduct(newproduct));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
