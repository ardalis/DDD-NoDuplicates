using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
{
    /// <summary>
    /// See https://github.com/ardalis/AggregateEvents
    /// </summary>
    public class Catalog : IAggregateRoot
    {
        public int Id { get; set; }

        // exposing as list just to make repo code simpler
        public List<Product> Products { get; private set; } = new List<Product>();

        public void AddProduct(Product product)
        {
            ValidateNameNotAlreadyInUse(product.Name);
            product.CatalogId = this.Id;
            Products.Add(product);
        }

        private void ValidateNameNotAlreadyInUse(string newName)
        {
            if (Products.Any(p => p.Name == newName)) throw new System.Exception("Duplicate name.");
        }

        public class ProductNameChangeHandler : INotificationHandler<ProductNameChangeRequested>
        {
            private readonly CatalogRepository _catalogRepository;

            public ProductNameChangeHandler(CatalogRepository catalogRepository)
            {
                _catalogRepository = catalogRepository;
            }

            public Task Handle(ProductNameChangeRequested notification, CancellationToken cancellationToken)
            {
                var catalog = _catalogRepository.GetById(notification.Product.CatalogId);
                if (catalog.Products.Any(p => p.Id == notification.Product.Id))
                {
                    catalog.ValidateNameNotAlreadyInUse(notification.NewName);
                }
                return Task.CompletedTask;
            }
        }
    }
}
