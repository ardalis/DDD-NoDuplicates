using System.Linq;

namespace NoDuplicatesDesigns._03_PassDataToMethod
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

        public void UpdateName(string newName, string[] allOtherProductNames)
        {
            if (allOtherProductNames.Contains(newName)) throw new System.Exception("Duplicate name.");

            Name = newName;
        }
    }
}
