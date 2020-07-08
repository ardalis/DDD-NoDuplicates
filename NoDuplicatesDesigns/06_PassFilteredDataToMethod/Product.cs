using System.Linq;

namespace NoDuplicatesDesigns._06_PassFilteredDataToMethod
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

        public void UpdateName(string newName, string[] matchingNames)
        {
            if (Name == newName) return;

            if (matchingNames.Count() >= 1) throw new System.Exception("Duplicate name.");

            Name = newName;
        }
    }
}
