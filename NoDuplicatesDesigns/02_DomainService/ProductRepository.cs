using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NoDuplicatesDesigns._02_DomainService
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void UpdateName(Product product, string newName)
        {
            if (_productRepository.List(p => p.Name == newName && p.Id != product.Id).Any()) throw new Exception("Duplicate name.");

            var productToUpdate = _productRepository.GetById(product.Id);
            productToUpdate.Name = newName;
            _productRepository.Update(productToUpdate);
        }

        public void Add(Product product)
        {
            if (_productRepository.List(p => p.Name == product.Name).Any()) throw new Exception("Duplicate name.");

            _productRepository.Add(product);
        }
    }

    public class ProductRepository
    {
        private Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(k => k.Key == id).Value;

            return new Product() { Id = product.Id, Name = product.Name };
        }

        public IEnumerable<Product> List(Expression<Func<Product,bool>> filterExpression)
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

            _products[product.Id].Name = product.Name;
        }
    }
}
