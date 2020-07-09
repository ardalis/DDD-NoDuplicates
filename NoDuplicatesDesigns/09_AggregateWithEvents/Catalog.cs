using System.Collections.Generic;
using System.Linq;

namespace NoDuplicatesDesigns._09_AggregateWithEvents
{
    /// <summary>
    /// See https://github.com/ardalis/AggregateEvents
    /// </summary>
    public class Catalog : IAggregateRoot
    {
        public int Id { get; set; }

        // exposing as list just to make repo code simpler
        public List<Product> Products { get; private set; } = new List<Product>();
        protected bool _eventsWiredUp = false;

        public void AddProduct(Product product)
        {
            ValidateNameNotAlreadyInUse(product, new Product.ProductNameChangeRequestedEventArgs(product.Name));
            Products.Add(product);
            product.NameChangeRequestedEvent += ValidateNameNotAlreadyInUse;
        }

        public void ValidateNameNotAlreadyInUse(object sender, Product.ProductNameChangeRequestedEventArgs e)
        {
            if (Products.Any(p => p.Name == e.NewName)) throw new System.Exception("Duplicate name.");
        }

        // Needs to be called after Catalog is fetched from repository/ORM
        public void WireUpEvents()
        {
            if (_eventsWiredUp) return;
            foreach(var product in Products)
            {
                product.NameChangeRequestedEvent += ValidateNameNotAlreadyInUse;
            }
        }
    }
}
