namespace NoDuplicatesDesigns._07_AggregateWithAnemicChildren
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
        public string Name { get; internal set; } // so the service can set but a client in another assembly cannot
    }
}
