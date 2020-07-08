using System;
using System.Linq;

namespace NoDuplicatesDesigns._05_MethodInjectionFunction
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

        public void UpdateName(string newName, Action<string, Product> nameValidator = null)
        {
            if(nameValidator != null) nameValidator.Invoke(newName, this);

            Name = newName;
        }
    }
}
