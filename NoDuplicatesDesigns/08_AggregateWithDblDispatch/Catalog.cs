using System.Collections.Generic;
using System.Linq;

namespace NoDuplicatesDesigns._08_AggregateWithDoubleDispatch
{
    public class Catalog : IAggregateRoot
    {
        public int Id { get; set; }

        // exposing as list just to make repo code simpler
        public List<Product> Products { get; private set; } = new List<Product>();

        public void AddProduct(Product product)
        {
            ValidateNameNotAlreadyInUse(product.Name);
            Products.Add(product);
        }

        public void ValidateNameNotAlreadyInUse(string newName)
        {
            if (Products.Any(p => p.Name == newName)) throw new System.Exception("Duplicate name.");
        }
    }
}
