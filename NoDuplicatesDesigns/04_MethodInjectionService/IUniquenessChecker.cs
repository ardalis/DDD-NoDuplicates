using System;
using System.Linq;

namespace NoDuplicatesDesigns._04_MethodInjectionService
{
    public interface IUniquenessChecker
    {
        void ValidateNameIsUnique(string name, Product productBeingUpdated);
    }

    public class UniquessCheckerService : IUniquenessChecker
    {
        private readonly ProductRepository _productRepository;

        public UniquessCheckerService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void ValidateNameIsUnique(string name, Product productBeingUpdated)
        {
            if (_productRepository
                .List(p => p.Name == name && p.Id != productBeingUpdated.Id)
                .Any()) throw new Exception("Duplicate name.");
        }
    }
}