using System;
using System.Collections.Generic;
using System.Linq;

namespace NoDuplicatesDesigns._10_AggregateWithMediatR
{
    public class CatalogRepository
    {
        private Dictionary<int, Catalog> _entities = new Dictionary<int, Catalog>();

        public Catalog GetById(int id)
        {
            var catalog = _entities.FirstOrDefault(k => k.Key == id).Value;

            var newCatalog = new Catalog() { Id = catalog.Id };

            foreach (var product in catalog.Products)
            {
                newCatalog.Products.Add(new Product(product.Name) { Id = product.Id, CatalogId = newCatalog.Id });
            }
            return newCatalog;
        }

        public void Add(Catalog catalog)
        {
            _entities.Add(catalog.Id, catalog);
        }

        public void Update(Catalog catalog)
        {
            if (!_entities.ContainsKey(catalog.Id)) throw new Exception("No such id.");

            var existingCatalog = _entities[catalog.Id];
            existingCatalog.Products.Clear();

            foreach (var product in catalog.Products)
            {
                existingCatalog.Products.Add(new Product(product.Name) { Id = product.Id, CatalogId = existingCatalog.Id });
            }
        }
    }
}
