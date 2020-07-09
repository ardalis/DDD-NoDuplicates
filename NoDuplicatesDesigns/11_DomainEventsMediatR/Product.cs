namespace NoDuplicatesDesigns._11_DomainEventsMediatR
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
        public string Name { get; internal set; } // accessible setter to make fake repository easier

        public void UpdateName(string newName)
        {
            if (Name == newName) return;
            DomainEvents.Raise(new ProductNameChangeRequested(this, newName)).GetAwaiter().GetResult();
            Name = newName;
        }
    }
}
