using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._07_AggregateWithAnemicChildren
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
            catalog.Products.Add(new Product { Id = TEST_ID1, Name = TEST_NAME });
            catalog.Products.Add(new Product { Id = TEST_ID2, Name = Guid.NewGuid().ToString() });
            _catalogRepository.Add(catalog);
        }

        //[Fact]
        //public void UpdatesNameGivenNewUniqueName()
        //{
        //    var product = _productRepository.GetById(TEST_ID2);
        //    string newName = Guid.NewGuid().ToString();
        //    var productService = new ProductService(_productRepository);

        //    productService.UpdateName(product, newName);

        //    Assert.Equal(newName, product.Name);
        //}

        //[Fact]
        //public void UpdatesNameGivenCurrentName()
        //{
        //    var product = _productRepository.GetById(TEST_ID2);
        //    string newName = product.Name;
        //    var productService = new ProductService(_productRepository);

        //    productService.UpdateName(product, newName);

        //    Assert.Equal(newName, product.Name);
        //}

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

        //[Fact]
        //public void ThrowsExceptionGivenDuplicateName()
        //{
        //    var product = _productRepository.GetById(TEST_ID2);
        //    var productService = new ProductService(_productRepository);

        //    var result = Assert.Throws<Exception>(() => productService.UpdateName(product, TEST_NAME));

        //    Assert.Equal("Duplicate name.", result.Message);
        //}

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
