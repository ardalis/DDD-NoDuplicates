namespace NoDuplicatesDesigns._08_AggregateWithDoubleDispatch
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
        public string Name { get; private set; }

        public void UpdateName(string newName, Catalog catalog)
        {
            if (Name == newName) return;
            catalog.ValidateNameNotAlreadyInUse(newName);
            Name = newName;
        }
    }
}
