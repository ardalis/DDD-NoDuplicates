using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._09_AggregateWithEvents
{
    public class ProductUpdateNameTests
    {
        private CatalogRepository _catalogRepository = new CatalogRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;
        private const int TEST_CATALOG_ID = 123;

        public ProductUpdateNameTests()
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
        public void UpdatesNameGivenNewUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var catalog = _catalogRepository.GetById(TEST_CATALOG_ID);
            var product = catalog.Products.First();

            product.UpdateName(newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void UpdatesNameGivenCurrentName()
        {
            var catalog = _catalogRepository.GetById(TEST_CATALOG_ID);
            var product = catalog.Products.First();
            string newName = product.Name;

            product.UpdateName(newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateName()
        {
            var catalog = _catalogRepository.GetById(TEST_CATALOG_ID);
            catalog.WireUpEvents(); // this is a hack
            var product = catalog.Products.First(p => p.Id == TEST_ID2);

            var result = Assert.Throws<Exception>(() => product.UpdateName(TEST_NAME));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
