using System;
using System.Collections.Generic;
using System.Linq;

namespace NoDuplicatesDesigns._01_Database
{
    public class ProductRepository
    {
        private Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(k => k.Key == id).Value;

            return new Product() { Id = product.Id, Name = product.Name };
        }

        public void Add(Product product)
        {
            if (_products.ContainsKey(product.Id)) throw new Exception("Duplicate id.");
            if (_products.Values.Any(p => p.Name == product.Name)) throw new Exception("Duplicate name.");

            _products.Add(product.Id, product);
        }

        public void Update(Product product)
        {
            if (!_products.ContainsKey(product.Id)) throw new Exception("No such id.");
            if (_products.Any(p => p.Value.Name == product.Name && p.Key != product.Id)) throw new Exception("Duplicate name.");

            _products[product.Id].Name = product.Name;
        }
    }
}
