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
        protected bool _eventsWiredUp = false;

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

        public class ProductNameChangeHandler : IRequestHandler<ProductNameValidationRequest>
        {
            private readonly CatalogRepository _catalogRepository;

            public ProductNameChangeHandler(CatalogRepository catalogRepository)
            {
                _catalogRepository = catalogRepository;
            }

            public Task<Unit> Handle(ProductNameValidationRequest request, CancellationToken cancellationToken)
            {
                var catalog = _catalogRepository.GetById(request.Product.CatalogId);
                if (catalog.Products.Any(p => p.Id == request.Product.Id))
                {
                    catalog.ValidateNameNotAlreadyInUse(request.NewName);
                }

                return Unit.Task;
            }
        }
    }
}
