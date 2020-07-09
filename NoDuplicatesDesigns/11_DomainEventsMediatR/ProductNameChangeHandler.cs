using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoDuplicatesDesigns._11_DomainEventsMediatR
{
    public class ProductNameChangeHandler : INotificationHandler<ProductNameChangeRequested>
    {
        private readonly ProductRepository _productRepository;

        public ProductNameChangeHandler(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task Handle(ProductNameChangeRequested notification, CancellationToken cancellationToken)
        {
            var existingNames = _productRepository.List(p => p.Name == notification.NewName);
            if (existingNames.Any())
            {
                throw new System.Exception("Duplicate name.");
            }
            return Task.CompletedTask;
        }
    }

}
