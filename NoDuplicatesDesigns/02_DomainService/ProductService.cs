using System;
using System.Linq;

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
}
