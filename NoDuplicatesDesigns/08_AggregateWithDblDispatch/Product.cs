﻿namespace NoDuplicatesDesigns._08_AggregateWithDoubleDispatch
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
        public int CatalogId { get; set; }
        public string Name { get; private set; }

        public void UpdateName(string newName, Catalog catalog)
        {
            if (Name == newName) return;

            // nothing forces catalog to be the *correct* catalog instance so check it here
            if (catalog.Id != CatalogId) throw new System.Exception("Wrong catalog passed in");
            catalog.ValidateNameNotAlreadyInUse(newName);
            Name = newName;
        }
    }
}
