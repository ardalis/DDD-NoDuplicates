using System.Linq;

namespace NoDuplicatesDesigns._04_MethodInjectionService
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
        public string Name { get; private set; }

        public void UpdateName(string newName, IUniquenessChecker checker)
        {
            checker.ValidateNameIsUnique(newName, this);

            Name = newName;
        }
    }
}
