using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NoDuplicatesDesigns._05_MethodInjectionFunction
{
    public class ProductRepository
    {
        private Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(k => k.Key == id).Value;

            return new Product(product.Name) { Id = product.Id };
        }

        public IEnumerable<Product> List(Expression<Func<Product, bool>> filterExpression)
        {
            return _products.Values.AsQueryable().Where(filterExpression).AsEnumerable();
        }

        public void Add(Product product)
        {
            if (_products.ContainsKey(product.Id)) throw new Exception("Duplicate id.");

            _products.Add(product.Id, product);
        }

        public void Update(Product product)
        {
            if (!_products.ContainsKey(product.Id)) throw new Exception("No such id.");

            _products[product.Id].UpdateName(product.Name);
        }
    }
}
