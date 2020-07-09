using MediatR;

namespace NoDuplicatesDesigns._11_DomainEventsMediatR
{
    public class ProductNameChangeRequested : INotification
    {
        public ProductNameChangeRequested(Product product, string newName)
        {
            Product = product;
            NewName = newName;
        }

        public Product Product { get; }
        public string NewName { get; }
    }
}
