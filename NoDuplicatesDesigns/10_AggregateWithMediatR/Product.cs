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
            DomainActions.ValidationRequest(new ProductNameValidationRequest(this, newName)).GetAwaiter().GetResult();
            Name = newName;
        }
    }
}
