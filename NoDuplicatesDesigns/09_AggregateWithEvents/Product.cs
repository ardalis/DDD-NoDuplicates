using System;

namespace NoDuplicatesDesigns._09_AggregateWithEvents
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

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
        public delegate void ProductNameChangeRequestedEventHandler(object sender, ProductNameChangeRequestedEventArgs e);
        public event EventHandler<ProductNameChangeRequestedEventArgs> NameChangeRequestedEvent;

        public class ProductNameChangeRequestedEventArgs : EventArgs
        {
            public ProductNameChangeRequestedEventArgs(string newName)
            {
                NewName = newName;
            }

            public string NewName { get; }
        }

        public void UpdateName(string newName)
        {
            if (Name == newName) return;
            OnRaiseNameChangeRequested(new ProductNameChangeRequestedEventArgs(newName));
            Name = newName;
        }

        protected virtual void OnRaiseNameChangeRequested(ProductNameChangeRequestedEventArgs e)
        {
            EventHandler<ProductNameChangeRequestedEventArgs> eventHandler = NameChangeRequestedEvent;

            if (eventHandler == null) return;

            eventHandler(this, e);
        }
    }
}
