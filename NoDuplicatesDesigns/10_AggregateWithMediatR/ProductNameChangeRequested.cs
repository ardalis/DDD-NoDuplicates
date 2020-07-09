using MediatR;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
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
