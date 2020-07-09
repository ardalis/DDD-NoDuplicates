using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace NoDuplicatesDesigns._11_DomainEventsMediatR
{
    public class ProductUpdateNameTests
    {
        private ProductRepository _productRepository = new ProductRepository();
        private const string TEST_NAME = "Test Name";
        private const int TEST_ID1 = 1;
        private const int TEST_ID2 = 2;
        private const int TEST_CATALOG_ID = 123;
        private ServiceProvider _serviceProvider;

        public ProductUpdateNameTests()
        {
            SeedData();
            SetUpMediatR();
        }

        private void SetUpMediatR()
        {
            _serviceProvider = BuildServiceProvider();
            DomainEvents.Mediator = () => BuildMediator(_serviceProvider);
        }

        private ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(DomainEvents));
            services.AddSingleton(_productRepository);

            return services.BuildServiceProvider();
        }

        private IMediator BuildMediator(ServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IMediator>();
        }

        private void SeedData()
        {
            _productRepository.Add(new Product(TEST_NAME) { Id = TEST_ID1 });
            _productRepository.Add(new Product(Guid.NewGuid().ToString()) { Id = TEST_ID2 });
        }

        [Fact]
        public void UpdatesNameGivenNewUniqueName()
        {
            string newName = Guid.NewGuid().ToString();
            var product = _productRepository.GetById(TEST_ID1);

            product.UpdateName(newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void UpdatesNameGivenCurrentName()
        {
            var product = _productRepository.GetById(TEST_ID1);
            string newName = product.Name;

            product.UpdateName(newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ThrowsExceptionGivenDuplicateName()
        {
            var product = _productRepository.GetById(TEST_ID2);

            var result = Assert.Throws<Exception>(() => product.UpdateName(TEST_NAME));

            Assert.Equal("Duplicate name.", result.Message);
        }
    }
}
