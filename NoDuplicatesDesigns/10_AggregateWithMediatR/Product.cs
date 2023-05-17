using MediatR;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
{
    public class Product
    {
        public Product()
        {
        }
        public Product(string name)
        {
            Name = name;
        }
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public string Name { get; private set; }

        public void UpdateName(string newName)
        {
            if (Name == newName) return;
            DomainEvents.Raise(new ProductNameChangeRequested(this, newName)).GetAwaiter().GetResult();
            Name = newName;
        }

        // alternately without the static DomainEvents class just pass IMediator around
        public void UpdateName2(string newName, IMediator mediator)
        {
            if (Name == newName) return;
            mediator.Publish(new ProductNameChangeRequested(this, newName));
            Name = newName;
        }

    }
}
