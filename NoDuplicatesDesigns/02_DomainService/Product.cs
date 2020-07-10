namespace NoDuplicatesDesigns._02_DomainService
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
        public decimal Price { get; private set; }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }
    }
}
