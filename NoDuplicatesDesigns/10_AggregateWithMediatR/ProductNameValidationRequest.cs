using MediatR;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
{
    public class ProductNameValidationRequest : IRequest
    {
        public ProductNameValidationRequest(Product product, string newName)
        {
            Product = product;
            NewName = newName;
        }

        public Product Product { get; }
        public string NewName { get; }
    }
}
