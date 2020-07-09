using System.Collections.Generic;
using System.Linq;

namespace NoDuplicatesDesigns._07_AggregateWithAnemicChildren
{
    public class Catalog : IAggregateRoot
    {
        public int Id { get; set; }

        // exposing as list just to make repo code simpler
        public List<Product> Products { get; private set; } = new List<Product>();

        public void AddProduct(Product product)
        {
            if (Products.Any(p => p.Name == product.Name)) throw new System.Exception("Duplicate name.");
            Products.Add(product);
        }

        public void UpdateProductName(Product product, string newName)
        {
            if (product.Name == newName) return;

            var productToUpdate = Products.First(p => p.Id == product.Id);
            if (Products.Any(p => p.Name == newName)) throw new System.Exception("Duplicate name.");

            productToUpdate.Name = newName;
        }
    }
}
